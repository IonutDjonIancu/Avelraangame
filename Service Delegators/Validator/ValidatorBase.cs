#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class ValidatorBase
{
    private readonly Snapshot snapshot;

    private ValidatorBase() { }
    internal ValidatorBase(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    internal Character GetCharacter(CharacterIdentity characterIdentity)
    {
        var character = snapshot.Players.Find(p => p.Identity.Id == characterIdentity.PlayerId)!.Characters.Find(c => c.Identity.Id == characterIdentity.Id)! ?? throw new Exception("Character not found.");

        return character;
    }

    internal void ValidatePosition(Position position)
    {
        ValidateObject(position, "Position is null.");

        var fullName = Utils.GetLocationFullName(position);

        if (!GameplayLore.Locations.All.Select(s => s.FullName).ToList().Contains(fullName)) throw new Exception("Position data is wrong or incomplete.");
    }

    internal void ValidateCharacterPlayerCombination(CharacterIdentity characterIdentity)
    {
        ValidateGuid(characterIdentity.Id);
        ValidateGuid(characterIdentity.PlayerId);

        var player = snapshot.Players.Find(p => p.Identity.Id == characterIdentity.PlayerId)!;

        if (!player.Characters.Exists(c => c.Identity!.Id == characterIdentity.Id)) throw new Exception("Character not found.");
    }

    internal void ValidateIfPlayerExists(string playerId)
    {
        ValidateGuid(playerId);
        if (!snapshot.Players.Exists(p => p.Identity.Id == playerId)) throw new Exception("Player not found");
    }

    internal void ValidateString(string str, string message = "")
    {
        if (string.IsNullOrWhiteSpace(str)) throw new Exception(message.Length > 0 ? message : "The provided string is invalid.");
    }

    internal static void ValidateObject(object? obj, string message = "")
    {
        if (obj == null) throw new Exception(message.Length > 0 ? message : $"Object found null.");
    }

    internal void ValidateNumber(int num, string message = "")
    {
        if (num <= 0) throw new Exception(message.Length > 0 ? message : "Number cannot be smaller or equal to zero.");
    }

    internal void ValidateGuid(string str, string message = "")
    {
        ValidateString(str);

        var isGuidValid = Guid.TryParse(str, out var id);

        if (!isGuidValid) throw new Exception(message.Length > 0 ? message : "Invalid guid.");

        if (id == Guid.Empty) throw new Exception("Guid cannot be an empty guid.");
    }
}

#pragma warning restore CA1822 // Mark members as static