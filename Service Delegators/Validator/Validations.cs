using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;
using Newtonsoft.Json;

namespace Service_Delegators;

//TODO: split class in multiple service level validations
public interface IValidations
{
    #region api
    string ValidateApiRequest(Request request);
    #endregion

    #region database
    void ValidateDatabaseExportImportOperations(string requesterId);
    void ValidateDatabasePlayerImport(string requesterId, string playerJson);
    #endregion

    #region items
    void ValidateCreateItemWithTypeAndSubtype(string type, string subtype);
    #endregion

    #region player
    void ValidatePlayerCreate(string playerName);
    void ValidatePlayerLogin(PlayerLogin login);
    void ValidatePlayerUpdateName(string newPlayerName, string playerId);
    void ValidatePlayerDelete(string playerId);
    #endregion

    #region character
    void ValidateCharacterUpdateName(string name, CharacterIdentity identity);
    void ValidateCharacterMaxNrAllowed(string playerId);
    void ValidateCharacterCreateTraits(CharacterRacialTraits traits, string playerId);
    void ValidateCharacterBeforeDelete(CharacterIdentity identity);
    void ValidateCharacterLearnSpecialSkill(CharacterAddSpecialSkill trait);
    void ValidateCharacterBeforeKill(CharacterIdentity identity);
    void ValidateCharacterAddWealth(int wealth, CharacterIdentity identity);
    void ValidateCharacterAddFame(string fame, CharacterIdentity identity);
    void ValidateCharacterEquipUnequipItem(CharacterEquip equip, bool toEquip);
    void ValidateAttributesBeforeIncrease(string attribute, string attributeType, CharacterIdentity identity);
    void ValidateCharacterBeforeTravel(CharacterTravel travel);
    void ValidateMercenaryBeforeHire(CharacterHireMercenary hireMercenary);
    void ValidateCharacterItemBeforeSell(CharacterTrade tradeItem);
    void ValidateCharacterItemBeforeBuy(CharacterTrade tradeItem);
    void ValidateCharacterBeforeBuyProvisions(CharacterTrade tradeItem);
    void ValidateCharacterBeforeGiveProvisions(CharacterTrade tradeItem);
    void ValidateCharacterBeforeGiveWealth(CharacterTrade tradeItem);
    void ValidateCharacterBeforeGiveItem(CharacterTrade tradeItem);
    #endregion

    #region gameplay
    void ValidateLocation(string locationName);
    void ValidateLocationEffortLevel(int locationEffortLevel);
    #endregion

    #region battleboard
    void ValidateBeforeBattleboardGet(BattleboardActor actor);
    void ValidateBeforeBattleboardFind(string battleboardId);
    void ValidateBeforeBattleboardCreate(BattleboardActor actor);
    void ValidateBeforeBattleboardJoin(BattleboardActor actor);
    void ValidateBeforeBattleboardKick(BattleboardActor actor);
    void ValidateBeforeBattleboardLeave(BattleboardActor actor);
    void ValidateBattleboardOnCombatStart(BattleboardActor actor);
    void ValidateBattleboardOnAttack(BattleboardActor actor);
    void ValidateBattleboardOnCast(BattleboardActor actor);
    void ValidateBattleboardOnMend(BattleboardActor actor);
    void ValidateBattleboardOnHide(BattleboardActor actor);
    void ValidateBattleboardOnTraps(BattleboardActor actor);
    void ValidateBattleboardOnRest(BattleboardActor actor);
    void ValidateBattleboardOnLetAiAct(BattleboardActor actor);
    void ValidateBattleboardOnEndRound(BattleboardActor actor);
    void ValidateBattleboardOnEndCombat(BattleboardActor actor);
    void ValidateBattleboardOnMakeCamp(BattleboardActor actor);
    #endregion
}

public class Validations : IValidations
{
    private readonly object _lockApi = new();
    private readonly object _lockDatabase = new();
    private readonly object _lockPlayers = new();
    private readonly object _lockCharacters = new();
    private readonly object _lockBattleboards = new();

    private readonly AppSettings appSettings;
    private readonly Snapshot snapshot;

    public Validations(
        Snapshot snapshot,
        AppSettings appSettings)
    {
        this.snapshot = snapshot;
        this.appSettings = appSettings;
    }

    #region api validations
    public string ValidateApiRequest(Request request)
    {
        lock (_lockApi)
        {
            var player = GetPlayerByName_p(request.PlayerName);

            if (appSettings.AdminData.Banned.Contains(player.Identity.Name.ToLower())) throw new Exception("Player is banned.");
            if (request.Token != player.Identity.Token) throw new Exception("Token mismatch.");

            return player.Identity.Id;
        }
    }
    #endregion

    #region database validations
    public void ValidateDatabaseExportImportOperations(string requesterId)
    {
        lock (_lockDatabase)
        {
            ValidatePlayerIsAdmin_p(requesterId);
        }
    }

