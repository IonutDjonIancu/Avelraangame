namespace Service_Delegators;

public interface IDiceCustomRollsLogic
{
    int Roll1dN(int upperLimit);
    int RollNdN(int lowerLimit, int upperLimit);
    bool RollTrueFalse();
}

public class DiceCustomRollsLogic : IDiceCustomRollsLogic
{
    private readonly Random random;

    public DiceCustomRollsLogic()
    {
        random = new();
    }

    public bool RollTrueFalse()
    {
        return random.Next(1, 3) == 1;
    }
    public int Roll1dN(int upperLimit)
    {
        return random.Next(1, upperLimit + 1);
    }
    public int RollNdN(int lowerLimit, int upperLimit)
    {
        return random.Next(lowerLimit, upperLimit + 1);
    }
}
