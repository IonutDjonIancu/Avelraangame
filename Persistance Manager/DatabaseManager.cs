using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;
using Persistance_Manager.Validators;

namespace Persistance_Manager;

public class DatabaseManager : IDatabaseManager
{
    private static readonly string currentDir = Directory.GetCurrentDirectory();

    private readonly DatabaseManagerValidator validate;
    internal readonly DatabaseManagerInfo info = new()
    {
        SecretKeys = new()
        {
            "x9wi98mKiWhSba2", // db key
            "BQxevwF37aNAznk" // test key
        },

        Admins = new()
        {
            "iiancu85@gmail.com"
        },

        Banned = new()
        {
            "JaneDoe@gmail.com"
        }
    };

    public DatabaseSnapshot Snapshot { get; set; }
    public MetadataManager Metadata { get; set; }

    public DatabaseManager(DatabaseManagerConfig dbmconfig)
    {
        validate = new DatabaseManagerValidator(this);
        validate.KeyInSecretKeys(dbmconfig.Key);

        validate.ValidateString(dbmconfig.DbPath);
        info.DbPath = $"{currentDir}{dbmconfig.DbPath}";
        validate.FileAtPath(info.DbPath);

        validate.ValidateString(dbmconfig.DbPlayersPath);
        info.DbPlayersPath = $"{currentDir}{dbmconfig.DbPlayersPath}";
        info.PlayerFilePaths = UploadPlayerFilePaths(info.DbPlayersPath);

        validate.ValidateString(dbmconfig.DbTraitsPath);
        info.DbTraitsPath = $"{currentDir}{dbmconfig.DbTraitsPath}";

        validate.ValidateString(dbmconfig.LogPath);
        info.LogPath = $"{currentDir}{dbmconfig.LogPath}";
        validate.FileAtPath(info.LogPath);

        Snapshot = CreateDatabaseSnapshot(info);
        Metadata = new MetadataManager(this);
    }

    /// <summary>
    /// Call this method each time the snapshot db object is modified.
    /// </summary>
    /// <returns></returns>
    public async void Persist()
    {
        await SaveDatabaseSnapshot();
    }

    public async void PersistPlayer(Player player, bool toRemove = false)
    {
        if (player == null) throw new Exception("Unable to find player");

        if (toRemove)
        {
            RemovePlayerSnapshot(player.Identity.Id);
        }
        else
        {
            await SavePlayerSnapshot(player);
        }
    }

    #region privates
    private static List<string> UploadPlayerFilePaths(string dbPlayersPath)
    {
        return Directory.GetFiles(dbPlayersPath).ToList();
    }

    private void RemovePlayerSnapshot(string playerId, int tries = 0)
    {
        validate.TriesLimit(tries);

        try
        {
            tries++;
            var path = $"{info.DbPlayersPath}\\Player{playerId}.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task SavePlayerSnapshot(Player player, int tries = 0)
    {
        var playerJson = JsonConvert.SerializeObject(Snapshot.Players.Find(p => p.Identity.Id == player.Identity.Id)) ?? throw new Exception("Could not serialize player object from snapshot.");
        
        try
        {
            tries++;
            var path = $"{info.DbPlayersPath}\\Player{player.Identity.Id}.json";
            await File.WriteAllTextAsync(path, playerJson);
        }
        catch (Exception)
        {
            Thread.Sleep(300);
            await SavePlayerSnapshot(player, tries);
        }
    }

    private async Task SaveDatabaseSnapshot(int tries = 0)
    {
        validate.TriesLimit(tries);

        Snapshot.DbDate = DateTime.Now;
        var dbJson = JsonConvert.SerializeObject(Snapshot);

        try
        {
            tries++;
            await File.WriteAllTextAsync(info.DbPath, dbJson);
        }
        catch (Exception)
        {
            Thread.Sleep(300);
            await SaveDatabaseSnapshot(tries);
        }
    }

    private static DatabaseSnapshot CreateDatabaseSnapshot(DatabaseManagerInfo dbmInfo)
    {
        var snapshot = new DatabaseSnapshot()
        {
            DbDate = DateTime.Now,
            Players = ReadPlayerFiles(dbmInfo.PlayerFilePaths),
            CharacterStubs = new List<CharacterStub>(),
            Items = new List<Item>(),
            Traits = ReadTraitsFile(dbmInfo.DbTraitsPath)
        };

        return snapshot;
    }

    private static List<Player> ReadPlayerFiles(List<string> paths)
    {
        var list = new List<Player>();

        foreach (var path in paths)
        {
            var text = File.ReadAllText(path);
            var player = JsonConvert.DeserializeObject<Player>(text);
            list.Add(player);
        }

        return list;
    }

    private static List<HeroicTrait> ReadTraitsFile(string path)
    {
        var text = File.ReadAllText(path);
        var listOfTraits = JsonConvert.DeserializeObject<List<HeroicTrait>>(text);

        return listOfTraits;
    }
    #endregion
}