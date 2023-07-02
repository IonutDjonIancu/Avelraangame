using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class MetadataService : IMetadataService
{
    private readonly IDatabaseService dbs;

    public MetadataService(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

    public Players GetPlayers()
    {
        var players = new Players
        {
            Count = dbs.Snapshot.Players.Count
        };

        foreach (var player in dbs.Snapshot.Players)
        {
            players.PlayerNames.Add(player.Identity.Name);
        }

        return players;
    }

    public List<string> GetRaces()
    {
        return CharactersLore.Races.All;
    }

    public List<string> GetCultures()
    {
        return CharactersLore.Cultures.All;
    }

    public List<string> GetClasses()
    {
        return CharactersLore.Classes.All;
    }

    public List<HeroicTrait> GetHeroicTraits()
    {
        return TraitsLore.All;
    }

    public List<Party> GetParties()
    {

        return dbs.Snapshot.Parties;
    }

    public List<string> GetAvelraanRegions()
    {
        throw new NotImplementedException();
    }
}
