using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardBattleFormationLogic
{
    Battleboard MoveToBattleFormation(BattleboardCharacter battleboardCharacter);
    Battleboard SwapInBattleFormation(BattleboardCharacter battleboardCharacter);
    Battleboard RemoveFromBattleFormation(BattleboardCharacter battleboardCharacter);
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
                battleboard.GoodGuys.BattleFormation.Add(battleboardCharacter.FirstTargetId);
            }
            else
            {
                battleboard.BadGuys.BattleFormation.Add(battleboardCharacter.FirstTargetId);
            }

            return battleboard;
        }
    }

    public Battleboard SwapInBattleFormation(BattleboardCharacter battleboardCharacter)
    {
        lock (_lock)
        {
            var (character, battleboard) = GetCharacterAndBattleboard(battleboardCharacter);
            Character targettedCharacter;

            if (character.Status.Gameplay.IsBattleboardGoodGuy)
            {
                battleboard.GoodGuys.BattleFormation.Remove(battleboardCharacter.FirstTargetId);
                battleboard.GoodGuys.BattleFormation.Add(battleboardCharacter.SecondTargetId);
                targettedCharacter = battleboard.GoodGuys.Characters.Find(s => s.Identity.Id == battleboardCharacter.FirstTargetId)!;
            }
            else
            {
                battleboard.BadGuys.BattleFormation.Remove(battleboardCharacter.FirstTargetId);
                battleboard.BadGuys.BattleFormation.Add(battleboardCharacter.SecondTargetId);
                targettedCharacter = battleboard.BadGuys.Characters.Find(s => s.Identity.Id == battleboardCharacter.FirstTargetId)!;
            }

            targettedCharacter.Sheet.Assets.ActionsLeft -= 1;
            return battleboard;
        }
    }

    public Battleboard RemoveFromBattleFormation(BattleboardCharacter battleboardCharacter)
    {
        lock (_lock)
        {
            var (character, battleboard) = GetCharacterAndBattleboard(battleboardCharacter);

            if (character.Status.Gameplay.IsBattleboardGoodGuy)
            {
                battleboard.GoodGuys.BattleFormation.Remove(battleboardCharacter.FirstTargetId);
            }
            else
            {
                battleboard.BadGuys.BattleFormation.Remove(battleboardCharacter.FirstTargetId);
            }

            return battleboard;
        }
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
