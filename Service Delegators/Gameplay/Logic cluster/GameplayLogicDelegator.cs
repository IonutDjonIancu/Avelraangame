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
            Id = Guid.NewGuid().ToString(),
            CreationDate = DateTime.Now.ToShortDateString(),
            IsAdventuring = false
        };

        dbs.Snapshot.Parties.Add(party);

        dbs.PersistDatabase();

        return party;
    }

    internal Party JoinParty(string partyId, CharacterIdentity charIdentity)
    {
        var party = dbs.Snapshot.Parties.Find(s => s.Id == partyId);
        var character = dbs.Snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == charIdentity.Id)!;

        party ??= CreateParty();
        var paperdoll = charService.CalculatePaperdoll(character);
        party.PartyMembers.Add(paperdoll);
            
        if (string.IsNullOrWhiteSpace(party.PartyLeadId))
        {
            party.PartyLeadId = charIdentity.Id;
        }
        
        character.Info.IsInParty = true;

        dbs.PersistPlayer(charIdentity.PlayerId);
        dbs.PersistDatabase();

        return party;
    }

    internal Party LeaveParty(string partyId, CharacterIdentity charIdentity)
    {
        var party = dbs.Snapshot.Parties.Find(s => s.Id == partyId)!;
        var character = dbs.Snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == charIdentity.Id)!;

        var member = party.PartyMembers.Find(s => s.CharacterId == charIdentity.Id)!;

        character.LevelUp.StatPoints += member.LevelUp.StatPoints;
        character.LevelUp.SkillPoints += member.LevelUp.SkillPoints;
        character.LevelUp.DeedsPoints += member.LevelUp.DeedsPoints;

        if (party.PartyLeadId == charIdentity.Id)
        {
            party.Loot.ForEach(s => character.Supplies.Add(s));
            party.Loot.Clear();

            if (party.PartyMembers.Count > 0)
            {
                party.PartyLeadId = party.PartyMembers.First().CharacterId!;
            }
            else
            {
                party.PartyLeadId = string.Empty;
            }
        }
        party.PartyMembers.Remove(member);
        character.Info.IsInParty = false;

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
            && s.PartyMembers.Count == 0).ToList();
        if (oldParties.Count == 0) return;

        oldParties.ForEach(s => dbs.Snapshot.Parties.Remove(s));
    }
    #endregion
}
