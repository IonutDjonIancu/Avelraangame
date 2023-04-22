using Service_Delegators;

namespace Avelraangame.Factories;

public interface IServiceFactory
{
    IDiceRollService DicerollService { get; init; }
    IPlayerService PlayerService { get; init; }
    IItemService ItemService { get; init; }
    ICharacterService CharacterService { get; init; }
    INpcService NpcService { get; init; }
}