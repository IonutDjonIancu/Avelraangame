namespace Service_Delegators;

public class DiceRollService : IDiceRollService
{
    private readonly DiceRollValidator validator;
    private readonly DiceRollLogicDelegator logic;

    public DiceRollService(IDatabaseService databaseService)
    {
        validator = new DiceRollValidator(databaseService.Snapshot);
        logic = new DiceRollLogicDelegator();
    }

    public int Roll_20_noReroll()
    {
        return logic.Roll20noReroll();
    }
    public int Roll_20_withReroll()
    {
        return logic.Roll20withReroll();
    }
    public int Roll_100_noReroll()
    {
        return logic.Roll100noReroll();
    }
    public int Roll_100_withReroll()
    {
        return logic.Roll100withReroll();
    }
    public (int grade, int crits) Roll_gameplay_dice(string tradition, int skill)
    {
        validator.ValidateTradition(tradition);
        return logic.RollGameplayDice(tradition, skill);
    }
    public bool Roll_par_impar()
    {
        return logic.RollParImpar();
    }
    public int Roll_1_to_n(int upperLimit)
    {
        return logic.Roll1ToN(upperLimit);
    }
    public int Roll_n_to_n(int lowerLimit, int upperLimit)
    {
        return logic.RollNToN(lowerLimit, upperLimit);
    }
}
