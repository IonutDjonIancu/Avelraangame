using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public static class Utils
{
    public static Location GetLocationByLocationName(string locationName)
    {
        var position = GetPositionByLocation(locationName);
        var locationFullName = GetLocationFullName(position);
        return GameplayLore.Locations.All.Find(s => s.FullName == locationFullName)!;
    }

    public static string GetLocationFullName(Position position)
    {
        return $"{position.Region}_{position.Subregion}_{position.Land}_{position.Location}";
    }

    public static Position GetPositionByFullName(string fullName)
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

    public static Position GetPositionByLocation(string location)
    {
        var locationFullName = GameplayLore.Locations.All.Select(s => s.FullName).ToList().Find(s => s == location) ?? throw new Exception("Location not found.");

        return GetPositionByFullName(locationFullName);
    }

    public static Character GetPlayerCharacter(IDatabaseService dbs, CharacterIdentity identity)
    {
        return dbs.Snapshot.Players.Find(s => s.Identity.Id == identity.PlayerId)!.Characters.Find(s => s.Identity.Id == identity.Id)!;
    }
}
