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

            var quest = new Quest
            {
                QuestType = template.QuestType,
                Fame = template.Fame,
                Description = template.Description,
                Result = template.Result,
                IsRepeatable = template.IsRepeatable,
                Id = Guid.NewGuid().ToString(),
                EffortLvl = dice.Roll_1_to_n(effortLvl),
                Encounters = dice.Roll_1_to_n(effortLvl),
                Reward = GeneratePossibleReward(effortLvl)
            };

            quest.EncountersLeft = quest.Encounters;

            quests.Add(quest);
        }
    }

    private string GeneratePossibleReward(int effortLvl)
    {
        var rewards = new List<string>();

        var effort = dice.Roll_1_to_n(effortLvl);

        if (effort >= GameplayLore.Effort.Normal) rewards.Add(GameplayLore.QuestReward.Loot);
        if (effort >= GameplayLore.Effort.Gifted) rewards.Add(GameplayLore.QuestReward.Stats);
        if (effort >= GameplayLore.Effort.Chosen) rewards.Add(GameplayLore.QuestReward.Skills);
        if (effort >= GameplayLore.Effort.Hero) rewards.Add(GameplayLore.QuestReward.SpecialSkills);

        rewards.Add(GameplayLore.QuestReward.Item);
        rewards.Add(GameplayLore.QuestReward.Wealth);

        var rewardIndex = dice.Roll_1_to_n(rewards.Count) - 1;

        return rewards[rewardIndex];
    }
    #endregion
}
