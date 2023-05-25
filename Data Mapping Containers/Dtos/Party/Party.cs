namespace Data_Mapping_Containers.Dtos;

public class Party
{
    public string Id { get; set; }
    public string CreationDate { get; set; }
    public string PartyLeadId { get; set; }
    public List<string> CharacterIds { get; set; } = new();
}
