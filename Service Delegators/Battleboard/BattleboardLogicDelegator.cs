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
    Battleboard RemoveFromBattleFormation(BattleboardCharacter battleboardCharacter);
}

public class BattleboardLogicDelegator : IBattleboardLogicDelegator
{
    private readonly IValidations validations;
    private readonly IBattleboardCRUDLogic crudLogic;
    private readonly IBattleboardBattleFormationLogic formationLogic;

    public BattleboardLogicDelegator(
        IValidations validations,
        IBattleboardCRUDLogic crudLogic,
        IBattleboardBattleFormationLogic formationLogic)
    {
        this.crudLogic = crudLogic;
        this.validations = validations;
        this.formationLogic = formationLogic;
    }

    public Battleboard GetBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardGet(battleboardCharacter);
        return crudLogic.GetBattleboard(battleboardCharacter);
    }

    public Battleboard CreateBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardCreate(battleboardCharacter);
        return crudLogic.CreateBattleboard(battleboardCharacter);
    }

    public Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardJoin(battleboardCharacter);
        return crudLogic.JoinBattleboard(battleboardCharacter);
    }

    public Battleboard KickFromBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardKick(battleboardCharacter);
        return crudLogic.KickFromBattleboard(battleboardCharacter);
    }

    public void LeaveBattleboard(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBeforeBattleboardLeave(battleboardCharacter);
        crudLogic.LeaveBattleboard(battleboardCharacter);
    }

    public Battleboard MoveToBattleFormation(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBattleFormationOnMoveTo(battleboardCharacter);
        return formationLogic.MoveToBattleFormation(battleboardCharacter);
    }

    public Battleboard SwapInBattleFormation(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBattleFormationOnSwap(battleboardCharacter);
        return formationLogic.SwapInBattleFormation(battleboardCharacter);
    }

    public Battleboard RemoveFromBattleFormation(BattleboardCharacter battleboardCharacter)
    {
        validations.ValidateBattleFormationOnRemoveFrom(battleboardCharacter);
        return formationLogic.RemoveFromBattleFormation(battleboardCharacter);
    }
}
