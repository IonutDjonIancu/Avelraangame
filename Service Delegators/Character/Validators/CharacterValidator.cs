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

    internal void ValidateMaxNumberOfCharacters(string playerId)
    {
        var playerCharsCount = snapshot.Players.Find(p => p.Identity.Id == playerId)!.Characters!.Count;

        if (playerCharsCount >= 5) throw new Exception("Max number of characters reached (5 characters allowed per player)");
    }

    internal void ValidateOriginsOnSaveCharacter(CharacterOrigins origins, string playerId)
    {
        ValidateObject(origins);
        
        if (!snapshot.CharacterStubs!.Exists(s => s.PlayerId == playerId)) throw new Exception("No stub templates found for this player.");

        ValidateRace(origins.Race);
        ValidateCulture(origins.Culture);
        ValidateTradition(origins.Tradition);
        ValidateClass(origins.Class);

        ValidateRaceCultureCombination(origins);
    }

    internal void ValidateCharacterLearnHeroicTrait(CharacterHeroicTrait trait)
    {
        ValidateCharacterPlayerCombination(trait.CharacterIdentity);
        ValidateGuid(trait.HeroicTraitId);
        if (trait.Skill != null)
        {
            ValidateString(trait.Skill);
            if (!CharactersLore.Skills.All.Contains(trait.Skill)) throw new Exception("No such Skill was found with the indicated skill name.");
        }

        var character = GetCharacter(trait.CharacterIdentity);
        var heroicTrait = TraitsLore.All.Find(t => t.Identity.Id == trait.HeroicTraitId) ?? throw new Exception("No such Heroic Trait found with the provided id.");

        if (heroicTrait.DeedsCost > character.LevelUp.DeedsPoints) throw new Exception("Character does not have enough Deeds points to aquire said Heroic Trait.");

        if (heroicTrait.Subtype == TraitsLore.Subtype.onetime
            && character.HeroicTraits.Exists(t => t.Identity.Id == heroicTrait.Identity.Id)) throw new Exception("Character already has that Heroic Trait and it can only be learned once.");
    }

    internal void ValidateCharacterEquipUnequipItem(CharacterEquip equip, bool toEquip)
    {
        ValidateObject(equip);
        ValidateGuid(equip.CharacterIdentity.Id);
        ValidateGuid(equip.ItemId);
        ValidateCharacterPlayerCombination(equip.CharacterIdentity);
        ValidateString(equip.InventoryLocation);
        if (!ItemsLore.InventoryLocation.All.Contains(equip.InventoryLocation)) throw new Exception("Equipment location does not fit any possible slot in inventory.");

        if (!toEquip) return;

        var character = GetCharacter(equip.CharacterIdentity);
        var itemSubtype = (character!.Supplies!.Find(i => i.Identity.Id == equip.ItemId)?.Subtype) ?? throw new Exception("No such item found on this character.");
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

    internal void ValidateCharacterHasStatsPoints(CharacterIdentity chr)
    {
        var hasPoints = snapshot.Players.Find(p => p.Identity.Id == chr.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Id)!.LevelUp.StatPoints > 0;

        if (!hasPoints) throw new Exception($"Character does not have any stat points to distribute.");
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
        var chr = GetCharacter(charIdentity);

        if (chr.Status.IsLockedForModify) throw new Exception("Cannot modify character at this time.");
    }

    internal void ValidateIfCharacterInGameplay(CharacterIdentity charIdentity)
    {
        var character = GetCharacter(charIdentity);

        if (character.Status.IsInQuest 
            || character.Status.IsInArena
            || character.Status.IsInStory) throw new Exception("Unable to modify character during gameplay.");
    }

    #region private methods
    private static void ValidateRaceCultureCombination(CharacterOrigins origins)
    {
        string message = "Invalid race culture combination";

        if (origins.Race == CharactersLore.Races.Human)
        {
            if (!CharactersLore.Cultures.Human.All.Contains(origins.Culture)) throw new Exception(message);
        }
        else if (origins.Race == CharactersLore.Races.Elf)
        {
            if (!CharactersLore.Cultures.Elf.All.Contains(origins.Culture)) throw new Exception(message);
        }
        else if (origins.Race == CharactersLore.Races.Dwarf)
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

    private void ValidateRace(string race)
    {
        ValidateString(race, "Invalid race string.");
        if (!CharactersLore.Races.All.Contains(race)) throw new Exception($"Race {race} not found.");
    }
    #endregion
}
