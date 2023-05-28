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

    internal void ValidatePartyBeforeJoin(string partyId, CharacterIdentity charIdentity)
    {
        if (!snapshot.Parties.Exists(s => s.Id == partyId)) Throw("Party with specified id does not exist.");
    
        ValidateCharacterPlayerCombination(charIdentity);

        var party = snapshot.Parties.FirstOrDefault(s => s.Id == partyId)!;

        if (party.CharacterIds.Contains(charIdentity.Id)) Throw("Character is already in this party.");
    }
}
