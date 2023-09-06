using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class TestBase : IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _provider;

    public readonly IPlayerLogicDelegator _playerLogicDelegator;

    public TestBase()
    {
        // setup of DI container for tests

        var services = new ServiceCollection();
        ConfigureServices(services);
        _provider = services.BuildServiceProvider();
        _scope = _provider.CreateScope();

        _playerLogicDelegator = _provider.GetRequiredService<IPlayerLogicDelegator>();
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

}
