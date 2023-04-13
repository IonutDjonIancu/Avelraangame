#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterLevelupLogic
{
    private readonly IDatabaseManager dbm;

    public CharacterLevelupLogic(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
    }

    internal Character IncreaseSkills(CharacterUpdate charUpdate, string playerId)
    {
        var storedChar = dbm.Metadata.GetCharacterById(charUpdate.CharacterId, playerId);

        if      (charUpdate.Skill == CharactersLore.Skills.Combat) storedChar.Sheet.Skills.Combat++;
        else if (charUpdate.Skill == CharactersLore.Skills.Arcane) storedChar.Sheet.Skills.Arcane++;
        else if (charUpdate.Skill == CharactersLore.Skills.Psionics) storedChar.Sheet.Skills.Psionics++;
        else if (charUpdate.Skill == CharactersLore.Skills.Hide) storedChar.Sheet.Skills.Hide++;
        else if (charUpdate.Skill == CharactersLore.Skills.Traps) storedChar.Sheet.Skills.Traps++;
        else if (charUpdate.Skill == CharactersLore.Skills.Tactics) storedChar.Sheet.Skills.Tactics++;
        else if (charUpdate.Skill == CharactersLore.Skills.Social) storedChar.Sheet.Skills.Social++;
        else if (charUpdate.Skill == CharactersLore.Skills.Apothecary) storedChar.Sheet.Skills.Apothecary++;
        else if (charUpdate.Skill == CharactersLore.Skills.Travel) storedChar.Sheet.Skills.Travel++;
        else if (charUpdate.Skill == CharactersLore.Skills.Sail) storedChar.Sheet.Skills.Sail++;
        else throw new Exception("Unrecognized skill.");

        storedChar.LevelUp.SkillPoints--;

        var player = dbm.Metadata.GetPlayerById(playerId);

        dbm.PersistPlayer(player);

        return storedChar;
    }

    internal Character IncreaseStats(CharacterUpdate charUpdate, string playerId)
    {
        var storedChar = dbm.Metadata.GetCharacterById(charUpdate.CharacterId, playerId);

        if      (charUpdate.Stat == CharactersLore.Stats.Strength) storedChar.Sheet.Stats.Strength++;
        else if (charUpdate.Stat == CharactersLore.Stats.Constitution) storedChar.Sheet.Stats.Constitution++;
        else if (charUpdate.Stat == CharactersLore.Stats.Agility) storedChar.Sheet.Stats.Agility++;
        else if (charUpdate.Stat == CharactersLore.Stats.Willpower) storedChar.Sheet.Stats.Willpower++;
        else if (charUpdate.Stat == CharactersLore.Stats.Perception) storedChar.Sheet.Stats.Perception++;
        else if (charUpdate.Stat == CharactersLore.Stats.Abstract) storedChar.Sheet.Stats.Abstract++;
        else throw new Exception("Unrecognized stat.");

        storedChar.LevelUp.StatPoints--;

        var player = dbm.Metadata.GetPlayerById(playerId);

        dbm.PersistPlayer(player);

        return storedChar;
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
