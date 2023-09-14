using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardBattleFormationLogic
{
    Battleboard MoveToBattleFormation(BattleboardCharacter battleboardCharacter);
    Battleboard SwapInBattleFormation(BattleboardCharacter battleboardCharacter, CharacterIdentity characterIdentityToEnter);
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
        throw new NotImplementedException();
    }

    public Battleboard SwapInBattleFormation(BattleboardCharacter battleboardCharacter, CharacterIdentity characterIdentityToEnter)
    {
        throw new NotImplementedException();
    }

    public Battleboard LeaveBattleFormation(BattleboardCharacter battleboardCharacter)
    {
        throw new NotImplementedException();
    }
}
