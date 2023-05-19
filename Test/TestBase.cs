using Service_Delegators;

namespace Tests;

public class TestBase
{
    private static readonly string testSecret = "BQxevwF37aNAznk";
    private static readonly string dbTestPath = "\\Resources\\Database files\\AvelraanTestDb.json";
    private static readonly string dbPlayersPath = "\\Resources\\Database files\\Players";
    private static readonly string logsPath = "\\Resources\\Log files\\Logs.txt";

    protected readonly IDatabaseManager dbm;
    protected readonly IDiceRollService diceService;
    protected readonly IPlayerService playerService;
    protected readonly IItemService itemService;
    protected readonly ICharacterService characterService;
    protected readonly INpcService npcService;

    protected readonly DatabaseManagerSnapshot snapshot;

    protected TestBase()
	{
        var dbmConfig = new DatabaseManagerConfig
        {
            Key = testSecret,
            DbPath = dbTestPath,
            LogPath = logsPath,
            DbPlayersPath = dbPlayersPath,
            AvelraanEmail = "example@gmail.com",
            AvelraanPassword = "password",
        };

        dbm = new DatabaseManager(dbmConfig);

        diceService = new DiceRollService();
        playerService = new PlayerService(dbm);
        itemService = new ItemService(diceService);
        characterService = new CharacterService(dbm, diceService, itemService);
        npcService = new NpcService(dbm, diceService, itemService, characterService);
    }

    protected string CreatePlayer(string playerName)
    {
        dbm.Snapshot.Players!.Clear();
        playerService.CreatePlayer(playerName);

        return dbm.Snapshot.Players!.Find(p => p.Identity.Name == playerName)!.Identity.Id;
    }
}
