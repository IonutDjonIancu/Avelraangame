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
    private readonly IBattleboardCRUDLogic crudLogic;

    public BattleboardLogicDelegator(
        IValidations validations,
        IBattleboardCRUDLogic crudLogic)
    {
        this.crudLogic = crudLogic;
        this.validations = validations;
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
