using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class DiceRollValidator : ValidatorBase
{
    private readonly SnapshotOld snapshot;

    internal DiceRollValidator(SnapshotOld snapshot)
        : base(snapshot)
    {
        this.snapshot = snapshot;
    }

    
}
