﻿namespace Data_Mapping_Containers.Dtos;

public class DatabaseManagerInfo
{
    public string DbPath { get; set; }
    public string DbPlayersPath { get; set; }
    public string DbRulebookPath { get; set; }
    public string LogPath { get; set; }
    public List<string> PlayerFilePaths { get; set; }

    public string AvelraanEmail { get; set; }
    public string AvelraanPassword { get; set; }
}
