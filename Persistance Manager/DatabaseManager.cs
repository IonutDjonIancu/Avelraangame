using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;
using Persistance_Manager.Validators;

namespace Persistance_Manager;

public class DatabaseManager : IDatabaseManager
{
    private static readonly string currentDir = Directory.GetCurrentDirectory();

    private readonly DatabaseManagerValidator validator;

    internal readonly DatabaseManagerInfo info = new()
    {
        SecretKeys = new()
        {
            "x9wi98mKiWhSba2", // db key
            "BQxevwF37aNAznk" // test key
        },

        Admins = new()
        {
            "djon"
        },

        Banned = new()
        {
            "JaneDoe"
        }
    };

    public Snapshot Snapshot { get; set; }
    public MetadataManager Metadata { get; set; }

    public DatabaseManager(DatabaseManagerConfig dbmconfig)
    {
        validator = new DatabaseManagerValidator(this);
        validator.KeyInSecretKeys(dbmconfig.Key);

        validator.ValidateString(dbmconfig.DbPath);
        info.DbPath = $"{currentDir}{dbmconfig.DbPath}";
        validator.FileAtPath(info.DbPath);

        validator.ValidateString(dbmconfig.DbPlayersPath);
        info.DbPlayersPath = $"{currentDir}{dbmconfig.DbPlayersPath}";
        info.PlayerFilePaths = UploadPlayerFilePaths(info.DbPlayersPath);

        validator.ValidateString(dbmconfig.DbRulebookPath);
        info.DbRulebookPath = $"{currentDir}{dbmconfig.DbRulebookPath}";

        validator.ValidateString(dbmconfig.LogPath);
        info.LogPath = $"{currentDir}{dbmconfig.LogPath}";

        validator.ValidateString(dbmconfig.AvelraanEmail);
        info.AvelraanEmail = dbmconfig.AvelraanEmail;

        validator.ValidateString(dbmconfig.AvelraanPassword);
        info.AvelraanPassword = dbmconfig.AvelraanPassword;

        Snapshot = CreateSnapshot(info);
        Metadata = new MetadataManager(this);
    }

    /// <summary>
    /// Call this method each time the snapshot db object is modified.
    /// </summary>
    /// <returns></returns>
    public async void PersistDatabase()
    {
        await PersistDatabaseFromSnapshot();
    }

    public async void PersistPlayer(Player player)
    {
        if (player == null) throw new Exception("Player object cannot be null.");

        Thread.Sleep(100);
        await PersistPlayerFromSnapshot(player);
    }

    public void RemovePlayer(Player player)
    {
        if (player == null) throw new Exception("Player object cannot be null.");

        RemovePlayerPersistance(player.Identity.Id);
    }

    #region private methods
    private static List<string> UploadPlayerFilePaths(string dbPlayersPath)
    {
        return Directory.GetFiles(dbPlayersPath).ToList();
    }

    private void RemovePlayerPersistance(string playerId, int tries = 0)
    {
        validator.TriesLimit(tries);

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

    private async Task PersistPlayerFromSnapshot(Player player, int tries = 0)
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
            await PersistPlayerFromSnapshot(player, tries);
        }
    }

    private async Task PersistDatabaseFromSnapshot(int tries = 0)
    {
        validator.TriesLimit(tries);

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
            await PersistDatabaseFromSnapshot(tries);
        }
    }

    private static Snapshot CreateSnapshot(DatabaseManagerInfo dbmInfo)
    {
        var avelraanDbFile = File.ReadAllText(dbmInfo.DbPath);
        var avDb = JsonConvert.DeserializeObject<Database>(avelraanDbFile);

        var snapshot = new Snapshot()
        {
            DbDate = avDb.DbDate,

            Rulebook = ReadRulebookFile(dbmInfo.DbRulebookPath),
            Players = ReadPlayerFiles(dbmInfo.PlayerFilePaths),
            Traits = LoadTraitsFromLore(),
            
            CharacterStubs = avDb.CharacterStubs,
            Items = avDb.Items,
            Parties = avDb.Parties
        };

        return snapshot;
    }

    private static Rulebook ReadRulebookFile(string path)
    {
        var text = File.ReadAllText(path);
        var rulebook = JsonConvert.DeserializeObject<Rulebook>(text);

        return rulebook;
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

    private static List<HeroicTrait> LoadTraitsFromLore()
    {
        var listOfTraits = new List<HeroicTrait>();

        foreach (var item in TraitsLore.PassiveTraits.All)
        {
            listOfTraits.Add(item);
        }
        foreach (var item in TraitsLore.ActiveTraits.All)
        {
            listOfTraits.Add(item);
        }
        foreach (var item in TraitsLore.BonusTraits.All)
        {
            listOfTraits.Add(item);
        }

        return listOfTraits;
    }
    #endregion
}