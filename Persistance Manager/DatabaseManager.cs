using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;
using Persistance_Manager.Validators;

namespace Persistance_Manager;

public class DatabaseManager : IDatabaseManager
{
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
        info.DbPath = $"{Directory.GetCurrentDirectory()}{dbmconfig.DbPath}";
        validate.FileAtPath(info.DbPath);

        validate.ValidateString(dbmconfig.LogPath);
        info.LogPath = $"{Directory.GetCurrentDirectory()}{dbmconfig.LogPath}";
        validate.FileAtPath(info.LogPath);

        Snapshot = CreateDatabaseSnapshot(info.DbPath);
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

    #region privates
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

    private static DatabaseSnapshot CreateDatabaseSnapshot(string path)
    {
        var text = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<DatabaseSnapshot>(text);
    }
    #endregion
}