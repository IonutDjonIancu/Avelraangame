using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class DiceRollService : IDiceRollService
{
    private readonly DiceRollValidator validator;
    private readonly DiceRollLogicDelegator logic;

    public DiceRollService(IDatabaseService databaseService)
    {
        validator = new DiceRollValidator(databaseService.Snapshot);
        logic = new DiceRollLogicDelegator();
    }

    public int Roll_20_noReroll()
    {
        return logic.Roll20noReroll();
    }
    public int Roll_20_withReroll()
    {
        return logic.Roll20withReroll();
    }
    public int Roll_100_noReroll()
    {
        return logic.Roll100noReroll();
    }
    public int Roll_100_withReroll()
    {
        return logic.Roll100withReroll();
    }
    public int Roll_character_dice(Character character, string skill)
    {
        validator.ValidateCharacterBeforeRoll(character, skill);
        return logic.RollGameplayDice(character, skill);
    }
    public bool Roll_par_impar()
    {
        return logic.RollParImpar();
    }
    public int Roll_1_to_n(int upperLimit)
    {
        return logic.Roll1ToN(upperLimit);
    }
    public int Roll_n_to_n(int lowerLimit, int upperLimit)
    {
        return logic.RollNToN(lowerLimit, upperLimit);
    }
}
