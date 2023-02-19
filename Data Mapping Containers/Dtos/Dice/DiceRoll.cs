namespace Data_Mapping_Containers.Dtos;

public class DiceRoll
{
    public int Roll { get; set; }
    public int Grade { get; set; }
    public List<int>? Dice { get; set; }
    public int Crits { get; set; }
}
