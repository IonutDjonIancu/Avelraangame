﻿namespace Avelraangame;

public class AppSettingsConfigManager : IAppSettingsConfigManager
{
    private readonly IConfiguration _configuration;

	public AppSettingsConfigManager(IConfiguration configuration)
	{
		_configuration = configuration;	
	}

    public string DbPath
    {
        get
        {
            return _configuration["AppSettings:DbPath"];
        }
    }

    public string DbPlayers
    {
        get
        {
            return _configuration["AppSettings:DbPlayersPath"];
        }
    }

    public string DbTraits
    {
        get
        {
            return _configuration["AppSettings:DbTraitsPath"];
        }
    }

    public string DbTestPath
    {
        get
        {
            return _configuration["AppSettings:DbTestPath"];
        }
    }

    public string DbKey
    {
        get
        {
            return _configuration["AppSettings:DbKey"];
        }
    }

    public string LogPath
    {
        get
        {
            return _configuration["AppSettings:LogPath"];
        }
    }

    public string AvelraanEmail
    {
        get
        {
            return _configuration["AppSettings:AvelraanEmail"];
        }
    }

    public string AvelraanEmailPass
    {
        get
        {
            return _configuration["AppSettings:AvelraanEmailPass"];
        }
    }
}
