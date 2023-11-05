using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardLogicDelegator
{
    List<Battleboard> GetBattleboards();
    Battleboard FindBattleboard(string battleboardId);
    Battleboard FindCharacterBattleboard(BattleboardActor actor);

    Battleboard CreateBattleboard(BattleboardActor actor);
    Battleboard JoinBattleboard(BattleboardActor actor);
    Battleboard KickFromBattleboard(BattleboardActor actor);
    void LeaveBattleboard(BattleboardActor actor);

    Battleboard StartCombat(BattleboardActor actor);
    Battleboard Attack(BattleboardActor actor);
    Battleboard Cast(BattleboardActor actor);
    Battleboard Mend(BattleboardActor actor);
    Battleboard Hide(BattleboardActor actor);
    Battleboard Traps(BattleboardActor actor);
    Battleboard Rest(BattleboardActor actor);
    Battleboard LetAiAct(BattleboardActor actor);
    Battleboard EndRound(BattleboardActor actor);
    Battleboard EndCombat(BattleboardActor actor);
    
    Battleboard MakeCamp(BattleboardActor actor);
}

public class BattleboardLogicDelegator : IBattleboardLogicDelegator
{
    private readonly IValidations validations;
    private readonly IBattleboardCRUDLogic crudLogic;
    private readonly IBattleboardCombatLogic combatLogic;
    private readonly IBattleboardNonCombatLogic nonCombatLogic;

    public BattleboardLogicDelegator(
        IValidations validations,
        IBattleboardCRUDLogic crudLogic,
        IBattleboardCombatLogic combatLogic,
        IBattleboardNonCombatLogic nonCombatLogic)
    {
        this.crudLogic = crudLogic;
        this.validations = validations;
        this.combatLogic = combatLogic;
        this.nonCombatLogic = nonCombatLogic;
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

    public Battleboard FindCharacterBattleboard(BattleboardActor actor)
    {
        validations.ValidateBeforeBattleboardGet(actor);
        return crudLogic.GetBattleboard(actor);
    }

    public Battleboard CreateBattleboard(BattleboardActor actor)
    {
        validations.ValidateBeforeBattleboardCreate(actor);
        return crudLogic.CreateBattleboard(actor);
    }

    public Battleboard JoinBattleboard(BattleboardActor actor)
    {
        validations.ValidateBeforeBattleboardJoin(actor);
        return crudLogic.JoinBattleboard(actor);
    }

    public Battleboard KickFromBattleboard(BattleboardActor actor)
    {
        validations.ValidateBeforeBattleboardKick(actor);
        return crudLogic.RemoveFromBattleboard(actor);
    }

    public void LeaveBattleboard(BattleboardActor actor)
    {
        validations.ValidateBeforeBattleboardLeave(actor);
        crudLogic.RemoveFromBattleboard(actor);
    }

    public Battleboard StartCombat(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnCombatStart(actor);
        return combatLogic.StartCombat(actor);
    }

    public Battleboard Attack(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnAttack(actor);
        return combatLogic.Attack(actor);
    }

    public Battleboard Cast(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnCast(actor);
        return combatLogic.Cast(actor);
    }

    public Battleboard Mend(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnMend(actor);
        return combatLogic.Mend(actor);
    }

    public Battleboard Hide(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnHide(actor);
        return combatLogic.Hide(actor);
    }

    public Battleboard Traps(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnTraps(actor);
        return combatLogic.Traps(actor);
    }

    public Battleboard Rest(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnRest(actor);
        return combatLogic.Rest(actor);
    }

    public Battleboard LetAiAct(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnLetAiAct(actor);
        return combatLogic.LetAiAct(actor);
    }

    public Battleboard EndRound(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnEndRound(actor);
        return combatLogic.EndRound(actor);
    }

    public Battleboard EndCombat(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnEndCombat(actor);
        return combatLogic.EndCombat(actor);
    }

    public Battleboard MakeCamp(BattleboardActor actor)
    {
        validations.ValidateBattleboardOnMakeCamp(actor);
        return nonCombatLogic.MakeCamp(actor);
    }
}
