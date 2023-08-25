using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayValidator : ValidatorBase
{
    private readonly SnapshotOld snapshot;

    internal GameplayValidator(SnapshotOld snapshot)
        : base(snapshot)
    {
        this.snapshot = snapshot;
    }

}
