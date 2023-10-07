using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

public interface IBattleboardCombatLogic
{
    Combat CreateCombat(string battleboardId);
    Combat Attack(tbd);
    Combat Cast(tbd);
    Combat Defend(tbd);
    Combat Mend(tbd);
    Combat Hide(tbd);
    Combat Traps(tbd);
    Combat Pass(tbd);
    Combat LetAiAct(tbd);
    void DeleteCombat(Battleboard battleboard);
}

public class BattleboardCombatLogic : IBattleboardCombatLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;
    private readonly IPersistenceService persistence;

    public BattleboardCombatLogic(
        Snapshot snapshot,
        IPersistenceService persistence)
    {
        this.snapshot = snapshot;
    }

    public Combat CreateCombat(string battleboardId)
    {
        lock (_lock)
        {
            var board = snapshot.Battleboards.Find(s => s.Id == battleboardId)!;
            board.IsInCombat = true;

            var listOfCombatants = new List<Character>();
            board.GoodGuys.Characters.ForEach(s =>
            {
                s.Status.Gameplay.IsInCombat = true;

                if (board.GoodGuys.BattleFormation.Contains(s.Identity.Id))
                {
                    listOfCombatants.Add(s);
                }

            });
            board.BadGuys.Characters.ForEach(s =>
            {
                s.Status.Gameplay.IsInCombat = true;

                if (board.BadGuys.BattleFormation.Contains(s.Identity.Id))
                {
                    listOfCombatants.Add(s);
                }
            });










        }
    }



}
