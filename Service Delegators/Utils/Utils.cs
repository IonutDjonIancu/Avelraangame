using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal static class Utils
{
    internal static string GetLocationFullName(Position position)
    {
        return $"{position.Region}_{position.Subregion}_{position.Land}_{position.Location}";
    }

    internal static Character GetPlayerCharacter(IDatabaseService dbs, CharacterIdentity identity)
    {
        return dbs.Snapshot.Players.Find(s => s.Identity.Id == identity.PlayerId)!.Characters.Find(s => s.Identity.Id == identity.Id)!;
    }


}
