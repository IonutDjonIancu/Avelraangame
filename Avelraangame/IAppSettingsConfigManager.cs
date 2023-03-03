namespace Avelraangame;

public interface IAppSettingsConfigManager
{
    string DbPath { get; }
    string DbPlayers { get; }
    string DbTestPath { get; }
    string DbKey { get; }
    string LogPath { get; }
    string AvelraanEmail { get; }
    string AvelraanEmailPass { get; }
}
