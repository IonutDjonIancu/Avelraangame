using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal static class ItemTypesAndSubtypes
{
    internal static void SetItemTypeAndSubtype(Item item, IDiceLogicDelegator dice)
    {
        var roll = dice.Roll_d20_noReroll();

        if (roll >= 17) { item.Type = ItemsLore.Types.Protection; SetProtectionSubtype(item, dice); }
        else if (roll >= 5) { item.Type = ItemsLore.Types.Weapon; SetWeaponSubtype(item, dice); }
        else  /*(roll >=  1)*/  { item.Type = ItemsLore.Types.Wealth; SetWealthSubtype(item, dice); }
    }

    internal static void SetItemInventoryLocation(Item item)
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

    #region private methods
    private static void SetProtectionSubtype(Item item, IDiceLogicDelegator dice)
    {
        var roll = dice.Roll_d20_noReroll();

        if (roll >= 15) item.Subtype = ItemsLore.Subtypes.Protections.Armour;
        else if (roll >= 8) item.Subtype = ItemsLore.Subtypes.Protections.Helm;
        else  /*(roll >=  1)*/  item.Subtype = ItemsLore.Subtypes.Protections.Shield;
    }

    private static void SetWeaponSubtype(Item item, IDiceLogicDelegator dice)
    {
        var roll = dice.Roll_d20_noReroll();

        if (roll >= 18) item.Subtype = ItemsLore.Subtypes.Weapons.Sword;
        else if (roll >= 17) item.Subtype = ItemsLore.Subtypes.Weapons.Pike;
        else if (roll >= 16) item.Subtype = ItemsLore.Subtypes.Weapons.Crossbow;
        else if (roll >= 14) item.Subtype = ItemsLore.Subtypes.Weapons.Polearm;
        else if (roll >= 11) item.Subtype = ItemsLore.Subtypes.Weapons.Mace;
        else if (roll >= 8) item.Subtype = ItemsLore.Subtypes.Weapons.Axe;
        else if (roll >= 7) item.Subtype = ItemsLore.Subtypes.Weapons.Dagger;
        else if (roll >= 5) item.Subtype = ItemsLore.Subtypes.Weapons.Bow;
        else if (roll >= 4) item.Subtype = ItemsLore.Subtypes.Weapons.Sling;
        else  /*(roll >=  1)*/  item.Subtype = ItemsLore.Subtypes.Weapons.Spear;
    }

    private static void SetWealthSubtype(Item item, IDiceLogicDelegator dice)
    {
        var roll = dice.Roll_d20_noReroll();

        if (roll >= 17) item.Subtype = ItemsLore.Subtypes.Wealth.Gems;
        else if (roll >= 15) item.Subtype = ItemsLore.Subtypes.Wealth.Trinket;
        else if (roll >= 11) item.Subtype = ItemsLore.Subtypes.Wealth.Valuables;
        else  /*(roll >=  1)*/  item.Subtype = ItemsLore.Subtypes.Wealth.Goods;
    }
    #endregion
}
