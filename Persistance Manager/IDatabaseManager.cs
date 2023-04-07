﻿using Data_Mapping_Containers.Dtos;

namespace Persistance_Manager;

public interface IDatabaseManager
{
    MetadataManager Metadata { get; set; }
    DatabaseSnapshot Snapshot { get; set; }

    void Persist();

    void PersistPlayer(Player player);
    void RemovePlayer(Player player);

    bool OverwriteDatabase(DatabaseOverwrite overwrite);
}