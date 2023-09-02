using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public static class Utils
{
    public static Location GetLocationByLocationName(string locationName)
    {
        var locationFullName = GameplayLore.Locations.All.Select(s => s.FullName).ToList().Find(s => s.Contains(locationName)) ?? throw new Exception("Location not found.");

        return GameplayLore.Locations.All.Find(s => s.FullName == locationFullName)!;
    }

    public static Location GetLocationByLocationFullName(string locationFullName)
    {
        return GameplayLore.Locations.All.Find(s => s.FullName == locationFullName)!;
    }

    public static Location GetLocationByPosition(Position position)
    {
        return GameplayLore.Locations.All.Find(s => s.FullName == position.GetPositionFullName())!;
    }

    public static string GetLocationFullNameFromPosition(Position position)
    {
        return $"{position.Region}_{position.Subregion}_{position.Land}_{position.Location}";
    }

    public static Position GetPositionByLocationFullName(string fullName)
    {
        return GameplayLore.Locations.All.Find(s => s.FullName == fullName)!.Position ?? throw new Exception("Location not found.");
    }

    public static Character GetPlayerCharacter(CharacterIdentity identity, Snapshot snapshot)
    {
        var player = snapshot.Players.Find(p => p.Identity.Id == identity.PlayerId) ?? throw new Exception("Player not found.");
        return player.Characters.Find(p => p.Identity!.Id == identity.Id) ?? throw new Exception("Character not found."); 
    }
}
