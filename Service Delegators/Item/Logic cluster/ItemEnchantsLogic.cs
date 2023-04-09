using Data_Mapping_Containers.Dtos;

namespace Service_Delegators.Logic_Cluster;

internal class ItemEnchantsLogic
{
    private readonly IDiceRollService dice;

    internal ItemEnchantsLogic(IDiceRollService dice)
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

    #region private methods
    private void Imbue(Item item)
    {
        IncreaseRandomSkill(dice.Roll_d20(true) * item.Level, item);
        IncreaseRandomAsset(dice.Roll_d20(true) * item.Level, item);
        IncreaseRandomStat(dice.Roll_d20(true) * item.Level, item);

        item.Sheet.Psionics -= dice.Roll_d20(true) * item.Level;
        item.Sheet.Purge -= dice.Roll_d20(true) * item.Level;
    }

    private void Strengthen(Item item)
    {
        if      (item.Type == ItemsLore.Types.Weapon)       item.Sheet.Harm += dice.Roll_d20(true) * item.Level;
        else if (item.Type == ItemsLore.Types.Protection)   item.Sheet.Defense += dice.Roll_dX(6) * item.Level;
        else  /*(item.Type == ItemsLore.Types.Wealth)*/     item.Value *= item.Level;

        if(item.Level >= 3) item.Sheet.Purge += dice.Roll_d20(true);
    }

    private void SetForCommons(Item item)
    {
        var indexOfQuality = dice.Roll_dX(ItemsLore.Qualities.Common.Count) - 1;
        item.Quality = ItemsLore.Qualities.Common[indexOfQuality];

        if (item.Type == ItemsLore.Types.Weapon)
        {
            var bonus = dice.Roll_d20();
            item.Sheet.Harm += bonus;
            item.Value += bonus * 2;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            var bonus = dice.Roll_dX(6) + 3;
            item.Sheet.Defense += bonus;
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
            item.Sheet.Harm += bonus;
            item.Value += bonus * 5;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            var bonus = dice.Roll_d20() + 5;
            item.Sheet.Defense += bonus;
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
            item.Sheet.Harm += bonus;
            item.Value += bonus * 5;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            var bonus = dice.Roll_d20() + 10;
            item.Sheet.Defense += bonus;
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
        var chosenStat = CharactersLore.Stats.All[skillIndex];

        if      (chosenStat == CharactersLore.Stats.Strength)        item.Sheet.Strength += amount;
        else if (chosenStat == CharactersLore.Stats.Constitution)    item.Sheet.Constitution += amount;
        else if (chosenStat == CharactersLore.Stats.Willpower)       item.Sheet.Willpower += amount;
        else if (chosenStat == CharactersLore.Stats.Agility)         item.Sheet.Agility += amount;
        else if (chosenStat == CharactersLore.Stats.Perception)      item.Sheet.Perception += amount;
        else  /*(choseStat == CharactersLore.Stats.Abstract)*/      item.Sheet.Abstract += amount;
    }

    private void IncreaseRandomAsset(int amount, Item item)
    {
        var skillIndex = dice.Roll_dX(CharactersLore.Assets.All.Count) - 1;
        var chosenAsset = CharactersLore.Assets.All[skillIndex];

        if      (chosenAsset == CharactersLore.Assets.Stamina)  item.Sheet.Endurance += amount;
        else if (chosenAsset == CharactersLore.Assets.Harm)     item.Sheet.Harm += amount;
        else if (chosenAsset == CharactersLore.Assets.Armour)   item.Sheet.Defense += amount;
        else if (chosenAsset == CharactersLore.Assets.Purge)    item.Sheet.Purge += amount;
        else if (chosenAsset == CharactersLore.Assets.Health)   item.Sheet.Health += amount;
        else  /*(chosenAsset == CharactersLore.Assets.Mana)*/   item.Sheet.Mana += amount;
    }

    private void IncreaseRandomSkill(int amount, Item item)
    {
        var skillIndex = dice.Roll_dX(CharactersLore.Skills.All.Count) - 1;
        var chosenSkill = CharactersLore.Skills.All[skillIndex];

        if      (chosenSkill == CharactersLore.Skills.Combat)       item.Sheet.Combat += amount;
        else if (chosenSkill == CharactersLore.Skills.Arcane)       item.Sheet.Arcane += amount;
        else if (chosenSkill == CharactersLore.Skills.Psionics)     item.Sheet.Psionics += amount;
        else if (chosenSkill == CharactersLore.Skills.Hide)         item.Sheet.Hide += amount;
        else if (chosenSkill == CharactersLore.Skills.Traps)        item.Sheet.Traps += amount;
        else if (chosenSkill == CharactersLore.Skills.Tactics)      item.Sheet.Tactics += amount;
        else if (chosenSkill == CharactersLore.Skills.Social)       item.Sheet.Social += amount;
        else if (chosenSkill == CharactersLore.Skills.Apothecary)   item.Sheet.Apothecary += amount;
        else if (chosenSkill == CharactersLore.Skills.Sail)         item.Sheet.Sail += amount;
        else  /*(chosenSkill == CharactersLore.Skills.Travel)*/     item.Sheet.Travel += amount;
    }
    #endregion
}
