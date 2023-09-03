namespace Data_Mapping_Containers.Dtos;

public class SpecialSkill
{
    public SpecialSkillIdentity Identity { get; set; }

    public string Description { get; set; }
    public string Lore { get; set; }
    
    public string Type { get; set; }
    public string Subtype { get; set; }
    public string Category { get; set; }

    public int DeedsCost { get; set; }
}
