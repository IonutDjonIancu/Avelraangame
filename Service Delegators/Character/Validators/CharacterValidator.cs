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

        var (hasChangesToName, hasChangesToStats, hasChangesToSkills) = DiscoverChangesAtUpdate(charUpdate, playerId);

        if (hasChangesToName) ValidateName(charUpdate.Name);
        if (hasChangesToStats) ValidateCharacterStatsOnUpdate(charUpdate, playerId);
        if (hasChangesToSkills) ValidateCharacterSkillsOnUpdate(charUpdate, playerId);
    }

    public void ValidateCharacterOnDelete(string characterId, string playerId)
    {
        ValidateGuid(characterId);

        ValidateIfCharacterExists(playerId, characterId);
    }

    #region privates
    private (bool name, bool stats, bool skills) DiscoverChangesAtUpdate(CharacterUpdate newChar, string playerId)
    {
        var oldChar = metadata.GetCharacter(newChar.CharacterId, playerId);
        bool name = false;
        bool stats = false;
        bool skills = false;

        if (newChar.Name != oldChar.Info.Name)
        {
            name = true;
        }
        if (newChar.Doll.Strength != oldChar.Doll.Strength ||
            newChar.Doll.Constitution != oldChar.Doll.Constitution ||
            newChar.Doll.Agility != oldChar.Doll.Agility ||
            newChar.Doll.Willpower != oldChar.Doll.Willpower ||
            newChar.Doll.Perception != oldChar.Doll.Perception ||
            newChar.Doll.Abstract != oldChar.Doll.Abstract)
        {
            stats = true;
        }
        if (newChar.Doll.Combat != oldChar.Doll.Combat ||
            newChar.Doll.Arcane != oldChar.Doll.Arcane ||
            newChar.Doll.Psionics != oldChar.Doll.Psionics ||
            newChar.Doll.Hide != oldChar.Doll.Hide ||
            newChar.Doll.Traps != oldChar.Doll.Traps ||
            newChar.Doll.Tactics != oldChar.Doll.Tactics ||
            newChar.Doll.Social != oldChar.Doll.Social ||
            newChar.Doll.Apothecary != oldChar.Doll.Apothecary ||
            newChar.Doll.Travel != oldChar.Doll.Travel ||
            newChar.Doll.Sail != oldChar.Doll.Sail)
        {
            skills = true;
        }

        return (name, stats, skills);
    }

    private string ValidatePlayer(string playerName)
    {
        ValidateString(playerName);

        var player = dbm.Snapshot.Players.Find(p => p.Identity.Name == playerName);

        if (player == null) Throw("Player not found.");

        return player.Identity.Id;
    }

    private void ValidateCharacterStatsOnUpdate(CharacterUpdate newChar, string playerId)
    {
        ValidateObject(newChar.Doll);
        var oldChar = dbm.Snapshot.Players!.Find(p => p.Identity.Id == playerId)!.Characters!.Find(c => c.Identity!.Id == newChar.CharacterId);

        // do not allow decrease of stats
        if (newChar.Doll.Strength < oldChar.Doll.Strength ||
            newChar.Doll.Constitution < oldChar.Doll.Constitution ||
            newChar.Doll.Agility < oldChar.Doll.Agility ||
            newChar.Doll.Willpower < oldChar.Doll.Willpower ||
            newChar.Doll.Perception < oldChar.Doll.Perception ||
            newChar.Doll.Abstract < oldChar.Doll.Abstract)
        {
            Throw("Cannot decrease stats.");
        }

        var sumOfStats = CharacterOperations.SumStats(newChar.Doll!);
        var sumOfOldStats = CharacterOperations.SumStats(oldChar!.Doll!) + oldChar.LevelUp!.StatPoints;

        if (sumOfStats > sumOfOldStats) Throw("No stat points.");
    }

    private void ValidateCharacterSkillsOnUpdate(CharacterUpdate newChar, string playerId)
    {
        ValidateObject(newChar.Doll);
        var oldChar = dbm.Snapshot.Players!.Find(p => p.Identity.Id == playerId)!.Characters!.Find(c => c.Identity!.Id == newChar.CharacterId);

        // do not allow decrease of skills
        if (newChar.Doll.Combat < oldChar.Doll.Combat ||
            newChar.Doll.Arcane < oldChar.Doll.Arcane ||
            newChar.Doll.Psionics < oldChar.Doll.Psionics ||
            newChar.Doll.Hide < oldChar.Doll.Hide ||
            newChar.Doll.Traps < oldChar.Doll.Traps ||
            newChar.Doll.Tactics < oldChar.Doll.Tactics ||
            newChar.Doll.Social < oldChar.Doll.Social ||
            newChar.Doll.Apothecary < oldChar.Doll.Apothecary ||
            newChar.Doll.Travel < oldChar.Doll.Travel ||
            newChar.Doll.Sail < oldChar.Doll.Sail)
        {
            Throw("Cannot decrease skills.");
        }

        var sumOfSkills = CharacterOperations.SumSkills(newChar.Doll!);
        var sumOfOldSkills = CharacterOperations.SumSkills(oldChar!.Doll!) + oldChar.LevelUp!.SkillPoints;

        if (sumOfSkills > sumOfOldSkills) Throw("No skills points.");
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

#pragma warning restore CS8602 // Dereference of a possibly null reference.
