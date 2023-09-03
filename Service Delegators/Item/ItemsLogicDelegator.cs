using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IItemsLogicDelegator
{
    Item GenerateRandomItem();
    Item GenerateSpecificItem(string type, string subtype);
}

public class ItemsLogicDelegator : IItemsLogicDelegator
{
    private readonly Validations validations;
    private readonly IItemCreateLogic itemCreateLogic;

    public ItemsLogicDelegator(
        Validations validations,
        IItemCreateLogic itemCreateLogic)
    {
        this.validations = validations;
        this.itemCreateLogic = itemCreateLogic;
    }

    public Item GenerateRandomItem()
    {
        return itemCreateLogic.CreateItem();
    }

    public Item GenerateSpecificItem(string type, string subtype)
    {
        validations.CreateItemWithTypeAndSubtype(type, subtype);
        return itemCreateLogic.CreateItem(type, subtype);
    }
}
