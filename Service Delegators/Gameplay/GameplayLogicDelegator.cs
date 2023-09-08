using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IGameplayLogicDelegator
{
    Location GetLocation(Position position);
}

public class GameplayLogicDelegator : IGameplayLogicDelegator
{
    public IValidations validations;
    public IGameplayLocationsLogic locations;

    public GameplayLogicDelegator(
        IValidations validations, 
        IGameplayLocationsLogic locations)
    {
        this.validations = validations;
        this.locations = locations;
    }

    public Location GetLocation(Position position)
    {
        validations.ValidateLocation(position.Location);
        return locations.GetOrGenerateLocation(position);
    }
}
