namespace Data_Mapping_Containers.Dtos;

/// <summary>
/// This is the application's cache store.
/// </summary>
public class Snapshot
{
    public List<CharacterStub> Stubs { get; init; } = new();

    public List<Player> Players { get; init; } = new();

    public List<Location> Locations { get; init; } = new();

    public List<Battleboard> Battleboards { get; init; }
}
