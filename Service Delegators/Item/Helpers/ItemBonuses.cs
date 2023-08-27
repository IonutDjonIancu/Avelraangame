using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal static class ItemBonuses
{
    internal static void SetItemBonuses(Item item, IDiceLogicDelegator dice)
    {
        if (item.Level >= 1) SetForCommons(item, dice);
        if (item.Level >= 2) SetForRefineds(item, dice);
        if (item.Level >= 3) SetForMasterworks(item, dice);
    }

    internal static void StrengthenOrImbue(Item item, IDiceLogicDelegator dice)
    {
        if (item.HasTaint)
        {
            Imbue(item, dice);
        }
        else
        {
            Strengthen(item, dice);
        }
    }

    #region private methods
    private static void Imbue(Item item, IDiceLogicDelegator dice)
    {
        IncreaseRandomSkill(dice.Roll_d20_noReroll() * item.Level, item, dice);
        IncreaseRandomAsset(dice.Roll_d20_withReroll() * item.Level, item, dice);
        IncreaseRandomStat(dice.Roll_d20_withReroll() * item.Level, item, dice);

        item.Sheet.Skills.Psionics -= dice.Roll_d20_withReroll() * item.Level;
        item.Sheet.Assets.Purge -= dice.Roll_d20_withReroll() * item.Level;
    }

    private static void Strengthen(Item item, IDiceLogicDelegator dice)
    {
        if (item.Type == ItemsLore.Types.Weapon) item.Sheet.Assets.Harm += dice.Roll_d20_withReroll() * item.Level;
        else if (item.Type == ItemsLore.Types.Protection) item.Sheet.Assets.Defense += dice.Roll_1_to_n(6) * item.Level;
        else  /*(item.Type == ItemsLore.Types.Wealth)*/     item.Value *= item.Level;

        if (item.Level >= 3) item.Sheet.Assets.Purge += dice.Roll_d20_withReroll();
    }

    private static void IncreaseRandomStat(int amount, Item item, IDiceLogicDelegator dice)
    {
        var skillIndex = dice.Roll_1_to_n(CharactersLore.Stats.All.Count) - 1;
        var chosenStat = CharactersLore.Stats.All[skillIndex];

        if (chosenStat == CharactersLore.Stats.Strength) item.Sheet.Stats.Strength += amount;
        else if (chosenStat == CharactersLore.Stats.Constitution) item.Sheet.Stats.Constitution += amount;
        else if (chosenStat == CharactersLore.Stats.Willpower) item.Sheet.Stats.Willpower += amount;
        else if (chosenStat == CharactersLore.Stats.Agility) item.Sheet.Stats.Agility += amount;
        else if (chosenStat == CharactersLore.Stats.Perception) item.Sheet.Stats.Perception += amount;
        else  /*(choseStat == CharactersLore.Stats.Abstract)*/      item.Sheet.Stats.Abstract += amount;
    }

    private static void IncreaseRandomAsset(int amount, Item item, IDiceLogicDelegator dice)
    {
        var skillIndex = dice.Roll_1_to_n(CharactersLore.Assets.All.Count) - 1;
        var chosenAsset = CharactersLore.Assets.All[skillIndex];

        if (chosenAsset == CharactersLore.Assets.Resolve) item.Sheet.Assets.Resolve += amount;
        else if (chosenAsset == CharactersLore.Assets.Harm) item.Sheet.Assets.Harm += amount;
        else if (chosenAsset == CharactersLore.Assets.Spot) item.Sheet.Assets.Spot += amount;
        else if (chosenAsset == CharactersLore.Assets.Defense) item.Sheet.Assets.Defense += amount;
        else if (chosenAsset == CharactersLore.Assets.Purge) item.Sheet.Assets.Purge += amount;
        else  /*(chosenAsset == CharactersLore.Assets.Mana)*/   item.Sheet.Assets.Mana += amount;
    }

    private static void IncreaseRandomSkill(int amount, Item item, IDiceLogicDelegator dice)
    {
        var skillIndex = dice.Roll_1_to_n(CharactersLore.Skills.All.Count) - 1;
        var chosenSkill = CharactersLore.Skills.All[skillIndex];

        if (chosenSkill == CharactersLore.Skills.Combat) item.Sheet.Skills.Combat += amount;
        else if (chosenSkill == CharactersLore.Skills.Arcane) item.Sheet.Skills.Arcane += amount;
        else if (chosenSkill == CharactersLore.Skills.Psionics) item.Sheet.Skills.Psionics += amount;
        else if (chosenSkill == CharactersLore.Skills.Hide) item.Sheet.Skills.Hide += amount;
        else if (chosenSkill == CharactersLore.Skills.Traps) item.Sheet.Skills.Traps += amount;
        else if (chosenSkill == CharactersLore.Skills.Tactics) item.Sheet.Skills.Tactics += amount;
        else if (chosenSkill == CharactersLore.Skills.Social) item.Sheet.Skills.Social += amount;
        else if (chosenSkill == CharactersLore.Skills.Apothecary) item.Sheet.Skills.Apothecary += amount;
        else if (chosenSkill == CharactersLore.Skills.Sail) item.Sheet.Skills.Sail += amount;
        else  /*(chosenSkill == CharactersLore.Skills.Travel)*/     item.Sheet.Skills.Travel += amount;
    }

    private static void SetForCommons(Item item, IDiceLogicDelegator dice)
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

    private static void SetForRefineds(Item item, IDiceLogicDelegator dice)
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

    private static void SetForMasterworks(Item item, IDiceLogicDelegator dice)
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
    #endregion
}
