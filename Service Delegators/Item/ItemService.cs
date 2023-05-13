using Data_Mapping_Containers.Dtos;
using Service_Delegators.Logic_Cluster;

namespace Service_Delegators;

public class ItemService : IItemService
{
    private readonly ItemValidator validator;
    private readonly ItemLogicDelegator logic;

    public ItemService(IDiceRollService diceRollService)
    {
        validator = new ItemValidator();
        logic = new ItemLogicDelegator(diceRollService);
    }
    
    public Item GenerateRandomItem()
    {
        return logic.GenerateItem();
    }

    public Item GenerateSpecificItem(string type, string subtype)
    {
        validator.ValidateTypeAndSubtypeOnGenerate(type, subtype);
        return logic.GenerateItem(type, subtype);
    }
}
