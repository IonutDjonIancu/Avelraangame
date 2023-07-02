using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayPartyLogic
{
    private readonly IDatabaseService dbs;

    private GameplayPartyLogic() { }
    internal GameplayPartyLogic(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

    internal Party CreateParty(Position position, bool isSinglePlayerOnly = false)
    {
        SanitizePartiesOnCreate();

        var party = new Party
        {
            Identity = new PartyIdentity()
            {
                Id = Guid.NewGuid().ToString(),
                PartyLeadId = string.Empty
            },
            CreationDate = DateTime.Now.ToShortDateString(),
            IsSinglePlayerOnly = isSinglePlayerOnly,

            Position = position,

            Wealth = 0,
            Food = 1
        };

        dbs.Snapshot.Parties.Add(party);

        dbs.PersistDatabase();

        return party;
    }

    internal Party JoinParty(string partyId, CharacterIdentity charIdentity, bool isSinglePlayerOnly = false)
    {
        var character = dbs.Snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == charIdentity.Id)!;
        var party = dbs.Snapshot.Parties.Find(s => s.Identity.Id == partyId) ?? CreateParty(character.Position, isSinglePlayerOnly);

        party.Characters.Add(charIdentity);

        if (string.IsNullOrWhiteSpace(party.Identity.PartyLeadId))
        {
            party.Identity.PartyLeadId = charIdentity.Id;
        }

        character.Status.IsInParty = true;
        character.Status.PartyId = partyId;

        dbs.PersistPlayer(charIdentity.PlayerId);
        dbs.PersistDatabase();

        return party;
    }

    internal Party LeaveParty(string partyId, CharacterIdentity charIdentity)
    {
        var party = dbs.Snapshot.Parties.Find(s => s.Identity.Id == partyId)!;
        var character = dbs.Snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == charIdentity.Id)!;

        party.Characters.Remove(charIdentity);
        character.Status.IsInParty = false;
        character.Status.PartyId = string.Empty;

        if (party.Characters.Count > 0)
        {
            party.Identity.PartyLeadId = party.Characters.First().Id!;
        }
        else
        {
            dbs.Snapshot.Parties.Remove(party);
        }

        dbs.PersistDatabase();
        dbs.PersistPlayer(charIdentity.PlayerId);

        return party;
    }


    #region private methods
    private void SanitizePartiesOnCreate()
    {
        var sevenDaysAgo = DateTime.Now - DateTime.Now.AddDays(-7);

        var oldParties = dbs.Snapshot.Parties.Where(
            s => DateTime.Parse(s.CreationDate) < DateTime.Now - sevenDaysAgo
            && string.IsNullOrEmpty(s.Status.QuestId)
            && s.Characters.Count == 0).ToList();
        if (oldParties.Count == 0) return;

        oldParties.ForEach(s => dbs.Snapshot.Parties.Remove(s));
    }
    #endregion


}
