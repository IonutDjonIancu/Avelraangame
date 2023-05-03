using Data_Mapping_Containers.Dtos;

namespace Persistance_Manager;

public interface IDatabaseManager
{
    MetadataManager Metadata { get; set; }
    Snapshot Snapshot { get; set; }

    void PersistDatabase();

    void PersistPlayer(Player player);
    void RemovePlayer(Player player);
}