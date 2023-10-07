using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardLogicDelegator
{
    List<Battleboard> GetBattleboards();
    Battleboard FindBattleboard(string battleboardId);
    Battleboard FindCharacterBattleboard(BattleboardCharacter battleboardCharacter);

    Battleboard CreateBattleboard(BattleboardCharacter battleboardCharacter);
    Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter);
    Battleboard KickFromBattleboard(BattleboardCharacter battleboardCharacter);
    void LeaveBattleboard(BattleboardCharacter battleboardCharacter);

    Battleboard MakeCamp(BattleboardCharacter battleboardCharacter);

    Battleboard StartCombat(string battleboardId);
    Battleboard EndCombat(Battleboard battleboard);
    Battleboard Attack(tbd);
    Battleboard Cast(tbd);
    Battleboard Defend(tbd);
    Battleboard Mend(tbd);
    Battleboard Hide(tbd);
    Battleboard Traps(tbd);
    Battleboard Pass(tbd);
    Battleboard LetAiAct(tbd);
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

    public List<Battleboard> GetBattleboards()
    {
        return crudLogic.GetBattleboards();
    }

    public Battleboard FindBattleboard(string battleboardId)
    {
        validations.ValidateBeforeBattleboardFind(battleboardId);
        return crudLogic.FindBattleboard(battleboardId);
    }

    public Battleboard FindCharacterBattleboard(BattleboardCharacter battleboardCharacter)
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

    public Combat StartCombat(string battleboardId)
    {
        validations.ValidateBattleboardBeforeCombatCreate(battleboardId);


    }
}
