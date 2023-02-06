using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

public class CharacterMetadata
{
    private readonly IDatabaseManager dbm;

    private CharacterMetadata() { }

    internal CharacterMetadata(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
    }

    internal Character GetCharacter(string playerId, string charId)
    {
        return dbm.Snapshot.Players!.Find(p => p.Identity.Id == playerId)!.Characters!.Find(c => c.Identity!.Id == charId)!;
    }

    internal bool DoesCharacterExist(string playerId, string charId)
    {
        return dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId).Characters.Exists(c => c.Identity.Id == charId);
    }

}
