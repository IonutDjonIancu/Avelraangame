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

        var isCharInAnotherParty = snapshot.Parties
            .SelectMany(s => s.PartyMembers)
            .Select(s => s.CharacterId)
            .Contains(charIdentity.Id);
        if (isCharInAnotherParty) Throw("Character is already in a party.");
    }

    internal void ValidatePartyOnLeave(string partyId, CharacterIdentity charIdentity)
    {
        if (!snapshot.Parties.Exists(s => s.Id == partyId)) Throw("Party with specified id does not exist.");

        ValidateCharacterPlayerCombination(charIdentity);

        var isCharInAnotherParty = snapshot.Parties
           .Where(s => s.Id != partyId)
           .SelectMany(s => s.PartyMembers)
           .Select(s => s.CharacterId)
           .Contains(charIdentity.Id);
        if (isCharInAnotherParty) Throw("Character is in a different party.");

        var party = snapshot.Parties.FirstOrDefault(s => s.Id == partyId)!;
        if (party.IsAdventuring) Throw("Cannot abandon party during adventuring.");

        if (!party.PartyMembers.Select(s => s.CharacterId).Contains(charIdentity.Id)) Throw("This party does not have this character.");
    }

    internal void ValidateWarpartyOnCreate(string partyId)
    {
        var party = snapshot.Parties.Find(s => s.Id == partyId);

        if (party is null) Throw("No party was found with supplied id.");
        if (party!.IsAdventuring) Throw("Party already adventuring.");

        throw new NotImplementedException();
    }
}
