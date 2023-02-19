namespace Data_Mapping_Containers.Dtos;

public class Player
{
    public PlayerIdentity Identity { get; set; }

    public string? LastAction { get; set; }
    public bool? IsAdmin { get; set; }

    public List<Character>? Characters { get; set; }
}
