using Data_Mapping_Containers.Pocos;

namespace Data_Mapping_Containers.Dtos;

public class CharacterBase
{
    public CharacterIdentity Identity { get; set; } = new();
    public CharacterInfo Info { get; set; } = new();
    public CharacterStatus Status { get; set; } = new();
    public Position Position { get; set; } = new();

    public CharacterLevelUp LevelUp { get; set; } = new();
    public CharacterSheet Sheet { get; set; } = new();

    public CharacterInventory Inventory { get; set; } = new();
    public List<Item> Supplies { get; set; } = new();

    public List<HeroicTrait> HeroicTraits { get; set; } = new();
}