    public void ValidateDatabasePlayerImport(string requesterId, string playerJson)
    {
        ValidateDatabaseExportImportOperations(requesterId);

        try
        {
            JsonConvert.DeserializeObject<Player>(playerJson);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to parse player json string. JsonConvert threw error: {ex}");
        }
    }
    #endregion

    #region player validations
    public void ValidatePlayerCreate(string playerName)
    {
        lock (_lockPlayers)
        {
            ValidateString_p(playerName);
            if (playerName.Length > 20) throw new Exception($"Player name: {playerName} is too long, 20 characters max.");
            if (snapshot.Players.Count >= 20) throw new Exception("Server has reached the limit number of players, please contact admins.");

            // we don't care for player misspelling their names
            // names will be unique during creation
            if (snapshot.Players.Exists(p => p.Identity.Name.ToLower() == playerName.ToLower())) throw new Exception("Name unavailable.");
        }
    }

    public void ValidatePlayerLogin(PlayerLogin login)
    {
        lock (_lockPlayers)
        {
            ValidateObject_p(login);
            ValidateString_p(login.PlayerName);
            ValidateString_p(login.Code);

            // we don't care for player misspelling their names
            // names will be unique during creation
            if (!snapshot.Players.Exists(p => p.Identity.Name.ToLower() == login.PlayerName.ToLower())) throw new Exception("Player does not exist.");
        }
    }

    public void ValidatePlayerUpdateName(string newPlayerName, string playerId)
    {
        lock (_lockPlayers)
        {
            ValidateString_p(newPlayerName);
            ValidatePlayerExists_p(playerId);
        }
    }

    public void ValidatePlayerDelete(string playerId)
    {
        lock (_lockPlayers)
        {
            ValidatePlayerExists_p(playerId);
        }
    }
    #endregion

    #region item validations
    public void ValidateCreateItemWithTypeAndSubtype(string type, string subtype)
    {
        ValidateString_p(type); 
        ValidateString_p(subtype);

        if (!ItemsLore.Types.All.Contains(type)) throw new Exception("Wrong item type.");
        if (!ItemsLore.Subtypes.All.Contains(subtype)) throw new Exception("Wrong item subtype.");
    }
    #endregion

    #region character validations
    public void ValidateCharacterUpdateName(string name, CharacterIdentity identity)
    {
        lock (_lockCharacters)
        {
            ValidateString_p(name);
            if (name.Length >= 20) throw new Exception("Character name too long.");

            ValidateCharacterIsLocked_p(ServicesUtils.GetPlayerCharacter(identity, snapshot));
        }
    }

    public void ValidateCharacterMaxNrAllowed(string playerId)
    {
        lock (_lockCharacters)
        {
            var playerCharsCount = snapshot.Players.Find(p => p.Identity.Id == playerId)!.Characters.Where(s => s.Status.Gameplay.IsAlive).ToList().Count;
            
            if (playerCharsCount >= 5) throw new Exception("Max number of alive characters reached (5 alive characters allowed per player).");
        }
    }

    public void ValidateCharacterCreateTraits(CharacterRacialTraits traits, string playerId)
    {
        lock (_lockCharacters)
        {
            ValidateObject_p(traits);
            
            if (!snapshot.Stubs!.Exists(s => s.PlayerId == playerId)) throw new Exception("No stub templates found for this player.");

            ValidateRace_p(traits.Race);
            ValidateCulture_p(traits.Culture);
            ValidateTradition_p(traits.Tradition);
            ValidateClass_p(traits.Class);

            ValidateRaceCultureCombination_p(traits);
        }
    }

    public void ValidateCharacterBeforeDelete(CharacterIdentity identity)
    {
        lock (_lockCharacters) 
        { 
            ValidateObject_p(identity);
            ValidateCharacterIsLocked_p(ServicesUtils.GetPlayerCharacter(identity, snapshot));
        }
    }
    public void ValidateCharacterLearnSpecialSkill(CharacterAddSpecialSkill spsk)
    {
        lock (_lockCharacters)
        {
            var character = ServicesUtils.GetPlayerCharacter(spsk.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);
            ValidateGuid_p(spsk.SpecialSkillId);

            if (spsk.AppliesToSkill.Length > 0)
            {
                ValidateString_p(spsk.AppliesToSkill);
                if (!CharactersLore.Skills.All.Contains(spsk.AppliesToSkill)) throw new Exception("No such Skill was found with the indicated skill name.");
            }

            var specialSkill = SpecialSkillsLore.All.Find(t => t.Identity.Id == spsk.SpecialSkillId) ?? throw new Exception("No such Heroic Trait found with the provided id.");

            if (specialSkill.DeedsCost > character.LevelUp.DeedPoints) throw new Exception("Character does not have enough Deeds points to aquire said Heroic Trait.");

            if (specialSkill.Subtype == SpecialSkillsLore.Subtype.Onetime
                && character.Sheet.SpecialSkills.Exists(t => t.Identity.Id == specialSkill.Identity.Id)) throw new Exception("Character already has that Heroic Trait and it can only be learned once.");
        }
    }
    public void ValidateCharacterBeforeKill(CharacterIdentity identity)
    {
        lock (_lockCharacters)
        {
            var character = ServicesUtils.GetPlayerCharacter(identity, snapshot);
            if (!character.Status.Gameplay.IsAlive) throw new Exception("Character is already dead.");
        }
    }

