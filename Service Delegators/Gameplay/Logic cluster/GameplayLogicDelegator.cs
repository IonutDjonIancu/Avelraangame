using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayLogicDelegator
{
    private readonly IDatabaseService dbs;
    private readonly IDiceRollService diceService;
    private readonly ICharacterService charService;

    private GameplayLogicDelegator() { }
    internal GameplayLogicDelegator(
        IDatabaseService databaseService,
        IDiceRollService diceRollService,
        ICharacterService characterService)
    {
        dbs = databaseService;
        diceService = diceRollService;
        charService = characterService;
    }

    internal Party CreateParty()
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
            IsAdventuring = false
        };

        dbs.Snapshot.Parties.Add(party);

        dbs.PersistDatabase();

        return party;
    }

    internal Party JoinParty(string partyId, CharacterIdentity charIdentity)
    {
        var party = dbs.Snapshot.Parties.Find(s => s.Identity.Id == partyId);
        var character = dbs.Snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == charIdentity.Id)!;

        party ??= CreateParty();
        party.Characters.Add(charIdentity);
            
        if (string.IsNullOrWhiteSpace(party.Identity.PartyLeadId))
        {
            party.Identity.PartyLeadId = charIdentity.Id;
        }
        
        character.Info.IsInParty = true;

        dbs.PersistPlayer(charIdentity.PlayerId);
        dbs.PersistDatabase();

        return party;
    }

    internal Party LeaveParty(string partyId, CharacterIdentity charIdentity)
    {
        var party = dbs.Snapshot.Parties.Find(s => s.Identity.Id == partyId)!;
        var character = dbs.Snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == charIdentity.Id)!;

        var member = party.Paperdolls.Find(s => s.CharacterId == charIdentity.Id)!;
        if (member != null)
        {
            character.LevelUp.StatPoints += member.LevelUp.StatPoints;
            character.LevelUp.SkillPoints += member.LevelUp.SkillPoints;
            character.LevelUp.DeedsPoints += member.LevelUp.DeedsPoints;
            party.Paperdolls.Remove(member);
        }

        if (party.Identity.PartyLeadId == charIdentity.Id)
        {
            party.Loot.ForEach(s => character.Supplies.Add(s));
            party.Loot.Clear();
            party.Identity.PartyLeadId = string.Empty;
        }

        party.Characters.Remove(charIdentity);
        character.Info.IsInParty = false;

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
            && !s.IsAdventuring
            && s.Characters.Count == 0).ToList();
        if (oldParties.Count == 0) return;

        oldParties.ForEach(s => dbs.Snapshot.Parties.Remove(s));
    }
    #endregion
}
