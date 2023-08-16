using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterValidator : ValidatorBase
{
    private readonly Snapshot snapshot;

    internal CharacterValidator(Snapshot snapshot)
        : base(snapshot)
    {
        this.snapshot = snapshot;
    }

    internal void ValidatePlayerCharacterNpc(CharacterIdentity identity, string npcId)
    {
        ValidateCharacterPlayerCombination(identity);

        var character = Utils.GetPlayerCharacter(snapshot, identity);

        if (!character.Mercenaries.Exists(s => s.Identity.Id == npcId)) throw new Exception("Npc not found for the indicated character.");
    }

    internal void ValidateMercenaryBeforeHire(CharacterHireMercenary hireMercenary)
    {
        ValidateCharacterPlayerCombination(hireMercenary.CharacterIdentity);
        ValidateIfCharacterIsLocked(hireMercenary.CharacterIdentity);

        var character = Utils.GetPlayerCharacter(snapshot, hireMercenary.CharacterIdentity);

        var location = snapshot.Locations.Find(s => s.FullName == Utils.GetLocationFullNameFromPosition(character.Status.Position)) ?? throw new Exception("Location has not been visited yet.");
        var merc = location.Mercenaries.Find(s => s.Identity.Id == hireMercenary.MercenaryId) ?? throw new Exception("This mercenary does not exist at this location.");

        if (merc.Status.Worth > character.Status.Wealth) throw new Exception($"Mercenary's worth is {merc.Status.Worth}, but your character's wealth is only about {character.Status.Wealth}.");
    }

    internal void ValidateBeforeTravel(CharacterTravel positionTravel)
    {
        ValidateCharacterPlayerCombination(positionTravel.CharacterIdentity);
        ValidateIfCharacterIsLocked(positionTravel.CharacterIdentity);
        
        var character = Utils.GetPlayerCharacter(snapshot, positionTravel.CharacterIdentity);

        if (!character.Status.IsAlive) throw new Exception("Unable to travel, your character is dead.");

        if (character.Inventory.Provisions <= 0) throw new Exception("You don't have any provisions to travel.");
        if (character.Mercenaries.Select(s => s.Inventory.Provisions).Any(s => s <= 0)) throw new Exception("One or more of your mercenaries does not have enough provisions to travel.");

        var totalProvisions = character.Inventory.Provisions
            + character.Mercenaries.Select(s => s.Inventory.Provisions).Sum();

        if (totalProvisions == 0) throw new Exception("Not enough provisions to travel.");

        var destinationFullName = Utils.GetLocationFullNameFromPosition(positionTravel.Destination);
        if (!GameplayLore.Locations.All.Select(s => s.FullName).ToList().Contains(destinationFullName)) throw new Exception("No such destination is known.");
    }

    internal void ValidateMaxNumberOfCharacters(string playerId)
    {
        var playerCharsCount = snapshot.Players.Find(p => p.Identity.Id == playerId)!.Characters.Where(s => s.Status.IsAlive).ToList().Count;

        if (playerCharsCount >= 5) throw new Exception("Max number of characters reached (5 alive characters allowed per player).");
    }

    internal void ValidateTraitsOnSaveCharacter(CharacterTraits traits, string playerId)
    {
        ValidateObject(traits);
        
        if (!snapshot.CharacterStubs!.Exists(s => s.PlayerId == playerId)) throw new Exception("No stub templates found for this player.");

        ValidateRace(traits.Race);
        ValidateCulture(traits.Culture);
        ValidateTradition(traits.Tradition);
        ValidateClass(traits.Class);

        ValidateRaceCultureCombination(traits);
    }

    internal void ValidateCharacterLearnHeroicTrait(CharacterSpecialSkillAdd trait)
    {
        ValidateCharacterPlayerCombination(trait.CharacterIdentity);
        ValidateIfCharacterIsLocked(trait.CharacterIdentity);
        ValidateGuid(trait.SpecialSkillId);
        if (trait.Subskill != null)
        {
            ValidateString(trait.Subskill);
            if (!CharactersLore.Skills.All.Contains(trait.Subskill)) throw new Exception("No such Skill was found with the indicated skill name.");
        }

        var character = Utils.GetPlayerCharacter(snapshot, trait.CharacterIdentity);
        var specialSkill = SpecialSkillsLore.All.Find(t => t.Identity.Id == trait.SpecialSkillId) ?? throw new Exception("No such Heroic Trait found with the provided id.");

        if (specialSkill.DeedsCost > character.LevelUp.DeedsPoints) throw new Exception("Character does not have enough Deeds points to aquire said Heroic Trait.");

        if (specialSkill.Subtype == SpecialSkillsLore.Subtype.Onetime
            && character.Sheet.SpecialSkills.Exists(t => t.Identity.Id == specialSkill.Identity.Id)) throw new Exception("Character already has that Heroic Trait and it can only be learned once.");
    }

    internal void ValidateCharacterEquipUnequipItem(CharacterEquip equip, bool toEquip)
    {
        ValidateObject(equip);
        ValidateGuid(equip.CharacterIdentity.Id);
        ValidateGuid(equip.ItemId);
        ValidateCharacterPlayerCombination(equip.CharacterIdentity);
        ValidateIfCharacterIsLocked(equip.CharacterIdentity);
        ValidateString(equip.InventoryLocation);
        if (!ItemsLore.InventoryLocation.All.Contains(equip.InventoryLocation)) throw new Exception("Equipment location does not fit any possible slot in inventory.");

        if (!toEquip) return;

        var character = Utils.GetPlayerCharacter(snapshot, equip.CharacterIdentity);
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

    internal void ValidateStatExists(string stat)
    {
        ValidateString(stat);
        if (!CharactersLore.Stats.All.Contains(stat)) throw new Exception($"Stat {stat} does not math any possible character stats.");
    }

    internal void ValidateAssetExists(string asset)
    {
        ValidateString(asset);
        if (!CharactersLore.Assets.All.Contains(asset)) throw new Exception($"Ssset {asset} does not math any possible character assets.");
    }

    internal void ValidateCharacterHasStatsPoints(CharacterIdentity chr)
    {
        var hasPoints = snapshot.Players.Find(p => p.Identity.Id == chr.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Id)!.LevelUp.StatPoints > 0;

        if (!hasPoints) throw new Exception($"Character does not have any stat points to distribute.");
    }

    internal void ValidateCharacterHasAssetsPoints(CharacterIdentity chr)
    {
        var hasPoints = snapshot.Players.Find(p => p.Identity.Id == chr.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Id)!.LevelUp.AssetPoints > 0;

        if (!hasPoints) throw new Exception($"Character does not have any asset points to distribute.");
    }

    internal void ValidateCharacterHasSkillsPoints(CharacterIdentity chr)
    {
        var hasPoints = snapshot.Players.Find(p => p.Identity.Id == chr.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Id)!.LevelUp.SkillPoints > 0;

        if (!hasPoints) throw new Exception($"Character does not have any skill points to distribute.");
    }

    internal void ValidateSkillExists(string skill)
    {
        ValidateString(skill);
        if (!CharactersLore.Skills.All.Contains(skill)) throw new Exception($"Skill {skill} does not math any possible character skills.");
    }

    internal void ValidateCharacterName(string name)
    {
        ValidateString(name, "Invalid string for name.");
        if (name.Length < 3) throw new Exception("Character name is too short, minimum of 3 letters allowed.");
        if (name.Length > 30) throw new Exception("Character name is too long, maximum of 30 letters allowed.");
    }

    internal void ValidateIfCharacterIsLocked(CharacterIdentity charIdentity)
    {
        var chr = Utils.GetPlayerCharacter(snapshot, charIdentity);

        if (chr.Status.IsLockedToModify) throw new Exception($"Unable to modify character at this time.");
    }

    #region private methods
    private static void ValidateRaceCultureCombination(CharacterTraits origins)
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

    private void ValidateClass(string classes)
    {
        ValidateString(classes, "Invalid class string.");
        if (!CharactersLore.Classes.All.Contains(classes)) throw new Exception($"Class {classes} not found.");
    }

    private void ValidateCulture(string culture)
    {
        ValidateString(culture, "Invalid culture string.");
        if (!CharactersLore.Cultures.All.Contains(culture)) throw new Exception($"Culture {culture} not found.");
    }

    private void ValidateTradition(string tradition)
    {
        ValidateString(tradition, "Invalid tradition string.");
        if (!CharactersLore.Tradition.All.Contains(tradition)) throw new Exception($"Tradition {tradition} not found.");
    }

    private void ValidateRace(string race)
    {
        ValidateString(race, "Invalid race string.");
        if (!CharactersLore.Races.Playable.All.Contains(race)) throw new Exception($"Race {race} not found.");
    }
    #endregion
}
