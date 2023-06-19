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

    public Party CreateParty()
    {
        return logic.CreateParty();
    }

    public Party JoinParty(string partyId, CharacterIdentity charIdentity)
    {
        validator.ValidatePartyBeforeJoin(charIdentity);
        return logic.JoinParty(partyId, charIdentity);
    }

    public Party LeaveParty(string partyId, CharacterIdentity charIdentity)
    {
        validator.ValidatePartyOnLeave(partyId, charIdentity);
        return logic.LeaveParty(partyId, charIdentity);
    }
}
