using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardQuestLogic
{
    Battleboard StartQuest(BattleboardActor actor);
}

public class BattleboardQuestLogic : IBattleboardQuestLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;
    private readonly IDiceLogicDelegator dice;

    public BattleboardQuestLogic(
        Snapshot snapshot,
        IDiceLogicDelegator dice)
    {
        this.snapshot = snapshot;
        this.dice = dice;
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


}
