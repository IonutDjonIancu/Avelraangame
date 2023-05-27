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
        validator = new GameplayValidator(databaseService);
        logic = new GameplayLogicDelegator(databaseService, diceRollService);
    }

    public Party CreateParty()
    {
        return logic.CreateParty();
    }
}
