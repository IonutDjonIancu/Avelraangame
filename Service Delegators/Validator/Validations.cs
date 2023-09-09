using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;

namespace Service_Delegators;

public interface IValidations
{
    #region api
    string ValidateApiRequest(Request request);
    #endregion

    #region database
    void ValidateDatabaseExportImportOperations(string requesterId);
    void ValidateDatabasePlayerImport(string requesterId, string playerJson);
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
    void ValidateCharacterCreateTraits(CharacterTraits traits, string playerId);
    void ValidateCharacterBeforeDelete(CharacterIdentity identity);
    void ValidateCharacterLearnSpecialSkill(CharacterAddSpecialSkill trait);
    void ValidateCharacterBeforeKill(CharacterIdentity identity);
    void ValidateCharacterAddWealth(int wealth, CharacterIdentity identity);
    void ValidateCharacterAddFame(string fame, CharacterIdentity identity);
    void ValidateCharacterEquipUnequipItem(CharacterEquip equip, bool toEquip);
    void ValidateAttributesBeforeIncrease(string attribute, string attributeType, CharacterIdentity identity);
    void ValidateCharacterBeforeTravel(CharacterTravel travel);
    void ValidateMercenaryBeforeHire(CharacterHireMercenary hireMercenary);
    void ValidateCharacterItemBeforeSell(CharacterItemTrade tradeItem);
    void ValidateCharacterItemBeforeBuy(CharacterItemTrade tradeItem);
    #endregion

    #region gameplay
    void ValidateLocation(string locationName);
    #endregion

    #region items
    void CreateItemWithTypeAndSubtype(string type, string subtype);
    #endregion
}

