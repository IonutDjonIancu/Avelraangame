using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardLogicDelegator
{
    Battleboard CreateBattleboard(CharacterIdentity partyLeadIdentity);
    Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter, bool isGood);
    void LeaveBattleboard(BattleboardCharacter battleboardCharacter);

    Battleboard MoveToBattleFormation(BattleboardCharacter battleboardCharacter);
    Battleboard SwapInBattleFormation(BattleboardCharacter battleboardCharacter, CharacterIdentity characterIdentityToEnter);
    Battleboard LeaveBattleFormation(BattleboardCharacter battleboardCharacter);
}

public class BattleboardLogicDelegator : IBattleboardLogicDelegator
{
    private readonly IValidations validations;
    private readonly IBattleboardCreateLogic createLogic;

    public BattleboardLogicDelegator(
        IValidations validations,
        IBattleboardCreateLogic createLogic)
    {
        this.createLogic = createLogic;
        this.validations = validations;
    }

    public Battleboard CreateBattleboard(CharacterIdentity partyLeadIdentity)
    {
        validations.ValidateBeforeBattleboardCreate(partyLeadIdentity);
        return createLogic.CreateBattleboard(partyLeadIdentity);
    }

    public Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter, bool isGood)
    {
        validations.ValidateBeforeBattleboardJoin(battleboardCharacter);
        return createLogic.JoinBattleboard(battleboardCharacter, isGood);
    }

    public void LeaveBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardLeave(battleboardCharacter);
        createLogic.LeaveBattleboard(battleboardCharacter);
    }

}
