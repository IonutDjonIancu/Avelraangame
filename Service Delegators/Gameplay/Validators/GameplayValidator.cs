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
        ValidateCharacterPlayerCombination(charIdentity);
        IsCharacterInAnotherParty(charIdentity);

        var character = snapshot.Players.Find(p => p.Identity.Id == charIdentity.PlayerId)!.Characters.Find(c => c.Identity.Id == charIdentity.Id)!;

        var party = snapshot.Parties.Find(p => p.Identity.Id == partyId);
        if (party != null && party!.Characters.Count > 0)
        {
            if (character.Position.Location != party!.Position.Location) throw new Exception($"Unable to join party. You must first travel to their location, {party.Position.Location}, in order to join.");
        }
    }

    internal void ValidatePartyOnLeave(string partyId, CharacterIdentity charIdentity)
    {
        if (!snapshot.Parties.Exists(p => p.Identity.Id == partyId)) throw new Exception("Party with specified id does not exist.");

        ValidateCharacterPlayerCombination(charIdentity);

        var party = snapshot.Parties.FirstOrDefault(p => p.Identity.Id == partyId)!;
        
        if (!party.Characters.Select(s => s.Id).Contains(charIdentity.Id)) throw new Exception("This party does not have this character.");
        if (party.IsAdventuring) throw new Exception("Cannot abandon party during adventuring.");
    }

    internal void ValidateQuestOnBegin(string partyId, string questName, CharacterIdentity charIdentity)
    {
        if (!QuestsLore.All.Select(s => s.Name).Contains(questName)) throw new Exception("No such quest exists.");

        ValidateCharacterPlayerCombination(charIdentity);

        var party = snapshot.Parties.Find(s => s.Identity.Id == partyId) ?? throw new Exception("No such party was found.");

        if (!string.IsNullOrWhiteSpace(party.QuestId)) throw new Exception();
        {
            // not finished            
        }

        throw new NotImplementedException();
    }

    #region private methods
    private void IsCharacterInAnotherParty(CharacterIdentity charIdentity)
    {
        var isCharInAnotherParty = snapshot.Parties
            .SelectMany(p => p.Characters)
            .Select(c => c.Id)
            .Contains(charIdentity.Id);
        if (isCharInAnotherParty) throw new Exception("Character is already in a party.");
    }
    #endregion
}
