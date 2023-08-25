namespace Service_Delegators;

public class ItemLogic : IItemLogic
{
    private readonly IDiceLogicDelegator dice;

    public ItemLogic(IDiceLogicDelegator dice)
    {
        this.dice = dice;
    }


    public int DoSomeRoll()
    {
        return dice.Roll_d20_noReroll();
    }

}
