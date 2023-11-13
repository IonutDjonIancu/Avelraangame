namespace Data_Mapping_Containers.Dtos;

public class Quest
{
    public int EffortLevel { get; set; }

    public string Description { get; set; }

    public int EncountersLeft { get; set; }

    public bool IsRepeatable { get; set; }
    public string Reward { get; set; }

}
