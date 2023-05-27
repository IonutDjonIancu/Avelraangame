namespace Service_Delegators;

internal class GameplayValidator : ValidatorBase
{
    private readonly IDatabaseService dbs;

    private GameplayValidator() { }
    internal GameplayValidator(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

}
