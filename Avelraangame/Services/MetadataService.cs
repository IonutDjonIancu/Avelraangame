using Data_Mapping_Containers.Dtos;

namespace Avelraangame;

public interface IMetadataService
{
    List<Location> GetAllLocations();
    List<string> GetClasses();
    List<string> GetCultures();
    Player GetPlayer(string playerId);
    Players GetPlayers();
    List<string> GetRaces();
    List<SpecialSkill> GetSpecialSkills();
}

public class MetadataService : IMetadataService
{
    private readonly object playersLock = new();
    private readonly Snapshot snapshot;

    public MetadataService(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    #region player data
    public Players GetPlayers()
    {
        lock (playersLock)
        {
            return new Players
            {
                Count = snapshot.Players.Count,
                PlayerNames = snapshot.Players.Select(s => s.Identity.Name).ToList()
            };
        }
    }

    public Player GetPlayer(string playerId)
    {
        lock (playersLock)
        {
            return snapshot.Players.Find(s => s.Identity.Id == playerId) ?? throw new Exception("Player not found.");
        }
    }

    public List<SpecialSkill> GetSpecialSkills()
    {
        return SpecialSkillsLore.All;
    }
    #endregion

    #region character data
    public List<string> GetRaces()
    {
        return CharactersLore.Races.Playable.All;
    }

    public List<string> GetCultures()
    {
        return CharactersLore.Cultures.All;
    }

    public List<string> GetClasses()
    {
        return CharactersLore.Classes.All;
    }
    #endregion

    #region gameplay data
    public List<Location> GetAllLocations()
    {
        return GameplayLore.Locations.All;
    }
    #endregion
}
