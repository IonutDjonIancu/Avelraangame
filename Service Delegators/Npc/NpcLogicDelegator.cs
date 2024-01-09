using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface INpcLogicDelegator
{
    Character GenerateBadGuy(string locationName);
    Character GenerateGoodGuy(string locationName);

    int CalculateNpcWorth(Character character, int locationEffortLvl);
}

public class NpcLogicDelegator : INpcLogicDelegator
{
    private readonly IValidations validations;
    private readonly INpcCreateLogic npcCreateLogic;
    private readonly INpcGameplayLogic npcGameplayLogic;

    public NpcLogicDelegator(
        IValidations validations,
        INpcCreateLogic npcCreateLogic,
        INpcGameplayLogic npcGameplayLogic)
    {
        this.validations = validations;
        this.npcCreateLogic = npcCreateLogic;
        this.npcGameplayLogic = npcGameplayLogic;
    }

    public Character GenerateGoodGuy(string locationName)
    {
        validations.ValidateLocation(locationName);
        return npcCreateLogic.GenerateNpcCharacter(locationName, true);
    }

    public Character GenerateBadGuy(string locationName)
    {
        validations.ValidateLocation(locationName);
        return npcCreateLogic.GenerateNpcCharacter(locationName, false);
    }

    public int CalculateNpcWorth(Character character, int locationEffortLvl)
    {
        validations.ValidateBeforeNpcCalculateWorth(character, locationEffortLvl);
        return npcGameplayLogic.CalculateNpcWorth(character, locationEffortLvl);
    }
}