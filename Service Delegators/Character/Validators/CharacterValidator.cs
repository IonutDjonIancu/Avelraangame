﻿#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Validators;
using Persistance_Manager;

namespace Service_Delegators.Validators;

public class CharacterValidator : ValidatorBase
{
    private readonly IDatabaseManager dbm;
    private readonly CharacterMetadata charMetadata;

    public CharacterValidator(
        IDatabaseManager manager,
        CharacterMetadata charMetadata)
    {
        dbm = manager;
        this.charMetadata = charMetadata;
    }

    public void ValidateSkillsToDistribute(string charId, string skill, string playerId)
    {
        var skillPoints = charMetadata.GetCharacter(charId, playerId).LevelUp.SkillPoints;

        if (skillPoints <= 0) Throw("No skill points to distribute.");
        if (!CharactersLore.Skills.All.Contains(skill)) Throw($"Unable to determine skill name: {skill}.");
    }

    public void ValidateStatsToDistribute(string charId, string stat, string playerId)
    {
        var statPoints = charMetadata.GetCharacter(charId, playerId).LevelUp.StatPoints;

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
        ValidateTradition(origins.Tradition);
        ValidateClass(origins.Class);

        ValidateRaceCultureCombination(origins);
    }

    public void ValidateCharacterOnNameUpdate(CharacterUpdate charUpdate, string playerId)
    {
        ValidateObject(charUpdate);
        ValidateGuid(charUpdate.CharacterId);
        ValidateIfCharacterExists(playerId, charUpdate.CharacterId);
        ValidateName(charUpdate.Name);
    }

    public void ValidateCharacterOnDelete(string characterId, string playerId)
    {
        ValidateGuid(characterId);

        ValidateIfCharacterExists(playerId, characterId);
    }

    public void ValidateCharacterEquipUnequipItem(CharacterEquip equip, string playerId, bool isEquip)
    {
        ValidateObject(equip);
        ValidateGuid(equip.CharacterId);
        ValidateGuid(equip.ItemId);
        ValidateIfCharacterExists(playerId, equip.CharacterId);
        ValidateString(equip.Location);
        if (!CharactersLore.Equipment.All.Contains(equip.Location)) Throw("Equipment location does not fit any possible slot in inventory.");

        if (!isEquip) return;
        
        var chr = charMetadata.GetCharacter(equip.CharacterId, playerId);
        var itemSubtype = chr.Supplies.Find(i => i.Identity.Id == equip.ItemId)?.Subtype;
        if (itemSubtype == null) Throw("No such item found on this character.");

        var isItemAtCorrectLocation = false;

        if (itemSubtype == ItemsLore.Subtypes.Protections.Helmet)
        {
            isItemAtCorrectLocation = 
                CharactersLore.Equipment.Head.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Protections.Armour)
        {
            isItemAtCorrectLocation = 
                CharactersLore.Equipment.Body.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Protections.Shield)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Mainhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Offhand.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Axe)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Mainhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Offhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Ranged.Equals(equip.Location) ||
                CharactersLore.Equipment.Serviceweapon.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Bow)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Ranged.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Crossbow)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Mainhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Offhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Ranged.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Dagger)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Mainhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Offhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Ranged.Equals(equip.Location) ||
                CharactersLore.Equipment.Serviceweapon.Equals(equip.Location) ||
                CharactersLore.Equipment.Heraldry.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Mace)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Mainhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Offhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Serviceweapon.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Pike)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Mainhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Offhand.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Polearm)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Mainhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Offhand.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Sling)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Ranged.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Spear)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Mainhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Offhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Ranged.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Sword)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Mainhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Offhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Serviceweapon.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Sword)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Mainhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Offhand.Equals(equip.Location) ||
                CharactersLore.Equipment.Serviceweapon.Equals(equip.Location);
        }
        else if (itemSubtype == ItemsLore.Subtypes.Wealth.Gems ||
            itemSubtype == ItemsLore.Subtypes.Wealth.Valuables ||
            itemSubtype == ItemsLore.Subtypes.Wealth.Trinket)
        {
            isItemAtCorrectLocation =
                CharactersLore.Equipment.Heraldry.Equals(equip.Location);

            if (chr.Inventory.Heraldry.Count >= 5)
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

    #region privates
    private void ValidateIfCharacterExists(string playerId, string characterId)
    {
        if (!charMetadata.DoesCharacterExist(playerId, characterId)) Throw("Character not found.");
    }

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

    private void ValidateTradition(string tradition)
    {
        ValidateString(tradition, "Invalid tradition.");
        if (!CharactersLore.Traditions.All.Contains(tradition)) Throw($"Invalid tradition {tradition}.");
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