    public void ValidateCharacterAddWealth(int wealth, CharacterIdentity identity)
    {
        lock (_lockCharacters)
        {
            ValidateNumberGreaterThanZero_p(wealth);
            var character = ServicesUtils.GetPlayerCharacter(identity, snapshot);
            ValidateCharacterIsLocked_p(character);
        }
    }

    public void ValidateCharacterAddFame(string fame, CharacterIdentity identity)
    {
        lock (_lockCharacters)
        {
            ValidateString_p(fame);
            ServicesUtils.GetPlayerCharacter(identity, snapshot);
        }
    }

    public void ValidateCharacterEquipUnequipItem(CharacterEquip equip, bool toEquip)
    {
        lock (_lockCharacters)
        {
            ValidateObject_p(equip);
            ValidateGuid_p(equip.CharacterIdentity.Id);
            ValidateGuid_p(equip.ItemId);
            ValidateString_p(equip.InventoryLocation);
            
            var character = ServicesUtils.GetPlayerCharacter(equip.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);

            if (!ItemsLore.InventoryLocation.All.Contains(equip.InventoryLocation)) throw new Exception("Equipment location does not fit any possible slot in inventory.");

            if (!toEquip) return;

            var itemSubtype = (character!.Inventory.Supplies!.Find(i => i.Identity.Id == equip.ItemId)?.Subtype) ?? throw new Exception("No such item found on this character.");
            bool isItemAtCorrectLocation;

            // protection
            if (itemSubtype == ItemsLore.Subtypes.Protections.Helm)
            {
                isItemAtCorrectLocation =
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Head;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Protections.Armour)
            {
                isItemAtCorrectLocation =
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Body;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Protections.Shield)
            {
                isItemAtCorrectLocation =
                     equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand;
            }
            //weapons
            else if (itemSubtype == ItemsLore.Subtypes.Weapons.Sword)
            {
                isItemAtCorrectLocation =
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Weapons.Pike)
            {
                isItemAtCorrectLocation =
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Weapons.Crossbow)
            {
                isItemAtCorrectLocation =
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Weapons.Polearm)
            {
                isItemAtCorrectLocation =
                   equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Weapons.Mace)
            {
                isItemAtCorrectLocation =
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Weapons.Axe)
            {
                isItemAtCorrectLocation =
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand ||
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Weapons.Dagger)
            {
                isItemAtCorrectLocation =
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand ||
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Weapons.Bow)
            {
                isItemAtCorrectLocation =
                    equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Weapons.Sling)
            {
                isItemAtCorrectLocation =
                   equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                   equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
            }
            else if (itemSubtype == ItemsLore.Subtypes.Weapons.Spear)
            {
                isItemAtCorrectLocation =
                   equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                   equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
            }
            // wealth
            else if (itemSubtype == ItemsLore.Subtypes.Wealth.Gems ||
                itemSubtype == ItemsLore.Subtypes.Wealth.Valuables ||
                itemSubtype == ItemsLore.Subtypes.Wealth.Trinket)
            {
                isItemAtCorrectLocation =
                   equip.InventoryLocation == ItemsLore.InventoryLocation.Heraldry;

                if (character.Inventory!.Heraldry!.Count >= 5)
                {
                    throw new Exception("Heraldry is full, unequip some of the items first.");
                }
            }
            else
            {
                isItemAtCorrectLocation = false;
            }

            if (!isItemAtCorrectLocation) throw new Exception("Item is being equipped at incorrect location.");
        }
    }

    public void ValidateAttributesBeforeIncrease(string attribute, string attributeType, CharacterIdentity identity)
    {
        lock (_lockCharacters)
        {
            ValidateString_p(attribute);
            ValidateString_p(attributeType);
            
            var character = ServicesUtils.GetPlayerCharacter(identity, snapshot);
            ValidateCharacterIsLocked_p(character);

            if (!CharactersLore.AttributeTypes.All.Contains(attributeType)) throw new Exception("Wrong attribute type");

            if (attributeType == CharactersLore.AttributeTypes.Stats)
            {
                if (!CharactersLore.Stats.All.Contains(attribute)) throw new Exception("This stat does not exist.");
                if (character.LevelUp.StatPoints <= 0) throw new Exception("You have no stat points to use.");
            } 
            else if (attributeType == CharactersLore.AttributeTypes.Assets)
            {
                if (!CharactersLore.Assets.All.Contains(attribute)) throw new Exception("This asset does not exist.");
                if (character.LevelUp.AssetPoints <= 0) throw new Exception("You have no asset points to use.");
            } 
            else if (attributeType == CharactersLore.AttributeTypes.Skills)
            {
                if (!CharactersLore.Skills.All.Contains(attribute)) throw new Exception("This skill does not exist.");
                if (character.LevelUp.SkillPoints <= 0) throw new Exception("You have no skill points to use.");
            }
        }
    }

