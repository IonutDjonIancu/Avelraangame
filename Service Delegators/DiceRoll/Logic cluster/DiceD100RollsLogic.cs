namespace Service_Delegators;

public interface IDiceD100Logic
{
    int RollD100noReroll();
    int RollD100withReroll();
}

public class DiceD100RollsLogic : IDiceD100Logic
{
    private readonly Random random;

    public DiceD100RollsLogic()
    {
        random = new();
    }

    public int RollD100noReroll()
    {
        return random.Next(1, 101);
    }

    public int RollD100withReroll()
    {
        var totalRoll = random.Next(1, 101);

        if (totalRoll > 95)
        {
            totalRoll += RollD100noReroll();
        }

        return totalRoll;
    }
}
