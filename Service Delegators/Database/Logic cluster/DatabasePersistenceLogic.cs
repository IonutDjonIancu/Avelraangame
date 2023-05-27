using Newtonsoft.Json;
using Persistance_Manager;

namespace Service_Delegators;

internal class DatabasePersistenceLogic
{
    private readonly IDatabaseManager dbm;

    private DatabasePersistenceLogic() { }

    internal DatabasePersistenceLogic(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
    }

    internal async void SaveDbToDiskFile()
    {
        dbm.Snapshot.DbDate = DateTime.Now;
        var dbJson = JsonConvert.SerializeObject(dbm.Snapshot);
        var dbPath = dbm.Info.DbPath;

        // remove players or all other non-essential information when saving snapshot to disk
        // create another object before saving to disk from snapshot

        await SaveToFileOnDisk(dbJson, dbPath);
    }

    internal async void SavePlayerToDiskFile(string playerId)
    {
        var player = dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId);
        var playerJson = JsonConvert.SerializeObject(player);
        var playerPath = $"{dbm.Info.DbPlayersPath}\\Player{playerId}.json";

        await SaveToFileOnDisk(playerJson, playerPath);
    }

    internal async void RemovePlayerFile(string playerId)
    {
        var playerPath = $"{dbm.Info.DbPlayersPath}\\Player{playerId}.json";

        await RemoveFileFromDisk(playerPath);
    }


    #region private methods 
    private async Task SaveToFileOnDisk(string json, string path, int tries = 0)
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

    private async Task RemoveFileFromDisk(string path, int tries = 0)
    {
        if (tries >= 3) throw new Exception($"Unable to remove file from disk at path: {path}.");

        try
        {
            tries++;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (Exception)
        {
            Thread.Sleep(300);
            await RemoveFileFromDisk(path, tries);
        }
    }
    #endregion
}
