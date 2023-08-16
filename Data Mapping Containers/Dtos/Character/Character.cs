using Data_Mapping_Containers.Pocos;

namespace Data_Mapping_Containers.Dtos;

public class Character : ICharacter
{
    public CharacterIdentity Identity { get; set; } = new();
    public CharacterStatus Status { get; set; } = new();
    
    public CharacterLevelUp LevelUp { get; set; } = new();
    public CharacterSheet Sheet { get; set; } = new();

    public CharacterInventory Inventory { get; set; } = new();
    public List<Character> Mercenaries { get; set; } = new();
}
