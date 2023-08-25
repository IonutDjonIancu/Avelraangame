using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;

namespace Service_Delegators;

public interface IDiceLogicDelegator
{
    int Roll_d20_noReroll();
    int Roll_d20_withReroll();

    int Roll_d100_noReroll();
    int Roll_d100_withReroll();

    bool Roll_true_false();
    int Roll_1_to_n(int upperLimit);
    int Roll_n_to_n(int lowerLimit, int upperLimit);

    int Roll_character_gameplay_dice(bool isOffense, string attribute, Character character);
}

public class DiceLogicDelegator : IDiceLogicDelegator
{
    private readonly IDiceD20Logic d20RollsLogic;
    private readonly IDiceD100Logic d100RollsLogic;
    private readonly IDiceCustomRollsLogic customRollsLogic;

    public DiceLogicDelegator(
        IDiceD20Logic d20RollsLogic,
        IDiceD100Logic d100RollsLogic,
        IDiceCustomRollsLogic customRollsLogic)
    {
        this.d20RollsLogic = d20RollsLogic;
        this.d100RollsLogic = d100RollsLogic;
        this.customRollsLogic = customRollsLogic;
    }

    #region d20Rolls
    public int Roll_d20_noReroll()
    {
        return d20RollsLogic.RollD20noReroll();
    }

    public int Roll_d20_withReroll()
    {
        return d20RollsLogic.RollD20withReroll();
    }
    #endregion

    #region d100Rolls
    public int Roll_d100_noReroll()
    {
        return d100RollsLogic.RollD100noReroll();
    }

    public int Roll_d100_withReroll()
    {
        return d100RollsLogic.RollD100withReroll();
    }
    #endregion

    #region customRolls
    public int Roll_1_to_n(int upperLimit)
    {
        return customRollsLogic.Roll1dN(upperLimit);
    }

    public int Roll_n_to_n(int lowerLimit, int upperLimit)
    {
        return customRollsLogic.RollNdN(lowerLimit, upperLimit);
    }

    public bool Roll_true_false()
    {
        return customRollsLogic.RollTrueFalse();
    }
    public int Roll_character_gameplay_dice(bool isOffense, string attribute, Character character)
    {
        return customRollsLogic.RollGameplayDice(isOffense, attribute, character);
    }
    #endregion
}
