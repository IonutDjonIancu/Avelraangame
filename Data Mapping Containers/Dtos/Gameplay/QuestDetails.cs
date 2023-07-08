namespace Data_Mapping_Containers.Dtos;

public class QuestDetails
{
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string RewardDescription { get; set; }
    public bool IsRepeatable { get; set; }
    public int EffortRequired { get; set; }
    /// <summary>
    /// This refers to the Land that this quest will be generated
    /// </summary>
    public List<string> AvailableAt { get; set; }
    public QuestReward Reward { get; set; }
}
