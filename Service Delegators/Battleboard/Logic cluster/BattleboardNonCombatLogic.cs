using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardNonCombatLogic
{
    Battleboard MakeCamp(BattleboardActor actor);
}

public class BattleboardNonCombatLogic : IBattleboardNonCombatLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;
    private readonly IDiceLogicDelegator dice;

    public BattleboardNonCombatLogic(
       Snapshot snapshot,
       IDiceLogicDelegator dice)
    {
        this.snapshot = snapshot;
        this.dice = dice;
    }

    public Battleboard MakeCamp(BattleboardActor actor)
    {
        lock (_lock)
        {
            var (attacker, board) = GetAttackerBoard(actor, snapshot);

            foreach (var member in board.GetAllCharacters())
            {
                member.Status.Gameplay.IsLocked = false;
                member.Status.Gameplay.IsHidden = false;

                member.Sheet.Assets.ResolveLeft = member.Sheet.Assets.Resolve;
                member.Sheet.Assets.ManaLeft = member.Sheet.Assets.Mana;
                member.Sheet.Assets.ActionsLeft = member.Sheet.Assets.Actions;
            }

            var index = dice.Roll_1_to_n(GameplayLore.Camping.All.Count) - 1;
            board.LastActionResult = GameplayLore.Camping.All[index];

            return board;
        }
    }

    #region private methods
    private (Character attacker, Battleboard board) GetAttackerBoard(BattleboardActor actor, Snapshot snapshot)
    {
        var attacker = BattleboardHelpers.GetCharacter(actor, snapshot);
        var board = BattleboardHelpers.GetBattleboard(attacker.Status.Gameplay.BattleboardId, snapshot);

        return (attacker, board);
    }
    #endregion
}
