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
        IncreaseRandomSkill(dice.Roll_d20_withReroll() * item.Level, item);
        IncreaseRandomAsset(dice.Roll_d20_withReroll() * item.Level, item);
        IncreaseRandomStat(dice.Roll_d20_withReroll() * item.Level, item);

        item.Sheet.Skills.Psionics -= dice.Roll_d20_withReroll() * item.Level;
        item.Sheet.Assets.Purge -= dice.Roll_d20_withReroll() * item.Level;
    }

    private void Strengthen(Item item)
    {
        if      (item.Type == ItemsLore.Types.Weapon)       item.Sheet.Assets.Harm += dice.Roll_d20_withReroll() * item.Level;
        else if (item.Type == ItemsLore.Types.Protection)   item.Sheet.Assets.Defense += dice.Roll_1_to_n(6) * item.Level;
        else  /*(item.Type == ItemsLore.Types.Wealth)*/     item.Value *= item.Level;

        if (item.Level >= 3) item.Sheet.Assets.Purge += dice.Roll_d20_withReroll();
    }

    private void SetForCommons(Item item)
    {
        var indexOfQuality = dice.Roll_1_to_n(ItemsLore.Qualities.Common.Count) - 1;
        item.Quality = ItemsLore.Qualities.Common[indexOfQuality];

        if (item.Type == ItemsLore.Types.Weapon)
        {
            var bonus = dice.Roll_d20_noReroll();
            item.Sheet.Assets.Harm += bonus;
            item.Value += bonus * 2;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            var bonus = dice.Roll_1_to_n(6) + 3;
            item.Sheet.Assets.Defense += bonus;
            item.Value += bonus * 10;
        }
        else /* Wealth */
        {
            var bonus = dice.Roll_d20_withReroll() + 10;
            item.Value += bonus;
        }
    }

    private void SetForRefineds(Item item)
    {
        var indexOfQuality = dice.Roll_1_to_n(ItemsLore.Qualities.Refined.Count) - 1;
        item.Quality = ItemsLore.Qualities.Refined[indexOfQuality];

        if (item.Type == ItemsLore.Types.Weapon)
        {
            var bonus = dice.Roll_d20_withReroll();
            item.Sheet.Assets.Harm += bonus;
            item.Value += bonus * 5;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            var bonus = dice.Roll_1_to_n(6) + dice.Roll_1_to_n(6);
            item.Sheet.Assets.Defense += bonus;
            item.Value += bonus * 10;
        }
        else /* Wealth */
        {
            var bonus = dice.Roll_d20_withReroll() + 10;
            item.Value += bonus;
        }
    }

    private void SetForMasterworks(Item item)
    {
        var indexOfQuality = dice.Roll_1_to_n(ItemsLore.Qualities.Masterwork.Count) - 1;
        item.Quality = ItemsLore.Qualities.Masterwork[indexOfQuality];

        if (item.Type == ItemsLore.Types.Weapon)
        {
            var bonus = dice.Roll_d20_withReroll() * 2;
            item.Sheet.Assets.Harm += bonus;
            item.Value += bonus * 5;
        }
        else if (item.Type == ItemsLore.Types.Protection)
        {
            var bonus = dice.Roll_d20_noReroll() + dice.Roll_1_to_n(6);
            item.Sheet.Assets.Defense += bonus;
            item.Value += bonus * 10;
        }
        else /* Wealth */
        {
            var bonus = dice.Roll_d20_withReroll() + 500;
            item.Value += bonus;
        }
    }

    private void IncreaseRandomStat(int amount, Item item)
    {
        var skillIndex = dice.Roll_1_to_n(CharactersLore.Stats.All.Count) - 1;
        var chosenStat = CharactersLore.Stats.All[skillIndex];

        if      (chosenStat == CharactersLore.Stats.Strength)       item.Sheet.Stats.Strength += amount;
        else if (chosenStat == CharactersLore.Stats.Constitution)   item.Sheet.Stats.Constitution += amount;
        else if (chosenStat == CharactersLore.Stats.Willpower)      item.Sheet.Stats.Willpower += amount;
        else if (chosenStat == CharactersLore.Stats.Agility)        item.Sheet.Stats.Agility += amount;
        else if (chosenStat == CharactersLore.Stats.Perception)     item.Sheet.Stats.Perception += amount;
        else  /*(choseStat == CharactersLore.Stats.Abstract)*/      item.Sheet.Stats.Abstract += amount;
    }

    private void IncreaseRandomAsset(int amount, Item item)
    {
        var skillIndex = dice.Roll_1_to_n(CharactersLore.Assets.All.Count) - 1;
        var chosenAsset = CharactersLore.Assets.All[skillIndex];

        if      (chosenAsset == CharactersLore.Assets.Resolve)  item.Sheet.Assets.Resolve += amount;
        else if (chosenAsset == CharactersLore.Assets.Harm)     item.Sheet.Assets.Harm += amount;
        else if (chosenAsset == CharactersLore.Assets.Spot)     item.Sheet.Assets.Spot += amount;
        else if (chosenAsset == CharactersLore.Assets.Defense)  item.Sheet.Assets.Defense += amount;
        else if (chosenAsset == CharactersLore.Assets.Purge)    item.Sheet.Assets.Purge += amount;
        else  /*(chosenAsset == CharactersLore.Assets.Mana)*/   item.Sheet.Assets.Mana += amount;
    }

    private void IncreaseRandomSkill(int amount, Item item)
    {
        var skillIndex = dice.Roll_1_to_n(CharactersLore.Skills.All.Count) - 1;
        var chosenSkill = CharactersLore.Skills.All[skillIndex];

        if      (chosenSkill == CharactersLore.Skills.Combat)       item.Sheet.Skills.Combat += amount;
        else if (chosenSkill == CharactersLore.Skills.Arcane)       item.Sheet.Skills.Arcane += amount;
        else if (chosenSkill == CharactersLore.Skills.Psionics)     item.Sheet.Skills.Psionics += amount;
        else if (chosenSkill == CharactersLore.Skills.Hide)         item.Sheet.Skills.Hide += amount;
        else if (chosenSkill == CharactersLore.Skills.Traps)        item.Sheet.Skills.Traps += amount;
        else if (chosenSkill == CharactersLore.Skills.Tactics)      item.Sheet.Skills.Tactics += amount;
        else if (chosenSkill == CharactersLore.Skills.Social)       item.Sheet.Skills.Social += amount;
        else if (chosenSkill == CharactersLore.Skills.Apothecary)   item.Sheet.Skills.Apothecary += amount;
        else if (chosenSkill == CharactersLore.Skills.Sail)         item.Sheet.Skills.Sail += amount;
        else  /*(chosenSkill == CharactersLore.Skills.Travel)*/     item.Sheet.Skills.Travel += amount;
    }
    #endregion
}
