namespace Data_Mapping_Containers.Dtos;

public class DbRequestsInfo
{
    public string Password { get; set; }
    public string Secret { get; set; }

    public string? SnapshotJsonString { get; set; }
    public string? PlayerJsonString { get; set; }

    public bool IsShortOperation { get; set; } = false;
}
