namespace Data_Mapping_Containers.Dtos;

public class Location
{
    // hardcoded in the gameplay lore file
    public string Name { get; set; }
    public string FullName { get; set; }
    public Position Position { get; set; }

    public string Description { get; set; }
    public int Effort { get; set; }
    public int TravelCostFromArada { get; set; }

    // dynamically generated at player visit on get
    public string LastTimeVisited { get; set; }
    public List<Item> Market { get; set; } = new();
    public List<Character> Mercenaries { get; set; } = new();
    public List<Quest> Quests { get; set; } = new();
}
