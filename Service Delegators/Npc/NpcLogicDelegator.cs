using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface INpcLogicDelegator
{
    Character GenerateBadGuy(string locationName);
    Character GenerateGoodGuy(string locationName);
}

public class NpcLogicDelegator : INpcLogicDelegator
{
    private readonly IValidations validations;
    private readonly INpcCreateLogic createLogic;

    public NpcLogicDelegator(
        IValidations validations,
        INpcCreateLogic createLogic)
    {
        this.validations = validations;
        this.createLogic = createLogic;
    }

    public Character GenerateGoodGuy(string locationName)
    {
        validations.ValidateLocation(locationName);
        return createLogic.GenerateNpcCharacter(locationName, true);
    }

    public Character GenerateBadGuy(string locationName)
    {
        validations.ValidateLocation(locationName);
        return createLogic.GenerateNpcCharacter(locationName, false);
    }
}