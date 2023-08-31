using Data_Mapping_Containers.Dtos;
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
    public static void LoadAppSnapshot(WebApplicationBuilder builder)
    {
        var snapshot = new Snapshot();
        builder.Services.AddSingleton(snapshot);
    }

    // this is the validations service for all subsequent services
    public static void LoadValidationsService(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IValidations, Validations>();
    }

    public static void LoadMetadataService(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IMetadataService, MetadataService>();
    }

    // these will be the general services that will contain the business logic of the app
    public static void LoadBusinessLogicServices(WebApplicationBuilder builder)
    {
        LoadPersistenceService(builder);
        LoadDatabaseService(builder);
        LoadDiceService(builder);
        LoadItemsService(builder);
        LoadPlayerService(builder);
        LoadCharacterService(builder);
    }

    #region private methods
    private static void LoadPersistenceService(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IPersistenceService, PersistenceService>();
    }

    private static void LoadDatabaseService(WebApplicationBuilder builder)
    {
        // delegator
        builder.Services.AddTransient<IDatabaseLogicDelegator, DatabaseLogicDelegator>();
        // subservices
        builder.Services.AddTransient<IDatabaseExportLogic, DatabaseExportLogic>();
        builder.Services.AddTransient<IDatabaseImportLogic, DatabaseImportLogic>();

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
        builder.Services.AddTransient<IItemLogicDelegator, ItemLogicDelegator>();
        // subservices
        builder.Services.AddTransient<IItemCreateLogic, ItemCreateLogic>();
    }

    private static void LoadPlayerService(WebApplicationBuilder builder)
    {
        // delegator
        builder.Services.AddTransient<IPlayerLogicDelegator, PlayerLogicDelegator>();
        // subservices
        builder.Services.AddTransient<IPlayerAuthLogic, PlayerAuthLogic>();
        builder.Services.AddTransient<IPlayerOperationsLogic, PlayerOperationsLogic>();
    }

    private static void LoadCharacterService(WebApplicationBuilder builder)
    {
        // delegator
        builder.Services.AddTransient<ICharacterLogicDelegator, CharacterLogicDelegator>();
        // subservices
        builder.Services.AddTransient<ICharacterSheetLogic, CharacterSheetLogic>();
        builder.Services.AddTransient<ICharacterCreateLogic, CharacterCreateLogic>();
        builder.Services.AddTransient<ICharacterInfoLogic, CharacterInfoLogic>();
    }
    #endregion
}
