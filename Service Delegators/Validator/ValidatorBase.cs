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

    internal void ValidateCharacterPlayerCombination(CharacterIdentity characterIdentity)
    {
        ValidateGuid(characterIdentity.Id);
        ValidateGuid(characterIdentity.PlayerId);

        var player = snapshot.Players.Find(p => p.Identity.Id == characterIdentity.PlayerId)!;

        if (!player.Characters.Exists(c => c.Identity!.Id == characterIdentity.Id)) Throw("Character not found.");
    }

    internal void ValidateIfPlayerExists(string playerId)
    {
        ValidateGuid(playerId);
        if (!snapshot.Players.Exists(p => p.Identity.Id == playerId)) Throw("Player not found");
    }

    internal void ValidateString(string str, string message = "")
    {
        if (string.IsNullOrWhiteSpace(str)) Throw(message.Length > 0 ? message : "The provided string is invalid.");
    }

    internal static void ValidateObject(object? obj, string message = "")
    {
        if (obj == null) Throw(message.Length > 0 ? message : $"Object found null.");
    }

    internal void ValidateNumber(int num, string message = "")
    {
        if (num <= 0) Throw(message.Length > 0 ? message : "Number cannot be smaller or equal to zero.");
    }

    internal void ValidateGuid(string str, string message = "")
    {
        ValidateString(str);

        var isGuidValid = Guid.TryParse(str, out var id);

        if (!isGuidValid) Throw(message.Length > 0 ? message : "Invalid guid.");

        if (id == Guid.Empty) Throw("Guid cannot be an empty guid.");
    }

    internal static void Throw(string message)
    {
        throw new Exception(message);
    }
}

#pragma warning restore CA1822 // Mark members as static