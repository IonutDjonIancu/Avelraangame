using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class DiceRollValidator : ValidatorBase
{
    private readonly Snapshot snapshot;

    internal DiceRollValidator(Snapshot snapshot)
        : base(snapshot)
    {
        this.snapshot = snapshot;
    }

    
}
