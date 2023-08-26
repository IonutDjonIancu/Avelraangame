using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

public interface IPlayerLogicDelegator
{
    Authenticator CreatePlayer(string playerName);
    string LoginPlayer(PlayerLogin login);
    void UpdatePlayerName(string newPlayerName, string playerId);
    void DeletePlayer(string playerId);
}

public class PlayerLogicDelegator : IPlayerLogicDelegator
{
    private readonly IValidations validations;
    private readonly IPersistenceService persistence;
    private readonly IPlayerAuthLogic playerAuthLogic;
    private readonly IPlayerOperationsLogic playerOpsLogic;


    public PlayerLogicDelegator(
        IValidations validations,
        IPersistenceService persistence,
        IPlayerAuthLogic playerAuthLogic,
        IPlayerOperationsLogic playerOpsLogic)
    {
        this.validations = validations;
        this.persistence = persistence;
        this.playerAuthLogic = playerAuthLogic;
        this.playerOpsLogic = playerOpsLogic;
    }

    public Authenticator CreatePlayer(string playerName)
    {
        validations.CreatePlayer(playerName);
        var (auth, player) = playerAuthLogic.AuthenticatePlayer(playerName);
        persistence.PersistPlayer(player);

        return auth;
    }

    public string LoginPlayer(PlayerLogin login)
    {
        validations.LoginPlayer(login);
        var player = playerAuthLogic.AuthorizePlayer(login);
        persistence.PersistPlayer(player);

        return player.Identity.Token;
    }

    public void UpdatePlayerName(string newPlayerName, string playerId)
    {
        validations.UpdatePlayerName(newPlayerName, playerId);
        var player = playerOpsLogic.UpdateName(newPlayerName, playerId);
        persistence.PersistPlayer(player);
    }

    public void DeletePlayer(string playerId)
    {
        validations.DeletePlayer(playerId);
        playerOpsLogic.Remove(playerId);
        persistence.RemovePlayer(playerId);
    }
}
