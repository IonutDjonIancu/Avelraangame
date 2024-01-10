namespace Service_Delegators;

public interface IDatabaseLogicDelegator
{
    void ExportSnapshot(string requesterId);
    void ImportSnapshot(string requesterId, string snapshotJson);
    void ImportPlayer(string requesterId, string playerJson);
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

    public void ExportSnapshot(string requesterId)
    {
        validations.ValidateSnapshotExportImportOperations(requesterId, string.Empty, true);
        exportLogic.ExportSnapshot();
    }

    public void ImportSnapshot(string requesterId, string snapshotJson)
    {
        validations.ValidateSnapshotExportImportOperations(requesterId, snapshotJson, false);
        importLogic.ImportSnapshot(snapshotJson);
    }

    public void ImportPlayer(string requesterId, string playerJson)
    {
        validations.ValidateDatabasePlayerImport(requesterId, playerJson);
        importLogic.ImportPlayer(playerJson);
    }
}
