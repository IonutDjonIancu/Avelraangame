using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Service_Delegators;

public static class ServicesUtils
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

    public static Location GetLoreLocationByPosition(Position position)
    {
        return GameplayLore.Locations.All.Find(s => s.FullName == position.GetPositionFullName())!;
    }

    public static Location GetSnapshotLocationByPosition(Position position, Snapshot snapshot)
    {
        return snapshot.Locations.Find(s => s.FullName == GetLocationFullNameFromPosition(position)) ?? throw new Exception("Location not found.");
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
        var player = snapshot.Players.Find(s => s.Identity.Id == identity.PlayerId) ?? throw new Exception("Player not found.");

        var character = player.Characters.Find(s => s.Identity.Id == identity.Id);
        if (character != null) return character;

        foreach (var chara in player.Characters)
        {
            character = chara.Mercenaries.Find(s => s.Identity.Id == identity.Id);
            if (character != null) return character;
        }

        throw new Exception("Character not found.");
    }
}
