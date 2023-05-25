namespace Service_Delegators;

internal class GameplayValidator : ValidatorBase
{
    private readonly IDatabaseService dbs;

    internal GameplayValidator(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }


}
