using Data_Mapping_Containers.Pocos;

namespace Service_Delegators;

public class ItemDelegator : IItemDelegator
{
    private Snapshot snapshot;

    private IItemLogic _logic;

    public ItemDelegator(
        Snapshot snapshot,
        IItemLogic itemLogic)
    {
        this.snapshot = snapshot;
        _logic = itemLogic;
    }

    public int DoSomeRoll()
    {
        return _logic.DoSomeRoll();
    }
}
