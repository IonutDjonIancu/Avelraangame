using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using Serilog;

namespace Avelraangame.Factories;

public class FactoryManager : IFactoryManager
{
    public IDatabaseManager Dbm { get; init; }
    public IServiceFactory ServiceFactory { get; init; }

    private FactoryManager() { }

    public FactoryManager(IAppSettingsConfigManager configManager)
    {
        var config = new DatabaseManagerConfig()
        {
            DbPath = configManager.DbPath,
            DbPlayersPath = configManager.DbPlayersPath,
            DbRulebookPath = configManager.DbRulebookPath,

            LogPath = configManager.LogPath,

            Key = configManager.DbKey,

            AvelraanEmail = configManager.AvelraanEmail,
            AvelraanPassword = configManager.AvelraanEmailPass
        };
        
        try
        {
            Dbm = new DatabaseManager(config);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"{ex.Message}\n{ex.StackTrace}");
            throw new Exception("Unable to instantiate DatabaseManager, check logs for more details.");
        }

        ServiceFactory = new ServiceFactory(Dbm);
    }
}
