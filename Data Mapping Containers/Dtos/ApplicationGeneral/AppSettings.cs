namespace Data_Mapping_Containers.Dtos;

public class AppSettings
{
    public string DbPath { get; set; }
    public string DbTestPath { get; set; }
    public string DbPlayersPath { get; set; }
    public string LogPath { get; set; }
    public string AvelraanEmail { get; set; }
    public string AvelraanEmailPass { get; set; }
    public string AveelraanSecretKey { get; set; }

    public AdminData AdminData { get; set; }
}
