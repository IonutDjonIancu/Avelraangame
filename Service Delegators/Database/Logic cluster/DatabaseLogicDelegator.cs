using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;
using Persistance_Manager;

namespace Service_Delegators;

internal class DatabaseLogicDelegator
{
    private readonly DatabasePersistenceLogic persistLogic;
    private readonly DatabaseExportLogic exportLogic;
    private readonly DatabaseImportLogic importLogic;

    private DatabaseLogicDelegator() { }

    internal DatabaseLogicDelegator(IDatabaseManager databaseManager)
    {
        persistLogic = new DatabasePersistenceLogic(databaseManager);
        exportLogic = new DatabaseExportLogic(databaseManager);
        importLogic = new DatabaseImportLogic(databaseManager);
    }

    internal void SaveDb()
    {
        persistLogic.SaveDbToDiskFile();
    }
    internal void SavePlayer(string playerId)
    {
        persistLogic.SavePlayerToDiskFile(playerId);
    }


    internal void RemovePlayer(string playerId)
    {
        persistLogic.RemovePlayerFile(playerId);
    }

    internal void ExportLogs(int days)
    {
        exportLogic.ExportLogs(days);
    }

    internal void ExportDatabase()
    {
        exportLogic.ExportDatabase();
    }

    internal void ImportDatabase(string databaseJsonString)
    {
        var database = JsonConvert.DeserializeObject<Database>(databaseJsonString);
        importLogic.ImportDatabase(database);
        persistLogic.SaveDbToDiskFile();
    }

    internal void ImportPlayer(string playerJsonString)
    {
        var player = JsonConvert.DeserializeObject<Player>(playerJsonString);
        importLogic.ImportPlayer(player);
        persistLogic.SavePlayerToDiskFile(player.Identity.Id);
    }


}
