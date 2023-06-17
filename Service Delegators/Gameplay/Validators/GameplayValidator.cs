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

    internal void ValidatePartyBeforeJoin(CharacterIdentity charIdentity)
    {
        ValidateCharacterPlayerCombination(charIdentity);
        IsCharacterInAnotherParty(charIdentity);
    }

    internal void ValidatePartyOnLeave(string partyId, CharacterIdentity charIdentity)
    {
        if (!snapshot.Parties.Exists(p => p.Identity.Id == partyId)) Throw("Party with specified id does not exist.");

        ValidateCharacterPlayerCombination(charIdentity);

        var party = snapshot.Parties.FirstOrDefault(p => p.Identity.Id == partyId)!;
        
        if (!party.Characters.Select(s => s.Id).Contains(charIdentity.Id)) Throw("This party does not have this character.");
        if (party.IsAdventuring) Throw("Cannot abandon party during adventuring.");
    }

    #region private methods
    private void IsCharacterInAnotherParty(CharacterIdentity charIdentity)
    {
        var isCharInAnotherParty = snapshot.Parties
            .SelectMany(p => p.Characters)
            .Select(c => c.Id)
            .Contains(charIdentity.Id);
        if (isCharInAnotherParty) Throw("Character is already in a party.");
    }
    #endregion
}
