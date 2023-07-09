namespace Data_Mapping_Containers.Dtos;

public class Location
{
    // hardcoded in the gameplay lore file
    public string Name { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }
    public string Effort { get; set; }
    public int EffortLevel { get; set; }
    public int TravelToCost { get; set; }

    // dynamically generated at player visit on get
    public DateTime LastTimeVisited { get; set; }
    public List<string> PossibleQuests { get; set; } = new();
    public List<Item> Market { get; set; } = new();
    public List<NpcCharacter> Npcs { get; set; } = new();
}
