using Data_Mapping_Containers;
using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IGameplayLogicDelegator
{
    Location GetOrGenerateLocation(Position position);
    Ladder GetLadder();
}

public class GameplayLogicDelegator : IGameplayLogicDelegator
{
    public IValidations validations;
    public IGameplayLocationsLogic locations;
    public IGameplayCharactersLogic characters;

    public GameplayLogicDelegator(
        IValidations validations, 
        IGameplayLocationsLogic locations,
        IGameplayCharactersLogic characters)
    {
        this.validations = validations;
        this.locations = locations;
        this.characters = characters;
    }

    public Location GetOrGenerateLocation(Position position)
    {
        validations.ValidateLocation(position.Location);
        return locations.GetOrGenerateLocation(position);
    }

    public Ladder GetLadder()
    {
        return characters.GetCharactersLadder();
    }
}
