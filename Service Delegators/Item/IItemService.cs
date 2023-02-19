using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;
public interface IItemService
{
    Item GenerateRandomItem();
    Item GenerateSpecificItem(string type, string subtype);
}
