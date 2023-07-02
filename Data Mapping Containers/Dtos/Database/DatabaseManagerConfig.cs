﻿namespace Data_Mapping_Containers.Dtos;

public class DatabaseManagerConfig
{
    public string DbPath { get; set; }
    public string DbPlayersPath { get; set; }
    public string DbMapPath { get; set; }

    public string LogPath { get; set; }

    public string AvelraanEmail { get; set; }
    public string AvelraanPassword { get; set; }
    public string AveelraanSecretKey { get; set; }
}
