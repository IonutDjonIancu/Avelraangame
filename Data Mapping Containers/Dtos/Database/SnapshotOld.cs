namespace Data_Mapping_Containers.Dtos;

public class SnapshotOld
{
    public DateTime LastAction { get; set; }
    public List<string> Admins { get; set; }
    public List<string> Banned { get; set; }
    public List<CharacterStub> CharacterStubs { get; set; }

    public List<Player> Players { get; set; }

    public List<Location> Locations { get; set; }
}
