namespace Data_Mapping_Containers.Dtos;

public class DatabaseSnapshot
{
    public DateTime DbDate { get; set; }
    public List<CharacterStub>? CharacterStubs { get; set; }

    public List<Player> Players { get; set; }
    public List<Item>? Items { get; set; }
    public List<CharacterTrait>? Traits { get; set; }
}
