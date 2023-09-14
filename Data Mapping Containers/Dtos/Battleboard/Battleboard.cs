namespace Data_Mapping_Containers.Dtos;

public class Battleboard
{
    public string Id { get; set; }
    public bool IsInCombat { get; set; }

    public Party GoodGuys { get; set; } = new();
    public Party BadGuys { get; set; } = new();

    public List<string> BattleOrder { get; set; } = new();
}
