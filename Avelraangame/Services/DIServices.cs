using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;
using Persistance_Manager;
using Service_Delegators;

namespace Avelraangame;

public interface IDIServices { }

public class DIServices : IDIServices
{
    private const string AppSettings = "AppSettings";


    // this is the appsettings.json file loaded in the memory for the resource strings
    public static void LoadAppSettings(WebApplicationBuilder builder)
    {
        var appSettings = new AppSettings();
        builder.Configuration.GetSection(AppSettings).Bind(appSettings);
        builder.Services.AddSingleton(appSettings);
    }

    // this is the cache system
    public static void LoadSnapshotSystem(WebApplicationBuilder builder)
    {
        var snapshot = new Snapshot();
        builder.Services.AddSingleton(snapshot);
    }

    // these will be the general services that will contain the business logic of the app
    public static void LoadAvelraanServices(WebApplicationBuilder builder)
    {
        LoadPersistenceService(builder);
        LoadDiceService(builder);
        LoadItemsService(builder);
    }

    #region private methods
    private static void LoadPersistenceService(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IPersistenceService, PersistenceService>();
    }

    private static void LoadDiceService(WebApplicationBuilder builder)
    {
        // delegator
        builder.Services.AddTransient<IDiceLogicDelegator, DiceLogicDelegator>();
        // subservices
        builder.Services.AddTransient<IDiceD20Logic, DiceD20RollsLogic>();
        builder.Services.AddTransient<IDiceD100Logic, DiceD100RollsLogic>();
        builder.Services.AddTransient<IDiceCustomRollsLogic, DiceCustomRollsLogic>();
    }

    private static void LoadItemsService(WebApplicationBuilder builder)
    {
        // delegator
        builder.Services.AddTransient<IItemDelegator, ItemDelegator>();
        // subservices
        builder.Services.AddTransient<IItemLogic, ItemLogic>();
    }
    #endregion
}
