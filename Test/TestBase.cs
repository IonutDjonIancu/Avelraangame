using Service_Delegators;

namespace Tests;

public class TestBase
{
    private static readonly string testSecret = "BQxevwF37aNAznk";
    private static readonly string dbTestPath = "\\Resources\\Database files\\AvelraanTestDb.json";
    private static readonly string dbPlayersPath = "\\Resources\\Database files\\Players";
    private static readonly string dbRulebookPath = "\\Resources\\Game rules\\Rulebook.json";
    private static readonly string logsPath = "\\Resources\\Log files\\Logs.txt";

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
            DbPath = dbTestPath,
            LogPath = logsPath,
            DbPlayersPath = dbPlayersPath,
            DbRulebookPath = dbRulebookPath,
            AvelraanEmail = "example@gmail.com",
            AvelraanPassword = "password",
        };

        dbm = new DatabaseManager(dbmConfig);

        diceService = new DiceRollService();
        playerService = new PlayerService(dbm);
        itemService = new ItemService(diceService);
        charService = new CharacterService(dbm, diceService, itemService);
    }
}
