using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;

namespace Persistance_Manager;

public interface IPersistenceService
{
    void PersistPlayer(Player player);
    void RemovePlayer(string playerId);
}

public class PersistenceService : IPersistenceService
{
    private readonly AppSettings appSettings;

    public PersistenceService(AppSettings appSettings)
    {
        this.appSettings = appSettings;
    }

    public void PersistPlayer(Player player)
    {
        var playerJson = JsonConvert.SerializeObject(player);
        var path = $"{Directory.GetCurrentDirectory()}{appSettings.DbPlayersPath}\\Player{player.Identity.Id}.json";

        SaveFileOnDisk(playerJson, path);
    }

    public void RemovePlayer(string playerId)
    {
        var path = $"{Directory.GetCurrentDirectory()}{appSettings.DbPlayersPath}\\Player{playerId}.json";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            throw new Exception("Player file not found to remove.");
        }
    }

    #region private methods
    private static void SaveFileOnDisk(string json, string path, int tries = 0)
    {
        if (tries >= 3) throw new Exception($"Unable to persist file to disk at path: {path}.");

        try
        {
            tries++;
            if (File.Exists(path)) File.Delete(path);

            File.WriteAllTextAsync(path, json);
        }
        catch (Exception)
        {
            Thread.Sleep(300);
            SaveFileOnDisk(json, path, tries);
        }
    }
    #endregion
}
