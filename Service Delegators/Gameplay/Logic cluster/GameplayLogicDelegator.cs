using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayLogicDelegator
{
    private readonly GameplayQuestsLogic questLogic;
    private readonly GameplayTravelLogic travelLogic;

    private GameplayLogicDelegator() { }
    internal GameplayLogicDelegator(
        IDatabaseService databaseService,
        IDiceRollService diceRollService,
        IItemService itemService,
        INpcService npcService)
    {
        questLogic = new GameplayQuestsLogic(databaseService, diceRollService, itemService, npcService);
        travelLogic = new GameplayTravelLogic(databaseService);
    }

    internal Location GenerateLocation(Position position)
    {
        return questLogic.GenerateLocation(position);
    }

    internal void MoveToLocation(PositionTravel positionTravel)
    {
        travelLogic.GoToLocation(positionTravel);
    }
}
