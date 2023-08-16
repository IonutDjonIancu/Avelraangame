using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface INpcService
{
    Character GenerateGoodGuyNpc(string location);
    Character GenerateBadGuyNpc(string location);
}