using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class DatabaseImportLogic
{
    private readonly IDatabaseManager dbm;

    private DatabaseImportLogic() { }
    internal DatabaseImportLogic(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
    }

    internal void ImportDatabase(Database database)
    {
        dbm.Snapshot.LastAction = database.DbDate;
        dbm.Snapshot.CharacterStubs = database.CharacterStubs;
        dbm.Snapshot.Parties = database.Parties;
    }

    internal void ImportPlayer(Player newPlayer)
    {
        var oldPlayer = dbm.Snapshot.Players.Find(p => p.Identity.Id == newPlayer.Identity.Id);

        if (oldPlayer != null)
        {
            dbm.Snapshot.Players.Remove(oldPlayer);
        }

        dbm.Snapshot.Players.Add(newPlayer);
    }
}
