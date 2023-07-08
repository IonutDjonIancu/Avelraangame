using Newtonsoft.Json;
using Persistance_Manager;
using Data_Mapping_Containers.Dtos;

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
        dbm.Snapshot.LastAction = DateTime.Now;

        var database = new Database
        {
            DbDate = dbm.Snapshot.LastAction,
            CharacterStubs = dbm.Snapshot.CharacterStubs,
            Parties = dbm.Snapshot.Parties
        };

        var dbJson = JsonConvert.SerializeObject(database);
        var dbPath = dbm.Info.DbPath;

        await SaveFileToDisk(dbJson, dbPath);
    }

    internal async void SavePlayerToDiskFile(string playerId)
    {
        var player = dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId);
        var playerJson = JsonConvert.SerializeObject(player);
        var playerPath = $"{dbm.Info.DbPlayersPath}\\Player{playerId}.json";

        await SaveFileToDisk(playerJson, playerPath);
    }

    internal async void SaveLocationsToDiskFile()
    {
        var locationJson = JsonConvert.SerializeObject(dbm.Snapshot.Map);
        var locationPath = dbm.Info.DbMapPath;

        await SaveFileToDisk(locationJson, locationPath);
    }

    internal async void RemovePlayerFile(string playerId)
    {
        var playerPath = $"{dbm.Info.DbPlayersPath}\\Player{playerId}.json";

        await RemoveFileFromDisk(playerPath);
    }


    #region private methods 
    private async Task SaveFileToDisk(string json, string path, int tries = 0)
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
            await SaveFileToDisk(json, path, tries);
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
