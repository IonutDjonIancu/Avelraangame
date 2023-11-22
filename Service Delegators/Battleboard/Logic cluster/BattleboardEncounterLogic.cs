using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Service_Delegators;

public interface IBattleboardEncounterLogic
{
    Battleboard NextEncounter(BattleboardActor actor);
}

public class BattleboardEncounterLogic : IBattleboardEncounterLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;
    private readonly IDiceLogicDelegator diceLogic;
    private readonly IItemsLogicDelegator itemsLogic;
    private readonly INpcGameplayLogic npcGameplayLogic;

    public BattleboardEncounterLogic(
        Snapshot snapshot,
        IDiceLogicDelegator diceLogic,
        IItemsLogicDelegator itemsLogic,
        INpcGameplayLogic npcGameplayLogic)
    {
        this.snapshot = snapshot;
        this.npcGameplayLogic = npcGameplayLogic;
        this.diceLogic = diceLogic;
        this.itemsLogic = itemsLogic;
    }

    public Battleboard NextEncounter(BattleboardActor actor)
    {
        lock (_lock)
        {
            var attacker = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            var board = BattleboardUtils.GetBattleboard(attacker, snapshot);

            board.Quest.EncountersLeft--;

            var encounterType = GameplayLore.EncounterType.All[diceLogic.Roll_1_to_n(GameplayLore.EncounterType.All.Count) - 1];

            return encounterType switch
            {
                GameplayLore.EncounterType.SaveVs => RunSaveVs(board),
                _ => throw new NotImplementedException(),
            };
        }


    }

    #region private methods
    private Battleboard RunSaveVs(Battleboard board)
    {
        var statToRoll = CharactersLore.Stats.All[diceLogic.Roll_1_to_n(CharactersLore.Stats.All.Count) - 1]!;
        var character = board.GoodGuys[diceLogic.Roll_1_to_n(board.GoodGuys.Count) - 1]!;

        var roll = statToRoll switch
        {
            CharactersLore.Stats.Strength => diceLogic.Roll_game_dice(true, CharactersLore.Stats.Strength, character),
            _ => throw new NotImplementedException(),
        };

        if (roll >= board.Quest.EffortLvl)
        {
            
        }



        return board;
    }

    #endregion
}
