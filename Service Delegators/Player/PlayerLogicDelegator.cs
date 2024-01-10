using Data_Mapping_Containers.Dtos;

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
    private readonly IPlayerAuthLogic playerAuth;
    private readonly IPlayerOperationsLogic playerOps;


    public PlayerLogicDelegator(
        IValidations validations,
        IPlayerAuthLogic playerAuth,
        IPlayerOperationsLogic playerOps)
    {
        this.validations = validations;
        this.playerAuth = playerAuth;
        this.playerOps = playerOps;
    }

    public Authenticator CreatePlayer(string playerName)
    {
        validations.ValidatePlayerCreate(playerName);
        return playerAuth.AuthenticatePlayer(playerName);
    }

    public string LoginPlayer(PlayerLogin login)
    {
        validations.ValidatePlayerLogin(login);
        return playerAuth.AuthorizePlayer(login).Identity.Token;
    }

    public void UpdatePlayerName(string newPlayerName, string playerId)
    {
        validations.ValidatePlayerUpdateName(newPlayerName, playerId);
        playerOps.UpdateName(newPlayerName, playerId);
    }

    public void DeletePlayer(string playerId)
    {
        validations.ValidatePlayerDelete(playerId);
        playerOps.Remove(playerId);
    }
}
