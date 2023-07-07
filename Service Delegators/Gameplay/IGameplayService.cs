using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IGameplayService
{
    Location GetLocation(Position position);

}