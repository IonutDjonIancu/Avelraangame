namespace Data_Mapping_Containers.Dtos;

public class CharacterSheet
{
    public CharacterStats Stats { get; set; } = new();
    public CharacterAssets Assets { get; set; } = new();
    public CharacterSkills Skills { get; set; } = new();
    
    public List<SpecialSkill> SpecialSkills { get; set; } = new();
    //TODO: must add NegativePerks
}
