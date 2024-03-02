using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;
using static Service_Delegators.Enums;

namespace Service_Delegators;

public static class ServicesUtils
{
    public static Location GetLocationByLocationName(string locationName)
    {
        var locationFullName = GameplayLore.Locations.All.Select(s => s.FullName).ToList().Find(s => s.Contains(locationName)) ?? throw new Exception("Location not found.");

        return GameplayLore.Locations.All.Find(s => s.FullName == locationFullName)!;
    }

    public static Location GetLocationByPositionFullName(string locationFullName)
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

    public static Difficulty GetDifficultyFromEffort(int effort)
    {
        return effort switch
        {
            <= 50 => Difficulty.Normal,
            <= 100 => Difficulty.Gifted,
            <= 200 => Difficulty.Chosen,
            <= 500 => Difficulty.Hero,
            <= 1000 => Difficulty.Olympian,
            _ => Difficulty.Planar,
        };
    }

    public static int CalculateWorth(Character character, IDiceLogicDelegator dice)
    {
        var location = GetLoreLocationByPosition(character.Status.Position);

        int[] stats =
        {
            character.Sheet.Stats.Strength,
            character.Sheet.Stats.Constitution,
            character.Sheet.Stats.Agility,
            character.Sheet.Stats.Willpower,
            character.Sheet.Stats.Perception,
            character.Sheet.Stats.Abstract
        };

        int[] skills =
        {
            character.Sheet.Skills.Arcane,
            character.Sheet.Skills.Psionics,
            character.Sheet.Skills.Hide,
            character.Sheet.Skills.Traps,
            character.Sheet.Skills.Tactics,
            character.Sheet.Skills.Social,
            character.Sheet.Skills.Apothecary,
            character.Sheet.Skills.Travel,
            character.Sheet.Skills.Sail
        };

        int[] assets =
        {
            character.Sheet.Assets.Harm,
            character.Sheet.Assets.Spot,
            (int)character.Sheet.Assets.Purge,
            (int)character.Sheet.Assets.Defense,
            (int)character.Sheet.Assets.DefenseFinal,
            character.Sheet.Assets.Resolve,
            character.Sheet.Assets.ResolveLeft,
            character.Sheet.Assets.Mana,
            character.Sheet.Assets.ManaLeft,
            (int)character.Sheet.Assets.Actions,
            (int)character.Sheet.Assets.ActionsLeft
        };

        var statsAvg = stats.Average();
        var assetsAvg = assets.Average();
        var skillsAvg = skills.Average();
        var allAverage = (statsAvg + skillsAvg + assetsAvg) / 3;

        return (int)(location.Effort + allAverage);
    }
}
