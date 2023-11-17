namespace Data_Mapping_Containers.Dtos;

public class Quest
{
    public string QuestType { get; set; }
    public string Fame { get; set; }
    public string Description { get; set; }
    public string Result { get; set; }
    public bool IsRepeatable { get; set; }

    // dynamically generated when the party picks up the quest
    public string Id { get; set; }
    public int EffortLvl { get; set; }
    public int Encounters { get; set; }
    public int EncountersLeft { get; set; }
    public string Reward { get; set; }
}
