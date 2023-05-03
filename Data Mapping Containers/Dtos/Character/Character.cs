﻿using Data_Mapping_Containers.Pocos;

namespace Data_Mapping_Containers.Dtos;

public class Character
{
    public CharacterIdentity? Identity { get; set; }
    public CharacterInfo? Info { get; set; }

    public CharacterLevelUp? LevelUp { get; set; }

    public CharacterSheet? Sheet { get; set; }
    
    public CharacterInventory? Inventory { get; set; }
    public List<Item>? Supplies { get; set; }

    public List<HeroicTrait> HeroicTraits { get; set; }

    public bool IsAlive { get; set; }
    public bool IsInParty { get; set; }
}
