namespace Data_Mapping_Containers.Dtos;

public class CharacterIncreaseAttributes
{
    public CharacterIdentity CharacterIdentity { get; set; } = new();

    public string Stat { get; set; }
    public string Asset { get; set; }
    public string Skill { get; set; }
}
