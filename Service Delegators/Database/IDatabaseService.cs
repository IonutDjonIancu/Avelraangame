using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IDatabaseService
{
    DatabaseManagerSnapshot Snapshot { get; set; }

    #region database
    void PersistDatabase();
    void ExportDatabase(string playerId);
    void ImportDatabase(string databaseJsonString, string playerId);
    #endregion

    #region player
    void PersistPlayer(string playerId);
    void DeletePlayer(string playerId);
    void ImportPlayer(string playerJsonString, string playerId);
    #endregion

    #region logs
    void ExportLogs(int days, string playerId);
    #endregion
}