using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IDiceRollService
{
    public bool FlipCoin();

    public int Roll_d20(bool hasReroll = false);
    public int Roll_d100(bool hasReroll = false);

    public int Roll_1dX(int upperLimit);
    public int Roll_XdY(int lowerLimit, int upperLimit);

    DiceRoll Roll_d20_NoHeritage(int bonus = 0);
    DiceRoll Roll_d20_Traditional(int bonus = 0);
    DiceRoll Roll_d20_Martial(int bonus = 0);
}