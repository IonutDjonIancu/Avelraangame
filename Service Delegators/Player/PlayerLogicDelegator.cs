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
    private readonly IPlayerAuthLogic playerAuth;
    private readonly IPlayerOperationsLogic playerOps;


    public PlayerLogicDelegator(
        IValidations validations,
        IPersistenceService persistence,
        IPlayerAuthLogic playerAuth,
        IPlayerOperationsLogic playerOps)
    {
        this.validations = validations;
        this.persistence = persistence;
        this.playerAuth = playerAuth;
        this.playerOps = playerOps;
    }

    public Authenticator CreatePlayer(string playerName)
    {
        validations.ValidatePlayerCreate(playerName);
        var (auth, player) = playerAuth.AuthenticatePlayer(playerName);
        persistence.PersistPlayer(player.Identity.Id);

        return auth;
    }

    public string LoginPlayer(PlayerLogin login)
    {
        validations.ValidatePlayerLogin(login);
        var player = playerAuth.AuthorizePlayer(login);
        persistence.PersistPlayer(player.Identity.Id);

        return player.Identity.Token;
    }

    public void UpdatePlayerName(string newPlayerName, string playerId)
    {
        validations.ValidatePlayerUpdateName(newPlayerName, playerId);
        var player = playerOps.UpdateName(newPlayerName, playerId);
        persistence.PersistPlayer(player.Identity.Id);
    }

    public void DeletePlayer(string playerId)
    {
        validations.ValidatePlayerDelete(playerId);
        playerOps.Remove(playerId);
        persistence.RemovePlayer(playerId);
    }
}
