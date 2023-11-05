using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class BattleboardUtils
{
    public static Battleboard GetBattleboard(Character attacker, Snapshot snapshot)
    {
        return snapshot.Battleboards.Find(s => s.Id == attacker.Status.Gameplay.BattleboardId) ?? throw new Exception("Battleboard not found");
    }

    public static (Character attacker, Battleboard board) GetAttackerBoard(BattleboardActor actor, Snapshot snapshot)
    {
        var attacker = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot)!;
        var battleboard = GetBattleboard(attacker, snapshot);

        return (attacker, battleboard);
    }

    public static (Character attacker, Battleboard board, Character defender) GetAttackerBoardDefender(BattleboardActor actor, Snapshot snapshot)
    {
        var (attacker, board) = GetAttackerBoard(actor, snapshot);
        var defender = board.GetAllCharacters().Find(s => s.Identity.Id == actor.TargetId) ?? throw new Exception("No target found on this battleboard.");

        return (attacker, board, defender);
    }
}
