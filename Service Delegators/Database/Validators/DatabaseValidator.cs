using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class DatabaseValidator : ValidatorBase
{
    private readonly Snapshot snapshot;

    internal DatabaseValidator(Snapshot snapshot)
        : base(snapshot)
    {
        this.snapshot = snapshot;
    }

    internal void ValidatePlayerIsAdmin(string playerId)
    {
        var playerName = snapshot.Players.Find(p => p.Identity.Id == playerId)!.Identity.Name;

        if (!snapshot.Admins.Contains(playerName)) throw new Exception("Action not allowed, player is not an admin.");
    }
}
