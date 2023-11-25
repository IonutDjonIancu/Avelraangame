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
                GameplayLore.EncounterType.SaveVsStats  => RunSaveVsStatsLogic(board),
                GameplayLore.EncounterType.Combat       => RunCombatLogic(board),
                GameplayLore.EncounterType.Curse        => RunCurseLogic(board),
                GameplayLore.EncounterType.Disease      => RunDiseaseLogic(board),
                GameplayLore.EncounterType.LoseWealth   => RunLoseWealthLogic(board),
                GameplayLore.EncounterType.Boon         => RunBoonLogic(board),
                GameplayLore.EncounterType.ItemFind     => RunItemFindLogic(board),
                GameplayLore.EncounterType.Storyline    => RunStorylineLogic(board),
                GameplayLore.EncounterType.GainWealth   => RunGainWealthLogic(board),
                _ => throw new NotImplementedException(),
            };
        }
    }

    #region private methods
    private Battleboard RunSaveVsStatsLogic(Battleboard board)
    {
        var statToRoll = CharactersLore.Stats.All[diceLogic.Roll_1_to_n(CharactersLore.Stats.All.Count) - 1]!;
        var character = board.GoodGuys[diceLogic.Roll_1_to_n(board.GoodGuys.Count) - 1]!;

        var roll = statToRoll switch
        {
            CharactersLore.Stats.Strength => diceLogic.Roll_game_dice(true, CharactersLore.Stats.Strength, character),
            CharactersLore.Stats.Constitution => diceLogic.Roll_game_dice(true, CharactersLore.Stats.Constitution, character),
            CharactersLore.Stats.Agility => diceLogic.Roll_game_dice(true, CharactersLore.Stats.Agility, character),
            CharactersLore.Stats.Willpower => diceLogic.Roll_game_dice(true, CharactersLore.Stats.Willpower, character),
            CharactersLore.Stats.Perception => diceLogic.Roll_game_dice(true, CharactersLore.Stats.Perception, character),
            CharactersLore.Stats.Abstract => diceLogic.Roll_game_dice(true, CharactersLore.Stats.Abstract, character),
            _ => throw new NotImplementedException(),
        };

        if (roll < board.Quest.EffortLvl)
        {
            var resultNumber = board.Quest.EffortLvl - roll;

            character.Sheet.Assets.ResolveLeft -= resultNumber * 50;
            character.Status.Gameplay.IsAlive = character.Sheet.Assets.ResolveLeft > 0;

            var resultText = GameplayLore.EncounterTypeResults.SaveVs.All[diceLogic.Roll_1_to_n(GameplayLore.EncounterTypeResults.SaveVs.All.Count) - 1];
            board.LastActionResult = 
                resultText 
                + $" << {character.Status.Name} takes {resultNumber}% dmg. >>" 
                + (character.Status.Gameplay.IsAlive ? $" {character.Status.Name} survives this." : $" {character.Status.Name} is no longer alive.");
        }
        else
        {
            board.LastActionResult = $"{character.Status.Name} saved vs successfully.";
        }

        return board;
    }

    private Battleboard RunCombatLogic(Battleboard board)
    {
        throw new NotImplementedException();
    }

    private Battleboard RunCurseLogic(Battleboard board)
    {
        throw new NotImplementedException();
    }

    private Battleboard RunDiseaseLogic(Battleboard board)
    {
        throw new NotImplementedException();
    }

    private Battleboard RunLoseWealthLogic(Battleboard board)
    {
        throw new NotImplementedException();
    }

    private Battleboard RunBoonLogic(Battleboard board)
    {
        throw new NotImplementedException();
    }

    private Battleboard RunItemFindLogic(Battleboard board)
    {
        throw new NotImplementedException();
    }

    private Battleboard RunStorylineLogic(Battleboard board)
    {
        throw new NotImplementedException();
    }

    private Battleboard RunGainWealthLogic(Battleboard board)
    {
        throw new NotImplementedException();
    }

    #endregion
}
