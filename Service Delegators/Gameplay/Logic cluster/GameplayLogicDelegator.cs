using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayLogicDelegator
{
    private readonly IDiceRollService diceService;
    private readonly IDatabaseService dbs;

    private GameplayLogicDelegator() { }
    internal GameplayLogicDelegator(
        IDatabaseService databaseService,
        IDiceRollService diceRollService)
    {
        dbs = databaseService;
        diceService = diceRollService;
    }

    internal Party CreateParty()
    {
        var party = new Party
        {
            Id = Guid.NewGuid().ToString(),
            CreationDate = DateTime.Now.ToShortDateString(),
        };

        dbs.Snapshot.Parties.Add(party);

        dbs.PersistDatabase();

        return party;
    }
}
