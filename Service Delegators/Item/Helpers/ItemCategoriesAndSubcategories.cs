using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal static class ItemCategoriesAndSubcategories
{
    internal static void SetItemCategoryAndDescription(Item item, IDiceLogicDelegator dice)
    {
        if (item.Type == ItemsLore.Types.Weapon) SetCategoryAndDescriptionFor(ItemsLore.Categories.Weapons[item.Subtype], item, dice);
        else if (item.Type == ItemsLore.Types.Protection) SetCategoryAndDescriptionFor(ItemsLore.Categories.Protections[item.Subtype], item, dice);
        else  /*(item.Type == ItemsLore.Types.Wealth)*/     SetCategoryAndDescriptionFor(ItemsLore.Categories.Wealth[item.Subtype], item, dice);
    }

    internal static void SetItemSubcategory(Item item)
    {
        // protection
        if (item.Subtype == ItemsLore.Subtypes.Protections.Helm) item.Subcategory = ItemsLore.Subcategories.Garment;
        else if (item.Subtype == ItemsLore.Subtypes.Protections.Armour) item.Subcategory = ItemsLore.Subcategories.Garment;
        else if (item.Subtype == ItemsLore.Subtypes.Protections.Shield) item.Subcategory = ItemsLore.Subcategories.Onehanded;
        // weapons
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Sword) item.Subcategory = ItemsLore.Subcategories.Onehanded;
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Mace) item.Subcategory = ItemsLore.Subcategories.Onehanded;
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Axe) item.Subcategory = ItemsLore.Subcategories.Onehanded;
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Dagger) item.Subcategory = ItemsLore.Subcategories.Onehanded;
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Spear) item.Subcategory = ItemsLore.Subcategories.Onehanded;
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Pike) item.Subcategory = ItemsLore.Subcategories.Twohanded;
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Polearm) item.Subcategory = ItemsLore.Subcategories.Twohanded;
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Crossbow) item.Subcategory = ItemsLore.Subcategories.Ranged;
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Bow) item.Subcategory = ItemsLore.Subcategories.Ranged;
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Sling) item.Subcategory = ItemsLore.Subcategories.Ranged;
        // wealth
        else if (item.Subtype == ItemsLore.Subtypes.Wealth.Gems) item.Subcategory = ItemsLore.Subcategories.Garment;
        else if (item.Subtype == ItemsLore.Subtypes.Wealth.Valuables) item.Subcategory = ItemsLore.Subcategories.Garment;
        else if (item.Subtype == ItemsLore.Subtypes.Wealth.Trinket) item.Subcategory = ItemsLore.Subcategories.Garment;
    }

    #region private methods
    private static void SetCategoryAndDescriptionFor(Dictionary<string, string> category, Item item, IDiceLogicDelegator dice)
    {
        var count = category.Keys.Count;
        var position = dice.Roll_1_to_n(count);
        var element = category.ElementAt(position - 1);

        item.Category = element.Key;
        item.Description = element.Value;
    }
    #endregion
}
