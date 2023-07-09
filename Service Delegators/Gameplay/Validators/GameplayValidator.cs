using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayValidator : ValidatorBase
{
    private readonly Snapshot snapshot;

    internal GameplayValidator(Snapshot snapshot)
        : base(snapshot)
    {
        this.snapshot = snapshot;
    }

}
