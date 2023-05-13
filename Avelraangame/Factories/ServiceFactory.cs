using Persistance_Manager;
using Service_Delegators;

namespace Avelraangame.Factories;

public class ServiceFactory : IServiceFactory
{
    public IDatabaseService DatabaseService { get; set; } // must remain set instead of init to modify the cache(snapshot)
    public IDiceRollService DicerollService { get; init; }
    public IPlayerService PlayerService { get; init; }
    public IItemService ItemService { get; init; }
    public ICharacterService CharacterService { get; init; }
    public INpcService NpcService { get; init; }

    private ServiceFactory() { }

    public ServiceFactory(IDatabaseManager databaseManager)
    {
        DatabaseService     = new DatabaseService(databaseManager);
        DicerollService     = new DiceRollService();
        PlayerService       = new PlayerService(DatabaseService);
        ItemService         = new ItemService(DicerollService);
        CharacterService    = new CharacterService(DatabaseService, DicerollService, ItemService);
        NpcService          = new NpcService(DatabaseService, DicerollService, ItemService, CharacterService);
    }
}
