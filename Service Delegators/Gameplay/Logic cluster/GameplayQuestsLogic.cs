using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayQuestsLogic
{
    private readonly IDatabaseService dbs;
    private readonly IDiceRollService diceService;

    private GameplayQuestsLogic() { }
    internal GameplayQuestsLogic(
        IDatabaseService databaseService,
        IDiceRollService diceRollService)
    {
        dbs = databaseService;
        diceService = diceRollService;
    }

    internal List<Quest> GetQuestsAtPartyLocation(string partyId)
    {
        var region = dbs.Snapshot.Parties.Find(p => p.Identity.Id == partyId)!.Position.Region;
        var subregion = dbs.Snapshot.Parties.Find(p => p.Identity.Id == partyId)!.Position.Subregion;
        var land = dbs.Snapshot.Parties.Find(p => p.Identity.Id == partyId)!.Position.Land;
        var location = dbs.Snapshot.Parties.Find(p => p.Identity.Id == partyId)!.Position.Location;

        var position = new Position()
        {
            Region = region,
            Subregion = subregion,
            Land = land,
            Location = location
        };

        return QuestsLore.All.Where(s => s.Position == position).ToList();
    }

    internal Quest BeginQuestForParty(string partyId, string questName)
    {
        var questData = QuestsLore.All.Find(s => s.Name == questName)!;
        var party = dbs.Snapshot.Parties.Find(s => s.Identity.Id == partyId)!;

        // quest logic
        var quest = new Quest
        {
            Id = Guid.NewGuid().ToString(),
            PartyId = party.Identity.Id,
            IsInEncounter = false,
            Name = questData.Name,
            Description = questData.Description,
            Difficulty = questData.Difficulty,
            Effort = questData.Effort,
            Ratio = questData.Ratio,
            Position = questData.Position,
            IsRepeatable = questData.IsRepeatable
        };

        GenerateEncounters(quest);
        GenerateReward(quest);

        // party logic
        party.Status.QuestId = quest.Id;
        foreach (var charIdentity in party.Characters)
        {
            var player = dbs.Snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!;
            var character = player.Characters.Find(s => s.Identity.Id == charIdentity.Id)!;

            character.Status.HasAttributesLocked = true;
            dbs.PersistPlayer(charIdentity.PlayerId);
        }

        dbs.PersistDatabase();

        return quest;
    }

    #region private methods
    private void GenerateEncounters(Quest quest)
    {
        var party = dbs.Snapshot.Parties.Find(s => s.Identity.Id == quest.PartyId)!;

        var encounter = new QuestEncounter()
        {
            Id = Guid.NewGuid().ToString(),
            PartyLeadId = party.Identity.PartyLeadId,
        };

        var diploRoll = diceService.Roll_d100();
        var utilRoll = diceService.Roll_d100();
        var overRoll = diceService.Roll_d100();
        if      (diploRoll <= quest.Ratio.Diplomacy)    encounter.Type = GameplayLore.Quests.Encounters.Types.Diplomacy;
        else if (utilRoll <= quest.Ratio.Utilitarian)   encounter.Type = GameplayLore.Quests.Encounters.Types.Utilitarian;
        else if (overRoll <= quest.Ratio.Overcome)      encounter.Type = GameplayLore.Quests.Encounters.Types.Overcome;
        else  /*(overRoll <= quest.Ratio.Fight)*/       encounter.Type = GameplayLore.Quests.Encounters.Types.Fight;

        var diploChoicesCount = GameplayLore.Quests.Encounters.Diplomacy.Choices.All.Count;
        var choiceIndex = diceService.Roll_1dX(diploChoicesCount) - 1;
        encounter.DiplomacyChoice = GameplayLore.Quests.Encounters.Diplomacy.Choices.All[choiceIndex];

        var diploPassesCount = GameplayLore.Quests.Encounters.Diplomacy.Passes.All.Count;
        var passIndex = diceService.Roll_1dX(diploPassesCount) - 1;
        encounter.DiplomacyPass = GameplayLore.Quests.Encounters.Diplomacy.Passes.All[passIndex];




    }

    private void GenerateReward(Quest quest)
    {
        throw new NotImplementedException();
    }
    #endregion
}
