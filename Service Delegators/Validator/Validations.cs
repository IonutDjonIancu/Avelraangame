using Data_Mapping_Containers.Dtos;
using System.Xml.Linq;

namespace Service_Delegators;

public interface IValidations
{
    #region player
    void CreatePlayer(string playerName);
    void LoginPlayer(PlayerLogin login);
    void UpdatePlayerName(string newPlayerName, string playerId);
    void DeletePlayer(string playerId);
    #endregion
}

public class Validations : IValidations
{
    private readonly Snapshot snapshot;
    private readonly object playersLock = new();

    public Validations(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    #region player validations
    public void CreatePlayer(string playerName)
    {
        ValidateString(playerName);
        if (playerName.Length > 20) throw new Exception($"Player name: {playerName} is too long, 20 characters max.");

        lock (playersLock)
        {
            if (snapshot.Players.Count >= 20) throw new Exception("Server has reached the limit number of players, please contact admins.");
            if (snapshot.Players.Exists(p => p.Identity.Name.ToLower() == playerName.ToLower())) throw new Exception("Name unavailable.");
        }
    }

    public void LoginPlayer(PlayerLogin login)
    {
        ValidateObject(login);
        ValidateString(login.PlayerName);
        ValidateString(login.Code);

        lock (playersLock)
        {
            // we don't care for player misspelling their names
            // names will be unique anyway at creation
            if (!snapshot.Players.Exists(p => p.Identity.Name.ToLower() == login.PlayerName.ToLower())) throw new Exception("Player does not exist.");
        }
    }

    public void UpdatePlayerName(string newPlayerName, string playerId)
    {
        ValidateString(newPlayerName);
        ValidatePlayerExists(playerId);
    }

    public void DeletePlayer(string playerId)
    {
        ValidatePlayerExists(playerId);
    }
    #endregion


    #region private methods
    private static void ValidateString(string str, string message = "")
    {
        if (string.IsNullOrWhiteSpace(str)) throw new Exception(message.Length > 0 ? message : "The provided string is invalid.");
    }

    private static void ValidateObject(object? obj, string message = "")
    {
        if (obj == null) throw new Exception(message.Length > 0 ? message : $"Object found null.");
    }

    private void ValidateNumberGreaterThanZero(int num, string message = "")
    {
        if (num <= 0) throw new Exception(message.Length > 0 ? message : "Number cannot be smaller or equal to zero.");
    }

    private void ValidateGuid(string str, string message = "")
    {
        ValidateString(str);

        var isGuidValid = Guid.TryParse(str, out var id);

        if (!isGuidValid) throw new Exception(message.Length > 0 ? message : "Invalid guid.");

        if (id == Guid.Empty) throw new Exception("Guid cannot be an empty guid.");
    }

    private void ValidatePlayerExists(string playerId)
    {
        ValidateString(playerId);

        lock (playersLock)
        {
            if (!snapshot.Players.Exists(p => p.Identity.Id == playerId)) throw new Exception("Player not found.");
        }
    }

    #endregion
}
