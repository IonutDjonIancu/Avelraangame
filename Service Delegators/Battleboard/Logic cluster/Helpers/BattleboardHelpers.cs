using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class BattleboardHelpers
{
    public static Character GetCharacter(BattleboardActor actor, Snapshot snapshot)
    {
        var player = snapshot.Players.Find(s => s.Identity.Id == actor.MainActor.PlayerId)!;

        var character = player.Characters.Find(s => s.Identity.Id == actor.MainActor.Id);
        if (character != null) return character;

        foreach (var chara in player.Characters)
        {
            character = chara.Mercenaries.Find(s => s.Identity.Id == actor.MainActor.Id);
            if (character != null) return character;
        }

        return null;
    }

    public static Battleboard GetBattleboard(string battleboardId, Snapshot snapshot)
    {
        return snapshot.Battleboards.Find(s => s.Id == battleboardId)!;
    }

}
