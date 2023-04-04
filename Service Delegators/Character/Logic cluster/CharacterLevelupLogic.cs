#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterLevelupLogic
{
    private readonly IDatabaseManager dbm;
    private readonly CharacterMetadata charMetadata;

    public CharacterLevelupLogic(
        IDatabaseManager databaseManager,
        CharacterMetadata charMetadata)
    {
        this.charMetadata = charMetadata;
        dbm = databaseManager;
    }

    internal Character IncreaseSkills(CharacterUpdate charUpdate, string playerId)
    {
        var storedChar = charMetadata.GetCharacter(charUpdate.CharacterId, playerId);

        if      (charUpdate.Skill == CharactersLore.Skills.Combat) storedChar.Sheet.Combat++;
        else if (charUpdate.Skill == CharactersLore.Skills.Arcane) storedChar.Sheet.Arcane++;
        else if (charUpdate.Skill == CharactersLore.Skills.Psionics) storedChar.Sheet.Psionics++;
        else if (charUpdate.Skill == CharactersLore.Skills.Hide) storedChar.Sheet.Hide++;
        else if (charUpdate.Skill == CharactersLore.Skills.Traps) storedChar.Sheet.Traps++;
        else if (charUpdate.Skill == CharactersLore.Skills.Tactics) storedChar.Sheet.Tactics++;
        else if (charUpdate.Skill == CharactersLore.Skills.Social) storedChar.Sheet.Social++;
        else if (charUpdate.Skill == CharactersLore.Skills.Apothecary) storedChar.Sheet.Apothecary++;
        else if (charUpdate.Skill == CharactersLore.Skills.Travel) storedChar.Sheet.Travel++;
        else if (charUpdate.Skill == CharactersLore.Skills.Sail) storedChar.Sheet.Sail++;
        else throw new Exception("Unrecognized skill.");

        storedChar.LevelUp.SkillPoints--;

        var player = dbm.Metadata.GetPlayerById(playerId);

        dbm.PersistPlayer(player);

        return storedChar;
    }

    internal Character IncreaseStats(CharacterUpdate charUpdate, string playerId)
    {
        var storedChar = charMetadata.GetCharacter(charUpdate.CharacterId, playerId);

        if      (charUpdate.Stat == CharactersLore.Stats.Strength) storedChar.Sheet.Strength++;
        else if (charUpdate.Stat == CharactersLore.Stats.Constitution) storedChar.Sheet.Constitution++;
        else if (charUpdate.Stat == CharactersLore.Stats.Agility) storedChar.Sheet.Agility++;
        else if (charUpdate.Stat == CharactersLore.Stats.Willpower) storedChar.Sheet.Willpower++;
        else if (charUpdate.Stat == CharactersLore.Stats.Perception) storedChar.Sheet.Perception++;
        else if (charUpdate.Stat == CharactersLore.Stats.Abstract) storedChar.Sheet.Abstract++;
        else throw new Exception("Unrecognized stat.");

        storedChar.LevelUp.StatPoints--;

        var player = dbm.Metadata.GetPlayerById(playerId);

        dbm.PersistPlayer(player);

        return storedChar;
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.