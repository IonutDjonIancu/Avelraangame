using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Service_Delegators;

public interface IItemCreateLogic
{
    Item CreateItem(string type = "", string subtype = "");
}

public class ItemCreateLogic : IItemCreateLogic
{
    private readonly IDiceLogicDelegator dice;

    public ItemCreateLogic(IDiceLogicDelegator dice)
    {
        this.dice = dice;
    }

    public Item CreateItem(string type = "", string subtype = "")
    {
        var item = new Item
        {
            Identity = new()
            {
                Id = Guid.NewGuid().ToString(),
                CharacterId = Guid.Empty.ToString(),
            }
        };

        SetItemLevelNameAndIcon(item, dice);
        
        if (type.Length > 0)
        {
            item.Type = type;
            item.Subtype = subtype;
        }
        else
        {
            ItemTypesAndSubtypes.SetItemTypeAndSubtype(item, dice);
        }

        ItemTypesAndSubtypes.SetItemInventoryLocation(item);
        ItemCategoriesAndSubcategories.SetItemCategoryAndDescription(item, dice);
        ItemCategoriesAndSubcategories.SetItemSubcategory(item);
        TaintItem(item, dice);
        ItemBonuses.SetItemBonuses(item, dice);
        ItemBonuses.StrengthenOrImbue(item, dice);
        ItemUpgrades.UpgradeItem(item, dice);
        NameItem(item);

        return item;
    }

    #region private methods
    private static void SetItemLevelNameAndIcon(Item item, IDiceLogicDelegator dice)
    {
        var roll = dice.Roll_d20_withReroll();
        if      (roll >= 100){ item.Level = 6; item.LevelName = ItemsLore.LevelNames.Relic; }
        else if (roll >= 80) { item.Level = 5; item.LevelName = ItemsLore.LevelNames.Artifact; }
        else if (roll >= 50) { item.Level = 4; item.LevelName = ItemsLore.LevelNames.Heirloom; }
        else if (roll >= 40) { item.Level = 3; item.LevelName = ItemsLore.LevelNames.Masterwork; }
        else if (roll >= 20) { item.Level = 2; item.LevelName = ItemsLore.LevelNames.Refined; }
        else  /*(roll >=   1)*/ { item.Level = 1; item.LevelName = ItemsLore.LevelNames.Common; }

        item.Icon = dice.Roll_1_to_n(3);
    }
    private static void TaintItem(Item item, IDiceLogicDelegator dice)
    {
        item.HasTaint = item.Level >= 3 && dice.Roll_d20_noReroll() % 2 == 0;
    }

    private static void NameItem(Item item)
    {
        item.Name = item.Level >= 5 ? item.Quality : $"{item.Quality} {item.Category.ToLowerInvariant()}";
    }
    #endregion
}
