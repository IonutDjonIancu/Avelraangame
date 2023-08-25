using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class DiceRollLogicDelegator
{
    private readonly Random random = new();

    #region d100 rolls
    internal int Roll100noReroll()
    {
        return random.Next(1, 101);
    }

    internal int Roll100withReroll()
    {
        var totalRoll = random.Next(1, 101);

        if (totalRoll > 95)
        {
            totalRoll += Roll100noReroll();
        }

        return totalRoll;
    }
    #endregion

    #region custom rolls
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

    #region gameplay rolls
    internal int RollGameplayDice(bool isOffense, string attribute, Character character)
    {
        //var grade = character.Status.Traits.Tradition == GameplayLore.Tradition.Martial
        //    ? 1 + Roll20withReroll() / 4
        //    : 1 + Roll100withReroll() / 20;

        var grade = 25;

        return GetRollGradeAndLevelup(grade, isOffense, attribute, character);
    }
    #endregion

    #region private methods
    private static int GetRollGradeAndLevelup(int grade, bool isOffense, string attribute, Character character)
    {
        var crit = grade / 5 - 1;
        var crits = crit > 0 ? crit : 0;
        var attrValue = GetAttributeValue(character, attribute);

        // this calculates that a 20 skill represents a 100%
        var grades = grade * attrValue * 5 / 100;

        if (isOffense) LevelUpCharacter(crits, character);

        return grades;
    }

    private static void LevelUpCharacter(int crits, Character character)
    {
        character.LevelUp.DeedsPoints   += crits * (character.Status.EntityLevel);
        character.LevelUp.StatPoints    += crits * (character.Status.EntityLevel + 1);
        character.LevelUp.AssetPoints   += crits * (character.Status.EntityLevel + 2);
        character.LevelUp.SkillPoints   += crits * (character.Status.EntityLevel + 3);
    }

    private static int GetAttributeValue(Character character, string attribute) => attribute switch
    {
        // stats
        CharactersLore.Stats.Strength       => character.Sheet.Stats.Strength,
        CharactersLore.Stats.Constitution   => character.Sheet.Stats.Constitution,
        CharactersLore.Stats.Agility        => character.Sheet.Stats.Agility,
        CharactersLore.Stats.Willpower      => character.Sheet.Stats.Willpower,
        CharactersLore.Stats.Perception     => character.Sheet.Stats.Perception,
        CharactersLore.Stats.Abstract       => character.Sheet.Stats.Abstract,

        // assets
        CharactersLore.Assets.Spot          => character.Sheet.Assets.Spot,

        // skills
        CharactersLore.Skills.Combat        => character.Sheet.Skills.Combat,
        CharactersLore.Skills.Arcane        => character.Sheet.Skills.Arcane,
        CharactersLore.Skills.Psionics      => character.Sheet.Skills.Psionics,
        CharactersLore.Skills.Hide          => character.Sheet.Skills.Hide,
        CharactersLore.Skills.Traps         => character.Sheet.Skills.Traps,
        CharactersLore.Skills.Tactics       => character.Sheet.Skills.Tactics,
        CharactersLore.Skills.Social        => character.Sheet.Skills.Social,
        CharactersLore.Skills.Apothecary    => character.Sheet.Skills.Apothecary,
        CharactersLore.Skills.Travel        => character.Sheet.Skills.Travel,
        CharactersLore.Skills.Sail          => character.Sheet.Skills.Sail,

        _ => throw new Exception($"Wrong attribute given: {attribute}")
    };
    #endregion
}
