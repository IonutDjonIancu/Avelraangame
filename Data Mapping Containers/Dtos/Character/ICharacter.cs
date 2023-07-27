using Data_Mapping_Containers.Pocos;

namespace Data_Mapping_Containers.Dtos;

public interface ICharacter
{
    public CharacterIdentity Identity { get; set; }
    public CharacterInfo Info { get; set; }
    public CharacterStatus Status { get; set; }
    public Position Position { get; set; }

    public CharacterLevelUp LevelUp { get; set; }
    public CharacterSheet Sheet { get; set; }

    public CharacterInventory Inventory { get; set; }
    public List<Item> Supplies { get; set; }

    public List<HeroicTrait> HeroicTraits { get; set; }
}
