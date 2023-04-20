#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterItemsLogic
{
    private readonly IDatabaseManager dbm;

    public CharacterItemsLogic(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
    }

    internal Character UnequipItem(CharacterEquip unequip, string playerId)
    {
        var character = dbm.Metadata.GetCharacterById(unequip.CharacterId, playerId);
        Item item;

        if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Head)
        {
            item = character.Inventory.Head;
            character.Inventory.Head = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Body)
        {
            item = character.Inventory.Body;
            character.Inventory.Body = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Shield)
        {
            item = character.Inventory.Shield;
            character.Inventory.Shield = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand)
        {
            item = character.Inventory.Mainhand;
            character.Inventory.Mainhand = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Offhand)
        {
            item = character.Inventory.Offhand;
            character.Inventory.Offhand = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Ranged)
        {
            item = character.Inventory.Ranged;
            character.Inventory.Ranged = null;
        }
        else
        {
            item = character.Inventory.Heraldry.Find(i => i.Identity.Id == unequip.CharacterId);
            character.Inventory.Heraldry.Remove(item);
        }

        character.Supplies.Add(item);

        dbm.PersistPlayer(dbm.Metadata.GetPlayerById(playerId));

        return character;
    }

    internal Character EquipItem(CharacterEquip equip, string playerId)
    {
        var character = dbm.Metadata.GetCharacterById(equip.CharacterId, playerId);
        var item = character.Supplies.Find(i => i.Identity.Id == equip.ItemId);

        if (equip.InventoryLocation == ItemsLore.InventoryLocation.Head)
        {
            if (character.Inventory.Head != null) UnequipItem(equip, playerId);
            character.Inventory.Head = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Body)
        {
            if (character.Inventory.Body != null) UnequipItem(equip, playerId);
            character.Inventory.Body = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Shield)
        {
            if (character.Inventory.Shield != null) UnequipItem(equip, playerId);
            character.Inventory.Shield = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand)
        {
            if (character.Inventory.Mainhand != null) UnequipItem(equip, playerId);
            character.Inventory.Mainhand = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand)
        {
            if (character.Inventory.Offhand != null) UnequipItem(equip, playerId);
            character.Inventory.Offhand = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged)
        {
            if (character.Inventory.Ranged != null) UnequipItem(equip, playerId);
            character.Inventory.Ranged = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Heraldry)
        {
            character.Inventory.Heraldry.Add(item);
        }

        character.Supplies.Remove(item);

        dbm.PersistPlayer(dbm.Metadata.GetPlayerById(playerId));

        return character;
    }

}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.