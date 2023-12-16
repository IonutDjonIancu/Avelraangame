using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;
using static Data_Mapping_Containers.Lore.GameplayLore;

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
    private readonly INpcLogicDelegator npcLogic;

    public BattleboardEncounterLogic(
        Snapshot snapshot,
        IDiceLogicDelegator diceLogic,
        IItemsLogicDelegator itemsLogic,
        INpcLogicDelegator npcLogic)
    {
        this.snapshot = snapshot;
        this.npcLogic = npcLogic;
        this.diceLogic = diceLogic;
        this.itemsLogic = itemsLogic;
    }

    public Battleboard NextEncounter(BattleboardActor actor)
    {
        lock (_lock)
        {
            var attacker = ServicesUtils.GetPlayerCharacter(actor.MainActor, snapshot);
            var board = BattleboardUtils.GetBattleboard(attacker, snapshot);

            return board.Quest.EncountersLeft > 0 
                ? GenerateNextEncounter(attacker, board)
                : GenerateLastEncounter(attacker, board);
        }
    }

    #region private methods
    private Battleboard GenerateLastEncounter(Character attacker, Battleboard board)
    {

    }

    private Battleboard GenerateNextEncounter(Character attacker, Battleboard board)
    {
        board.Quest.EncountersLeft--;

        var encounterType = EncounterType.All[diceLogic.Roll_1_to_n(EncounterType.All.Count) - 1];

        return encounterType switch
        {
            // bad
            EncounterType.SaveVsStats => RunSaveVsStatsLogic(board),
            EncounterType.Combat => RunCombatLogic(board),
            EncounterType.Curse => RunCurseLogic(board),
            EncounterType.Disease => RunDiseaseLogic(board),
            EncounterType.LoseWealth => RunLoseWealthLogic(board),

            // good
            EncounterType.Boon => RunBoonLogic(board),
            EncounterType.ItemFind => RunItemFindLogic(board),
            EncounterType.Storyline => RunStorylineLogic(board),
            EncounterType.GainWealth => RunGainWealthLogic(board),
            EncounterType.Experience => RunExperienceLogic(board),
            _ => throw new NotImplementedException(),
        };
    }

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

            var resultText = EncounterTypeResults.SaveVs.All[diceLogic.Roll_1_to_n(EncounterTypeResults.SaveVs.All.Count) - 1];
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
        var position = board.GoodGuys.First().Status.Position;
        var locationName = ServicesUtils.GetLocationByPositionFullName(position.GetPositionFullName()).Name;

        var nrOfMobs = RollForNrOfMobs();
        for (int i = 0; i < nrOfMobs; i++)
        {
            var mob = npcLogic.GenerateBadGuy(locationName);

            board.BadGuys.Add(mob);
        }
        
        board.IsInCombat = true;
        board.GetAllCharacters().ForEach(s => s.Status.Gameplay.IsLocked = true);

        int RollForNrOfMobs() 
        { 
            var rollForMobsLevel = diceLogic.Roll_d20_withReroll();
            int nrOfMobs;
            string extraMessage = string.Empty;

            if      (rollForMobsLevel >= 100)                          { nrOfMobs = diceLogic.Roll_1_to_n(2000) + 300; extraMessage = EncounterTexts.CombatEncounters.Planar; }
            else if (rollForMobsLevel >= 80 && rollForMobsLevel < 100) { nrOfMobs = diceLogic.Roll_1_to_n(1000) + 100; extraMessage = EncounterTexts.CombatEncounters.Olympian; }
            else if (rollForMobsLevel >= 60 && rollForMobsLevel < 80)  { nrOfMobs = diceLogic.Roll_1_to_n(500) + 50;  extraMessage = EncounterTexts.CombatEncounters.Hero; }
            else if (rollForMobsLevel >= 40 && rollForMobsLevel < 60)  { nrOfMobs = diceLogic.Roll_1_to_n(200) + 20; extraMessage = EncounterTexts.CombatEncounters.Chosen; }
            else if (rollForMobsLevel >= 20 && rollForMobsLevel < 40)  { nrOfMobs = diceLogic.Roll_1_to_n(100) + diceLogic.Roll_1_to_n(board.Quest.EffortLvl); extraMessage = EncounterTexts.CombatEncounters.Gifted; }
            else   /*rollForMobsLevel < 20*/                           { nrOfMobs = diceLogic.Roll_1_to_n(board.Quest.EffortLvl); extraMessage = EncounterTexts.CombatEncounters.Normal; }

            var resultText = EncounterTypeResults.Combat.All[diceLogic.Roll_1_to_n(EncounterTypeResults.Combat.All.Count) - 1];
            board.LastActionResult =$"{resultText} {extraMessage}";

            return nrOfMobs;
        }

        return board;
    }

    private Battleboard RunCurseLogic(Battleboard board)
    {
        var character = board.GoodGuys[diceLogic.Roll_1_to_n(board.GoodGuys.Count) - 1]!;
        var difficulty = ServicesUtils.GetDifficultyFromEffort(board.Quest.EffortLvl);
        var difficultyFactor = 0.1 * (int)difficulty;
        var resultText = EncounterTypeResults.Curse.All[diceLogic.Roll_1_to_n(EncounterTypeResults.Curse.All.Count) - 1];
        var extraMessage = string.Empty;

        CurseSkills(character);
        CurseStats(character);
        CurseAssets(character);
        CurseItem(character);
        CurseAllItems(character);
        CurseWealthWorthAndFame(character);

        void CurseSkills(Character character)
        {
            var skill = CharactersLore.Skills.All[diceLogic.Roll_1_to_n(CharactersLore.Skills.All.Count) - 1];

            switch (skill)
            {
                case CharactersLore.Skills.Melee: character.Sheet.Skills.Melee -= (int)(character.Sheet.Skills.Melee * difficultyFactor); break;
                case CharactersLore.Skills.Arcane: character.Sheet.Skills.Arcane -= (int)(character.Sheet.Skills.Arcane * difficultyFactor); break;
                case CharactersLore.Skills.Psionics: character.Sheet.Skills.Psionics -= (int)(character.Sheet.Skills.Psionics * difficultyFactor); break;
                case CharactersLore.Skills.Hide: character.Sheet.Skills.Hide -= (int)(character.Sheet.Skills.Hide * difficultyFactor); break;
                case CharactersLore.Skills.Traps: character.Sheet.Skills.Traps -= (int)(character.Sheet.Skills.Traps * difficultyFactor); break;
                case CharactersLore.Skills.Tactics: character.Sheet.Skills.Tactics -= (int)(character.Sheet.Skills.Tactics * difficultyFactor); break;
                case CharactersLore.Skills.Social: character.Sheet.Skills.Social -= (int)(character.Sheet.Skills.Social * difficultyFactor); break;
                case CharactersLore.Skills.Apothecary: character.Sheet.Skills.Apothecary -= (int)(character.Sheet.Skills.Apothecary * difficultyFactor); break;
                case CharactersLore.Skills.Travel: character.Sheet.Skills.Travel -= (int)(character.Sheet.Skills.Travel * difficultyFactor); break;
                case CharactersLore.Skills.Sail: character.Sheet.Skills.Sail -= (int)(character.Sheet.Skills.Sail * difficultyFactor); break;
                default: throw new NotImplementedException();
            }

            extraMessage += $"{EncounterTexts.CurseEncounters.Skills} {character.Status.Name}'s {skill} has been reduced by {(int)difficulty * 10}%. ";
            character.Status.Fame += $"\n{EncounterTexts.CurseEncounters.Skills}";
        }

        void CurseStats(Character character)
        {
            if (difficulty == Enums.Difficulty.Normal) return;

            var stat = CharactersLore.Stats.All[diceLogic.Roll_1_to_n(CharactersLore.Stats.All.Count) - 1];

            switch (stat)
            {
                case CharactersLore.Stats.Strength: character.Sheet.Stats.Strength -= (int)(character.Sheet.Stats.Strength * difficultyFactor); break;
                case CharactersLore.Stats.Constitution: character.Sheet.Stats.Constitution -= (int)(character.Sheet.Stats.Constitution * difficultyFactor); break;
                case CharactersLore.Stats.Agility: character.Sheet.Stats.Agility -= (int)(character.Sheet.Stats.Agility * difficultyFactor); break;
                case CharactersLore.Stats.Willpower: character.Sheet.Stats.Willpower -= (int)(character.Sheet.Stats.Willpower * difficultyFactor); break;
                case CharactersLore.Stats.Perception: character.Sheet.Stats.Perception -= (int)(character.Sheet.Stats.Perception * difficultyFactor); break;
                case CharactersLore.Stats.Abstract: character.Sheet.Stats.Abstract -= (int)(character.Sheet.Stats.Abstract * difficultyFactor); break;
                default: throw new NotImplementedException();
            }

            extraMessage += $"{EncounterTexts.CurseEncounters.Stats} {character.Status.Name}'s {stat} has been reduced by {(int)difficulty * 10}%. ";
            character.Status.Fame += $"\n{EncounterTexts.CurseEncounters.Stats}";
        }

        void CurseAssets(Character character)
        {
            if (difficulty == Enums.Difficulty.Normal
                || difficulty == Enums.Difficulty.Gifted) return;

            var asset = CharactersLore.Assets.All[diceLogic.Roll_1_to_n(CharactersLore.Assets.All.Count) - 1];

            switch (asset)
            {
                case CharactersLore.Assets.Resolve: character.Sheet.Assets.Resolve -= (int)(character.Sheet.Assets.Resolve * difficultyFactor); break;
                case CharactersLore.Assets.Harm: character.Sheet.Assets.Harm -= (int)(character.Sheet.Assets.Harm * difficultyFactor); break;
                case CharactersLore.Assets.Spot: character.Sheet.Assets.Spot -= (int)(character.Sheet.Assets.Spot * difficultyFactor); break;
                case CharactersLore.Assets.Defense: character.Sheet.Assets.Defense -= (int)(character.Sheet.Assets.Defense * difficultyFactor); break;
                case CharactersLore.Assets.Purge: character.Sheet.Assets.Purge -= (int)(character.Sheet.Assets.Purge * difficultyFactor); break;
                case CharactersLore.Assets.Mana: character.Sheet.Assets.Mana -= (int)(character.Sheet.Assets.Mana * difficultyFactor); break;
                case CharactersLore.Assets.Actions: character.Sheet.Assets.Actions -= (int)(character.Sheet.Assets.Actions * difficultyFactor); break;
                default: throw new NotImplementedException();
            }

            extraMessage += $"{EncounterTexts.CurseEncounters.Assets} {character.Status.Name}'s {asset} has been reduced by {(int)difficulty * 10}%. ";
            character.Status.Fame += $"\n{EncounterTexts.CurseEncounters.Assets}";
        }

        void CurseItem(Character character)
        {
            if (difficulty == Enums.Difficulty.Normal
                || difficulty == Enums.Difficulty.Gifted
                || difficulty == Enums.Difficulty.Chosen) return;

            var allItems = character.Inventory.GetAllEquipedItems();

            var item = allItems[diceLogic.Roll_1_to_n(allItems.Count) - 1];

            foreach (var property in item.Sheet.Skills.GetType().GetProperties())
            {
                var currentValue = (int)property.GetValue(item.Sheet.Skills)!;
                property.SetValue(item.Sheet.Skills, (int)(currentValue * (1 - difficultyFactor)));
            }

            extraMessage += $"{EncounterTexts.CurseEncounters.Item} {character.Status.Name}'s {item.Name} skills have been reduced by {(int)difficulty * 10}%. ";
            character.Status.Fame += $"\n{EncounterTexts.CurseEncounters.Item}";
        }

        void CurseAllItems(Character character)
        {
            if (difficulty == Enums.Difficulty.Normal
                || difficulty == Enums.Difficulty.Gifted
                || difficulty == Enums.Difficulty.Chosen
                || difficulty == Enums.Difficulty.Hero) return;

            for (int i = 0; i < diceLogic.Roll_1_to_n(6); i++)
            {
                CurseItem(character);
            }

            extraMessage += $"{EncounterTexts.CurseEncounters.Items} {character.Status.Name}'s items' skills have been reduced by {(int)difficulty * 10}%. ";
            character.Status.Fame += $"\n{EncounterTexts.CurseEncounters.Items}";
        }

        void CurseWealthWorthAndFame(Character character)
        {
            if (difficulty == Enums.Difficulty.Normal
                || difficulty == Enums.Difficulty.Gifted
                || difficulty == Enums.Difficulty.Chosen
                || difficulty == Enums.Difficulty.Hero
                || difficulty == Enums.Difficulty.Olympian) return;

            character.Status.Wealth = 0;
            character.Status.Worth = 0;
            character.Status.Fame = string.Empty;

            extraMessage += $"{EncounterTexts.CurseEncounters.WealthWorthAndFame} {character.Status.Name} has been forgotten by the folk.";
            character.Status.Fame += $"\n{EncounterTexts.CurseEncounters.WealthWorthAndFame}";
        }

        board.LastActionResult = $"{resultText} {extraMessage}";

        return board;
    }

    private Battleboard RunDiseaseLogic(Battleboard board)
    {
        var character = board.GoodGuys[diceLogic.Roll_1_to_n(board.GoodGuys.Count) - 1]!;

        foreach (var stat in character.Sheet.Stats.GetType().GetProperties())
        {
            var currentValue = (int)(stat.GetValue(character.Sheet.Stats)!);
            stat.SetValue(character.Sheet.Stats, (int)(currentValue * 0.9));
        }

        board.LastActionResult = $"{character.Status.Name} says '{EncounterTypeResults.Disease.All[diceLogic.Roll_1_to_n(EncounterTypeResults.Disease.All.Count) - 1]}'.";

        return board;
    }

    private Battleboard RunLoseWealthLogic(Battleboard board)
    {
        var character = board.GoodGuys[diceLogic.Roll_1_to_n(board.GoodGuys.Count) - 1]!;
        var difficulty = (int)ServicesUtils.GetDifficultyFromEffort(board.Quest.EffortLvl);
        var difficultyFactor = 0.1 * difficulty;
        character.Status.Wealth -= (int)(character.Status.Wealth * difficultyFactor);

        board.LastActionResult = EncounterTypeResults.LoseWealth.All[diceLogic.Roll_1_to_n(EncounterTypeResults.LoseWealth.All.Count) - 1];

        return board;
    }

    private Battleboard RunBoonLogic(Battleboard board)
    {
        var character = board.GoodGuys[diceLogic.Roll_1_to_n(board.GoodGuys.Count) - 1]!;

        var roll = diceLogic.Roll_1_to_n(3);

        if (roll == 1)
        {
            foreach (var stat in character.Sheet.Stats.GetType().GetProperties())
            {
                var currentValue = (int)stat.GetValue(character.Sheet.Stats)!;
                stat.SetValue(character.Sheet.Stats, currentValue * 0.01);
            }
        }
        else if (roll == 2)
        {
            foreach (var asset in character.Sheet.Assets.GetType().GetProperties())
            {
                var currentValue = (int)asset.GetValue(character.Sheet.Assets)!;
                asset.SetValue(character.Sheet.Assets, currentValue * 0.01);
            }
        }
        else
        {
            foreach (var skill in character.Sheet.Skills.GetType().GetProperties())
            {
                var currentValue = (int)skill.GetValue(character.Sheet.Skills)!;
                skill.SetValue(character.Sheet.Skills, currentValue * 0.01);
            }
        }

        board.LastActionResult = EncounterTypeResults.Boon.All[diceLogic.Roll_1_to_n(EncounterTypeResults.Boon.All.Count) - 1];

        return board;
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

    private Battleboard RunExperienceLogic(Battleboard board)
    {
        throw new NotImplementedException();
    }

    #endregion
}
