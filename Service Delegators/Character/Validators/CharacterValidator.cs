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

    public void ValidatePlayerOnCreateCharacter(string playerName)
    {
        ValidatePlayer(playerName);
    }

    public void ValidateOriginsOnSaveCharacter(CharacterOrigins origins, string playerId)
    {
        ValidateObject(origins);
        
        if (!dbm.Snapshot.CharacterStubs!.Exists(s => s.PlayerId == playerId)) Throw("No stub templates found for this player.");

        ValidateRace(origins.Race);
        ValidateCulture(origins.Culture);
        ValidateTradition(origins.Tradition);

        ValidateRaceCultureCombination(origins);
    }

    public void ValidateCharacterOnUpdate(CharacterUpdate charUpdate, string playerId)
    {
        ValidateObject(charUpdate);
        ValidateGuid(charUpdate.CharacterId);
        ValidateIfCharacterExists(playerId, charUpdate.CharacterId);

        if      (charUpdate.HasChangesToName) ValidateName(charUpdate.Name);
        else if (charUpdate.HasChangesToStats) ValidateCharacterStatsOnUpdate(charUpdate, playerId);
        else if (charUpdate.HasChangesToSkills) ValidateCharacterSkillsOnUpdate(charUpdate, playerId);
    }

    public void ValidateCharacterOnDelete(string characterId, string playerId)
    {
        ValidateGuid(characterId);

        ValidateIfCharacterExists(playerId, characterId);
    }

    #region privates
    private string ValidatePlayer(string playerName)
    {
        ValidateString(playerName);

        var player = dbm.Snapshot.Players.Find(p => p.Identity.Name == playerName);

        if (player == null) Throw("Player not found.");

        return player.Identity.Id;
    }

    private void ValidateCharacterStatsOnUpdate(CharacterUpdate charUpdate, string playerId)
    {
        ValidateObject(charUpdate.Doll);
        var oldChar = dbm.Snapshot.Players!.Find(p => p.Identity.Id == playerId)!.Characters!.Find(c => c.Identity!.Id == charUpdate.CharacterId);

        var sumOfStats = CharacterOperations.SumStats(charUpdate.Doll!);
        var sumOfOldStats = CharacterOperations.SumStats(oldChar!.Doll!) + oldChar.LevelUp!.StatPoints;

        if (sumOfStats > sumOfOldStats) Throw("Number of requested stat points is greater than the stored.");
    }

    private void ValidateCharacterSkillsOnUpdate(CharacterUpdate charUpdate, string playerId)
    {
        ValidateObject(charUpdate.Doll);
        var oldChar = dbm.Snapshot.Players!.Find(p => p.Identity.Id == playerId)!.Characters!.Find(c => c.Identity!.Id == charUpdate.CharacterId);

        var sumOfSkills = CharacterOperations.SumSkills(charUpdate.Doll!);
        var sumOfOldSkills = CharacterOperations.SumSkills(oldChar!.Doll!) + oldChar.LevelUp!.SkillPoints;

        if (sumOfSkills > sumOfOldSkills) Throw("Number of requested skill points is greater than the stored.");
    }

    

    

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
