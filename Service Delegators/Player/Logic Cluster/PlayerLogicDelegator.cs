using Independent_Modules;
using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class PlayerLogicDelegator
{
    private readonly IAuthenticatorModule authModule;
    private readonly PlayerAuthLogic authLogic;
    private readonly PlayerOperationsLogic opsLogic;

    private PlayerLogicDelegator() { }
    internal PlayerLogicDelegator(IDatabaseService databaseService)
    {
        authModule = new AuthenticatorModule();
        authLogic = new PlayerAuthLogic(databaseService, authModule);
        opsLogic = new PlayerOperationsLogic(databaseService);
    }

    internal Authenticator AuthenticatePlayer(string playerName)
    {
       return authLogic.AuthenticatePlayer(playerName);
    }

    internal string? AuthorizePlayer(PlayerLogin login)
    {
        return authLogic.AuthorizePlayer(login);
    }

    internal void RemovePlayer(string playerId)
    {
        opsLogic.RemovePlayer(playerId);
    }
}
