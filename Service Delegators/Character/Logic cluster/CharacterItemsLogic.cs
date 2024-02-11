using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Service_Delegators;

public interface ICharacterItemsLogic
{
    Character EquipItem(CharacterEquip equip);
    Character UnequipItem(CharacterEquip unequip);
    Character BuyOrSellItem(CharacterTrade tradeItem);
    Character BuyProvisions(CharacterTrade tradeItem);
    Character GiveProvisions(CharacterTrade tradeItem);
    Character GiveWealth(CharacterTrade tradeItem);
    Character GiveItem(CharacterTrade tradeItem);
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
            var character = ServicesUtils.GetPlayerCharacter(unequip.CharacterIdentity, snapshot);
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

            AddRemoveItemBonuses(character, item, false);

            character.Inventory.Supplies!.Add(item);

            return character;
        }
    }

    public Character EquipItem(CharacterEquip equip)
    {
        var hasToUnequip = false;
        var character = ServicesUtils.GetPlayerCharacter(equip.CharacterIdentity, snapshot);
        var item = character.Inventory.Supplies!.Find(i => i.Identity.Id == equip.ItemId)!;

        lock (_lock)
        {
            if (equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand) hasToUnequip = character.Inventory.Mainhand != null;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand) hasToUnequip = character.Inventory.Offhand != null;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged) hasToUnequip = character.Inventory.Ranged != null;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Head) hasToUnequip = character.Inventory.Head != null;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Body) hasToUnequip = character.Inventory.Body != null;
        }

        if (hasToUnequip) UnequipItem(equip);

        lock (_lock)
        {
            if      (equip.InventoryLocation == ItemsLore.InventoryLocation.Head) character.Inventory.Head = item;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Body) character.Inventory.Body = item;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand) character.Inventory.Mainhand = item;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand) character.Inventory.Offhand = item;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged) character.Inventory.Ranged = item;
            else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Heraldry) character.Inventory!.Heraldry!.Add(item);

            AddRemoveItemBonuses(character, item, true);

            character.Inventory.Supplies.Remove(item);

            return character;
        }
    }

    public Character BuyOrSellItem(CharacterTrade tradeItem)
    {
        lock (_lock)
        {
            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            var location = snapshot.Locations.Find(s => s.FullName == ServicesUtils.GetLocationFullNameFromPosition(character.Status.Position))!;

            if (tradeItem.IsToBuy.GetValueOrDefault())
            {
                var item = location.Market.Find(s => s.Identity.Id == tradeItem.ItemId)!;
                BuyItem(character, item, location);
            }
            else
            {
                var item = character.Inventory.Supplies.Find(s => s.Identity.Id == tradeItem.ItemId)!;
                SellItem(character, item, location);
            }

            return character;
        }
    }

    public Character BuyProvisions(CharacterTrade tradeItem)
    {
        lock (_lock)
        {
            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);

            character.Inventory.Provisions += tradeItem.Amount.GetValueOrDefault();
            character.Status.Wealth -= tradeItem.Amount.GetValueOrDefault() * 2;

            return character;
        }
    }

    public Character GiveProvisions(CharacterTrade tradeItem)
    {
        lock (_lock)
        {
            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            var target = ServicesUtils.GetPlayerCharacter(tradeItem.TargetIdentity!, snapshot);

            character.Inventory.Provisions -= tradeItem.Amount.GetValueOrDefault();
            target.Inventory.Provisions += tradeItem.Amount.GetValueOrDefault();

            return character;
        }
    }

    public Character GiveWealth(CharacterTrade tradeItem)
    {
        lock (_lock)
        {
            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            var target = ServicesUtils.GetPlayerCharacter(tradeItem.TargetIdentity!, snapshot);

            character.Status.Wealth -= tradeItem.Amount.GetValueOrDefault();
            target.Status.Wealth += tradeItem.Amount.GetValueOrDefault();

            return character;
        }
    }

    public Character GiveItem(CharacterTrade tradeItem)
    {
        lock (_lock)
        {
            var character = ServicesUtils.GetPlayerCharacter(tradeItem.CharacterIdentity, snapshot);
            var target = ServicesUtils.GetPlayerCharacter(tradeItem.TargetIdentity!, snapshot);

            var item = character.Inventory.Supplies.Find(s => s.Identity.Id == tradeItem.ItemId)!;

            character.Inventory.Supplies.Remove(item);
            target.Inventory.Supplies.Add(item);

            return character;
        }
    }

    #region private methods
    private static void SellItem(Character character, Item item, Location location)
    {
        var moneyBack = item.Value + item.Value * character.Sheet.Skills.Social / 1000;
        item.Value += (int)Math.Round(item.Value * 0.15);

        location.Market.Add(item);
        character.Inventory.Supplies.Remove(item);
        character.Status.Wealth += moneyBack;
    }

    private static void BuyItem(Character character, Item item, Location location)
    {
        var paySum = item.Value - item.Value * character.Sheet.Skills.Social / 1000;
        var finalSum = paySum <= 0 ? 1 : paySum;

        item.Identity.Id = character.Identity.Id;

        character.Inventory.Supplies.Add(item);
        character.Status.Wealth -= finalSum;
        location.Market.Remove(item);
    }

    private static void AddRemoveItemBonuses(Character character, Item item, bool toAdd)
    {
        var multiplier = 1;

        if (!toAdd) multiplier *= -1;

        // stats
        character.Sheet.Stats.Strength      += multiplier * item.Sheet.Stats.Strength;
        character.Sheet.Stats.Constitution  += multiplier * item.Sheet.Stats.Constitution;
        character.Sheet.Stats.Agility       += multiplier * item.Sheet.Stats.Agility;
        character.Sheet.Stats.Willpower     += multiplier * item.Sheet.Stats.Willpower;
        character.Sheet.Stats.Perception    += multiplier * item.Sheet.Stats.Perception;
        character.Sheet.Stats.Abstract      += multiplier * item.Sheet.Stats.Abstract;

        // assets
        character.Sheet.Assets.Harm         += multiplier * item.Sheet.Assets.Harm;
        character.Sheet.Assets.Spot         += multiplier * item.Sheet.Assets.Spot;
        character.Sheet.Assets.Purge        += multiplier * item.Sheet.Assets.Purge;
        character.Sheet.Assets.Defense      += multiplier * item.Sheet.Assets.Defense;
        character.Sheet.Assets.DefenseFinal = character.Sheet.Assets.Defense > 90 ? 90 : character.Sheet.Assets.Defense;
        character.Sheet.Assets.Resolve      += multiplier * item.Sheet.Assets.Resolve;
        character.Sheet.Assets.ResolveLeft  = character.Sheet.Assets.Resolve;
        character.Sheet.Assets.Mana         += multiplier * item.Sheet.Assets.Mana;
        character.Sheet.Assets.ManaLeft     = character.Sheet.Assets.Mana;
        character.Sheet.Assets.Actions      += multiplier * item.Sheet.Assets.Actions;
        character.Sheet.Assets.ActionsLeft  = character.Sheet.Assets.Actions;

        // skills
        character.Sheet.Skills.Melee       += multiplier * item.Sheet.Skills.Melee;
        character.Sheet.Skills.Arcane       += multiplier * item.Sheet.Skills.Arcane;
        character.Sheet.Skills.Psionics     += multiplier * item.Sheet.Skills.Psionics;
        character.Sheet.Skills.Hide         += multiplier * item.Sheet.Skills.Hide;
        character.Sheet.Skills.Traps        += multiplier * item.Sheet.Skills.Traps;
        character.Sheet.Skills.Tactics      += multiplier * item.Sheet.Skills.Tactics;
        character.Sheet.Skills.Social       += multiplier * item.Sheet.Skills.Social;
        character.Sheet.Skills.Apothecary   += multiplier * item.Sheet.Skills.Apothecary;
        character.Sheet.Skills.Travel       += multiplier * item.Sheet.Skills.Travel;
        character.Sheet.Skills.Sail         += multiplier * item.Sheet.Skills.Sail;
    }
    #endregion
}