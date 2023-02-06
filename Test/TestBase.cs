using Service_Delegators;

namespace Tests;

public class TestBase
{
    private static readonly string testSecret = "BQxevwF37aNAznk";
    private static readonly string dbTestPath = "\\Resources\\DatabaseFiles\\AvelraanTestDb.json";
    private static readonly string logsPath = "\\Resources\\LogFiles\\Logs.txt";

    protected readonly IDatabaseManager dbm;
    protected readonly IDiceRollService diceService;
    protected readonly IPlayerService playerService;
    protected readonly IItemService itemService;
    protected readonly ICharacterService charService;

    protected readonly DatabaseSnapshot snapshot;

    protected TestBase()
	{
        var dbmConfig = new DatabaseManagerConfig
        {
            Key = testSecret,
            DbPath = $"{dbTestPath}",
            LogPath = $"{logsPath}"
        };

        dbm = new DatabaseManager(dbmConfig);

        diceService = new DiceRollService();
        playerService = new PlayerService(dbm);
        itemService = new ItemService(dbm, diceService);
        charService = new CharacterService(dbm, diceService, itemService);
    }
}
