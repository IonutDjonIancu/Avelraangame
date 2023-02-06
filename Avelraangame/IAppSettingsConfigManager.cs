namespace Avelraangame;

public interface IAppSettingsConfigManager
{
    string DbPath { get; }
    string DbTestPath { get; }
    string DbKey { get; }
    string LogPath { get; }
    string AvelraanEmail { get; }
    string AvelraanEmailPass { get; }
}
