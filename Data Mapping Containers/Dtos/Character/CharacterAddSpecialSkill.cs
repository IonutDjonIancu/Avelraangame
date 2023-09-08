namespace Data_Mapping_Containers.Dtos;

public class CharacterAddSpecialSkill
{
    public CharacterIdentity CharacterIdentity { get; set; } = new();
    public string SpecialSkillId { get; set; }
    public string AppliesToSkill { get; set; }
}
