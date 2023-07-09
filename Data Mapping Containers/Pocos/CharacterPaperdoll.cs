using System.Data;

namespace Data_Mapping_Containers.Dtos;

public class CharacterPaperdoll
{
    public string? PlayerId { get; set; }
    public string? CharacterId { get; set; }
    public string? Name { get; set; }
    public string? Race { get; set; }

    public CharacterLevelUp LevelUp { get; set; } = new();

    public CharacterStats Stats { get; set; } = new();
    public CharacterAssets Assets { get; set; } = new();
    public CharacterSkills Skills { get; set; } = new();
    public int ActionTokens { get; set; }

    // these are the activate Heroic Traits which can be used during combat
    public List<HeroicTrait> SpecialSkills { get; set; } = new();

    public List<Item> Inventory { get; set; } = new();

    public int Wealth { get; set; }
}
