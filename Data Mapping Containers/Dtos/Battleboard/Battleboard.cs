namespace Data_Mapping_Containers.Dtos;

public class Battleboard
{
    public string Id { get; set; }

    public bool IsInCombat { get; set; }
    public List<string> BattleOrder { get; set; }

    public string GoodGuyPartyLead { get; set; }
    public List<Character> GoodGuys { get; set; } = new();

    public string BadGuyPartyLead { get; set; }
    public List<Character> BadGuys { get; set; } = new();
}
