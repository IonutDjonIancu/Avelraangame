using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardQuestLogic
{
    Battleboard StartQuest(BattleboardActor actor);
    Battleboard StopQuest(BattleboardActor actor);
}

public class BattleboardQuestLogic : IBattleboardQuestLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;
    private readonly INpcGameplayLogic npcGameplayLogic;

    public BattleboardQuestLogic(
        Snapshot snapshot, 
        INpcGameplayLogic npcGameplayLogic)
    {
        this.snapshot = snapshot;
        this.npcGameplayLogic = npcGameplayLogic;
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

                if (character.Status.Gameplay.IsNpc)
                {
                    character.Status.Worth = npcGameplayLogic.CalculateNpcWorth(character, location.Effort);
                    location.Mercenaries.Add(character);
                }

                character.Mercenaries.Clear();
            }

            return board;
        }
    }


}
