using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;

namespace Persistance_Manager;

public interface IPersistenceService
{
    void PersistPlayer(Player player);
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

    #region private methods
    private static void SaveFileOnDisk(string json, string path, int tries = 0)
    {
        if (tries >= 3) throw new Exception($"Unable to persist file to disk at path: {path}.");

        try
        {
            tries++;
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
