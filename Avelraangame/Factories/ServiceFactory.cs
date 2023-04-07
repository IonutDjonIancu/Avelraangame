using Persistance_Manager;
using Service_Delegators;

namespace Avelraangame.Factories;

public class ServiceFactory : IServiceFactory
{
    private readonly IDatabaseManager dbm;
    public IDiceRollService DicerollService { get; init; }
    public IPlayerService PlayerService { get; init; }
    public IItemService ItemService { get; init; }
    public ICharacterService CharacterService { get; init; }

    private ServiceFactory() { }

    public ServiceFactory(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
        DicerollService = new DiceRollService();
        PlayerService = new PlayerService(dbm);
        ItemService = new ItemService(DicerollService);
        CharacterService = new CharacterService(dbm, DicerollService, ItemService);
    }
}
