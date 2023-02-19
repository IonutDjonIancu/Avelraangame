using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using Service_Delegators.Logic_Cluster;

namespace Service_Delegators;

public class ItemService : IItemService
{
    private readonly ItemLogic logic;

    public ItemService(
        IDatabaseManager databaseOperations,
        IDiceRollService diceRollService)
    { 
        logic = new ItemLogic(databaseOperations, diceRollService);
    }
    
    public Item GenerateRandomItem()
    {
        return logic.GetARandomItem();
    }

    public Item GenerateSpecificItem(string type, string subtype)
    {
        return logic.GetASpecificItem(type, subtype);
    }
}
