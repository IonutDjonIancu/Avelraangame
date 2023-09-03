namespace Service_Delegators;

public interface IDiceD20Logic
{
    int RollD20noReroll();
    int RollD20withReroll(int roll = 0);
}

public class DiceD20RollsLogic : IDiceD20Logic

{
    private readonly Random random;

    public DiceD20RollsLogic()
    {
        random = new();
    }

    public int RollD20noReroll()
    {
        return random.Next(1, 21);
    }

    public int RollD20withReroll(int roll = 0)
    {
        var handRoll = random.Next(1, 21);
        var totalRoll = roll + handRoll;

        if (handRoll % 20 == 0)
        {
            totalRoll = RollD20withReroll(totalRoll);
        }

        return totalRoll;
    }
}
