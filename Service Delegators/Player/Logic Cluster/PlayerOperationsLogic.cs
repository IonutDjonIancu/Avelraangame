using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IPlayerOperationsLogic
{
    Player UpdateName(string newPlayerName, string playerId);
    void Remove(string playerId);
}

public class PlayerOperationsLogic : IPlayerOperationsLogic
{
    private readonly object _lock = new();
    private readonly Snapshot snapshot;

    public PlayerOperationsLogic(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public Player UpdateName(string newPlayerName, string playerId)
    {
        lock (_lock)
        {
            var player = snapshot.Players.Find(s => s.Identity.Id == playerId)!;
            player.Identity.Name = newPlayerName;

            return player;
        }
    }

    public void Remove(string playerName)
    {
        lock(_lock)
        {
            var player = snapshot.Players.Find(s => s.Identity.Name == playerName)!;
            snapshot.Players.Remove(player);
        }
    }
}
