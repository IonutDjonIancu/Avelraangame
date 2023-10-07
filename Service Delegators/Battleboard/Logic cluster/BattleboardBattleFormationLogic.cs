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

            if (character.Status.Gameplay.IsGoodGuy)
            {
                battleboard.GoodGuys.BattleFormation.Add(battleboardCharacter.TargetId);
            }
            else
            {
                battleboard.BadGuys.BattleFormation.Add(battleboardCharacter.TargetId);
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

            if (character.Status.Gameplay.IsGoodGuy)
            {
                battleboard.GoodGuys.BattleFormation.Remove(battleboardCharacter.TargetId);
                battleboard.GoodGuys.BattleFormation.Add(battleboardCharacter.SecondTargetId);
                targettedCharacter = battleboard.GoodGuys.Characters.Find(s => s.Identity.Id == battleboardCharacter.TargetId)!;
            }
            else
            {
                battleboard.BadGuys.BattleFormation.Remove(battleboardCharacter.TargetId);
                battleboard.BadGuys.BattleFormation.Add(battleboardCharacter.SecondTargetId);
                targettedCharacter = battleboard.BadGuys.Characters.Find(s => s.Identity.Id == battleboardCharacter.TargetId)!;
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

            if (character.Status.Gameplay.IsGoodGuy)
            {
                battleboard.GoodGuys.BattleFormation.Remove(battleboardCharacter.TargetId);
            }
            else
            {
                battleboard.BadGuys.BattleFormation.Remove(battleboardCharacter.TargetId);
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
