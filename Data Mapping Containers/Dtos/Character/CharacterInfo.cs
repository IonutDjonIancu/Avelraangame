namespace Data_Mapping_Containers.Dtos;

public class CharacterInfo
{
    public string? Name { get; set; }
    public int? EntityLevel { get; set; }

    public string Race { get; set; }
    public string Culture { get; set; }
    public string Tradition { get; set; }
    public string Class { get; set; }

    public string? Fame { get; set; }
    public int? Wealth { get; set; }

    public string? DateOfBirth { get; set; }
}
