using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayLogicDelegator
{
    private readonly GameplayQuestsLogic questLogic;

    private GameplayLogicDelegator() { }
    internal GameplayLogicDelegator(
        IDatabaseService databaseService,
        IDiceRollService diceRollService,
        IItemService itemService,
        INpcService npcService)
    {
        questLogic = new GameplayQuestsLogic(databaseService, diceRollService, itemService, npcService);
    }

    internal Location GenerateLocation(Position position)
    {
        return questLogic.GenerateLocation(position);
    }
}
