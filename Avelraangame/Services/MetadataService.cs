using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Avelraangame;

public interface IMetadataService
{
    List<Location> GetAllLocations();
    List<string> GetRaces();
    List<List<string>> GetCultures();
    List<string> GetClasses();
    Player GetPlayer(string playerId);
    Players GetPlayers();
    List<SpecialSkill> GetSpecialSkills();
    CharacterTraits GetTraits();
}

public class MetadataService : IMetadataService
{
    private readonly object _lock = new();
    private readonly Snapshot snapshot;

    public MetadataService(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    #region player data
    public Players GetPlayers()
    {
        lock (_lock)
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
        lock (_lock)
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

    public List<List<string>> GetCultures()
    {
        return CharactersLore.Cultures.All;
    }

    public List<string> GetClasses()
    {
        return CharactersLore.Classes.All;
    }

    public CharacterTraits GetTraits()
    {
        return new CharacterTraits
        {
            Races = CharactersLore.Races.Playable.All,
            Cultures = new CharacterCultures
            {
                Human = CharactersLore.Cultures.Human.All,
                Elf = CharactersLore.Cultures.Elf.All,
                Dwarf = CharactersLore.Cultures.Dwarf.All,
                Orc = CharactersLore.Cultures.Orc.All,
            },
            Classes = CharactersLore.Classes.All,
            Traditions = CharactersLore.Tradition.All
        };
    }
    #endregion

    #region gameplay data
    public List<Location> GetAllLocations()
    {
        return GameplayLore.Locations.All;
    }
    #endregion
}
