#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterItemsLogic
{
    private readonly IDatabaseManager dbm;
    private readonly CharacterMetadata charMetadata;

    public CharacterItemsLogic(
        IDatabaseManager databaseManager,
        CharacterMetadata charMetadata)
    {
        this.charMetadata = charMetadata;
        dbm = databaseManager;
    }

    internal Character UnequipItem(CharacterEquip unequip, string playerId)
    {
        var chr = charMetadata.GetCharacter(unequip.CharacterId, playerId);
        Item item;

        if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Head)
        {
            item = chr.Inventory.Head;
            chr.Inventory.Head = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Body)
        {
            item = chr.Inventory.Body;
            chr.Inventory.Body = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Shield)
        {
            item = chr.Inventory.Shield;
            chr.Inventory.Shield = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand)
        {
            item = chr.Inventory.Mainhand;
            chr.Inventory.Mainhand = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Offhand)
        {
            item = chr.Inventory.Offhand;
            chr.Inventory.Offhand = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Ranged)
        {
            item = chr.Inventory.Ranged;
            chr.Inventory.Ranged = null;
        }
        else
        {
            item = chr.Inventory.Heraldry.Find(i => i.Identity.Id == unequip.CharacterId);
            chr.Inventory.Heraldry.Remove(item);
        }

        chr.Supplies.Add(item);

        dbm.PersistPlayer(dbm.Metadata.GetPlayerById(playerId));

        return chr;
    }

    internal Character EquipItem(CharacterEquip equip, string playerId)
    {
        var chr = charMetadata.GetCharacter(equip.CharacterId, playerId);
        var item = chr.Supplies.Find(i => i.Identity.Id == equip.ItemId);

        if (equip.InventoryLocation == ItemsLore.InventoryLocation.Head)
        {
            if (chr.Inventory.Head != null) UnequipItem(equip, playerId);
            chr.Inventory.Head = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Body)
        {
            if (chr.Inventory.Body != null) UnequipItem(equip, playerId);
            chr.Inventory.Body = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Shield)
        {
            if (chr.Inventory.Shield != null) UnequipItem(equip, playerId);
            chr.Inventory.Shield = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand)
        {
            if (chr.Inventory.Mainhand != null) UnequipItem(equip, playerId);
            chr.Inventory.Mainhand = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand)
        {
            if (chr.Inventory.Offhand != null) UnequipItem(equip, playerId);
            chr.Inventory.Offhand = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged)
        {
            if (chr.Inventory.Ranged != null) UnequipItem(equip, playerId);
            chr.Inventory.Ranged = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Heraldry)
        {
            chr.Inventory.Heraldry.Add(item);
        }

        chr.Supplies.Remove(item);

        dbm.PersistPlayer(dbm.Metadata.GetPlayerById(playerId));

        return chr;
    }

}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.