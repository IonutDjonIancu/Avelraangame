using Data_Mapping_Containers.Dtos;

namespace Service_Delegators.Logic_Cluster;

internal class ItemLogicDelegator
{
    private readonly ItemCreateLogic createLogic;
    private readonly ItemClassificationLogic classifLogic;
    private readonly ItemEnchantsLogic enchantsLogic;
    private readonly ItemUpgradesLogic upgradesLogic;

    private ItemLogicDelegator() { }
    internal ItemLogicDelegator(IDiceRollService diceRollService)
    {
        createLogic = new ItemCreateLogic(diceRollService);
        classifLogic = new ItemClassificationLogic(diceRollService);
        enchantsLogic = new ItemEnchantsLogic(diceRollService);
        upgradesLogic = new ItemUpgradesLogic(diceRollService);
    }

    internal Item GenerateItem(string type = "", string subtype = "")
    {
        var item = createLogic.CreateItem();

        classifLogic.SetItemLevelAndLevelName(item);
        if (type.Length > 0)
        {
            item.Type = type;
            item.Subtype = subtype;
        }
        else
        {
            classifLogic.SetItemTypeAndSubtype(item);
        }
        classifLogic.SetItemInventoryLocation(item);
        classifLogic.SetItemCategoryAndDescription(item);
        classifLogic.SetItemSubcategory(item);
        classifLogic.TaintItem(item);

        enchantsLogic.SetItemBonuses(item);
        enchantsLogic.StrengthenOrImbue(item);

        upgradesLogic.UpgradeItem(item);

        createLogic.NameItem(item);

        return item;
    }
}