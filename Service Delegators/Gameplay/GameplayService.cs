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

    public Party CreateParty()
    {
        return logic.CreateParty();
    }

    public Party JoinParty(string partyId, CharacterIdentity charIdentity)
    {
        validator.ValidatePartyBeforeJoin(partyId, charIdentity);
        return logic.JoinParty(partyId, charIdentity);
    }
}
