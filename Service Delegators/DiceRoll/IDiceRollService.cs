using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IDiceRollService
{
    int Roll_20_noReroll();
    int Roll_20_withReroll();
    int Roll_100_noReroll();
    int Roll_100_withReroll();
    bool Roll_par_impar();
    int Roll_1_to_n(int upperLimit);
    int Roll_n_to_n(int lowerLimit, int upperLimit);

    int Roll_character_gameplay_dice(bool isOffense, string attribute, Character character);
}