using Data_Mapping_Containers.Dtos;

namespace Data_Mapping_Containers;

public class CharacterPaperdoll
{
    public CharacterStats Stats { get; set; }
    public CharacterAssets Assets { get; set; }
    public CharacterSkills Skills { get; set; }

    // these are the activate Heroic Traits which can be used during combat
    public List<HeroicTrait> SpecialSkills { get; set; }
}
