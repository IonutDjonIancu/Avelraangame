using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using System.Globalization;

namespace Service_Delegators.Logic_Cluster;

internal class ItemLogicDelegator
{
    private readonly IDiceRollService dice;
    private readonly ItemClassificationLogic itemClassif;
    private readonly ItemEnchantsLogic itemEnchants;
    private readonly ItemUpgradesLogic itemUpgrades;

    private ItemLogicDelegator() { }

    internal ItemLogicDelegator(IDiceRollService diceRollService)
    {
        dice = diceRollService;

        itemClassif = new ItemClassificationLogic(dice);
        itemEnchants = new ItemEnchantsLogic(dice);
        itemUpgrades = new ItemUpgradesLogic(dice);
    }

    internal Item GetARandomItem()
    {
        var item = CreateItem();
        
        ClassifyItem(item);
        TaintItem(item);
        EnchantItem(item);
        UpgradeItem(item);
        NameItem(item);

        return item;
    }

    internal Item GetASpecificItem(string type, string subtype)
    {
        var item = CreateItem();
        itemClassif.SetItemLevelAndLevelName(item);

        item.Type = new CultureInfo("en-US").TextInfo.ToTitleCase(type);
        item.Subtype = new CultureInfo("en-US").TextInfo.ToTitleCase(subtype);
        item.InventoryLocations = itemClassif.SetItemInventoryLocation(item.Subtype);

        try
        {
            itemClassif.SetItemCategoryAndDescription(item);
            TaintItem(item);
            EnchantItem(item);
            UpgradeItem(item);
            NameItem(item);

            return item;
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to create item, reason: {ex.Message}");
        }
    }

    #region private methods
    private void TaintItem(Item item)
    {
        item.HasTaint = dice.Roll_d20() % 2 == 0;
    }

    private static void NameItem(Item item)
    {
        item.Name = item.Level >= 5 ? item.Quality : $"{item.Quality} {item.Category.ToLowerInvariant()}";
    }

    private static Item CreateItem()
    {
        return new()
        {
            Identity = new ItemIdentity
            {
                Id = Guid.NewGuid().ToString(),
                CharacterId = "",
            }
        };
    }

    private void ClassifyItem(Item item)
    {
        itemClassif.SetItemLevelAndLevelName(item);
        itemClassif.SetItemTypeAndSubtypeAndInventoryLocations(item);
        itemClassif.SetItemCategoryAndDescription(item);
    }

    private void EnchantItem(Item item)
    {
        itemEnchants.SetItemBonuses(item);
        itemEnchants.StrengthenOrImbue(item);
    }

    private void UpgradeItem(Item item)
    {
        itemUpgrades.UpgradeItem(item);
    }
    #endregion
}