    public void ValidateCharacterBeforeTravel(CharacterTravel travel)
    {
        lock (_lockCharacters)
        {
            var character = ServicesUtils.GetPlayerCharacter(travel.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);
            ValidateCharacterIsAlive_p(character);

            if (character.Inventory.Provisions <= 0) throw new Exception("You don't have any provisions to travel.");
            if (character.Mercenaries.Select(s => s.Inventory.Provisions).Any(s => s <= 0)) throw new Exception("One or more of your mercenaries does not have enough provisions to travel.");

            var totalProvisions = character.Inventory.Provisions
                + character.Mercenaries.Select(s => s.Inventory.Provisions).Sum();

            if (totalProvisions == 0) throw new Exception("Not enough provisions to travel.");

            var destinationFullName = ServicesUtils.GetLocationFullNameFromPosition(travel.Destination);
            if (!GameplayLore.Locations.All.Select(s => s.FullName).ToList().Contains(destinationFullName)) throw new Exception("No such destination is known.");
        }
    }

    public void ValidateMercenaryBeforeHire(CharacterHireMercenary hireMercenary)
    {
        lock (_lockCharacters)
        {
            var character = ServicesUtils.GetPlayerCharacter(hireMercenary.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);

            var location = snapshot.Locations.Find(s => s.FullName == ServicesUtils.GetLocationFullNameFromPosition(character.Status.Position)) ?? throw new Exception("Location has not been visited yet.");
            var merc = location.Mercenaries.Find(s => s.Identity.Id == hireMercenary.MercenaryId) ?? throw new Exception("This mercenary does not exist at this location.");

            if (merc.Status.Worth > character.Status.Wealth) throw new Exception($"Mercenary's worth is {merc.Status.Worth}, but your character's wealth is {character.Status.Wealth}.");
        }
    }

