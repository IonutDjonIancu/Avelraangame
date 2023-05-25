using Service_Delegators;

namespace Avelraangame.Factories;

public interface IServiceFactory
{
    IDatabaseService DatabaseService { get; set; } // must remain set instead of init to modify the cache(snapshot)
    IMetadataService Metadata { get; set; }
    IDiceRollService DicerollService { get; init; }
    IPlayerService PlayerService { get; init; }
    IItemService ItemService { get; init; }
    ICharacterService CharacterService { get; init; }
    INpcService NpcService { get; init; }
    IGameplayService GameplayService { get; init; }
}