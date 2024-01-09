using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface INpcGameplayLogic
{
    int CalculateNpcWorth(Character character, int locationEffortLvl);
}

public class NpcGameplayLogic : INpcGameplayLogic
{
    private readonly IDiceLogicDelegator dice;

    public NpcGameplayLogic(IDiceLogicDelegator dice)
    {
        this.dice = dice;
    }

    public int CalculateNpcWorth(Character character, int locationEffortLvl)
    {
        int[] stats =
        {
            character.Sheet.Stats.Strength,
            character.Sheet.Stats.Constitution,
            character.Sheet.Stats.Agility,
            character.Sheet.Stats.Willpower,
            character.Sheet.Stats.Perception,
            character.Sheet.Stats.Abstract
        };

        int[] skills =
        {
            character.Sheet.Skills.Arcane,
            character.Sheet.Skills.Psionics,
            character.Sheet.Skills.Hide,
            character.Sheet.Skills.Traps,
            character.Sheet.Skills.Tactics,
            character.Sheet.Skills.Social,
            character.Sheet.Skills.Apothecary,
            character.Sheet.Skills.Travel,
            character.Sheet.Skills.Sail
        };

        int[] assets =
        {
            character.Sheet.Assets.Harm,
            character.Sheet.Assets.Spot,
            (int)character.Sheet.Assets.Purge,
            (int)character.Sheet.Assets.Defense,
            (int)character.Sheet.Assets.DefenseFinal,
            character.Sheet.Assets.Resolve,
            character.Sheet.Assets.ResolveLeft,
            character.Sheet.Assets.Mana,
            character.Sheet.Assets.ManaLeft,
            (int)character.Sheet.Assets.Actions,
            (int)character.Sheet.Assets.ActionsLeft
        };

        var effort = dice.Roll_1_to_n(locationEffortLvl) * character.Status.EntityLevel;
        var statsAvg = stats.Average();
        var assetsAvg = assets.Average();
        var skillsAvg = skills.Average();

        return (int)(effort + statsAvg + assetsAvg + skillsAvg);
    }




}
