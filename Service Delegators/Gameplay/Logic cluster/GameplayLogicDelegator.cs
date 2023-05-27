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
        SanitizePartiesOnCreate();

        var party = new Party
        {
            Id = Guid.NewGuid().ToString(),
            CreationDate = DateTime.Now.ToShortDateString(),
            IsAdventuring = false
        };

        dbs.Snapshot.Parties.Add(party);

        dbs.PersistDatabase();

        return party;
    }

    #region private methods
    private void SanitizePartiesOnCreate()
    {
        var sevenDaysAgo = DateTime.Now - DateTime.Now.AddDays(-7);

        var oldParties = dbs.Snapshot.Parties.Where(s => DateTime.Parse(s.CreationDate) < DateTime.Now - sevenDaysAgo && !s.IsAdventuring).ToList();
        if (oldParties.Count == 0) return;

        oldParties.ForEach(s => dbs.Snapshot.Parties.Remove(s));
    }
    #endregion
}
