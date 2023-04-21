﻿#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Validators;
using Persistance_Manager;

namespace Service_Delegators.Validators;

public class CharacterValidator : ValidatorBase
{
    private readonly IDatabaseManager dbm;

    public CharacterValidator(IDatabaseManager manager)
    {
        dbm = manager;
    }

    public void ValidateSkillsToDistribute(string charId, string skill, string playerId)
    {
        var skillPoints = dbm.Metadata.GetCharacterById(charId, playerId).LevelUp.SkillPoints;

        if (skillPoints <= 0) Throw("No skill points to distribute.");
        if (!CharactersLore.Skills.All.Contains(skill)) Throw($"Unable to determine skill name: {skill}.");
    }

    public void ValidateStatsToDistribute(string charId, string stat, string playerId)
    {
        var statPoints = dbm.Metadata.GetCharacterById(charId, playerId).LevelUp.StatPoints;

        if (statPoints <= 0) Throw("No stat points to distribute.");
        if (!CharactersLore.Stats.All.Contains(stat)) Throw($"Unable to determine stat name: {stat}.");
    }

    public void ValidateMaxNumberOfCharacters(string playerId)
    {
        var playerCharsCount = dbm.Metadata.GetPlayerById(playerId).Characters.Count;

        if (playerCharsCount >= 5) Throw("Max number of characters reached (5 characters allowed per player)");
    }

    public void ValidateOriginsOnSaveCharacter(CharacterOrigins origins, string playerId)
    {
        ValidateObject(origins);
        
        if (!dbm.Snapshot.CharacterStubs!.Exists(s => s.PlayerId == playerId)) Throw("No stub templates found for this player.");

        ValidateRace(origins.Race);
        ValidateCulture(origins.Culture);
        ValidateHeritage(origins.Heritage);
        ValidateClass(origins.Class);

        ValidateRaceCultureCombination(origins);
    }

    public void ValidateCharacterOnNameUpdate(CharacterUpdate charUpdate, string playerId)
    {
        ValidateObject(charUpdate);
        ValidateGuid(charUpdate.CharacterId);
        ValidateCharacterPlayerCombination(charUpdate.CharacterId, playerId);
        ValidateName(charUpdate.Name);
    }

    public void ValidateCharacterOnDelete(string characterId, string playerId)
    {
        ValidateGuid(characterId);

        ValidateCharacterPlayerCombination(characterId, playerId);
    }

    public void ValidateCharacterLearnHeroicTrait(CharacterHeroicTrait trait, string playerId)
    {
        ValidateGuid(trait.CharacterId);
        ValidateCharacterPlayerCombination(trait.CharacterId, playerId);
        var character = dbm.Snapshot.Players.First(p => p.Identity.Id == playerId).Characters!.First(c => c.Identity.Id == trait.CharacterId);

        ValidateGuid(trait.HeroicTraitId);
        var heroicTrait = dbm.Snapshot.Traits.Find(t => t.Identity.Id == trait.HeroicTraitId);
        if (heroicTrait == null) Throw("No such Heroic Trait found with the provided id.");

        if (heroicTrait.DeedsCost > character.LevelUp.DeedsPoints) Throw("Character does not have enough Deeds points to aquire said Heroic Trait.");

        if (heroicTrait.Subtype == TraitsLore.Subtype.onetime
            && character.HeroicTraits.Exists(t => t.Identity.Id == heroicTrait.Identity.Id)) Throw("Character already has that Heroic Trait and it can only be learned once.");

        if (!string.IsNullOrWhiteSpace(trait.Skill))
        {
            if (!CharactersLore.Skills.All.Contains(trait.Skill)) Throw("No such Skill was found with the indicated skill name.");
        }
    }

    public void ValidateCharacterEquipUnequipItem(CharacterEquip equip, string playerId, bool toEquip)
    {
        ValidateObject(equip);
        ValidateGuid(equip.CharacterId);
        ValidateGuid(equip.ItemId);
        ValidateCharacterPlayerCombination(equip.CharacterId, playerId);
        ValidateString(equip.InventoryLocation);
        if (!ItemsLore.InventoryLocation.All.Contains(equip.InventoryLocation)) Throw("Equipment location does not fit any possible slot in inventory.");

        if (!toEquip) return;
        
        var character = dbm.Metadata.GetCharacterById(equip.CharacterId, playerId);
        var itemSubtype = character.Supplies.Find(i => i.Identity.Id == equip.ItemId)?.Subtype;
        if (itemSubtype == null) Throw("No such item found on this character.");

        bool isItemAtCorrectLocation;

        // protection
        if (itemSubtype == ItemsLore.Subtypes.Protections.Helmet)
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
                equip.InventoryLocation == ItemsLore.InventoryLocation.Shield;
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

            if (character.Inventory.Heraldry.Count >= 5)
            {
                Throw("Heraldry is full, unequip some of the items first.");
            }
        }
        else
        {
            isItemAtCorrectLocation = false;
        }

        if (!isItemAtCorrectLocation) Throw("Unable to equip the item at said location.");
    }

    public void ValidateCharacterPlayerCombination(string characterId, string playerId)
    {
        if (!dbm.Metadata.DoesCharacterExist(characterId, playerId)) Throw("Character not found.");
    }

    #region private methods
    private void ValidateRaceCultureCombination(CharacterOrigins sublore)
    {
        string message = "Invalid race culture combination";

        if (sublore.Race == CharactersLore.Races.Human)
        {
            if (!CharactersLore.Cultures.Human.All.Contains(sublore.Culture)) Throw(message);
        }
        else if (sublore.Race == CharactersLore.Races.Elf)
        {
            if (!CharactersLore.Cultures.Elf.All.Contains(sublore.Culture)) Throw(message);
        }
        else if (sublore.Race == CharactersLore.Races.Dwarf)
        {
            if (!CharactersLore.Cultures.Dwarf.All.Contains(sublore.Culture)) Throw(message);
        }
    }

    private void ValidateClass(string classes)
    {
        ValidateString(classes, "Invalid class.");
        if (!CharactersLore.Classes.All.Contains(classes)) Throw($"Invalid class {classes}.");
    }

    private void ValidateHeritage(string heritage)
    {
        ValidateString(heritage, "Invalid heritage.");
        if (!CharactersLore.Heritage.All.Contains(heritage)) Throw($"Invalid heritage: {heritage}.");
    }

    private void ValidateCulture(string culture)
    {
        ValidateString(culture, "Invalid culture.");
        if (!CharactersLore.Cultures.All.Contains(culture)) Throw($"Invalid culture {culture}");
    }

    private void ValidateRace(string race)
    {
        ValidateString(race, "Invalid race.");
        if (!CharactersLore.Races.All.Contains(race)) Throw($"Invalid race {race}");
    }

    private void ValidateName(string name)
    {
        ValidateString(name, "Invalid name.");
        if (name.Length < 3) Throw("Character name is too short, minimum of 3 letters allowed.");
        if (name.Length > 30) Throw("Character name is too long, maximum of 30 letters allowed.");
    }
    #endregion
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
