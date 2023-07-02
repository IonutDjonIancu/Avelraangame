namespace Data_Mapping_Containers.Dtos;

public class QuestReward
{
    public string Id { get; set; }
    public string Description { get; set; }

    public List<Item> Items { get; set; } = new();
    public List<HeroicTrait> Traits { get; set; } = new();
    
    public int Wealth { get; set; }
    public int StatPoints { get; set; }
    public int SkillPoints { get; set; }
}
