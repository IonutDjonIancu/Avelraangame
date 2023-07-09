namespace Data_Mapping_Containers.Dtos;

public class Quest
{
    public string Id { get; set; }
    public Character PartyLead { get; set; }

    public Position Position { get; set; } = new();
    public List<QuestEncounter> Encounters { get; set; } = new();
    public string Effort { get; set; }
    public bool IsOngoing { get; set; }

    public QuestDetails Details { get; set; } = new();
}
