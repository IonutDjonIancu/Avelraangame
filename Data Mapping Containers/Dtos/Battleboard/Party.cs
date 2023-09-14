namespace Data_Mapping_Containers.Dtos;

public class Party
{
    public List<Character> Characters { get; set; } = new();

    public List<string> BattleFormation { get; set; } = new();

    public string PartyLeadId { get; set; }
}
