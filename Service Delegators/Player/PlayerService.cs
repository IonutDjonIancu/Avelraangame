using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Dtos.ApiDtos;
using Persistance_Manager;
using Service_Delegators.Validators;

namespace Service_Delegators;

public class PlayerService : IPlayerService
{
    private readonly IDatabaseManager dbm;
    private readonly PlayerLogicDelegator logic;
    private readonly PlayerValidator validator;

    public PlayerService(IDatabaseManager manager)
    {
        dbm = manager;
        validator = new PlayerValidator(dbm);
        logic = new PlayerLogicDelegator(dbm);

    }

    public Authenticator CreatePlayer(string playerName)
    {
        validator.ValidatePlayerOnCreate(playerName);

        return logic.AddPlayer(playerName);
    }

    public string LoginPlayer(PlayerLogin login)
    {
        validator.ValidatePlayerOnLogin(login);

        var token = logic.LoginPlayer(login);
        validator.ValidatePlayerCanLogin(token);

        return token!;
    }

    public bool DeletePlayer(string playerId)
    {
        var name = dbm.Metadata.GetPlayerById(playerId)!.Identity.Name;
        validator.ValidatePlayerExistsByName(name);

        return logic.RemovePlayer(playerId);
    }
}
