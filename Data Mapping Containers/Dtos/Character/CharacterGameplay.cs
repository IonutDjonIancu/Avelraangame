namespace Data_Mapping_Containers.Dtos;

public class CharacterGameplay
{
    public bool IsNpc { get; set; }
    public bool IsAlive { get; set; }
    public bool IsLocked { get; set; }
    public bool IsHidden { get; set; }

    public string BattleboardId { get; set; }
    public bool IsGoodGuy { get; set; }
}
