using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterItemsLogic
{
    private readonly IDatabaseService dbs;

    private CharacterItemsLogic() { }
    internal CharacterItemsLogic(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

    internal Character UnequipItem(CharacterEquip unequip)
    {
        var (character, player) = GetStoredCharacterAndPlayer(unequip.CharacterIdentity);
        Item item;

        if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Head)
        {
            item = character.Inventory!.Head!;
            character.Inventory.Head = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Body)
        {
            item = character.Inventory!.Body!;
            character.Inventory.Body = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand)
        {
            item = character.Inventory!.Mainhand!;
            character.Inventory.Mainhand = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Offhand)
        {
            item = character.Inventory!.Offhand!;
            character.Inventory.Offhand = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Ranged)
        {
            item = character.Inventory!.Ranged!;
            character.Inventory.Ranged = null;
        }
        else
        {
            item = character.Inventory!.Heraldry!.Find(i => i.Identity.Id == unequip.ItemId)!;
            character.Inventory.Heraldry.Remove(item);
        }

        character.Inventory.Supplies!.Add(item);

        dbs.PersistPlayer(player.Identity.Id);

        return character;
    }

    internal Character EquipItem(CharacterEquip equip)
    {
        var (character, player) = GetStoredCharacterAndPlayer(equip.CharacterIdentity); 
        var item = character.Inventory.Supplies!.Find(i => i.Identity.Id == equip.ItemId)!;

        if (equip.InventoryLocation == ItemsLore.InventoryLocation.Head)
        {
            if (character.Inventory!.Head != null) UnequipItem(equip);
            character.Inventory.Head = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Body)
        {
            if (character.Inventory!.Body != null) UnequipItem(equip);
            character.Inventory.Body = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand)
        {
            if (character.Inventory!.Mainhand != null) UnequipItem(equip);
            character.Inventory.Mainhand = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand)
        {
            if (character.Inventory!.Offhand != null) UnequipItem(equip);
            character.Inventory.Offhand = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged)
        {
            if (character.Inventory!.Ranged != null) UnequipItem(equip);
            character.Inventory.Ranged = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Heraldry)
        {
            character.Inventory!.Heraldry!.Add(item);
        }

        character.Inventory.Supplies.Remove(item);

        dbs.PersistPlayer(player.Identity.Id);

        return character;
    }

    #region private methods
    private (Character, Player) GetStoredCharacterAndPlayer(CharacterIdentity identity)
    {
        var player = dbs.Snapshot.Players.Find(p => p.Identity.Id == identity.PlayerId)!;
        var character = player.Characters.Find(p => p.Identity!.Id == identity.Id)!;

        return (character, player);
    }
    #endregion
}
