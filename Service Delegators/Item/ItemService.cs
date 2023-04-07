using Data_Mapping_Containers.Dtos;
using Service_Delegators.Logic_Cluster;

namespace Service_Delegators;

public class ItemService : IItemService
{
    private readonly ItemLogicDelegator logic;

    public ItemService(IDiceRollService diceRollService)
    { 
        logic = new ItemLogicDelegator(diceRollService);
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
