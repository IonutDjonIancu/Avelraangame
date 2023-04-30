namespace Data_Mapping_Containers.Dtos;

public class NpcPaperdoll
{
    public CharacterPaperdoll Paperdoll { get; set; }
    public int Money { get; set; }
    public List<Item> Items { get; set; }
}
