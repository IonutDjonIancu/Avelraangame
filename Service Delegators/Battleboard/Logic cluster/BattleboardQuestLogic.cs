using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Service_Delegators;

public interface IBattleboardQuestLogic
{
    Battleboard StartQuest(BattleboardActor actor);
    Battleboard EndQuest(BattleboardActor actor);
    Battleboard StopQuest(BattleboardActor actor);
}

public class BattleboardQuestLogic : IBattleboardQuestLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;
    private readonly IDiceLogicDelegator diceLogic;
    private readonly IItemsLogicDelegator itemsLogic;
    private readonly INpcLogicDelegator npcLogic;

    public BattleboardQuestLogic(
        Snapshot snapshot,
        IDiceLogicDelegator diceLogic,
        IItemsLogicDelegator itemsLogic,
        INpcLogicDelegator npcLogic)
    {
        this.snapshot = snapshot;
        this.npcLogic = npcLogic;
        this.diceLogic = diceLogic;
        this.itemsLogic = itemsLogic;   
    }

    public Battleboard StartQuest(BattleboardActor actor)
    {
        lock (_lock)
        {
            var attacker = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            var location = ServicesUtils.GetSnapshotLocationByPosition(attacker.Status.Position, snapshot);
            var board = BattleboardUtils.GetBattleboard(attacker, snapshot);
            var quest = location.Quests.Find(s => s.Id == actor.QuestId)!;

            board.Quest = quest;

            if (!quest.IsRepeatable) location.Quests.Remove(quest);

            board.LastActionResult = quest.Description;

            return board;
        }
    }

    public Battleboard EndQuest(BattleboardActor actor)
    {
        lock (_lock)
        {
            var attacker = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            var location = ServicesUtils.GetSnapshotLocationByPosition(attacker.Status.Position, snapshot);
            var board = BattleboardUtils.GetBattleboard(attacker, snapshot);
            var quest = board.Quest;

            GenerateReward(board.GoodGuys.Find(s => s.Identity.Id == board.GoodGuyPartyLeadId)!, quest.Reward, quest.EffortLvl);

            foreach (var character in board.GetAllCharacters())
            {
                var fame = $"{quest.Fame} of {location.Name}";

                if (!character.Status.Fame.Contains(fame))
                {
                    character.Status.Fame += string.IsNullOrWhiteSpace(character.Status.Fame) 
                        ? $"{fame}."
                        : $"\n{fame}.";
                }

                if (character.Status.Gameplay.IsNpc) ReturnMercHome(character, location);
            }

            board.Quest = new();
            board.LastActionResult = quest.Result;

            return board;
        }
    }

    

    public Battleboard StopQuest(BattleboardActor actor)
    {
        lock (_lock)
        {
            var attacker = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            var location = ServicesUtils.GetSnapshotLocationByPosition(attacker.Status.Position, snapshot);
            var board = BattleboardUtils.GetBattleboard(attacker, snapshot);
            var quest = board.Quest;

            if (!quest.IsRepeatable) 
            {
                quest.EncountersLeft = quest.Encounters;
            }

            location.Quests.Add(quest);

            board.Quest = new Quest();

            foreach (var character in board.GetAllCharacters())
            {
                character.Inventory.Provisions = (int)(character.Inventory.Provisions * 0.5);

                if (character.Status.Gameplay.IsNpc) ReturnMercHome(character, location);

                character.Mercenaries.Clear();
            }

            return board;
        }
    }

    #region private methods
    private void ReturnMercHome(Character mercenary, Location location)
    {
        mercenary.Identity.PlayerId = Guid.Empty.ToString();
        mercenary.Status.Worth = npcLogic.CalculateNpcWorth(mercenary, location.Effort);
        location.Mercenaries.Add(mercenary);
    }

    private void GenerateReward(Character character, string rewardType, int effortLevel)
    {
        if (rewardType == GameplayLore.QuestReward.Wealth)
        {
            character.Status.Wealth += 2 * diceLogic.Roll_1_to_n(effortLevel);
        }
        else if (rewardType == GameplayLore.QuestReward.Item)
        {
            var item = itemsLogic.GenerateRandomItem();
            character.Inventory.Supplies.Add(item);
        }
        else if (rewardType == GameplayLore.QuestReward.Loot)
        {
            for (var i = 0; i < diceLogic.Roll_d20_withReroll(); i++)
            {
                var item = itemsLogic.GenerateRandomItem();
                character.Inventory.Supplies.Add(item);
            }
        }
        else if (rewardType == GameplayLore.QuestReward.Stats)
        {
            character.LevelUp.StatPoints += 2 * diceLogic.Roll_1_to_n(effortLevel);
        }
        else if (rewardType == GameplayLore.QuestReward.Skills)
        {
            character.LevelUp.SkillPoints += 2 * diceLogic.Roll_1_to_n(effortLevel);
        }
        else // for deeds points to aquire special skills
        {
            character.LevelUp.DeedPoints += 2 * diceLogic.Roll_1_to_n(effortLevel);
        }

        throw new NotImplementedException();
    }
    #endregion
}
