using Data_Mapping_Containers.Dtos;

namespace Service_Delegators.Logic_Cluster;

internal class ItemClassification
{
    private readonly IDiceRollService dice;

    internal ItemClassification(IDiceRollService dice)
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

    #region privates
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
        else if (roll >=  8)    item.Subtype = ItemsLore.Subtypes.Protections.Helmet; 
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
        else if (roll >=  8)    item.Subtype = ItemsLore.Subtypes.Weapons.Axe; 
        else if (roll >=  7)    item.Subtype = ItemsLore.Subtypes.Weapons.Dagger; 
        else if (roll >=  5)    item.Subtype = ItemsLore.Subtypes.Weapons.Bow;
        else if (roll >=  4)    item.Subtype = ItemsLore.Subtypes.Weapons.Sling; 
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
