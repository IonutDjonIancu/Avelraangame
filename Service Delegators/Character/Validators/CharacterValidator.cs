#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Validators;
using Persistance_Manager;

namespace Service_Delegators.Validators;

public class CharacterValidator : ValidatorBase
{
    private readonly IDatabaseManager dbm;
    private readonly CharacterMetadata metadata;

    public CharacterValidator(
        IDatabaseManager manager,
        CharacterMetadata metadata)
    {
        dbm = manager;
        this.metadata = metadata;
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

    #region privates
    private void ValidateIfCharacterExists(string playerId, string characterId)
    {
        if (!metadata.DoesCharacterExist(playerId, characterId)) Throw("Character not found.");
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
