using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class GameplayService : IGameplayService
{
    private readonly GameplayValidator validator;
    private readonly GameplayLogicDelegator logic;

    public GameplayService(
        IDatabaseService databaseService,
        IDiceRollService diceRollService)
    {
        validator = new GameplayValidator(databaseService.Snapshot);
        logic = new GameplayLogicDelegator(databaseService, diceRollService);
    }

    public Party CreateParty(Position position, bool isSinglePlayerOnly = false)
    {
        validator.ValidatePartyOnCreate(position);

        return logic.CreateParty(position, isSinglePlayerOnly);
    }

    public Party JoinParty(string partyId, CharacterIdentity charIdentity, bool isSinglePlayerOnly = false)
    {
        validator.ValidatePartyBeforeJoin(partyId, charIdentity);
        return logic.JoinParty(partyId, charIdentity, isSinglePlayerOnly);
    }

    public Party LeaveParty(string partyId, CharacterIdentity charIdentity)
    {
        validator.ValidatePartyOnLeave(partyId, charIdentity);
        return logic.LeaveParty(partyId, charIdentity);
    }

    public List<Quest> GetQuests(string partyId)
    {
        validator.ValidateParty(partyId);

        return logic.GetQuestsAtPartyLocation(partyId);
    }

    public Quest ChooseQuest(string partyId, string questName)
    {
        validator.ValidateQuestOnBegin(partyId, questName);

        return logic.BeginQuestForParty(partyId, questName);
    }
}
