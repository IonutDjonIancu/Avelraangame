using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface INpcService
{
    NpcPaperdoll GenerateNpc(NpcInfo info);
}