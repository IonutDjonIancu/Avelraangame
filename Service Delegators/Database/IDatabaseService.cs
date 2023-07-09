using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IDatabaseService
{
    Snapshot Snapshot { get; set; }

    void PersistDatabase();
    void PersistPlayer(string playerId);

    #region database
    void ExportDatabase(string playerId);
    void ImportDatabase(string databaseJsonString, string playerId);
    #endregion

    #region player
    void DeletePlayer(string playerId);
    void ImportPlayer(string playerJsonString, string playerId);
    #endregion

    #region logs
    void ExportLogs(int days, string playerId);
    #endregion
}