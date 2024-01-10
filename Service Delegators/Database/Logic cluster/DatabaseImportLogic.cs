using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;

namespace Service_Delegators;

public interface IDatabaseImportLogic
{
    void ImportSnapshot(string snapshotJsonString);
    void ImportPlayer(string playerJsonString);
}

public class DatabaseImportLogic : IDatabaseImportLogic
{
    private Snapshot snapshot;
    private readonly IPlayerLogicDelegator players;

    private readonly object _lock = new();

    public DatabaseImportLogic(
        Snapshot snapshot,
        IPlayerLogicDelegator players)
    {
        this.snapshot = snapshot;
        this.players = players;
    }

    public void ImportSnapshot(string snapshotJsonString)
    {
        lock (_lock)
        {
            snapshot = JsonConvert.DeserializeObject<Snapshot>(snapshotJsonString)!;
        }
    }

    public void ImportPlayer(string playerJsonString)
    {
        lock (_lock)
        {
            var newPlayer = JsonConvert.DeserializeObject<Player>(playerJsonString)!;
            var oldPlayer = snapshot.Players.Find(p => p.Identity.Id == newPlayer.Identity.Id);

            if (oldPlayer != null)
            {
                players.DeletePlayer(oldPlayer.Identity.Id);
            }

            snapshot.Players.Add(newPlayer);
        }
    }
}
