using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Dtos.ApiDtos;

namespace Service_Delegators;

public interface IPlayerService
{
    Authenticator CreatePlayer(string playerName);
    string LoginPlayer(PlayerLogin login);
    bool DeletePlayer(string playerId);
}