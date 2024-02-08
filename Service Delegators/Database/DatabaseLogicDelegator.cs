using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IDatabaseLogicDelegator
{
    void ExportSnapshot(string requesterId, DbRequestsInfo dbRequestsInfo);
    void ImportSnapshot(string requesterId, DbRequestsInfo dbRequestsInfo);
    void ImportPlayer(string requesterId, DbRequestsInfo dbRequestsInfo);
    void Purge(string requesterId, DbRequestsInfo dbRequestsInfo);
}

public class DatabaseLogicDelegator : IDatabaseLogicDelegator
{
    private readonly IValidations validations;
    private readonly IDatabaseExportLogic exportLogic;
    private readonly IDatabaseImportLogic importLogic;

    public DatabaseLogicDelegator(
        IValidations validations,
        IDatabaseExportLogic exportLogic,
        IDatabaseImportLogic importLogic)
    {
        this.validations = validations;
        this.exportLogic = exportLogic;
        this.importLogic = importLogic;
    }

    public void ExportSnapshot(string requesterId, DbRequestsInfo dbRequestsInfo)
    {
        validations.ValidateSnapshotExportImportOperations(requesterId, dbRequestsInfo);
        exportLogic.ExportPlayers();
    }

    public void ImportSnapshot(string requesterId, DbRequestsInfo dbRequestsInfo)
    {
        validations.ValidateSnapshotExportImportOperations(requesterId, dbRequestsInfo);
        importLogic.ImportSnapshot(dbRequestsInfo.SnapshotJsonString!);
    }

    public void ImportPlayer(string requesterId, DbRequestsInfo dbRequestsInfo)
    {
        validations.ValidateDatabasePlayerImport(requesterId, dbRequestsInfo);
        importLogic.ImportPlayer(dbRequestsInfo.PlayerJsonString!);
    }

    public void Purge(string requesterId, DbRequestsInfo dbRequestsInfo)
    {
        validations.ValidateSnapshotExportImportOperations(requesterId, dbRequestsInfo);
        importLogic.Purge();
    }
}
