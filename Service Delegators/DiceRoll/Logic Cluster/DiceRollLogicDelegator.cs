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
    internal (int grade, int crits) RollGameplayDice(string tradition, int skill)
    {
        if (tradition == GameplayLore.Tradition.Martial)
        {
            return Roll20ForSkill(skill);
        }
        else
        {
            return Roll100ForSkill(skill);
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
    private (int grades, int crits) Roll20ForSkill(int skill)
    {
        int diceRoll = Roll20withReroll();
        var grade = 1 + diceRoll / 4;
        var crit = grade / 5 - 1;
        var crits = crit > 0 ? crit : 0;
        var grades = grade * skill * 5 / 100;

        return (grades, crits);
    }

    private (int grades, int crits) Roll100ForSkill(int skill)
    {
        int diceRoll = Roll100withReroll();
        var grade = 1 + diceRoll / 20;
        var crit = grade / 5 - 1;
        var crits = crit > 0 ? crit : 0;
        var grades = grade * skill * 5 / 100;

        return (grades, crits);
    }
    #endregion
}
