namespace Data_Mapping_Containers.Dtos;

public class Warparty
{
    public string Id { get; set; }
    public List<CharacterPaperdoll> Paperdolls { get; set; }
    public List<Item> Loot { get; set; }
}
