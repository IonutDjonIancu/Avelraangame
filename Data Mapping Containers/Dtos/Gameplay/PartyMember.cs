namespace Data_Mapping_Containers.Dtos;

public class PartyMember
{
    public string PlayerId { get; set; }
    public List<string> CharacterIds { get; set; } = new();
}
