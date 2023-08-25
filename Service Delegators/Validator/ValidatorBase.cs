#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class ValidatorBase
{
    private readonly SnapshotOld snapshot;

    private ValidatorBase() { }
    internal ValidatorBase(SnapshotOld snapshot)
    {
        this.snapshot = snapshot;
    }

    internal void ValidateCharacterBeforeRoll(Character character, string skill)
    {
        ValidateObject(character);

        if (!character.Status.IsAlive) throw new Exception("Character is dead or dying.");

        ValidateString(character.Status.Traits.Tradition);
        if (!GameplayLore.Tradition.All.Contains(character.Status.Traits.Tradition)) throw new Exception("Unrecognized tradition.");

        ValidateString(skill);
        if (!CharactersLore.Skills.All.Contains(skill)) throw new Exception("Skill does not exist.");
    }

    internal void ValidateLocation(string locationName)
    {
        ValidateString(locationName);

        _ = Utils.GetLocationByLocationName(locationName) ?? throw new Exception("Wrong location name.");
    }

    internal void ValidatePosition(Position position)
    {
        ValidateObject(position);

        var locationFullName = Utils.GetLocationFullNameFromPosition(position);
        _ = Utils.GetPositionByLocationFullName(locationFullName) ?? throw new Exception("Position data is wrong or incomplete.");
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