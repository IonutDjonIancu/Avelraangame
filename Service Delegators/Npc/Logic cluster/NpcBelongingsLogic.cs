using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;

namespace Service_Delegators;

internal class NpcBelongingsLogic
{
    private readonly IDiceRollService dice;
    private readonly IItemService itemsService;

    private NpcBelongingsLogic() { }
    internal NpcBelongingsLogic(
        IDiceRollService diceService,
        IItemService itemService)
    {
        dice = diceService;
        itemsService = itemService;
    }

    internal void SetNpcInventory(Character npc)
    {
        if (npc.Info.Origins.Race == GameplayLore.Rulebook.Npcs.Races.Animal
            || npc.Info.Origins.Race == GameplayLore.Rulebook.Npcs.Races.Elemental) return;

        npc.Inventory.Mainhand = HasItem() ? SetMainhandItem() : null;
        if (npc.Info.Origins.Race == GameplayLore.Rulebook.Npcs.Races.Undead) return;

        npc.Inventory.Head = HasItem() ? itemsService.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Helm) : null;
        npc.Inventory.Body = HasItem() ? itemsService.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Armour) : null;
        if (npc.Info.Origins.Race == GameplayLore.Rulebook.Npcs.Races.Fiend) return;

        npc.Inventory.Offhand = HasItem() ? SetOffhandItem(npc) : null;
        npc.Inventory.Ranged = HasItem() ? SetRangedItem() : null;
    }

    #region private methods
    private Item SetRangedItem()
    {
        var item = GetAWeapon();

        return item.Subcategory != ItemsLore.Subcategories.Ranged
            || item.Subtype != ItemsLore.Subtypes.Weapons.Spear
            || item.Subtype != ItemsLore.Subtypes.Weapons.Axe
            ? SetRangedItem() : item;
    }

    private Item SetOffhandItem(Character npc)
    {
        if (npc.Inventory.Mainhand?.Subcategory == ItemsLore.Subcategories.Twohanded) return null!;

        return HasItem() ? GetNonTwoHandedWeapon() : itemsService.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Shield);
    }

    private Item GetNonTwoHandedWeapon()
    {
        var item = GetAWeapon();

        return item.Subcategory == ItemsLore.Subcategories.Twohanded 
            || item.Subcategory == ItemsLore.Subcategories.Ranged 
            ? GetNonTwoHandedWeapon() : item; 
    }

    private Item SetMainhandItem()
    {
        var item = GetAWeapon();

        return item.Subcategory == ItemsLore.Subcategories.Ranged ? SetMainhandItem() : item;
    }

    private Item GetAWeapon()
    {
        var index = dice.Roll_XdY(0, ItemsLore.Subtypes.Weapons.All.Count - 1);
        var itemSubtype = ItemsLore.Subtypes.Weapons.All[index];

        return itemsService.GenerateSpecificItem("Weapon", itemSubtype);
    } 
    
    private bool HasItem()
    {
        return dice.FlipCoin();
    }
    #endregion
}
