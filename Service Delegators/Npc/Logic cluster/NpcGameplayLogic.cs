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
        return ServicesUtils.CalculateWorth(character, dice);
    }
}
