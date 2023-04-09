using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class DiceRollService : IDiceRollService
{
    private readonly DiceRollLogicDelegator logic;

    public DiceRollService()
    {
        logic = new DiceRollLogicDelegator();
    }

    public int Roll_d20(bool hasReroll = false)
    {
        if (hasReroll)
        {
            return logic.Roll1d20WithReroll();
        }
        else
        {
            return logic.Roll1d20NoReroll();
        }
    }

    public int Roll_d100(bool hasReroll = false)
    {
        if (hasReroll)
        {
            return logic.Roll1d100WithReroll();
        }
        else
        {
            return logic.Roll1d100NoReroll();
        }
    }

    public int Roll_dX(int x)
    {
        return logic.Roll1dNnoReroll(x);
    }

    public DiceRoll Roll_d20_NoHeritage(int bonus = 0)
    {
        return logic.GenerateRollFor(CharactersLore.Heritage.None, bonus);
    }

    public DiceRoll Roll_d20_Traditional(int bonus = 0)
    {
        return logic.GenerateRollFor(CharactersLore.Heritage.Traditional, bonus);
    }

    public DiceRoll Roll_d20_Martial(int bonus = 0)
    {
        return logic.GenerateRollFor(CharactersLore.Heritage.Martial, bonus);
    }
}
