namespace Data_Mapping_Containers.Dtos;

public class CharacterHeroicTrait
{
    public CharacterIdentity CharacterIdentity { get; set; } = new();
    public string HeroicTraitId { get; set; }
    public string Skill { get; set; }
}
