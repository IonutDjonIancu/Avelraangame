namespace Data_Mapping_Containers.Dtos;

public class Party
{
    public string Id { get; set; }
    public string CreationDate { get; set; }
    public bool IsAdventuring { get; set; }
    public string PartyLeadId { get; set; }
    public List<PartyMember> PartyMembers { get; set; } = new();
}
