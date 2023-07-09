namespace Data_Mapping_Containers.Dtos;

public class PositionTravel
{
    public CharacterIdentity CharacterIdentity { get; set; } = new();
    public Position Destination { get; set; }
}
