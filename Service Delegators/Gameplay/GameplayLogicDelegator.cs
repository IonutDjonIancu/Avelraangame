using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IGameplayLogicDelegator
{
    Location GetOrGenerateLocation(Position position);
    List<Quest> GenerateQuestsAtLocation(int locationEffortLevel);
    Ladder GetLadder();
}

public class GameplayLogicDelegator : IGameplayLogicDelegator
{
    public IValidations validations;
    public IGameplayLocationsLogic locations;
    public IGameplayCharactersLogic characters;
    public IGameplayQuestLogic quests;

    public GameplayLogicDelegator(
        IValidations validations, 
        IGameplayLocationsLogic locations,
        IGameplayCharactersLogic characters,
        IGameplayQuestLogic quests)
    {
        this.validations = validations;
        this.locations = locations;
        this.characters = characters;
        this.quests = quests;
    }

    public Location GetOrGenerateLocation(Position position)
    {
        validations.ValidateLocation(position.Location);
        return locations.GetOrGenerateLocation(position);
    }

    public List<Quest> GenerateQuestsAtLocation(int locationEffortLevel)
    {
        validations.ValidateLocationEffortLevel(locationEffortLevel);
        return quests.GenerateLocationQuests(locationEffortLevel);
    }


    public Ladder GetLadder()
    {
        return characters.GetCharactersLadder();
    }
}
