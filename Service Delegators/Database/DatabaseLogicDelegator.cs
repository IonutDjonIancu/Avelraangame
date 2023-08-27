namespace Service_Delegators;

public interface IDatabaseLogicDelegator
{
    void ExportDatabase(string requesterId);
    void ExportLogs(string requesterId, int days);
    void ImportPlayer(string requesterId, string playerJsonString);
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

    public void ExportDatabase(string requesterId)
    {
        validations.AtDatabaseExportImportOperations(requesterId);
        exportLogic.ExportDatabase();
    }

    public void ExportLogs(string requesterId, int days)
    {
        validations.AtDatabaseExportImportOperations(requesterId);
        exportLogic.ExportLogs(days);
    }

    public void ImportPlayer(string requesterId, string playerJsonString)
    {
        validations.AtPlayerImport(requesterId, playerJsonString);
        importLogic.ImportPlayer(playerJsonString);
    }
}
