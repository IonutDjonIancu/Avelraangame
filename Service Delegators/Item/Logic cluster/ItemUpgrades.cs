using Data_Mapping_Containers.Dtos;

namespace Service_Delegators.Logic_Cluster;

internal class ItemUpgrades
{
    private readonly IDiceRollService dice;

    internal ItemUpgrades(IDiceRollService dice)
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
                item.Doll.Abstract += 100;
                item.Doll.Mana += 100;
                item.Doll.Harm *= 3;
            }
            else
            {
                item.Doll.Purge += 10;
                item.Doll.Harm += 150;
            }
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            if (item.HasTaint)
            {
                item.Doll.Abstract += 100;
                item.Doll.Mana += 100;
                item.Doll.Armour += 20;
            }
            else
            {
                item.Doll.Purge += 20;
                item.Doll.Armour += 10;
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
                item.Doll.Abstract += 150;
                item.Doll.Mana += 1000;
                item.Doll.Harm *= 3;
            }
            else
            {
                item.Doll.Harm += 350;
            }
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            if (item.HasTaint)
            {
                item.Doll.Mana += 800;
                item.Doll.Armour += 50;
            }
            else
            {
                item.Doll.Armour += 50;
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
        item.Doll.Willpower += 200;
        item.Doll.Purge *= 2;
        item.Doll.Mana -= 1000;

        if (item.Type == ItemsLore.Types.Weapon)
        {
            item.Doll.Harm *= 3;
            item.Doll.Harm += 350;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            item.Doll.Armour += 25;
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
                item.Doll.Mana += 500;
                item.Doll.Harm *= 2;
            }
            else
            {
                item.Doll.Harm += 150;
            }
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            if (item.HasTaint)
            {
                item.Doll.Mana += 500;
                item.Doll.Armour += 10;
            }
            else
            {
                item.Doll.Armour += 15;
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
                item.Doll.Mana += 250;
                item.Doll.Harm *= 2;
            }
            else
            {
                item.Doll.Harm += 250;
            }
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            if (item.HasTaint)
            {
                item.Doll.Mana += 750;
                item.Doll.Armour += 50;
            }
            else
            {
                item.Doll.Armour += 55;
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
            item.Doll.Harm *= 5;

            if (item.HasTaint)
            {
                item.Doll.Mana += 50;
            }
            else
            {
                item.Doll.Harm += 250;
            }
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            if (item.HasTaint)
            {
                item.Doll.Mana += 50;
                item.Doll.Armour += 75;
            }
            else
            {
                item.Doll.Armour += 90;
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

        item.Doll.Harm += 150;
        item.Doll.Harm *= 3;
        item.Doll.Mana += 3000;
        item.Doll.Abstract *= 5;
        item.Doll.Purge -= 100;

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

        item.Doll.Harm += 550;
        item.Doll.Harm *= 2;
        item.Doll.Purge *= 2;

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

        item.Doll.Harm *= 10;
        item.Doll.Mana += 5000;
        item.Doll.Abstract *= 10;
        item.Doll.Abstract += 250;

        // HEROIC TRAITS
        // all awareness rolls are successful
        // imbues all other items worn by doubling their dolls
    }
    private static void UpgradeItemToSwordOfLuvoran(Item item)
    {
        item.Name = ItemsLore.Relics.SwordOfLuvoran;
        item.Type = ItemsLore.Types.Weapon;
        item.Subtype = ItemsLore.Subtypes.Weapons.Sword;
        item.Category = "Longsword";
        item.HasTaint = true;

        item.Doll.Harm += 1000;
        item.Doll.Mana += 1000;

        // HEROIC TRAITS
        // ignores 50% of armour
    }
    #endregion

    private static void DoubleStats(Item item)
    {
        item.Doll.Strength *= 2;
        item.Doll.Constitution *= 2;
        item.Doll.Willpower *= 2;
        item.Doll.Agility *= 2;
        item.Doll.Perception *= 2;
        item.Doll.Abstract *= 2;
    }
    private static void DoubleAssets(Item item)
    {
        item.Doll.Stamina *= 2;
        item.Doll.Harm *= 2;
        item.Doll.Armour *= 2;
        item.Doll.Purge *= 2;
        item.Doll.Health *= 2;
        item.Doll.Mana *= 2;
    }
    private static void DoubleSkills(Item item)
    {
        item.Doll.Combat *= 2;
        item.Doll.Arcane *= 2;
        item.Doll.Psionics *= 2;
        item.Doll.Hide *= 2;
        item.Doll.Traps *= 2;
        item.Doll.Tactics *= 2;
        item.Doll.Social *= 2;
        item.Doll.Apothecary *= 2;
        item.Doll.Travel *= 2;
        item.Doll.Sail *= 2;
    }
    #endregion
}
