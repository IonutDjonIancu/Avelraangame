using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayLogicDelegator
{
    private readonly IDiceRollService diceService;
    private readonly IDatabaseService dbs;

    private GameplayLogicDelegator() { }
    internal GameplayLogicDelegator(
        IDatabaseService databaseService,
        IDiceRollService diceRollService)
    {
        dbs = databaseService;
        diceService = diceRollService;
    }

    internal Party CreateParty()
    {
        SanitizePartiesOnCreate();

        var party = new Party
        {
            Id = Guid.NewGuid().ToString(),
            CreationDate = DateTime.Now.ToShortDateString(),
            IsAdventuring = false
        };

        dbs.Snapshot.Parties.Add(party);

        dbs.PersistDatabase();

        return party;
    }

    internal Party LeaveParty(string partyId, CharacterIdentity charIdentity)
    {
        var party = dbs.Snapshot.Parties.Find(s => s.Id == partyId)!;
        var character = dbs.Snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == charIdentity.Id)!;

        var member = party.PartyMembers.Find(s => s.PlayerId == charIdentity.PlayerId)!;
        member.CharacterIds.Remove(charIdentity.Id);

        if (member.CharacterIds.Count == 0)
        {
            party.PartyMembers.Remove(member);
        }

        if (party.PartyLeadId == charIdentity.Id)
        {
            if (party.PartyMembers.Count > 0)
            {
                party.PartyLeadId = party.PartyMembers.First().CharacterIds.First();
            }
            else
            {
                party.PartyLeadId = string.Empty;
            }
        }
        
        character.Info.IsInParty = false;

        dbs.PersistDatabase();
        dbs.PersistPlayer(charIdentity.PlayerId);

        return party;
    }

    internal Party JoinParty(string partyId, CharacterIdentity charIdentity)
    {
        var party = dbs.Snapshot.Parties.FirstOrDefault(s => s.Id == partyId)!;
        var character = dbs.Snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == charIdentity.Id)!;
            
        if (string.IsNullOrWhiteSpace(party.PartyLeadId))
        {
            party.PartyLeadId = charIdentity.Id;
        }
        // we still add the character to the party list of characters irrespective if he is the party lead or not

        if (party.PartyMembers.Select(s => s.PlayerId).Contains(charIdentity.PlayerId))
        {
            party.PartyMembers.First(s => s.PlayerId == charIdentity.PlayerId).CharacterIds.Add(charIdentity.Id);
        }
        else
        {
            var newMember = new PartyMember
            {
                PlayerId = charIdentity.PlayerId,
            };
            newMember.CharacterIds.Add(charIdentity.Id);

            party.PartyMembers.Add(newMember);
        }
        
        character.Info.IsInParty = true;

        dbs.PersistPlayer(charIdentity.PlayerId);
        dbs.PersistDatabase();

        return party;
    }

    public Warparty CreateWarparty(string partyId)
    {
        //var partyChars = dbs.Snapshot.Parties.Find(s => s.Id == partyId)!.CharacterIds;

        // we need dictionary of playerId and characterId
        throw new NotImplementedException();
    }

    #region private methods
    private void SanitizePartiesOnCreate()
    {
        var sevenDaysAgo = DateTime.Now - DateTime.Now.AddDays(-7);

        var oldParties = dbs.Snapshot.Parties.Where(
            s => DateTime.Parse(s.CreationDate) < DateTime.Now - sevenDaysAgo 
            && !s.IsAdventuring
            && s.PartyMembers.Count == 0).ToList();
        if (oldParties.Count == 0) return;

        oldParties.ForEach(s => dbs.Snapshot.Parties.Remove(s));
    }
    #endregion
}