    public void ValidateCharacterItemBeforeSell(CharacterTrade tradeItem)
    {
        lock (_lockCharacters)
        {
            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);

            var item = character.Inventory.Supplies.Find(s => s.Identity.Id == tradeItem.ItemId) ?? throw new Exception("Item not found on character supplies.");

            if (!snapshot.Locations.Exists(s => s.Position.Location == character.Status.Position.Location)) throw new Exception("Location has not been visited yet.");

            tradeItem.IsToBuy = false;
        }
    }

    public void ValidateCharacterItemBeforeBuy(CharacterTrade tradeItem)
    {
        lock (_lockCharacters)
        {
            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);

            var location = snapshot.Locations.Find(s => s.Position.Location == character.Status.Position.Location) ?? throw new Exception("Location has not been visited yet");
            var item = location.Market.Find(s => s.Identity.Id == tradeItem.ItemId) ?? throw new Exception("Item not found on this market or has already been sold.");

            if (character.Status.Wealth < item.Value) throw new Exception($"Unable to purchase item, item costs {item.Value}, your wealth is {character.Status.Wealth}");

            tradeItem.IsToBuy = true;
        }
    }

    public void ValidateCharacterBeforeBuyProvisions(CharacterTrade tradeItem)
    {
        lock (_lockCharacters)
        {
            ValidateObject_p(tradeItem);
            ValidateObject_p(tradeItem.CharacterIdentity);

            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);

            if (character.Status.Wealth < tradeItem.Amount * 2) throw new Exception($"Unable to buy supplies, item costs 2 wealth x {tradeItem.Amount} supplies requested, your wealth is {character.Status.Wealth}");
        }
    }

    public void ValidateCharacterBeforeGiveProvisions(CharacterTrade tradeItem)
    {
        lock (_lockCharacters)
        {
            ValidateObject_p(tradeItem);
            ValidateObject_p(tradeItem.CharacterIdentity);
            ValidateObject_p(tradeItem.TargetIdentity);

            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            var target = ServicesUtils.GetPlayerCharacter(tradeItem.TargetIdentity, snapshot);

            ValidateCharacterIsAlive_p(character);
            ValidateCharacterIsAlive_p(target);

            ValidateCharacterIsLocked_p(character);
            ValidateCharacterIsLocked_p(target);

            if (character.Inventory.Provisions <= 0) throw new Exception("You have no provisions left to send.");
            if (tradeItem.Amount > character.Inventory.Provisions) throw new Exception("You don't have that amount of provisions to give.");
        }
    }

    public void ValidateCharacterBeforeGiveWealth(CharacterTrade tradeItem)
    {
        lock (_lockCharacters)
        {
            ValidateObject_p(tradeItem);
            ValidateObject_p(tradeItem.CharacterIdentity);
            ValidateObject_p(tradeItem.TargetIdentity);

            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            var target = ServicesUtils.GetPlayerCharacter(tradeItem.TargetIdentity, snapshot);

            ValidateCharacterIsAlive_p(character);
            ValidateCharacterIsAlive_p(target);

            ValidateCharacterIsLocked_p(character);
            ValidateCharacterIsLocked_p(target);

            if (character.Status.Wealth <= 0) throw new Exception("You have no wealth to give.");
            if (tradeItem.Amount > character.Status.Wealth) throw new Exception("You don't have that amount of wealth to give.");
        }
    }

    public void ValidateCharacterBeforeGiveItem(CharacterTrade tradeItem)
    {
        lock (_lockCharacters)
        {
            ValidateObject_p(tradeItem);
            ValidateObject_p(tradeItem.CharacterIdentity);
            ValidateObject_p(tradeItem.TargetIdentity);
            ValidateString_p(tradeItem.ItemId);

            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            var target = ServicesUtils.GetPlayerCharacter(tradeItem.TargetIdentity, snapshot);

            ValidateCharacterIsAlive_p(character);
            ValidateCharacterIsAlive_p(target);

            ValidateCharacterIsLocked_p(character);
            ValidateCharacterIsLocked_p(target);

            if (!character.Inventory.Supplies.Exists(s => s.Identity.Id == tradeItem.ItemId)) throw new Exception("You have no such item among your supplies.");
        }
    }
    #endregion

    #region gameplay validations
    public void ValidateLocation(string locationName)
    {
        ValidateString_p(locationName);
        _ = ServicesUtils.GetLocationByLocationName(locationName) ?? throw new Exception("Wrong location name.");
    }

    public void ValidateLocationEffortLevel(int locationEffortLevel)
    {
        ValidateNumberGreaterThanZero_p(locationEffortLevel);
    }

    #endregion

    #region battleboard validations
    public void ValidateBeforeBattleboardFind(string battleboardId)
    {
        lock (_lockBattleboards)
        {
            ValidateGuid_p(battleboardId);
            GetBattleboard_p(battleboardId);
        }
    }

    public void ValidateBeforeBattleboardGet(BattleboardActor actor)
    {
        var boardNotFound = "Battleboard not found, character unlocked.";

        lock (_lockBattleboards)
        {
            ValidateObject_p(actor);
            ValidateObject_p(actor.MainActor);

            var character = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            var battleboard = snapshot.Battleboards.Find(s => s.Id == character.Status.Gameplay.BattleboardId);

            if (battleboard == null)
            {
                character.Status.Gameplay.BattleboardId = string.Empty;
                character.Status.Gameplay.IsGoodGuy = false;
                character.Status.Gameplay.IsLocked = false;

                character.Mercenaries.ForEach(s =>
                {
                    s.Status.Gameplay.BattleboardId = string.Empty;
                    s.Status.Gameplay.IsGoodGuy = false;
                    s.Status.Gameplay.IsLocked = false;
                });

                throw new Exception(boardNotFound);
            }
        }
    }

    public void ValidateBeforeBattleboardCreate(BattleboardActor actor)
    {
        var charAlreadyOnBoard = "Character is already on a battleboard.";

        lock (_lockBattleboards)
        {
            ValidateObject_p(actor);
            ValidateObject_p(actor.MainActor);

            var character = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            ValidateCharacterIsAlive_p(character);
            ValidateCharacterIsLocked_p(character);
            if (character.Status.Gameplay.BattleboardId != string.Empty) throw new Exception(charAlreadyOnBoard);
        }
    }

    public void ValidateBeforeBattleboardJoin(BattleboardActor actor)
    {
        var charAlreadyOnBoard = "Character is already on a battleboard.";
        var boardInCombat = "Unable to join battleboard during combat";
        var cannotAddMoreThan1Char = "You cannot add more than 1 character that you own to the party.";

        lock (_lockBattleboards)
        {
            ValidateObject_p(actor);
            ValidateObject_p(actor.MainActor);

            var character = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            ValidateCharacterIsAlive_p(character);
            ValidateCharacterIsLocked_p(character);
            if (character.Status.Gameplay.BattleboardId != string.Empty) throw new Exception(charAlreadyOnBoard);

            var battleboard = GetBattleboard_p(actor.BattleboardIdToJoin);

            if (battleboard.IsInCombat) throw new Exception(boardInCombat);
            if (battleboard.GoodGuys.Where(s => !s.Status.Gameplay.IsNpc && s.Identity.PlayerId == character.Identity.PlayerId).Any()) throw new Exception(cannotAddMoreThan1Char);
            if (battleboard.BadGuys.Where(s => !s.Status.Gameplay.IsNpc && s.Identity.PlayerId == character.Identity.PlayerId).Any()) throw new Exception(cannotAddMoreThan1Char);
        }
    }

    public void ValidateBeforeBattleboardKick(BattleboardActor actor)
    {
        var onlyPartyLeadAction = "Only party lead can kick battleboard members.";
        var charNotInYourParty = "Targetted character to be kicked not found in your party.";

        lock (_lockBattleboards)
        {
            ValidateObject_p(actor);
            ValidateObject_p(actor.MainActor);

            var character = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            ValidateCharacterIsLocked_p(character);

            var battleboard = GetBattleboard_p(character.Status.Gameplay.BattleboardId);
            ValidateCharacterIsOnBattleboard(battleboard, character);

            if (character.Status.Gameplay.IsGoodGuy)
            {
                if (battleboard.GoodGuyPartyLead != character.Identity.Id) throw new Exception(onlyPartyLeadAction);
                if (!battleboard.GoodGuys.Select(s => s.Identity.Id).Contains(actor.TargetId)) throw new Exception(charNotInYourParty);
            }
            else
            {
                if (battleboard.BadGuyPartyLead != character.Identity.Id) throw new Exception(onlyPartyLeadAction);
                if (!battleboard.BadGuys.Select(s => s.Identity.Id).Contains(actor.TargetId)) throw new Exception(charNotInYourParty);
            }
        }
    }

    public void ValidateBeforeBattleboardLeave(BattleboardActor actor)
    {
        var boardInCombat = "Unable to leave battleboard during combat.";

        lock (_lockBattleboards)
        {
            ValidateObject_p(actor);
            ValidateObject_p(actor.MainActor);

            var character = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            ValidateCharacterIsAlive_p(character);
            ValidateCharacterIsLocked_p(character);

            var battleboard = GetBattleboard_p(character.Status.Gameplay.BattleboardId);
            ValidateCharacterIsOnBattleboard(battleboard, character);

            if (battleboard.IsInCombat) throw new Exception(boardInCombat);
        }
    }

    public void ValidateBattleboardOnCombatStart(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            ValidateObject_p(actor);
            ValidateObject_p(actor.MainActor);

            var character = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            var board = GetBattleboard_p(character.Status.Gameplay.BattleboardId);

            if (board.IsInCombat) throw new Exception("Battleboard already in combat.");
            
            if (board.GoodGuyPartyLead != actor.MainActor.Id
                && board.BadGuyPartyLead != actor.MainActor.Id) throw new Exception("Only party leads can start combat.");

        }
    }

    public void ValidateBattleboardOnAttack(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            var (attacker, board, defender) = ValidateAttackerBoardDefender(actor);
            CheckIsAttackersTurn(attacker, board);

            ValidateCharacterIsAlive_p(attacker);
            ValidateTargetIsAlive_p(defender);
            ValidateTargetIsNotHidden_p(defender);

            var isTargetFriendly =
                board.GoodGuys.Select(s => s.Identity.Id).ToList().Contains(attacker.Identity.Id) && board.GoodGuys.Select(s => s.Identity.Id).ToList().Contains(defender.Identity.Id)
                || board.BadGuys.Select(s => s.Identity.Id).ToList().Contains(attacker.Identity.Id) && board.BadGuys.Select(s => s.Identity.Id).ToList().Contains(defender.Identity.Id);

            if (isTargetFriendly) throw new Exception("Cannot attack a friendly target.");
        }
    }

    public void ValidateBattleboardOnCast(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            var (attacker, board, defender) = ValidateAttackerBoardDefender(actor);
            CheckIsAttackersTurn(attacker, board);

            ValidateCharacterIsAlive_p(attacker);
            ValidateTargetIsAlive_p(defender);
            ValidateTargetIsNotHidden_p(defender);

            if (attacker.Sheet.Assets.ManaLeft <= 0) throw new Exception("No mana left to accrete for a spellcast.");
        }
    }

    public void ValidateBattleboardOnMend(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            var (attacker, board, defender) = ValidateAttackerBoardDefender(actor);
            CheckIsAttackersTurn(attacker, board);

            ValidateCharacterIsAlive_p(attacker);

            if (!defender.Status.Gameplay.IsAlive) throw new Exception("Your target is dead, there's nothing more you can do about it.");
            ValidateTargetIsNotHidden_p(defender);

            var isTargetFriendly =
                board.GoodGuys.Select(s => s.Identity.Id).ToList().Contains(attacker.Identity.Id) && board.GoodGuys.Select(s => s.Identity.Id).ToList().Contains(defender.Identity.Id)
                || board.BadGuys.Select(s => s.Identity.Id).ToList().Contains(attacker.Identity.Id) && board.BadGuys.Select(s => s.Identity.Id).ToList().Contains(defender.Identity.Id);

            if (!isTargetFriendly && board.IsInCombat) throw new Exception("Cannot mend an enemy during combat.");
        }
    }

    public void ValidateBattleboardOnHide(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            var (attacker, board) = ValidateAttackerBoard(actor);
            CheckIsAttackersTurn(attacker, board);

            ValidateCharacterIsAlive_p(attacker);
            if (attacker.Status.Gameplay.IsHidden) throw new Exception("You're already hidden.");
        }
    }

    public void ValidateBattleboardOnTraps(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            var (attacker, board) = ValidateAttackerBoard(actor);
            CheckIsAttackersTurn(attacker, board);

            ValidateCharacterIsAlive_p(attacker);
        }
    }

    public void ValidateBattleboardOnRest(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            var (attacker, board) = ValidateAttackerBoard(actor);
            CheckIsAttackersTurn(attacker, board);

            ValidateCharacterIsAlive_p(attacker);
        }
    }

    public void ValidateBattleboardOnLetAiAct(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            var (attacker, board) = ValidateAttackerBoard(actor);

            if (board.GoodGuyPartyLead != attacker.Identity.Id && board.BadGuyPartyLead != attacker.Identity.Id) throw new Exception("Only party leads can let Ai act.");
            
            var npc = board.GetAllCharacters().Find(s => s.Identity.Id == board.BattleOrder.First())!;

            if (npc.Identity.PlayerId != Guid.Empty.ToString()) throw new Exception($"This NPC is not owned by Ai.");

            ValidateCharacterIsAlive_p(attacker);
        }
    }

    public void ValidateBattleboardOnEndRound(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            ValidateObject_p(actor);
            ValidateObject_p(actor.MainActor);

            var attacker = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            var board = GetBattleboard_p(attacker.Status.Gameplay.BattleboardId);

            if (board.BattleOrder.Count > 0) throw new Exception("There are still characters with actions.");
        }
    }

    public void ValidateBattleboardOnEndCombat(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            var (attacker, board) = ValidateAttackerBoard(actor);

            if (board.GoodGuyPartyLead != attacker.Identity.Id && board.BadGuyPartyLead != attacker.Identity.Id) throw new Exception("Only party leads can end combat.");

            var areEnemiesAbout = (attacker.Status.Gameplay.IsGoodGuy && board.BadGuys.Where(s => s.Status.Gameplay.IsAlive).Any())
                                || (!attacker.Status.Gameplay.IsGoodGuy && board.GoodGuys.Where(s => s.Status.Gameplay.IsAlive).Any());

            if (areEnemiesAbout) throw new Exception("Unable to leave combat, there are enemies about.");
        }
    }

    public void ValidateBattleboardOnMakeCamp(BattleboardActor actor)
    {
        lock (_lockBattleboards)
        {
            var (attacker, board) = BattleboardUtils.GetAttackerBoard(actor, snapshot);

            if (attacker == null || board == null) throw new Exception("Unable to find attacker board combination.");

            if (board.GetAllCharacters().Where(s => s.Status.Gameplay.IsLocked).Any()) throw new Exception("Unable to make camp, some characters are still locked.");
        }
    }
    #endregion

    #region private methods

    // the _p suffix represents these methods are private and do not use the lock
    // this is to prevent trying to use the lock resource twice

    #region generic
    private static void ValidateString_p(string str, string message = "")
    {
        if (string.IsNullOrWhiteSpace(str)) throw new Exception(message.Length > 0 ? message : "The provided string is invalid.");
    }

    private static void ValidateObject_p(object? obj, string message = "")
    {
        if (obj == null) throw new Exception(message.Length > 0 ? message : $"Object found null.");
    }

    private static void ValidateNumberGreaterThanZero_p(int num, string message = "")
    {
        if (num <= 0) throw new Exception(message.Length > 0 ? message : "Number cannot be smaller or equal to zero.");
    }

    private static void ValidateGuid_p(string str, string message = "")
    {
        ValidateString_p(str);

        var isGuidValid = Guid.TryParse(str, out var id);

        if (!isGuidValid) throw new Exception(message.Length > 0 ? message : "Invalid guid.");

        if (id == Guid.Empty) throw new Exception("Guid cannot be an empty guid.");
    }
    #endregion

    #region player
    private void ValidatePlayerExists_p(string playerId)
    {
        ValidateString_p(playerId);
        GetPlayer_p(playerId);
    }

    private void ValidatePlayerIsAdmin_p(string playerId)
    {
        ValidatePlayerExists_p(playerId);

        var playerName = GetPlayer_p(playerId)!.Identity.Name;

        if (!appSettings.AdminData.Admins.Contains(playerName)) throw new Exception("Player is not an admin.");
    }
    #endregion

    #region character
    private static void ValidateClass_p(string classes)
    {
        ValidateString_p(classes, "Invalid class string.");
        if (!CharactersLore.Classes.All.Contains(classes)) throw new Exception($"Class {classes} not found.");
    }

    private static void ValidateCulture_p(string culture)
    {
        ValidateString_p(culture, "Invalid culture string.");
        if (!CharactersLore.Cultures.All.Contains(culture)) throw new Exception($"Culture {culture} not found.");
    }

    private static void ValidateTradition_p(string tradition)
    {
        ValidateString_p(tradition, "Invalid tradition string.");
        if (!CharactersLore.Tradition.All.Contains(tradition)) throw new Exception($"Tradition {tradition} not found.");
    }

    private static void ValidateRace_p(string race)
    {
        ValidateString_p(race, "Invalid race string.");
        if (!CharactersLore.Races.Playable.All.Contains(race)) throw new Exception($"Race {race} not found.");
    }

    private static void ValidateRaceCultureCombination_p(CharacterRacialTraits origins)
    {
        string invalidCombination = "Invalid race culture combination";

        if (origins.Race == CharactersLore.Races.Playable.Human)
        {
            if (!CharactersLore.Cultures.Human.All.Contains(origins.Culture)) throw new Exception(invalidCombination);
        }
        else if (origins.Race == CharactersLore.Races.Playable.Elf)
        {
            if (!CharactersLore.Cultures.Elf.All.Contains(origins.Culture)) throw new Exception(invalidCombination);
        }
        else if (origins.Race == CharactersLore.Races.Playable.Dwarf)
        {
            if (!CharactersLore.Cultures.Dwarf.All.Contains(origins.Culture)) throw new Exception(invalidCombination);
        }
    }

    private static void ValidateCharacterIsOnBattleboard(Battleboard battleboard, Character character)
    {
        var charNotOnBoard = "Character not on this battleboard.";

        if (!battleboard.GoodGuys.Exists(s => s.Identity.Id == character.Identity.Id)
            && !battleboard.BadGuys.Exists(s => s.Identity.Id == character.Identity.Id)) throw new Exception(charNotOnBoard);
    }

    private static void ValidateCharacterIsLocked_p(Character character)
    {
        var charIsLocked = "Cannot modify character at this time.";

        if (character.Status.Gameplay.IsLocked) throw new Exception(charIsLocked);
    }

    private static void ValidateCharacterIsAlive_p(Character character)
    {
        if (!character.Status.Gameplay.IsAlive) throw new Exception("Your character is dead.");
    }

    private static void ValidateTargetIsAlive_p(Character character)
    {
        if (!character.Status.Gameplay.IsAlive) throw new Exception("Your target is dead.");
    }

    private static void ValidateTargetIsNotHidden_p(Character character)
    {
        if (character.Status.Gameplay.IsHidden) throw new Exception("Battleboard character is hidden.");
    }
    #endregion

    #region battleboard 
    private static void CheckIsAttackersTurn(Character attacker, Battleboard board)
    {
        if (board.BattleOrder.First() != attacker.Identity.Id) throw new Exception("Wait your turn.");
    }

    private static void CheckDefenderIsOnBoard(string defenderId, Battleboard board)
    {
        if (!board.GetAllCharacters().Select(s => s.Identity.Id).Contains(defenderId)) throw new Exception("Character not found on battleboard.");
    }

    private (Character attacker, Battleboard board, Character defender) ValidateAttackerBoardDefender(BattleboardActor actor)
    {
        var (attacker, board) = ValidateAttackerBoard(actor);
        ValidateString_p(actor.TargetId);

        CheckDefenderIsOnBoard(actor.TargetId, board);
        var defender = board.GetAllCharacters().Find(s => s.Identity.Id == actor.TargetId)!;

        return (attacker, board, defender);
    }

    private (Character attacker, Battleboard board) ValidateAttackerBoard(BattleboardActor actor)
    {
        ValidateObject_p(actor);
        ValidateObject_p(actor.MainActor);

        var attacker = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
        var board = GetBattleboard_p(attacker.Status.Gameplay.BattleboardId);

        if (!board.BattleOrder.Contains(attacker.Identity.Id)) throw new Exception("Character is exhausted and has no action points left for round.");

        return (attacker, board);
    }

    private (Character attacker, Battleboard board) ValidateAttackerBoardOffCombat(BattleboardActor actor)
    {
        ValidateObject_p(actor);
        ValidateObject_p(actor.MainActor);
        ValidateString_p(actor.TargetId);

        var attacker = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
        var board = GetBattleboard_p(attacker.Status.Gameplay.BattleboardId);

        return (attacker, board);
    }

    #endregion

    #region getter methods
    private Player GetPlayer_p(string playerId)
    {
        return snapshot.Players.Find(s => s.Identity.Id == playerId) ?? throw new Exception("PLayer not found.");
    }

    private Player GetPlayerByName_p(string name)
    {
        return snapshot.Players.Find(s => s.Identity.Name == name) ?? throw new Exception("PLayer not found.");
    }

    private Battleboard GetBattleboard_p(string battleboardId)
    {
        var boardNotFound = "Battleboard not found.";

        return snapshot.Battleboards.Find(s => s.Id == battleboardId) ?? throw new Exception(boardNotFound);
    }
    #endregion
    #endregion
}