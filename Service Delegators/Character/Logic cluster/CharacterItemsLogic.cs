using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterItemsLogic
{
    Character EquipItem(CharacterEquip equip);
    Character UnequipItem(CharacterEquip unequip);
}

public class CharacterItemsLogic : ICharacterItemsLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;

    public CharacterItemsLogic(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public Character UnequipItem(CharacterEquip unequip)
    {
        lock (_lock)
        {
            var character = GetPlayerCharacter(unequip.CharacterIdentity);
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

            return character;
        }
    }

    public Character EquipItem(CharacterEquip equip)
    {
        var hasToUnequip = false;

        lock (_lock)
        {
            var character = GetPlayerCharacter(equip.CharacterIdentity);
            var item = character.Inventory.Supplies!.Find(i => i.Identity.Id == equip.ItemId);

            if (item == null) hasToUnequip = true;
        }

        if (hasToUnequip) UnequipItem(equip);

        lock (_lock)
        {
            var character = GetPlayerCharacter(equip.CharacterIdentity);
            var item = character.Inventory.Supplies!.Find(i => i.Identity.Id == equip.ItemId)!;

            if (equip.InventoryLocation == ItemsLore.InventoryLocation.Head) character.Inventory.Head = item;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Body) character.Inventory.Body = item;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand) character.Inventory.Mainhand = item;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand) character.Inventory.Offhand = item;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged) character.Inventory.Ranged = item;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Heraldry) character.Inventory!.Heraldry!.Add(item);

            character.Inventory.Supplies.Remove(item);

            return character;
        }
    }

    #region private methods
    private Character GetPlayerCharacter(CharacterIdentity identity)
    {
        var player = snapshot.Players.Find(p => p.Identity.Id == identity.PlayerId)!;
        return player.Characters.Find(p => p.Identity!.Id == identity.Id)!;
    }
    #endregion
}
