﻿using Data_Mapping_Containers.Dtos;
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
        builder.Services.AddSingleton<IValidations, Validations>(); // TODO: this needs to be refactored and moved validations to each service itself
    }

    public static void LoadMetadataService(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IMetadataService, MetadataService>();
    }

    // these will be the general services that will contain the business logic of the app
    public static void LoadBusinessLogicServices(WebApplicationBuilder builder)
    {
        LoadDatabaseService(builder);
        LoadDiceService(builder);
        LoadItemsService(builder);
        LoadPlayerService(builder);
        LoadCharacterService(builder);
        LoadNpcService(builder);
        LoadGameplayService(builder);
        LoadBattleboardService(builder);
    }

    #region private methods
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
        builder.Services.AddTransient<IItemsLogicDelegator, ItemsLogicDelegator>();
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
        builder.Services.AddTransient<ICharacterCRUDLogic, CharacterCRUDLogic>();
        builder.Services.AddTransient<ICharacterUpdateLogic, CharacterUpdateLogic>();
        builder.Services.AddTransient<ICharacterItemsLogic, CharacterItemsLogic>();
        builder.Services.AddTransient<ICharacterSpecialSkillsLogic, CharacterSpecialSkillsLogic>();
        builder.Services.AddTransient<ICharacterLevelupLogic, CharacterLevelupLogic>();
        builder.Services.AddTransient<ICharacterTravelLogic, CharacterTravelLogic>();
        builder.Services.AddTransient<ICharacterNpcInteraction, CharacterNpcInteraction>();
    }

    private static void LoadNpcService(WebApplicationBuilder builder)
    {
        // delegator
        builder.Services.AddTransient<INpcLogicDelegator, NpcLogicDelegator>();
        // subservices
        builder.Services.AddTransient<INpcCreateLogic, NpcCreateLogic>();
        builder.Services.AddTransient<INpcGameplayLogic, NpcGameplayLogic>();
    }

    private static void LoadGameplayService(WebApplicationBuilder builder)
    {
        // delegator
        builder.Services.AddTransient<IGameplayLogicDelegator, GameplayLogicDelegator>();
        // subservices
        builder.Services.AddTransient<IGameplayLocationsLogic, GameplayLocationsLogic>();
        builder.Services.AddTransient<IGameplayCharactersLogic, GameplayCharactersLogic>();
        builder.Services.AddTransient<IGameplayQuestLogic, GameplayQuestLogic>();
    }

    private static void LoadBattleboardService(WebApplicationBuilder builder)
    {
        // delegator
        builder.Services.AddTransient<IBattleboardLogicDelegator, BattleboardLogicDelegator>();
        // subservices
        builder.Services.AddTransient<IBattleboardCRUDLogic, BattleboardCRUDLogic>();
        builder.Services.AddTransient<IBattleboardCombatLogic, BattleboardCombatLogic>();
        builder.Services.AddTransient<IBattleboardNonCombatLogic, BattleboardNonCombatLogic>();
        builder.Services.AddTransient<IBattleboardQuestLogic, BattleboardQuestLogic>();
        builder.Services.AddTransient<IBattleboardEncounterLogic, BattleboardEncounterLogic>();
    }
    #endregion
}