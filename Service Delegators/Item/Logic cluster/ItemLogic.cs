namespace Service_Delegators;

public class ItemLogic : IItemLogic
{
    private readonly IDiceDelegator dice;

    public ItemLogic(IDiceDelegator dice)
    {
        this.dice = dice;
    }


    public int DoSomeRoll()
    {
        return dice.Roll_d20_noReroll();
    }

}
