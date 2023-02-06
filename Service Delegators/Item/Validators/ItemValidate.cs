using Persistance_Manager;

namespace Service_Delegators.Validators;

public class ItemValidate 
{
    private readonly DatabaseManager dbm;

    public ItemValidate(IDatabaseManager manager)
    {
        dbm = (DatabaseManager)manager;
    }
}
