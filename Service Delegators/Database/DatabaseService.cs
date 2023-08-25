using Persistance_Manager;
using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class DatabaseService : IDatabaseService
{
    private readonly DatabaseValidator validator;
    private readonly DatabaseLogicDelegator logic;

    public SnapshotOld Snapshot { get; set; } // This is the overall Avelraan cache, and will be the ONLY public property

    private DatabaseService() { }
    public DatabaseService(IDatabaseManager databaseManager)
    {
        Snapshot = databaseManager.Snapshot; 
        validator = new DatabaseValidator(databaseManager.Snapshot);
        logic = new DatabaseLogicDelegator(databaseManager);
    }

    public void PersistDatabase()
    {
        logic.SaveDb();
    }
    public void PersistPlayer(string playerId)
    {
        logic.SavePlayer(playerId);
    }

    #region database
    public void ExportDatabase(string requesterPlayerId)
    {
        validator.ValidatePlayerIsAdmin(requesterPlayerId);
        logic.ExportDatabase();
    }
    
    public void ImportDatabase(string databaseJsonString, string requesterPlayerId)
    {
        validator.ValidatePlayerIsAdmin(requesterPlayerId);
        logic.ImportDatabase(databaseJsonString);
    }
    #endregion

    #region player
    public void DeletePlayer(string requesterPlayerId)
    {
        logic.RemovePlayer(requesterPlayerId);
    }

    public void ImportPlayer(string playerJsonString, string requesterPlayerId)
    {
        validator.ValidatePlayerIsAdmin(requesterPlayerId);
        logic.ImportPlayer(playerJsonString);
    }
    #endregion

    #region logs
    public void ExportLogs(int days, string requesterPlayerId)
    {
        validator.ValidatePlayerIsAdmin(requesterPlayerId);
        logic.ExportLogs(days);
    }
    #endregion
}
