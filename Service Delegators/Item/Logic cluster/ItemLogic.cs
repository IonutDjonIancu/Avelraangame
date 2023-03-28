using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using System.Globalization;

namespace Service_Delegators.Logic_Cluster;

internal class ItemLogic
{
    private readonly IDatabaseManager dbm;
    private readonly IDiceRollService dice;
    private readonly ItemClassification classif;
    private readonly ItemEnchants enchants;
    private readonly ItemUpgrades upgrades;

    private ItemLogic() { }

    internal ItemLogic(
        IDatabaseManager databaseManager,
        IDiceRollService diceRollService)
    {
        dbm = databaseManager;
        dice = diceRollService;

        classif = new ItemClassification(dice);
        enchants = new ItemEnchants(dice);
        upgrades = new ItemUpgrades(dice);
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
        classif.SetItemLevelAndLevelName(item);

        item.Type = new CultureInfo("en-US").TextInfo.ToTitleCase(type);
        item.Subtype = new CultureInfo("en-US").TextInfo.ToTitleCase(subtype);
        item.InventoryLocations = classif.SetItemInventoryLocation(item.Subtype);

        try
        {
            classif.SetItemCategoryAndDescription(item);
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
        classif.SetItemLevelAndLevelName(item);
        classif.SetItemTypeAndSubtypeAndInventoryLocations(item);
        classif.SetItemCategoryAndDescription(item);
    }

    private void EnchantItem(Item item)
    {
        enchants.SetItemBonuses(item);
        enchants.StrengthenOrImbue(item);
    }

    private void UpgradeItem(Item item)
    {
        upgrades.UpgradeItem(item);
    }
    #endregion
}