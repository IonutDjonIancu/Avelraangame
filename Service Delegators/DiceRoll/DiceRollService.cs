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

    public DiceRoll Roll_d20_Traditionless(int bonus = 0)
    {
        return logic.GenerateRollFor(CharactersLore.Traditions.None, bonus);
    }

    public DiceRoll Roll_d20_Danarian(int bonus = 0)
    {
        return logic.GenerateRollFor(CharactersLore.Traditions.Ravanon, bonus);
    }

    public DiceRoll Roll_d20_Calvinian(int bonus = 0)
    {
        return logic.GenerateRollFor(CharactersLore.Traditions.Endarii, bonus);
    }
}
