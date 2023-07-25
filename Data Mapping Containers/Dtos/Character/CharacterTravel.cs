namespace Data_Mapping_Containers.Dtos;

public class CharacterTravel
{
    public CharacterIdentity CharacterIdentity { get; set; } = new();
    public Position Destination { get; set; }
}
