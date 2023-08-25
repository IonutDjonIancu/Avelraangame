using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IDiceDelegator
{
    int Roll_d20_noReroll();
    int Roll_d20_withReroll();
    int Roll_d100_noReroll();
    int Roll_d100_withReroll();
    bool Roll_true_false();
    int Roll_1_to_n(int upperLimit);
    int Roll_n_to_n(int lowerLimit, int upperLimit);

    int Roll_character_gameplay_dice(bool isOffense, string attribute, Character character);
}
