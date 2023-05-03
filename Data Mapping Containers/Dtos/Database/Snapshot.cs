namespace Data_Mapping_Containers.Dtos;

public class Snapshot
{
    public DateTime DbDate { get; set; }
    public List<CharacterStub> CharacterStubs { get; set; }
    public List<Party> Parties { get; set; }


    public List<Player> Players { get; set; }
    public List<Item> Items { get; set; }
    public List<HeroicTrait> Traits { get; set; }


    public Rulebook Rulebook { get; set; }
}
