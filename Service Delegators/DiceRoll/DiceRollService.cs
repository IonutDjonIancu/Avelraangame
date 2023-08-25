using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class DiceRollService : IDiceRollService
{
    private readonly Random random = new();

    private readonly DiceRollValidator validator;
    private readonly DiceRollLogicDelegator logic;

    private readonly DiceD20RollsLogic d20Rolls;

    public DiceRollService(IDatabaseService databaseService)
    {
        validator = new DiceRollValidator(databaseService.Snapshot);
        logic = new DiceRollLogicDelegator();
    }

    public int Roll_d20_noReroll()
    {
        return d20Rolls.RollD20noReroll();
    }
    public int Roll_d20_withReroll()
    {
        return d20Rolls.RollD20withReroll();
    }
    public int Roll_d100_noReroll()
    {
        return logic.Roll100noReroll();
    }
    public int Roll_d100_withReroll()
    {
        return logic.Roll100withReroll();
    }
    public bool Roll_true_false()
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
    public int Roll_character_gameplay_dice(bool isOffense, string attribute, Character character)
    {
        validator.ValidateCharacterBeforeRoll(character, attribute);
        return logic.RollGameplayDice(isOffense, attribute, character);
    }
}
