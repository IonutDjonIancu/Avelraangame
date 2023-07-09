using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface INpcService
{
    NpcCharacter GenerateBadGuyNpc(Position position, int effortUpper);
    NpcCharacter GenerateGoodGuyNpc(Position position, int effortUpper);
}