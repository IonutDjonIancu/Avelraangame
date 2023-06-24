namespace Data_Mapping_Containers.Dtos;

public class Encounter
{
    public string Id { get; set; }
    public List<CharacterPaperdoll> GoodGuys { get; set; } = new();

}
