using Persistance_Manager;
using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class DatabaseService : IDatabaseService
{
    private readonly DatabaseValidator validator;
    private readonly DatabaseLogicDelegator logic;

    public DatabaseManagerSnapshot Snapshot { get; set; } // This is the overall Avelraan cache, and will be the ONLY public property

    private DatabaseService() { }
    public DatabaseService(IDatabaseManager databaseManager)
    {
        Snapshot = databaseManager.Snapshot; 
        validator = new DatabaseValidator(databaseManager);
        logic = new DatabaseLogicDelegator(databaseManager);
    }

    #region database
    public void PersistDatabase()
    {
        logic.SaveDb();
    }

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
    public void PersistPlayer(string playerId)
    {
        logic.SavePlayer(playerId);
    }

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

    #region gets
    public List<Party> GetParties()
    {
        return Snapshot.Parties;
    }

    public Players GetPlayers()
    {
        var playersList = Snapshot.Players;
        var players = new Players
        {
            Count = playersList.Count
        };

        foreach (var player in playersList)
        {
            players.PlayerNames.Add(player.Identity.Name);
        }

        return players;
    }
    #endregion
}
