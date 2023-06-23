namespace Data_Mapping_Containers.Dtos;

public class Quest
{
    public string Id { get; set; }

    public string Location { get; set; }
    public string Difficulty { get; set; }
    public string Effort { get; set; }

    public string Description { get; set; }
    public Dictionary<string, string> Encounters { get; set; }



}
