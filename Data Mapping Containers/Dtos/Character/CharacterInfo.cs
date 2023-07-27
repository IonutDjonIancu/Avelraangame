namespace Data_Mapping_Containers.Dtos;

public class CharacterInfo
{
    public string Name { get; set; }
    public int EntityLevel { get; set; }
    public string DateOfBirth { get; set; }
    public CharacterOrigins Origins { get; set; } = new();
    public bool IsAlive { get; set; }
    public bool IsNpc { get; set; }

    public int Wealth { get; set; }

    public string Fame { get; set; }
    public int NrOfQuestsFinished { get; set; }
    public List<string> QuestsFinished { get; set; } = new();
}
