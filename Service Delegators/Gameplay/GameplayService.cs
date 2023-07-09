using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class GameplayService : IGameplayService
{
    private readonly GameplayValidator validator;
    private readonly GameplayLogicDelegator logic;

    public GameplayService(
        IDatabaseService databaseService,
        IDiceRollService diceRollService,
        IItemService itemService,
        INpcService npcService)
    {
        validator = new GameplayValidator(databaseService.Snapshot);
        logic = new GameplayLogicDelegator(databaseService, diceRollService, itemService, npcService);
    }

    public Location GetLocation(Position position)
    {
        validator.ValidatePosition(position);
        return logic.GenerateLocation(position);
    }

    public void TravelToLocation(PositionTravel positionTravel)
    {
        validator.ValidateBeforeTravel(positionTravel);

        logic.MoveToLocation(positionTravel);
    }

}
