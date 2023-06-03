using Data_Mapping_Containers.Dtos;
using Service_Delegators.Validators;

namespace Service_Delegators;

public class PlayerService : IPlayerService
{
    private readonly PlayerValidator validator;
    private readonly PlayerLogicDelegator logic;

    public PlayerService(IDatabaseService databaseService)
    {
        validator = new PlayerValidator(databaseService.Snapshot);
        logic = new PlayerLogicDelegator(databaseService);
    }

    public Authenticator CreatePlayer(string playerName)
    {
        validator.ValidatePlayerOnCreate(playerName);

        return logic.AuthenticatePlayer(playerName);
    }

    public string LoginPlayer(PlayerLogin login)
    {
        validator.ValidatePlayerOnLogin(login);

        var token = logic.AuthorizePlayer(login);

        validator.ValidatePlayerCanLogin(token);

        return token!;
    }

    public void DeletePlayer(string playerId)
    {
        validator.ValidateIfPlayerExists(playerId);
        
        logic.RemovePlayer(playerId);
    }
}
