using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IItemsLogicDelegator
{
    Item GenerateRandomItem();
    Item GenerateSpecificItem(string type, string subtype);
}

public class ItemsLogicDelegator : IItemsLogicDelegator
{
    private readonly IValidations validations;
    private readonly IItemCreateLogic itemCreateLogic;

    public ItemsLogicDelegator(
        IValidations validations,
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
        validations.ValidateCreateItemWithTypeAndSubtype(type, subtype);
        return itemCreateLogic.CreateItem(type, subtype);
    }
}
