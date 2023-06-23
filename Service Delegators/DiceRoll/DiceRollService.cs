using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class DiceRollService : IDiceRollService
{
    private readonly DiceRollLogicDelegator logic;

    public DiceRollService()
    {
        logic = new DiceRollLogicDelegator();
    }

    public bool FlipCoin()
    {
        return logic.FlipCoin();
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

    public int Roll_1dX(int x)
    {
        return logic.Roll1dXnoReroll(x);
    }

    public int Roll_XdY(int x, int y)
    {
        return logic.RollXdYnoReroll(x, y);
    }

    public DiceRoll Roll_d20_Common(int bonus = 0)
    {
        return logic.GenerateRollFor(CharactersLore.Tradition.Common, bonus);
    }

    public DiceRoll Roll_d20_Martial(int bonus = 0)
    {
        return logic.GenerateRollFor(CharactersLore.Tradition.Martial, bonus);
    }
}
