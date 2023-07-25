namespace Service_Delegators;

public interface IDiceRollService
{
    int Roll_20_noReroll();
    int Roll_20_withReroll();
    int Roll_100_noReroll();
    int Roll_100_withReroll();
    (int grade, int crits) Roll_gameplay_dice(string tradition, int skill);
    bool Roll_par_impar();
    int Roll_1_to_n(int upperLimit);
    int Roll_n_to_n(int lowerLimit, int upperLimit);
}