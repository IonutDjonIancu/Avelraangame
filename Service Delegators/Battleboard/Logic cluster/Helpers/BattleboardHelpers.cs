using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class BattleboardHelpers
{
    public static Character GetCharacter(BattleboardCharacter battleboardCharacter, Snapshot snapshot)
    {
        return snapshot.Players.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.Id)!;
    }

}
