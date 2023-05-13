using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterLevelupLogic
{
    private readonly IDatabaseService dbs;

    private CharacterLevelupLogic() { }
    internal CharacterLevelupLogic(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

    internal Character IncreaseSkills(string skill, CharacterIdentity identity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(identity);

        if      (skill == CharactersLore.Skills.Combat) storedChar.Sheet!.Skills.Combat++;
        else if (skill == CharactersLore.Skills.Arcane) storedChar.Sheet!.Skills.Arcane++;
        else if (skill == CharactersLore.Skills.Psionics) storedChar.Sheet!.Skills.Psionics++;
        else if (skill == CharactersLore.Skills.Hide) storedChar.Sheet!.Skills.Hide++;
        else if (skill == CharactersLore.Skills.Traps) storedChar.Sheet!.Skills.Traps++;
        else if (skill == CharactersLore.Skills.Tactics) storedChar.Sheet!.Skills.Tactics++;
        else if (skill == CharactersLore.Skills.Social) storedChar.Sheet!.Skills.Social++;
        else if (skill == CharactersLore.Skills.Apothecary) storedChar.Sheet!.Skills.Apothecary++;
        else if (skill == CharactersLore.Skills.Travel) storedChar.Sheet!.Skills.Travel++;
        else if (skill == CharactersLore.Skills.Sail) storedChar.Sheet!.Skills.Sail++;

        storedChar.LevelUp!.SkillPoints--;

        dbs.PersistPlayer(player.Identity.Id);

        return storedChar;
    }

    internal Character IncreaseStats(string stat, CharacterIdentity identity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(identity);

        if      (stat == CharactersLore.Stats.Strength) storedChar.Sheet!.Stats.Strength++;
        else if (stat == CharactersLore.Stats.Constitution) storedChar.Sheet!.Stats.Constitution++;
        else if (stat == CharactersLore.Stats.Agility) storedChar.Sheet!.Stats.Agility++;
        else if (stat == CharactersLore.Stats.Willpower) storedChar.Sheet!.Stats.Willpower++;
        else if (stat == CharactersLore.Stats.Perception) storedChar.Sheet!.Stats.Perception++;
        else if (stat == CharactersLore.Stats.Abstract) storedChar.Sheet!.Stats.Abstract++;

        storedChar!.LevelUp!.StatPoints--;

        dbs.PersistPlayer(player.Identity.Id);

        return storedChar;
    }

    #region private methods
    private (Character, Player) GetStoredCharacterAndPlayer(CharacterIdentity identity)
    {
        var player = dbs.Snapshot.Players.Find(p => p.Identity.Id == identity.PlayerId)!;
        var character = player.Characters!.Find(c => c.Identity!.Id == identity.Id)!;

        return (character, player);
    }
    #endregion
}
