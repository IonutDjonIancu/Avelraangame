using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface INpcService
{
    Character GenerateBadGuyNpc(string location);
    Character GenerateGoodGuyNpc(string location);
}