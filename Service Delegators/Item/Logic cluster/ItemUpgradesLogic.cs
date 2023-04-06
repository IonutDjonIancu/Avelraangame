﻿using Data_Mapping_Containers.Dtos;

namespace Service_Delegators.Logic_Cluster;

internal class ItemUpgradesLogic
{
    private readonly IDiceRollService dice;

    internal ItemUpgradesLogic(IDiceRollService dice)
    {
        this.dice = dice;
    }

    internal void UpgradeItem(Item item)
    {
        if      (item.Level <= 3)   item.Lore = "Not much can be said about this item.";
        else if (item.Level == 4)   UpgradeItemToHeirloom(item);
        else if (item.Level == 5)   UpgrageItemToArtifact(item);
        else  /*(item.Level >= 6)*/ UpgradeItemToRelic(item);
    }

    #region private methods
    #region heirloom
    private void UpgradeItemToHeirloom(Item item)
    {
        var randomHeirloom = dice.Roll_dX(ItemsLore.Heirlooms.All.Count) - 1;
        var heirloom = ItemsLore.Heirlooms.All[randomHeirloom];

        item.Value += 5000;
        DoubleSkills(item);

        item.Quality = heirloom.Quality;
        item.Lore = heirloom.Lore;

        if      (heirloom.Quality == ItemsLore.Heirlooms.Meteoric)      UpgradeItemToMeteoric(item);
        else if (heirloom.Quality == ItemsLore.Heirlooms.Dragon)        UpgradeItemToDragon(item);
        else if (heirloom.Quality == ItemsLore.Heirlooms.Galvron)       UpgradeItemToGalvron(item);
        else if (heirloom.Quality == ItemsLore.Heirlooms.Ectoquartz)    UpgradeItemToEctoquartz(item);
        else if (heirloom.Quality == ItemsLore.Heirlooms.Kemheil)       UpgradeItemToKemheil(item);
        else  /*(heirloom.Quality == ItemsLore.Heirlooms.Mithril)*/     UpgradeItemToMithril(item);
    }

    private static void UpgradeItemToMeteoric(Item item)
    {
        if (item.Type == ItemsLore.Types.Weapon)
        {
            if (item.HasTaint)
            {
                item.Sheet.Abstract += 100;
                item.Sheet.Mana += 100;
                item.Sheet.Harm *= 3;
            }
            else
            {
                item.Sheet.Purge += 10;
                item.Sheet.Harm += 150;
            }
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            if (item.HasTaint)
            {
                item.Sheet.Abstract += 100;
                item.Sheet.Mana += 100;
                item.Sheet.Armour += 20;
            }
            else
            {
                item.Sheet.Purge += 20;
                item.Sheet.Armour += 10;
            }
        }
        else /*(item.Type == ItemsLore.Types.Wealth)*/
        {
            item.Value += 100;
        }
    }
    private static void UpgradeItemToDragon(Item item)
    {
        if (item.Type == ItemsLore.Types.Weapon)
        {
            if (item.HasTaint)
            {
                item.Sheet.Abstract += 150;
                item.Sheet.Mana += 1000;
                item.Sheet.Harm *= 3;
            }
            else
            {
                item.Sheet.Harm += 350;
            }
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            if (item.HasTaint)
            {
                item.Sheet.Mana += 800;
                item.Sheet.Armour += 50;
            }
            else
            {
                item.Sheet.Armour += 50;
            }
        }
        else /*(item.Type == ItemsLore.Types.Wealth)*/
        {
            item.Value += 800;
        }
    }
    private static void UpgradeItemToGalvron(Item item)
    {
        item.HasTaint = false;
        item.Sheet.Willpower += 200;
        item.Sheet.Purge *= 2;
        item.Sheet.Mana -= 1000;

        if (item.Type == ItemsLore.Types.Weapon)
        {
            item.Sheet.Harm *= 3;
            item.Sheet.Harm += 350;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            item.Sheet.Armour += 25;
        }
        else /*(item.Type == ItemsLore.Types.Wealth)*/
        {
            item.Value += 1800;
        }
    }
    private static void UpgradeItemToEctoquartz(Item item)
    {
        if (item.Type == ItemsLore.Types.Weapon)
        {
            if (item.HasTaint)
            {
                item.Sheet.Mana += 500;
                item.Sheet.Harm *= 2;
            }
            else
            {
                item.Sheet.Harm += 150;
            }
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            if (item.HasTaint)
            {
                item.Sheet.Mana += 500;
                item.Sheet.Armour += 10;
            }
            else
            {
                item.Sheet.Armour += 15;
            }
        }
        else /*(item.Type == ItemsLore.Types.Wealth)*/
        {
            item.Value += 500;
        }
    }
    private static void UpgradeItemToKemheil(Item item)
    {
        if (item.Type == ItemsLore.Types.Weapon)
        {
            if (item.HasTaint)
            {
                item.Sheet.Mana += 250;
                item.Sheet.Harm *= 2;
            }
            else
            {
                item.Sheet.Harm += 250;
            }
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            if (item.HasTaint)
            {
                item.Sheet.Mana += 750;
                item.Sheet.Armour += 50;
            }
            else
            {
                item.Sheet.Armour += 55;
            }
        }
        else /*(item.Type == ItemsLore.Types.Wealth)*/
        {
            item.Value += 700;
        }
    }
    private static void UpgradeItemToMithril(Item item)
    {
        if (item.Type == ItemsLore.Types.Weapon)
        {
            item.Sheet.Harm *= 5;

            if (item.HasTaint)
            {
                item.Sheet.Mana += 50;
            }
            else
            {
                item.Sheet.Harm += 250;
            }
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            if (item.HasTaint)
            {
                item.Sheet.Mana += 50;
                item.Sheet.Armour += 75;
            }
            else
            {
                item.Sheet.Armour += 90;
            }
        }
        else /*(item.Type == ItemsLore.Types.Wealth)*/
        {
            item.Value += 1500;
        }
    }
    #endregion

