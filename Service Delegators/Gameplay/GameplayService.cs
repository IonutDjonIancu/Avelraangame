using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class GameplayService : IGameplayService
{
    private readonly GameplayValidator validator;
    private readonly GameplayLogicDelegator logic;

    public GameplayService(IDatabaseService databaseService)
    {
        validator = new GameplayValidator(databaseService.Snapshot);
        logic = new GameplayLogicDelegator(databaseService);
    }

    public Party CreateParty(bool isSinglePlayerOnly)
    {
        return logic.CreateParty(isSinglePlayerOnly);
    }

    public Party JoinParty(string partyId, bool isSinglePlayerOnly, CharacterIdentity charIdentity)
    {
        validator.ValidatePartyBeforeJoin(partyId, charIdentity);
        return logic.JoinParty(partyId, isSinglePlayerOnly, charIdentity);
    }

    public Party LeaveParty(string partyId, CharacterIdentity charIdentity)
    {
        validator.ValidatePartyOnLeave(partyId, charIdentity);
        return logic.LeaveParty(partyId, charIdentity);
    }

    public Quest BeginQuest(string partyId, string questName, CharacterIdentity charIdentity)
    {
        validator.ValidateQuestOnBegin(partyId, questName, charIdentity);

        return new Quest();
    }
}
