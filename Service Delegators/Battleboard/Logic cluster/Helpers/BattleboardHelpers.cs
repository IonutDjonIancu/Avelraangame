using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class BattleboardHelpers
{
    public static Character GetCharacter(BattleboardActor actor, Snapshot snapshot)
    {
        return snapshot.Players.Find(s => s.Identity.Id == actor.MainActor.PlayerId)!.Characters.Find(s => s.Identity.Id == actor.MainActor.Id)!;
    }

    public static Battleboard GetBattleboard(string battleboardId, Snapshot snapshot)
    {
        return snapshot.Battleboards.Find(s => s.Id == battleboardId)!;
    }

}
