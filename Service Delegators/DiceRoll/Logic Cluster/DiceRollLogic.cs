using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class DiceRollLogic
{
    private readonly Random random;
    
    internal DiceRollLogic()
    {
        random = new Random();
    }

    internal int Roll1d20NoReroll()
    {
        return random.Next(1, 21);
    }

    internal int Roll1d20WithReroll(int roll = 0) 
    {
        var handRoll = random.Next(1, 21);
        var totalRoll = roll + handRoll;

        if (handRoll % 20 == 0)
        {
            totalRoll = Roll1d20WithReroll(totalRoll);
        }

        return totalRoll;
    }

    internal int Roll1d100WithReroll(int roll = 0)
    {
        var handRoll = random.Next(1, 101);
        var totalRoll = roll + handRoll;

        if (handRoll > 95)
        {
            totalRoll = Roll1d100WithReroll(totalRoll);
        }

        return totalRoll;
    }

    internal int Roll1d100NoReroll()
    {
        return random.Next(1, 101);
    }

    internal int Roll1dNnoReroll(int upperLimit)
    {
        return random.Next(1, upperLimit + 1);
    }

    internal DiceRoll GenerateRollFor(string tradition, int bonus = 0)
    {
        var dice = new List<int>();

        int roll;
        if      (IsDanarian(tradition))     roll = Rolld20WithList(dice, bonus);
        else if (IsCalvinian(tradition))    roll = Rolld100WithList(dice, bonus);
        else  /*(none)*/                    roll = Rolld20WithList(dice, bonus);

        return new DiceRoll
        {
            Roll = roll,
            Grade = CalculateGradeFor(tradition, roll),
            Dice = dice,
            Crits = CalculateCritsFor(tradition, dice)
        };
    }

    #region private methods
    private int Rolld20WithList(List<int> dice, int bonus = 0)
    {
        var handRoll = random.Next(1, 21);
        dice.Add(handRoll);
        var totalRoll = bonus + handRoll;

        if (handRoll % 20 == 0)
        {
            totalRoll = Rolld20WithList(dice, totalRoll);
        }

        return totalRoll;
    }

    private int Rolld100WithList(List<int> dice, int bonus = 0)
    {
        var handRoll = random.Next(1, 101);
        dice.Add(handRoll);
        var roll = bonus + handRoll;

        if (handRoll > 95)
        {
            roll = Rolld100WithList(dice, roll);
        }

        return roll;
    }

    private static int CalculateGradeFor(string tradition, int roll)
    {
        if      (IsDanarian(tradition))     return (int)Math.Ceiling(roll / 4.00M);
        else if (IsCalvinian(tradition))    return (int)Math.Ceiling(roll / 20.00M);
        else  /*(none)*/                    return (int)Math.Ceiling(roll / 4.00M);
    }

    private static int CalculateCritsFor(string tradition, List<int> dice)
    {
        if      (IsDanarian(tradition))     return CritsByDiceCount(dice);
        else if (IsCalvinian(tradition))    return CritsByDiceCount(dice);
        else  /*(none)*/                    return CritsByDiceCount(dice);

        // other tradition styles influenced by crits from 19 or 18 will return a greater number of crits dice

    }

    private static int CritsByDiceCount(List<int> dice)
    {
        return dice.Count - 1;
    }

    private static bool IsDanarian(string tradition)
    {
        return tradition == CharactersLore.Traditions.Ravanon;
    }

    private static bool IsCalvinian(string tradition)
    {
        return tradition == CharactersLore.Traditions.Endarii;
    }
    #endregion
}
