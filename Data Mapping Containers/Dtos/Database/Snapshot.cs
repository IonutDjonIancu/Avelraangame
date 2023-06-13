namespace Data_Mapping_Containers.Dtos;

public class Snapshot
{
    public DateTime LastAction { get; set; }
    public List<string> Admins { get; set; }
    public List<string> Banned { get; set; }
    public List<CharacterStub> CharacterStubs { get; set; }
    public List<Party> Parties { get; set; }
    public List<Warparty> Warparties { get; set; }

    public List<Player> Players { get; set; }
    public List<Item> Items { get; set; }
}