    #region artifact
    private void UpgrageItemToArtifact(Item item)
    {
        var randomArtifact = dice.Roll_dX(ItemsLore.Artifacts.All.Count) - 1;
        var artifact = ItemsLore.Artifacts.All[randomArtifact];

        item.Value += 10000;
        DoubleSkills(item);
        DoubleStats(item);

        item.Name = artifact.Name;
        item.Quality = artifact.Quality;
        item.Lore = artifact.Lore;

        if (artifact.Quality == ItemsLore.Artifacts.MarshardDaggers) UpgradeItemToMarshardDaggers(item);
        else if (artifact.Quality == ItemsLore.Artifacts.HammerfistOfThunderLords) UpgradeItemToHammerfist(item);

    }
    private static void UpgradeItemToMarshardDaggers(Item item)
    {
        item.Name = ItemsLore.Artifacts.MarshardDaggers;
        item.Type = ItemsLore.Types.Weapon;
        item.Subtype = ItemsLore.Subtypes.Weapons.Dagger;
        item.Category = "Crystal daggers";
        item.HasTaint = true;

        item.Sheet.Harm += 150;
        item.Sheet.Harm *= 3;
        item.Sheet.Mana += 3000;
        item.Sheet.Abstract *= 5;
        item.Sheet.Purge -= 100;

        // HEROIC TRAITS
        // damage absorption
        // damage explosion

        // NEGATIVE PERKS
        // reduces spellcast around it
    }
    private static void UpgradeItemToHammerfist(Item item)
    {
        item.Name = ItemsLore.Artifacts.HammerfistOfThunderLords;
        item.Type = ItemsLore.Types.Weapon;
        item.Subtype = ItemsLore.Subtypes.Weapons.Mace;
        item.Category = "Maul hammer";
        item.HasTaint = false;

        item.Sheet.Harm += 550;
        item.Sheet.Harm *= 2;
        item.Sheet.Purge *= 2;

        // HEROIC TRAITS
        // damage absorption
        // damage explosion

        // NEGATIVE PERKS
        // reduces spellcast around it
    }
    #endregion

    #region relic
    private void UpgradeItemToRelic(Item item)
    {
        var randomRelic = dice.Roll_dX(ItemsLore.Relics.All.Count) - 1;
        var relic = ItemsLore.Relics.All[randomRelic];

        item.Value += 30000;
        DoubleSkills(item);
        DoubleAssets(item);
        DoubleStats(item);

        item.Name = relic.Name;
        item.Quality = relic.Quality;
        item.Lore = relic.Lore;
        item.Description = relic.Description;

        if (relic.Quality == ItemsLore.Relics.IlithosRod) UpgradeItemToIlithosRod(item);
        else if (relic.Quality == ItemsLore.Relics.SwordOfLuvoran) UpgradeItemToSwordOfLuvoran(item);
    }

    private static void UpgradeItemToIlithosRod(Item item)
    {
        item.Name = ItemsLore.Relics.IlithosRod;
        item.Type = ItemsLore.Types.Weapon;
        item.Subtype = ItemsLore.Subtypes.Weapons.Polearm;
        item.Category = "Metalic wooden rod";
        item.HasTaint = true;

        item.Sheet.Harm *= 10;
        item.Sheet.Mana += 5000;
        item.Sheet.Abstract *= 10;
        item.Sheet.Abstract += 250;

        // HEROIC TRAITS
        // all awareness rolls are successful
        // imbues all other items worn by doubling their character sheets
    }
    private static void UpgradeItemToSwordOfLuvoran(Item item)
    {
        item.Name = ItemsLore.Relics.SwordOfLuvoran;
        item.Type = ItemsLore.Types.Weapon;
        item.Subtype = ItemsLore.Subtypes.Weapons.Sword;
        item.Category = "Longsword";
        item.HasTaint = true;

        item.Sheet.Harm += 1000;
        item.Sheet.Mana += 1000;

        // HEROIC TRAITS
        // ignores 50% of armour
    }
    #endregion

    private static void DoubleStats(Item item)
    {
        item.Sheet.Strength *= 2;
        item.Sheet.Constitution *= 2;
        item.Sheet.Willpower *= 2;
        item.Sheet.Agility *= 2;
        item.Sheet.Perception *= 2;
        item.Sheet.Abstract *= 2;
    }
    private static void DoubleAssets(Item item)
    {
        item.Sheet.Stamina *= 2;
        item.Sheet.Harm *= 2;
        item.Sheet.Armour *= 2;
        item.Sheet.Purge *= 2;
        item.Sheet.Health *= 2;
        item.Sheet.Mana *= 2;
    }
    private static void DoubleSkills(Item item)
    {
        item.Sheet.Combat *= 2;
        item.Sheet.Arcane *= 2;
        item.Sheet.Psionics *= 2;
        item.Sheet.Hide *= 2;
        item.Sheet.Traps *= 2;
        item.Sheet.Tactics *= 2;
        item.Sheet.Social *= 2;
        item.Sheet.Apothecary *= 2;
        item.Sheet.Travel *= 2;
        item.Sheet.Sail *= 2;
    }
    #endregion
}