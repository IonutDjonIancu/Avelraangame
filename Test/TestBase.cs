using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class TestBase : IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _provider;

    public readonly Snapshot _snapshot;
    public readonly IPlayerLogicDelegator _players;
    public readonly IDiceLogicDelegator _dice;
    public readonly IItemsLogicDelegator _items;
    public readonly ICharacterLogicDelegator _characters;
    public readonly INpcLogicDelegator _npcs;
    public readonly IGameplayLogicDelegator _gameplay;

    public TestBase()
    {
        // setup of DI container for tests

        var services = new ServiceCollection();
        ConfigureServices(services);
        _provider = services.BuildServiceProvider();
        _scope = _provider.CreateScope();

        _snapshot = _provider.GetRequiredService<Snapshot>();
        _players = _provider.GetRequiredService<IPlayerLogicDelegator>();
        _dice = _provider.GetRequiredService<IDiceLogicDelegator>();
        _items = _provider.GetRequiredService<IItemsLogicDelegator>();
        _characters = _provider.GetRequiredService<ICharacterLogicDelegator>();
        _npcs = _provider.GetRequiredService<INpcLogicDelegator>();
        _gameplay = _provider.GetRequiredService<IGameplayLogicDelegator>();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // the service collection for the business logic

        LoadAppSettings(services);
        LoadSnapshot(services);
        LoadValidations(services);
        
        LoadPersistence(services);
        LoadDatabaseService(services);

        LoadPlayerService(services);

        LoadDiceService(services);
        LoadItemsService(services);
        LoadCharacterService(services);
        LoadNpcService(services);
        LoadGameplayService(services);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }

    private static void LoadAppSettings(IServiceCollection services)
    {
        var configBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");

        var configuration = configBuilder.Build();
        var appSettings = new AppSettings();
        configuration.Bind("AppSettings", appSettings);
        services.AddSingleton(appSettings);
    }

    private static void LoadSnapshot(IServiceCollection services)
    {
        var snapshot = new Snapshot();
        services.AddSingleton(snapshot);
    }

    private static void LoadValidations(IServiceCollection services)
    {
        services.AddSingleton<IValidations, Validations>();
    }

    private static void LoadPersistence(IServiceCollection services)
    {
        services.AddTransient<IPersistenceService, PersistenceService>();
    }

    private static void LoadDatabaseService(IServiceCollection services)
    {
        // delegator
        services.AddTransient<IDatabaseLogicDelegator, DatabaseLogicDelegator>();
        // subservices
        services.AddTransient<IDatabaseExportLogic, DatabaseExportLogic>();
        services.AddTransient<IDatabaseImportLogic, DatabaseImportLogic>();
    }

    private static void LoadPlayerService(IServiceCollection services)
    {
        // delegator
        services.AddTransient<IPlayerLogicDelegator, PlayerLogicDelegator>();
        // subservices
        services.AddTransient<IPlayerAuthLogic, PlayerAuthLogic>();
        services.AddTransient<IPlayerOperationsLogic, PlayerOperationsLogic>();
    }

    private static void LoadDiceService(IServiceCollection services)
    {
        // delegator
        services.AddTransient<IDiceLogicDelegator, DiceLogicDelegator>();
        // subservices
        services.AddTransient<IDiceD20Logic, DiceD20RollsLogic>();
        services.AddTransient<IDiceD100Logic, DiceD100RollsLogic>();
        services.AddTransient<IDiceCustomRollsLogic, DiceCustomRollsLogic>();
    }

    private static void LoadItemsService(IServiceCollection services)
    {
        // delegator
        services.AddTransient<IItemsLogicDelegator, ItemsLogicDelegator>();
        // subservices
        services.AddTransient<IItemCreateLogic, ItemCreateLogic>();
    }

    private static void LoadCharacterService(IServiceCollection services)
    {
        // delegator
        services.AddTransient<ICharacterLogicDelegator, CharacterLogicDelegator>();
        // subservices
        services.AddTransient<ICharacterSheetLogic, CharacterSheetLogic>();
        services.AddTransient<ICharacterCreateLogic, CharacterCreateLogic>();
        services.AddTransient<ICharacterInfoLogic, CharacterInfoLogic>();
        services.AddTransient<ICharacterItemsLogic, CharacterItemsLogic>();
        services.AddTransient<ICharacterSpecialSkillsLogic, CharacterSpecialSkillsLogic>();
        services.AddTransient<ICharacterLevelupLogic, CharacterLevelupLogic>();
        services.AddTransient<ICharacterTravelLogic, CharacterTravelLogic>();
        services.AddTransient<ICharacterNpcInteraction, CharacterNpcInteraction>();
    }

    private static void LoadNpcService(IServiceCollection services)
    {
        // delegator
        services.AddTransient<INpcLogicDelegator, NpcLogicDelegator>();
        // subservices
        services.AddTransient<INpcCreateLogic, NpcCreateLogic>();
    }

    private static void LoadGameplayService(IServiceCollection services)
    {
        // delegator
        services.AddTransient<IGameplayLogicDelegator, GameplayLogicDelegator>();
        // subservices
        services.AddTransient<IGameplayLocationsLogic, GameplayLocationsLogic>();
    }
}
