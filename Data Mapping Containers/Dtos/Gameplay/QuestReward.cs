namespace Data_Mapping_Containers.Dtos;

public class QuestReward
{
    public bool HasWealth { get; set; }
    public bool HasItem { get; set; }
    public bool HasLoot { get; set; } // several items
    public bool HasStats { get; set; }
    public bool HasSkills { get; set; }
    public bool HasTraits { get; set; }
}
