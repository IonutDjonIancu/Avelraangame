using Data_Mapping_Containers.Dtos;

namespace Persistance_Manager;

public interface IDatabaseManager
{
    DatabaseManagerInfo Info { get; init; }
    DatabaseManagerSnapshot Snapshot { get; set; }
}