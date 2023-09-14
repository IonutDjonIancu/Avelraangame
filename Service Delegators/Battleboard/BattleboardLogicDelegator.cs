using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardLogicDelegator
{
    Battleboard GetBattleboard(BattleboardCharacter battleboardCharacter);

    Battleboard CreateBattleboard(BattleboardCharacter battleboardCharacter);
    Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter);
    Battleboard KickFromBattleboard(BattleboardCharacter battleboardCharacter);
    void LeaveBattleboard(BattleboardCharacter battleboardCharacter);

    Battleboard MoveToBattleFormation(BattleboardCharacter battleboardCharacter);
    Battleboard SwapInBattleFormation(BattleboardCharacter battleboardCharacter);
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

    public Battleboard GetBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardGet(charIdentity);
        return createLogic.GetBattleboard(charIdentity);
    }

    public Battleboard CreateBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardCreate(partyLeadIdentity);
        return createLogic.CreateBattleboard(partyLeadIdentity);
    }

    public Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardJoin(battleboardCharacter);
        return createLogic.JoinBattleboard(battleboardCharacter, isGood);
    }

    public Battleboard KickFromBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardKick(charIdentity, characterIdKicked);
    }

    public void LeaveBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardLeave(battleboardCharacter);
        createLogic.LeaveBattleboard(battleboardCharacter);
    }

    public Battleboard MoveToBattleFormation(BattleboardCharacter battleboardCharacter)
    {


        throw new NotImplementedException();
    }

    public Battleboard SwapInBattleFormation(BattleboardCharacter battleboardCharacter)
    {
        throw new NotImplementedException();
    }

    public Battleboard LeaveBattleFormation(BattleboardCharacter battleboardCharacter)
    {
        throw new NotImplementedException();
    }
}
