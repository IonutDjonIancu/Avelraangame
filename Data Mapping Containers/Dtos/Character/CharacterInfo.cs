namespace Data_Mapping_Containers.Dtos;

public class CharacterInfo
{
    public string Name { get; set; }

    public int EntityLevel { get; set; }

    public CharacterOrigins Origins { get; set; } = new();

    public string Fame { get; set; }
    public int Wealth { get; set; }

    public string DateOfBirth { get; set; }

    public bool IsAlive { get; set; }
    public bool IsInParty { get; set; }
}
