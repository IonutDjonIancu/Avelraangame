using Persistance_Manager;

namespace Service_Delegators;

public class PartyService
{
    private readonly IDatabaseManager dbm;

    public PartyService(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
    }




    
}