public class Validations : IValidations
{
    private readonly AppSettings appSettings;
    private readonly Snapshot snapshot;
    private readonly object _lock = new();

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
        lock (_lock)
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
        lock (_lock)
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
        lock (_lock)
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
        lock (_lock)
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
        lock (_lock)
        {
            ValidateString_p(newPlayerName);
            ValidatePlayerExists_p(playerId);
        }
    }

    public void ValidatePlayerDelete(string playerId)
    {
        lock (_lock)
        {
            ValidatePlayerExists_p(playerId);
        }
    }
    #endregion

    #region item validations
    public void CreateItemWithTypeAndSubtype(string type, string subtype)
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
        lock (_lock)
        {
            ValidateString_p(name);
            if (name.Length >= 20) throw new Exception("Character name too long.");

            ValidateCharacterIsLocked_p(Utils.GetPlayerCharacter(identity, snapshot));
        }
    }

    public void ValidateCharacterMaxNrAllowed(string playerId)
    {
        lock (_lock)
        {
            var playerCharsCount = snapshot.Players.Find(p => p.Identity.Id == playerId)!.Characters.Where(s => s.Status.IsAlive).ToList().Count;
            
            if (playerCharsCount >= 5) throw new Exception("Max number of alive characters reached (5 alive characters allowed per player).");
        }
    }

    public void ValidateCharacterCreateTraits(CharacterTraits traits, string playerId)
    {
        lock (_lock)
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
        lock (_lock) 
        { 
            ValidateObject_p(identity);
            ValidateCharacterIsLocked_p(Utils.GetPlayerCharacter(identity, snapshot));
        }
    }

    public void ValidateCharacterLearnSpecialSkill(CharacterAddSpecialSkill spsk)
    {
        lock ( _lock)
        {
            var character = Utils.GetPlayerCharacter(spsk.CharacterIdentity, snapshot);
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
        lock ( _lock)
        {
            var character = Utils.GetPlayerCharacter(identity, snapshot);
            if (!character.Status.IsAlive) throw new Exception("Character is already dead.");
        }
    }

    public void ValidateCharacterAddWealth(int wealth, CharacterIdentity identity)
    {
        lock ( _lock)
        {
            ValidateNumberGreaterThanZero_p(wealth);
            var character = Utils.GetPlayerCharacter(identity, snapshot);
            ValidateCharacterIsLocked_p(character);
        }
    }

    public void ValidateCharacterAddFame(string fame, CharacterIdentity identity)
    {
        lock ( _lock)
        {
            ValidateString_p(fame);
            _ = Utils.GetPlayerCharacter(identity, snapshot);
        }
    }

    public void ValidateCharacterEquipUnequipItem(CharacterEquip equip, bool toEquip)
    {
        lock (_lock)
        {
            ValidateObject_p(equip);
            ValidateGuid_p(equip.CharacterIdentity.Id);
            ValidateGuid_p(equip.ItemId);
            ValidateString_p(equip.InventoryLocation);
            
            var character = Utils.GetPlayerCharacter(equip.CharacterIdentity, snapshot);
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
        lock (_lock)
        {
            ValidateString_p(attribute);
            ValidateString_p(attributeType);
            
            var character = Utils.GetPlayerCharacter(identity, snapshot);
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
        lock (_lock)
        {
            var character = Utils.GetPlayerCharacter(travel.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);
            ValidateCharacterIsAlive_p(character);

            if (character.Inventory.Provisions <= 0) throw new Exception("You don't have any provisions to travel.");
            if (character.Mercenaries.Select(s => s.Inventory.Provisions).Any(s => s <= 0)) throw new Exception("One or more of your mercenaries does not have enough provisions to travel.");

            var totalProvisions = character.Inventory.Provisions
                + character.Mercenaries.Select(s => s.Inventory.Provisions).Sum();

            if (totalProvisions == 0) throw new Exception("Not enough provisions to travel.");

            var destinationFullName = Utils.GetLocationFullNameFromPosition(travel.Destination);
            if (!GameplayLore.Locations.All.Select(s => s.FullName).ToList().Contains(destinationFullName)) throw new Exception("No such destination is known.");
        }
    }

    public void ValidateMercenaryBeforeHire(CharacterHireMercenary hireMercenary)
    {
        lock (_lock)
        {
            var character = Utils.GetPlayerCharacter(hireMercenary.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);

            var location = snapshot.Locations.Find(s => s.FullName == Utils.GetLocationFullNameFromPosition(character.Status.Position)) ?? throw new Exception("Location has not been visited yet.");
            var merc = location.Mercenaries.Find(s => s.Identity.Id == hireMercenary.MercenaryId) ?? throw new Exception("This mercenary does not exist at this location.");

            if (merc.Status.Worth > character.Status.Wealth) throw new Exception($"Mercenary's worth is {merc.Status.Worth}, but your character's wealth is {character.Status.Wealth}.");
        }
    }

    public void ValidateCharacterItemBeforeSell(CharacterItemTrade tradeItem)
    {
        lock (_lock)
        {
            var character = Utils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);

            var item = character.Inventory.Supplies.Find(s => s.Identity.Id == tradeItem.ItemId) ?? throw new Exception("Item not found on character supplies.");

            if (!snapshot.Locations.Exists(s => s.Position.Location == character.Status.Position.Location)) throw new Exception("Location has not been visited yet.");

            tradeItem.IsToBuy = false;
        }
    }

    public void ValidateCharacterItemBeforeBuy(CharacterItemTrade tradeItem)
    {
        lock (_lock)
        {
            var character = Utils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            ValidateCharacterIsLocked_p(character);

            var location = snapshot.Locations.Find(s => s.Position.Location == character.Status.Position.Location) ?? throw new Exception("Location has not been visited yet");
            var item = location.Market.Find(s => s.Identity.Id == tradeItem.ItemId) ?? throw new Exception("Item not found on this market or has already been sold.");

            if (character.Status.Wealth < item.Value) throw new Exception($"Unable to purchase item, item costs {item.Value}, your wealth is {character.Status.Wealth}");

            tradeItem.IsToBuy = true;
        }
    }
    #endregion

    #region gameplay validations
    public void ValidateLocation(string locationName)
    {
        ValidateString_p(locationName);
        _ = Utils.GetLocationByLocationName(locationName) ?? throw new Exception("Wrong location name.");
    }

    #endregion

    #region private methods

    // the _p suffix represents these methods are private and do not use the lock
    // this is to prevent trying to use a locked resource

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

    private static void ValidateRaceCultureCombination_p(CharacterTraits origins)
    {
        string message = "Invalid race culture combination";

        if (origins.Race == CharactersLore.Races.Playable.Human)
        {
            if (!CharactersLore.Cultures.Human.All.Contains(origins.Culture)) throw new Exception(message);
        }
        else if (origins.Race == CharactersLore.Races.Playable.Elf)
        {
            if (!CharactersLore.Cultures.Elf.All.Contains(origins.Culture)) throw new Exception(message);
        }
        else if (origins.Race == CharactersLore.Races.Playable.Dwarf)
        {
            if (!CharactersLore.Cultures.Dwarf.All.Contains(origins.Culture)) throw new Exception(message);
        }
    }

    private static void ValidateCharacterIsLocked_p(Character character)
    {
        if (character.Status.IsLockedToModify) throw new Exception("Cannot modify character at this time");
    }

    private static void ValidateCharacterIsAlive_p(Character character)
    {
        if (!character.Status.IsAlive) throw new Exception("Your character is dead.");
    }

    private Player GetPlayer_p(string playerId)
    {
        return snapshot.Players.Find(s => s.Identity.Id == playerId) ?? throw new Exception("PLayer not found.");
    }

    private Player GetPlayerByName_p(string name)
    {
        return snapshot.Players.Find(s => s.Identity.Name == name) ?? throw new Exception("PLayer not found.");
    }
    #endregion
}