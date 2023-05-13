using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using Serilog;

namespace Avelraangame.Factories;

public class FactoryManager : IFactoryManager
{
    public IServiceFactory ServiceFactory { get; init; }

    private FactoryManager() { }

    public FactoryManager(IAppSettingsConfigManager config)
    {
        var dbcfg = new DatabaseManagerConfig()
        {
            DbPath = config.DbPath,
            DbPlayersPath = config.DbPlayersPath,
            DbRulebookPath = config.DbRulebookPath,

            LogPath = config.LogPath,

            AvelraanEmail = config.AvelraanEmail,
            AvelraanPassword = config.AvelraanEmailPass,
            AveelraanSecretKey = config.AvelraanSecretKey
        };
        
        try
        {
            // setting up cache instance
            var dbManager = new DatabaseManager(dbcfg);

            // setting up services
            ServiceFactory = new ServiceFactory(dbManager); 
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"{ex.Message}\n{ex.StackTrace}");
            throw new Exception("Unable to instantiate DatabaseManager, check logs for more details.");
        }
    }
}
