namespace Service_Delegators;

internal class PlayerOperationsLogic
{
    private readonly IDatabaseService dbs;

    private PlayerOperationsLogic() { }
    internal PlayerOperationsLogic(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

    internal void RemovePlayer(string playerId)
    {
        var player = dbs.Snapshot.Players!.Find(p => p.Identity.Id == playerId);

        dbs.Snapshot.Players!.Remove(player!);

        dbs.DeletePlayer(playerId);
    }
}
