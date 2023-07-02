namespace Avelraangame;

public class AppSettingsConfigManager : IAppSettingsConfigManager
{
    private readonly IConfiguration configuration;

	public AppSettingsConfigManager(IConfiguration config)
	{
		configuration = config;	
	}

    public string DbPath                { get { return configuration["AppSettings:DbPath"]; } }
    public string DbTestPath            { get { return configuration["AppSettings:DbTestPath"]; } }
    public string DbPlayersPath         { get { return configuration["AppSettings:DbPlayersPath"]; } }
    public string DbMapPath             { get { return configuration["AppSettings:DbMapPath"]; } }
    public string LogPath               { get { return configuration["AppSettings:LogPath"]; } }
    public string AvelraanEmail         { get { return configuration["AppSettings:AvelraanEmail"]; } }
    public string AvelraanEmailPass     { get { return configuration["AppSettings:AvelraanEmailPass"]; } }
    public string AvelraanSecretKey     { get { return configuration["AppSettings:AvelraanSecretKey"]; } }
}
