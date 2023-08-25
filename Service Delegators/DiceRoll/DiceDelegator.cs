using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;
using Persistance_Manager;

namespace Service_Delegators;

public class DiceDelegator : IDiceDelegator
{
    private Snapshot snapshot;

    private readonly IDiceD20Logic d20RollsLogic;
    private readonly IDiceD100Logic d100RollsLogic;
    private readonly IDiceCustomRollsLogic customRollsLogic;

    public DiceDelegator(
        Snapshot snapshot,
        IPersistenceService persistence,
        IDiceD20Logic d20RollsLogic,
        IDiceD100Logic d100RollsLogic,
        IDiceCustomRollsLogic customRollsLogic)
    {
        this.snapshot = snapshot;
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
    #endregion



    public int Roll_character_gameplay_dice(bool isOffense, string attribute, Character character)
    {
        throw new NotImplementedException();
    }
}
