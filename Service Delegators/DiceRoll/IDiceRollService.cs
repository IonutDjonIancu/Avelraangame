using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IDiceRollService
{
    public int Roll_d20(bool hasReroll = false);

    public int Roll_dX(int n);

    DiceRoll Roll_d20_Traditionless(int bonus = 0);
    DiceRoll Roll_d20_Danarian(int bonus = 0);
    DiceRoll Roll_d20_Calvinian(int bonus = 0);
}