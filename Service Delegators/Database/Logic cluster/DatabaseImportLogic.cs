using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;
using Persistance_Manager;

namespace Service_Delegators;

public interface IDatabaseImportLogic
{
    void ImportPlayer(string playerJsonString);
}

public class DatabaseImportLogic : IDatabaseImportLogic
{
    private readonly Snapshot snapshot;
    private readonly IPersistenceService persistence;
    private readonly IPlayerLogicDelegator players;

    private readonly object _lock = new();

    public DatabaseImportLogic(
        Snapshot snapshot,
        IPersistenceService persistence,
        IPlayerLogicDelegator players)
    {
        this.snapshot = snapshot;
        this.persistence = persistence;
        this.players = players;
    }

    public void ImportPlayer(string playerJsonString)
    {
        var newPlayer = JsonConvert.DeserializeObject<Player>(playerJsonString);

        lock (_lock)
        {
            var oldPlayer = snapshot.Players.Find(p => p.Identity.Id == newPlayer.Identity.Id);

            if (oldPlayer != null)
            {
                players.DeletePlayer(oldPlayer.Identity.Id);
            }

            snapshot.Players.Add(newPlayer);
            persistence.PersistPlayer(newPlayer.Identity.Id);
        }
    }
}
