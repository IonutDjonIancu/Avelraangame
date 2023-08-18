namespace Data_Mapping_Containers.Dtos;

public class Position
{
    public string Region { get; set; }
    public string Subregion { get; set; }
    public string Land { get; set; }
    public string Location { get; set; }

    public string GetPositionFullName()
    {
        return $"{Region}_{Subregion}_{Land}_{Location}";
    }
}
