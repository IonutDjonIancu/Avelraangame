namespace Data_Mapping_Containers.Dtos;

public class CharacterStatus
{
    public string Name { get; set; }
    public int EntityLevel { get; set; }
    public string DateOfBirth { get; set; }
    public bool IsNpc { get; set; }
    public bool IsAlive { get; set; }
    public bool IsLockedToModify { get; set; }

    public CharacterTraits Traits { get; set; } = new();
    public CharacterGameplay Gameplay { get; set; } = new();
    public Position Position { get; set; } = new();

    public int Worth { get; set; }
    public int Wealth { get; set; }
    public string Fame { get; set; }

    public int NrOfQuestsFinished { get; set; }
    public List<string> QuestsFinished { get; set; } = new();
}
