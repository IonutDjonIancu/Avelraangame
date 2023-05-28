using Service_Delegators;

namespace Tests;

public class TestBase
{
    private static readonly string dbTestPath = "\\Resources\\Database files\\AvelraanTestDb.json";
    private static readonly string dbPlayersPath = "\\Resources\\Database files\\Players";
    private static readonly string logsPath = "\\Resources\\Log files\\Logs.txt";

    private readonly IDatabaseManager dbm;
    protected readonly IDatabaseService dbs;
    protected readonly IDiceRollService diceService;
    protected readonly IPlayerService playerService;
    protected readonly IItemService itemService;
    protected readonly ICharacterService characterService;
    protected readonly INpcService npcService;

    protected readonly Snapshot snapshot;

    protected TestBase()
	{
        var dbmConfig = new DatabaseManagerConfig
        {
            DbPath = dbTestPath,
            LogPath = logsPath,
            DbPlayersPath = dbPlayersPath,
            AvelraanEmail = "example@gmail.com",
            AvelraanPassword = "password",
        };

        dbm = new DatabaseManager(dbmConfig);
        dbs = new DatabaseService(dbm);

        diceService = new DiceRollService();
        playerService = new PlayerService(dbs);
        itemService = new ItemService(dbs, diceService);
        characterService = new CharacterService(dbs, diceService, itemService);
        npcService = new NpcService(dbs, diceService, itemService, characterService);
    }

    protected string CreatePlayer(string playerName)
    {
        snapshot.Players!.Clear();
        playerService.CreatePlayer(playerName);

        return snapshot.Players!.Find(p => p.Identity.Name == playerName)!.Identity.Id;
    }
}
