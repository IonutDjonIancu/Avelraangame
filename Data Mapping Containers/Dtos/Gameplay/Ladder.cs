namespace Data_Mapping_Containers.Dtos;

public class Ladder
{
    public List<CharacterLadder> CharactersByWorth { get; set; } = new();
    public List<CharacterLadder> CharactersByWealth { get; set; } = new();
}
