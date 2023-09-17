using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardBattleFormationLogic
{
    Battleboard MoveToBattleFormation(BattleboardCharacter battleboardCharacter);
    Battleboard SwapInBattleFormation(BattleboardCharacter battleboardCharacter);
    Battleboard LeaveBattleFormation(BattleboardCharacter battleboardCharacter);
}

public class BattleboardBattleFormationLogic : IBattleboardBattleFormationLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;

    public BattleboardBattleFormationLogic(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public Battleboard MoveToBattleFormation(BattleboardCharacter battleboardCharacter)
    {
        lock (_lock)
        {
            var (character, battleboard) = GetCharacterAndBattleboard(battleboardCharacter);

            if (character.Status.Gameplay.IsBattleboardGoodGuy)
            {
                battleboard.GoodGuys.BattleFormation.Add(battleboardCharacter.TargettedCharacterId);
            }
            else
            {
                battleboard.BadGuys.BattleFormation.Add(battleboardCharacter.TargettedCharacterId);
            }

            return battleboard;
        }
    }

    public Battleboard SwapInBattleFormation(BattleboardCharacter battleboardCharacter, CharacterIdentity characterIdentityToEnter)
    {
        throw new NotImplementedException();
    }

    public Battleboard LeaveBattleFormation(BattleboardCharacter battleboardCharacter)
    {
        throw new NotImplementedException();
    }

    #region private methods
    private (Character character, Battleboard battleboard) GetCharacterAndBattleboard(BattleboardCharacter battleboardCharacter)
    {
        var character = snapshot.Players.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.Id)!;
        var battleboard = snapshot.Battleboards.Find(s => s.Id == character.Status.Gameplay.BattleboardId)!;

        return (character, battleboard);
    }

    #endregion
}
