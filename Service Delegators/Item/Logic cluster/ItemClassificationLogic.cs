#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators.Logic_Cluster;

internal class ItemClassificationLogic
{
    private readonly IDiceRollService dice;

    private ItemClassificationLogic() { }
    internal ItemClassificationLogic(IDiceRollService dice)
	{
		this.dice = dice;
	}

    internal void SetItemLevelAndLevelName(Item item)
    {
        var roll = dice.Roll_d20(true);

        if      (roll >= 100)   { item.Level = 6; item.LevelName = ItemsLore.LevelNames.Relic; }
        else if (roll >=  80)   { item.Level = 5; item.LevelName = ItemsLore.LevelNames.Artifact; }
        else if (roll >=  50)   { item.Level = 4; item.LevelName = ItemsLore.LevelNames.Heirloom; }
        else if (roll >=  40)   { item.Level = 3; item.LevelName = ItemsLore.LevelNames.Masterwork; }
        else if (roll >=  20)   { item.Level = 2; item.LevelName = ItemsLore.LevelNames.Refined; }
        else  /*(roll >=   1)*/ { item.Level = 1; item.LevelName = ItemsLore.LevelNames.Common; }
    }

    internal void SetItemTypeAndSubtype(Item item)
    {
        var roll = dice.Roll_d20();

        if      (roll >= 17)    { item.Type = ItemsLore.Types.Protection; SetProtectionSubtype(item); }
        else if (roll >=  5)    { item.Type = ItemsLore.Types.Weapon; SetWeaponSubtype(item); }
        else  /*(roll >=  1)*/  { item.Type = ItemsLore.Types.Wealth; SetWealthSubtype(item); }
    }

    internal void SetItemCategoryAndDescription(Item item)
    {
        if      (item.Type == ItemsLore.Types.Weapon)       SetCategoryAndDescriptionFor(ItemsLore.Categories.Weapons[item.Subtype], item);
        else if (item.Type == ItemsLore.Types.Protection)   SetCategoryAndDescriptionFor(ItemsLore.Categories.Protections[item.Subtype], item);
        else  /*(item.Type == ItemsLore.Types.Wealth)*/     SetCategoryAndDescriptionFor(ItemsLore.Categories.Wealth[item.Subtype], item);
    }

    internal void TaintItem(Item item)
    {
        item.HasTaint = item.Level >= 3 && dice.Roll_d20() % 2 == 0;
    }

    internal void SetItemInventoryLocation(Item item)
    {
        // protection
        if (item.Subtype == ItemsLore.Subtypes.Protections.Helm)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Head);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Protections.Armour)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Body);

        }
        else if (item.Subtype == ItemsLore.Subtypes.Protections.Shield)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Mainhand);
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Offhand);

        }
        // weapons
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Sword)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Mainhand);
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Offhand);

        }
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Pike)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Mainhand);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Crossbow)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Mainhand);
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Ranged);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Polearm)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Mainhand);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Mace)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Mainhand);
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Offhand);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Axe)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Mainhand);
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Offhand);
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Ranged);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Dagger)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Mainhand);
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Offhand);
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Ranged);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Bow)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Ranged);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Sling)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Mainhand);
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Ranged);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Weapons.Spear)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Mainhand);
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Ranged);
        }
        // wealth
        else if (item.Subtype == ItemsLore.Subtypes.Wealth.Gems)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Heraldry);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Wealth.Valuables)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Heraldry);
        }
        else if (item.Subtype == ItemsLore.Subtypes.Wealth.Trinket)
        {
            item.InventoryLocations.Add(ItemsLore.InventoryLocation.Heraldry);
        }
    }

    internal void SetItemSubcategory(Item item)
    {
        // protection
        if      (item.Subtype == ItemsLore.Subtypes.Protections.Helm) item.Subcategory = ItemsLore.Subcategories.Garment;
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
    private void SetCategoryAndDescriptionFor(Dictionary<string, string> category, Item item)
    {
        var count = category.Keys.Count;
        var position = dice.Roll_dX(count);
        var element = category.ElementAt(position - 1);

        item.Category = element.Key;
        item.Description = element.Value;
    }

    private void SetProtectionSubtype(Item item)
    {
        var roll = dice.Roll_d20();

        if      (roll >= 15)    item.Subtype = ItemsLore.Subtypes.Protections.Armour;
        else if (roll >= 8)     item.Subtype = ItemsLore.Subtypes.Protections.Helm;
        else  /*(roll >=  1)*/  item.Subtype = ItemsLore.Subtypes.Protections.Shield;
    }

    private void SetWeaponSubtype(Item item)
    {
        var roll = dice.Roll_d20();

        if      (roll >= 18)    item.Subtype = ItemsLore.Subtypes.Weapons.Sword;
        else if (roll >= 17)    item.Subtype = ItemsLore.Subtypes.Weapons.Pike;
        else if (roll >= 16)    item.Subtype = ItemsLore.Subtypes.Weapons.Crossbow;
        else if (roll >= 14)    item.Subtype = ItemsLore.Subtypes.Weapons.Polearm;
        else if (roll >= 11)    item.Subtype = ItemsLore.Subtypes.Weapons.Mace;
        else if (roll >= 8)     item.Subtype = ItemsLore.Subtypes.Weapons.Axe;
        else if (roll >= 7)     item.Subtype = ItemsLore.Subtypes.Weapons.Dagger;
        else if (roll >= 5)     item.Subtype = ItemsLore.Subtypes.Weapons.Bow;
        else if (roll >= 4)     item.Subtype = ItemsLore.Subtypes.Weapons.Sling;
        else  /*(roll >=  1)*/  item.Subtype = ItemsLore.Subtypes.Weapons.Spear;
    }

    private void SetWealthSubtype(Item item)
    {
        var roll = dice.Roll_d20();

        if      (roll >= 17)    item.Subtype = ItemsLore.Subtypes.Wealth.Gems;
        else if (roll >= 15)    item.Subtype = ItemsLore.Subtypes.Wealth.Trinket;
        else if (roll >= 11)    item.Subtype = ItemsLore.Subtypes.Wealth.Valuables;
        else  /*(roll >=  1)*/  item.Subtype = ItemsLore.Subtypes.Wealth.Goods;
    }
    #endregion
}
#pragma warning restore CA1822 // Mark members as static
