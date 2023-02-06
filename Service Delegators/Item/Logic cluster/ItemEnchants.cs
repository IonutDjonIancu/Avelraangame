using Data_Mapping_Containers.Dtos;

namespace Service_Delegators.Logic_Cluster;

internal class ItemEnchants
{
    private readonly IDiceRollService dice;

    internal ItemEnchants(IDiceRollService dice)
	{
        this.dice = dice;
	}

    internal void SetItemBonuses(Item item)
    {
        if (item.Level >= 1) SetForCommons(item);
        if (item.Level >= 2) SetForRefineds(item);
        if (item.Level >= 3) SetForMasterworks(item);
    }

    internal void StrengthenOrImbue(Item item)
    {
        if (item.HasTaint)
        {
            Imbue(item);
        }
        else
        {
            Strengthen(item);
        }
    }

    #region privates
    private void Imbue(Item item)
    {
        IncreaseRandomSkill(dice.Roll_d20(true) * item.Level, item);
        IncreaseRandomAsset(dice.Roll_d20(true) * item.Level, item);
        IncreaseRandomStat(dice.Roll_d20(true) * item.Level, item);

        item.Doll.Psionics -= dice.Roll_d20(true) * item.Level;
        item.Doll.Purge -= dice.Roll_d20(true) * item.Level;
    }

    private void Strengthen(Item item)
    {
        if      (item.Type == ItemsLore.Types.Weapon)       item.Doll.Harm += dice.Roll_d20(true) * item.Level;
        else if (item.Type == ItemsLore.Types.Protection)   item.Doll.Armour += dice.Roll_dX(6) * item.Level;
        else  /*(item.Type == ItemsLore.Types.Wealth)*/     item.Value *= item.Level;

        if(item.Level >= 3) item.Doll.Purge += dice.Roll_d20(true);
    }

    private void SetForCommons(Item item)
    {
        var indexOfQuality = dice.Roll_dX(ItemsLore.Qualities.Common.Count) - 1;
        item.Quality = ItemsLore.Qualities.Common[indexOfQuality];

        if (item.Type == ItemsLore.Types.Weapon)
        {
            var bonus = dice.Roll_d20();
            item.Doll.Harm += bonus;
            item.Value += bonus * 2;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            var bonus = dice.Roll_dX(6) + 3;
            item.Doll.Armour += bonus;
            item.Value += bonus * 10;
        }
        else /* Wealth */
        {
            var bonus = dice.Roll_d20(true) + 10;
            item.Value += bonus;
        }
    }

    private void SetForRefineds(Item item)
    {
        var indexOfQuality = dice.Roll_dX(ItemsLore.Qualities.Refined.Count) - 1;
        item.Quality = ItemsLore.Qualities.Refined[indexOfQuality];

        if (item.Type == ItemsLore.Types.Weapon)
        {
            var bonus = dice.Roll_d20(true);
            item.Doll.Harm += bonus;
            item.Value += bonus * 5;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            var bonus = dice.Roll_d20() + 5;
            item.Doll.Armour += bonus;
            item.Value += bonus * 10;
        }
        else /* Wealth */
        {
            var bonus = dice.Roll_d20(true) + 10;
            item.Value += bonus;
        }
    }

    private void SetForMasterworks(Item item)
    {
        var indexOfQuality = dice.Roll_dX(ItemsLore.Qualities.Masterwork.Count) - 1;
        item.Quality = ItemsLore.Qualities.Masterwork[indexOfQuality];

        if (item.Type == ItemsLore.Types.Weapon)
        {
            var bonus = dice.Roll_d20(true) * 2;
            item.Doll.Harm += bonus;
            item.Value += bonus * 5;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            var bonus = dice.Roll_d20() + 10;
            item.Doll.Armour += bonus;
            item.Value += bonus * 10;
        }
        else /* Wealth */
        {
            var bonus = dice.Roll_d20(true) + 500;
            item.Value += bonus;
        }
    }

    private void IncreaseRandomStat(int amount, Item item)
    {
        var skillIndex = dice.Roll_dX(CharactersLore.Stats.All.Count) - 1;
        var choseStat = CharactersLore.Stats.All[skillIndex];

        if      (choseStat == CharactersLore.Stats.Strength)    item.Doll.Strength += amount;
        else if (choseStat == CharactersLore.Stats.Endurance)   item.Doll.Constitution += amount;
        else if (choseStat == CharactersLore.Stats.Resolve)     item.Doll.Willpower += amount;
        else if (choseStat == CharactersLore.Stats.Athletics)   item.Doll.Agility += amount;
        else if (choseStat == CharactersLore.Stats.Awareness)   item.Doll.Perception += amount;
        else  /*(choseStat == CharactersLore.Stats.Abstract)*/  item.Doll.Abstract += amount;
    }

    private void IncreaseRandomAsset(int amount, Item item)
    {
        var skillIndex = dice.Roll_dX(CharactersLore.Assets.All.Count) - 1;
        var chosenAsset = CharactersLore.Assets.All[skillIndex];

        if      (chosenAsset == CharactersLore.Assets.Stamina)  item.Doll.Stamina += amount;
        else if (chosenAsset == CharactersLore.Assets.Harm)     item.Doll.Harm += amount;
        else if (chosenAsset == CharactersLore.Assets.Armour)   item.Doll.Armour += amount;
        else if (chosenAsset == CharactersLore.Assets.Purge)    item.Doll.Purge += amount;
        else if (chosenAsset == CharactersLore.Assets.Health)   item.Doll.Health += amount;
        else  /*(chosenAsset == CharactersLore.Assets.Mana)*/   item.Doll.Mana += amount;
    }

    private void IncreaseRandomSkill(int amount, Item item)
    {
        var skillIndex = dice.Roll_dX(CharactersLore.Skills.All.Count) - 1;
        var chosenSkill = CharactersLore.Skills.All[skillIndex];

        if      (chosenSkill == CharactersLore.Skills.Combat)       item.Doll.Combat += amount;
        else if (chosenSkill == CharactersLore.Skills.Arcane)       item.Doll.Arcane += amount;
        else if (chosenSkill == CharactersLore.Skills.Psionics)     item.Doll.Psionics += amount;
        else if (chosenSkill == CharactersLore.Skills.Hide)         item.Doll.Hide += amount;
        else if (chosenSkill == CharactersLore.Skills.Traps)        item.Doll.Traps += amount;
        else if (chosenSkill == CharactersLore.Skills.Tactics)      item.Doll.Tactics += amount;
        else if (chosenSkill == CharactersLore.Skills.Social)       item.Doll.Social += amount;
        else if (chosenSkill == CharactersLore.Skills.Apothecary)   item.Doll.Apothecary += amount;
        else if (chosenSkill == CharactersLore.Skills.Sail)         item.Doll.Sail += amount;
        else  /*(chosenSkill == CharactersLore.Skills.Travel)*/     item.Doll.Travel += amount;
    }
    #endregion
}
