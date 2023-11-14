using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Service_Delegators;

public interface IGameplayQuestLogic
{
    List<Quest> GenerateLocationQuests(int locationEffortLevel);
}

public class GameplayQuestLogic : IGameplayQuestLogic
{
    public readonly object _lock = new();

    public readonly Snapshot snapshot;
    public readonly IDiceLogicDelegator dice;

    public GameplayQuestLogic(
        Snapshot snapshot,
        IDiceLogicDelegator dice)
    {
        this.snapshot = snapshot;
        this.dice = dice;
    }

    public List<Quest> GenerateLocationQuests(int locationEffortLevel)
    {
        var allRepeatableQuests = GameplayLore.Quests.Repeatable.All.Where(s => s.MaxEffortLvl <= locationEffortLevel).ToList();
        var allOneTimeQuests = GameplayLore.Quests.OneTime.All.Where(s => s.MaxEffortLvl <= locationEffortLevel).ToList();

        var maxRepeatableQuests = dice.Roll_1_to_n(10);
        var maxOneTimeQuests = dice.Roll_1_to_n(5);

        var quests = new List<Quest>();

        AddToQuestList(maxRepeatableQuests, locationEffortLevel, allRepeatableQuests, quests);
        AddToQuestList(maxOneTimeQuests, locationEffortLevel, allOneTimeQuests, quests);

        return quests;
    }

    #region private methods
    private void AddToQuestList(int amount, int effortLvl, List<QuestTemplate> templates, List<Quest> quests)
    {
        for (int i = 0; i < amount; i++)
        {
            var index = dice.Roll_1_to_n(templates.Count) - 1;
            var template = templates[index];

            var rewardIndex = dice.Roll_1_to_n(GameplayLore.QuestReward.All.Count) - 1;

            var quest = new Quest
            {
                QuestType = template.QuestType,
                Fame = template.Fame,
                Description = template.Description,
                Result = template.Result,
                IsRepeatable = template.IsRepeatable,
                EffortLvl = dice.Roll_1_to_n(effortLvl),
                EncountersLeft = dice.Roll_1_to_n(effortLvl),
                Reward = GameplayLore.QuestReward.All[rewardIndex]
            };

            quests.Add(quest);
        }
    }
    #endregion
}
