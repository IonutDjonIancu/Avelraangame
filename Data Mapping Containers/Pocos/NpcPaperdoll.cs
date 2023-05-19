namespace Data_Mapping_Containers.Dtos;

public class NpcPaperdoll
{
    public CharacterPaperdoll Paperdoll { get; set; }
    public List<Item> Items { get; set; }
    public int Wealth { get; set; }
}
