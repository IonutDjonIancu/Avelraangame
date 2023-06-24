namespace Data_Mapping_Containers.Dtos;

public class Reward
{
    public string Id { get; set; }

    public List<Item> Items { get; set; } = new();
    public int Wealth { get; set; }

    public List<HeroicTrait> Traits { get; set; } = new();
}
