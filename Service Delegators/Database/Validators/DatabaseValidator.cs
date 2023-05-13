using Persistance_Manager;

namespace Service_Delegators;

internal class DatabaseValidator : ValidatorBase
{
    private readonly IDatabaseManager dbm;

    internal DatabaseValidator(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
    }

    internal void ValidatePlayerIsAdmin(string playerId)
    {
        var playerName = dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId)!.Identity.Name;

        if (!dbm.Snapshot.Admins.Contains(playerName)) Throw("Action not allowed, player is not an admin.");
    }
}
