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
    protected readonly IGameplayService gameplayService;

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
        gameplayService = new GameplayService(dbs, diceService, characterService);
    }

    protected string CreatePlayer(string playerName)
    {
        dbs.Snapshot.Players!.Clear();
        playerService.CreatePlayer(playerName);

        return dbs.Snapshot.Players!.Find(p => p.Identity.Name == playerName)!.Identity.Id;
    }

    protected Character CreateHumanCharacter(string playerName)
    {
        var playerId = CreatePlayer(playerName);
        dbs.Snapshot.CharacterStubs.Clear();

        characterService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Tradition = CharactersLore.Tradition.Common,
            Class = CharactersLore.Classes.Warrior
        };

        return characterService.SaveCharacterStub(origins, playerId);
    }
}
