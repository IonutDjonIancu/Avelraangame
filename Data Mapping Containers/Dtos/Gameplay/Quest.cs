namespace Data_Mapping_Containers.Dtos;

public class Quest
{
    public string Id { get; set; }
    public string PartyId { get; set; }

    public bool IsInEncounter { get; set; }
    public List<QuestEncounter> Encounters { get; set; } = new();
    public QuestReward Reward { get; set; } = new();

    // these are hardcoded in the QuestsLore
    public string Name { get; set; }
    public string Description { get; set; }
    public string Difficulty { get; set; }
    public string Effort { get; set; }
    public QuestRatio Ratio { get; set; } = new();
    public Position Position { get; set; } = new();
    public bool IsRepeatable { get; set; }
}
