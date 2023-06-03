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

        var isCharacterInAnotherParty = 
            snapshot.Parties.SelectMany(s => s.CharacterIds).ToList().Contains(charIdentity.Id)
            || snapshot.Parties.Select(s => s.PartyLeadId).ToList().Contains(charIdentity.Id);
        if (isCharacterInAnotherParty) Throw("Character is already in another party.");
    }

    internal void ValidatePartyOnLeave(string partyId, CharacterIdentity charIdentity)
    {
        if (!snapshot.Parties.Exists(s => s.Id == partyId)) Throw("Party with specified id does not exist.");

        ValidateCharacterPlayerCombination(charIdentity);

        var isCharInParty = snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!.Characters!.Find(s => s.Identity.Id == charIdentity.Id)!.Info.IsInParty;
        if (!isCharInParty) Throw("Character is in no party.");

        var party = snapshot.Parties.FirstOrDefault(s => s.Id == partyId)!;
        if (party.IsAdventuring) Throw("Unable to abandon party during adventuring.");
        if (!party.CharacterIds.Contains(charIdentity.Id)) Throw("This party does not have this character.");
    }
}
