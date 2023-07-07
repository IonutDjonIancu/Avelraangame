using Newtonsoft.Json;
using Data_Mapping_Containers.Dtos;

namespace Persistance_Manager;

public class DatabaseManager : IDatabaseManager
{
    #region private props
    private static readonly List<string> admins = new()
    {
        "djon"
    };
    private static readonly List<string> banned = new()
    {
        "janedoe"
    };
    private static readonly string currentDir = Directory.GetCurrentDirectory();
    #endregion

    public DatabaseManagerInfo Info { get; init; }
    public Snapshot Snapshot { get; set; }

    public DatabaseManager(DatabaseManagerConfig dbmconfig)
    {
        Info = SetUpInfo(dbmconfig);
        Snapshot = SetUpSnaphot(Info);
    }

    #region private methods
    private static DatabaseManagerInfo SetUpInfo(DatabaseManagerConfig config)
    {
        return new DatabaseManagerInfo
        {
            DbPath = $"{currentDir}{config.DbPath}",
            DbPlayersPath = $"{currentDir}{config.DbPlayersPath}",
            DbMapPath = $"{currentDir}{config.DbMapPath}",

            LogPath = $"{currentDir}{config.LogPath}",

            AvelraanEmail = config.AvelraanEmail,
            AvelraanPassword = config.AvelraanPassword,
        };
    }

    private static Snapshot SetUpSnaphot(DatabaseManagerInfo info)
    {
        var avDbDiskFile = File.ReadAllText(info.DbPath);
        var avDatabase = JsonConvert.DeserializeObject<Database>(avDbDiskFile);

        return new Snapshot
        {
            LastAction = avDatabase.DbDate,
            Admins = admins,
            Banned = banned,

            // player files will be loaded to make sure we have the admins
            Players = ReadPlayerFiles(info.DbPlayersPath),

            // TODO: to refactor, not really necessary
            CharacterStubs = avDatabase.CharacterStubs,

            // TODO: to remove parties
            Parties = avDatabase.Parties,

            // no need to load the map from its lore class
            Map = new Map(),
        };
    }

    private static List<Player> ReadPlayerFiles(string playersPath)
    {
        var paths = Directory.GetFiles(playersPath);

        var list = new List<Player>();

        foreach (var path in paths)
        {
            var text = File.ReadAllText(path);
            var player = JsonConvert.DeserializeObject<Player>(text);
            list.Add(player);
        }

        return list;
    }

    private static async Task SaveToFileOnDisk(string json, string path, int tries = 0)
    {
        if (tries >= 3) throw new Exception($"Unable to persist file to disk at path: {path}.");

        try
        {
            tries++;
            await File.WriteAllTextAsync(path, json);
        }
        catch (Exception)
        {
            Thread.Sleep(300);
            await SaveToFileOnDisk(json, path, tries);
        }
    }
    #endregion
}