using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IPlayerService
{
    Authenticator CreatePlayer(string playerName);
    string LoginPlayer(PlayerLogin login);
    bool DeletePlayer(string playerId);
}