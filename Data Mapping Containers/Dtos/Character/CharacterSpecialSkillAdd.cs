namespace Data_Mapping_Containers.Dtos;

public class CharacterSpecialSkillAdd
{
    public CharacterIdentity CharacterIdentity { get; set; } = new();
    public string SpecialSkillId { get; set; }
    public string Subskill { get; set; }
}
