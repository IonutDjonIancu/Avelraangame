using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public static class Utils
{
    public static string GetLocationFullName(Position position)
    {
        return $"{position.Region}_{position.Subregion}_{position.Land}_{position.Location}";
    }

    public static Position GetLocationPosition(string fullName)
    {
        var substrings = fullName.Split('_');

        return new Position
        {
            Region = substrings[0],
            Subregion = substrings[1],
            Land = substrings[2],
            Location = substrings[3]
        };
    }

    public static Character GetPlayerCharacter(IDatabaseService dbs, CharacterIdentity identity)
    {
        return dbs.Snapshot.Players.Find(s => s.Identity.Id == identity.PlayerId)!.Characters.Find(s => s.Identity.Id == identity.Id)!;
    }


}
