using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class GameplayQuestLogic
{
    public readonly object _lock = new();

    public readonly Snapshot snapshot;
    public readonly IValidations validations;

    public GameplayQuestLogic(
        Snapshot snapshot,
        IValidations validations)
    {
        this.snapshot = snapshot;
        this.validations = validations;
    }

    public Quest GenerateQuest(int effortLevel)
    {


        Quest quest = new Quest();

        return quest;
    }

}
