using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class DiceRollLogicDelegator
{
    private readonly Random random = new();

    #region d20 rolls
    internal int Roll20noReroll()
    {
        return random.Next(1, 21);
    }

    internal int Roll20withReroll(int roll = 0) 
    {
        var handRoll = random.Next(1, 21);
        var totalRoll = roll + handRoll;

        if (handRoll % 20 == 0)
        {
            totalRoll = Roll20withReroll(totalRoll);
        }

        return totalRoll;
    }
    #endregion

    #region d100 rolls
    internal int Roll100noReroll()
    {
        return random.Next(1, 101);
    }

    internal int Roll100withReroll(int roll = 0)
    {
        var handRoll = random.Next(1, 101);
        var totalRoll = roll + handRoll;

        if (handRoll > 95)
        {
            totalRoll = Roll100withReroll(totalRoll);
        }

        return totalRoll;
    }
    #endregion

    #region custom rolls
    internal int RollGameplayDice(Character character, string skill)
    {
        if (character.Status.Traits.Tradition == GameplayLore.Tradition.Martial)
        {
            return CharacterRolld20(character, skill);
        }
        else
        {
            return CharacterRolld100(character, skill);
        }
    }
    
    internal bool RollParImpar()
    {
        return random.Next(1, 3) == 1;
    }

    internal int Roll1ToN(int upperLimit)
    {
        return random.Next(1, upperLimit + 1);
    }

    internal int RollNToN(int lowerLimit, int upperLimit)
    {
        return random.Next(lowerLimit, upperLimit + 1);
    }
    #endregion

    #region private methods
    private int CharacterRolld20(Character character, string skill)
    {
        int diceRoll = Roll20withReroll();
        var grade = 1 + diceRoll / 4;
        return GetGrades(character, skill, grade);
    }

    private int CharacterRolld100(Character character, string skill)
    {
        int diceRoll = Roll100withReroll();
        var grade = 1 + diceRoll / 20;
        return GetGrades(character, skill, grade);
    }

    private static int GetGrades(Character character, string skill, int grade)
    {
        var crit = grade / 5 - 1;
        var crits = crit > 0 ? crit : 0;
        var skillValue = GetSkillValue(character, skill);
        var grades = grade * skillValue * 5 / 100;

        for (var i = 0; i < crits; i++)
        {
            LevelUpCharacter(character);
        }

        return grades;
    }

    private static void LevelUpCharacter(Character character)
    {
        character.LevelUp.DeedsPoints += 1 * character.Status.EntityLevel;
        character.LevelUp.StatPoints += 2 * character.Status.EntityLevel;
        character.LevelUp.SkillPoints += 4 * character.Status.EntityLevel;
    }

    private static int GetSkillValue(Character character, string skill)
    {
        int skillValue;

        if      (skill == CharactersLore.Skills.Combat) skillValue = character.Sheet.Skills.Combat;
        else if (skill == CharactersLore.Skills.Arcane) skillValue = character.Sheet.Skills.Arcane;
        else if (skill == CharactersLore.Skills.Psionics) skillValue = character.Sheet.Skills.Psionics;
        else if (skill == CharactersLore.Skills.Hide) skillValue = character.Sheet.Skills.Hide;
        else if (skill == CharactersLore.Skills.Traps) skillValue = character.Sheet.Skills.Traps;
        else if (skill == CharactersLore.Skills.Tactics) skillValue = character.Sheet.Skills.Tactics;
        else if (skill == CharactersLore.Skills.Social) skillValue = character.Sheet.Skills.Social;
        else if (skill == CharactersLore.Skills.Apothecary) skillValue = character.Sheet.Skills.Apothecary;
        else if (skill == CharactersLore.Skills.Travel) skillValue = character.Sheet.Skills.Travel;
        else if (skill == CharactersLore.Skills.Sail) skillValue = character.Sheet.Skills.Sail;
        else throw new NotImplementedException();

        return skillValue;
    }

    #endregion
}
