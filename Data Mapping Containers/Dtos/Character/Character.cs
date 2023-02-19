using Data_Mapping_Containers.Pocos;

namespace Data_Mapping_Containers.Dtos;

public class Character
{
    public CharacterIdentity? Identity { get; set; }
    public CharacterInfo? Info { get; set; }

    public bool HasLevelUp { get; set; }
    public CharacterLevelUp? LevelUp { get; set; }

    public CharacterPaperDoll? Doll { get; set; }
    public List<CharacterTrait>? Traits { get; set; }
    
    public CharacterInventory? Inventory { get; set; }
    public List<Item>? Supplies { get; set; }
    
    public bool IsAlive { get; set; }
}
