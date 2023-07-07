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

    #region private methods
    private void ValidateIfCharacterIsInAnotherParty(CharacterIdentity charIdentity)
    {
        var isCharInAnotherParty = snapshot.Parties
            .SelectMany(p => p.Characters)
            .Select(c => c.Id)
            .Contains(charIdentity.Id);
        if (isCharInAnotherParty) throw new Exception("Character is already in a party.");
    }

    
    #endregion
}
