using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

public interface IBattleboardCombatLogic
{
    Battleboard StartCombat(string battleboardId);
    Battleboard Attack(tbd);
    Battleboard Cast(tbd);
    Battleboard Defend(tbd);
    Battleboard Mend(tbd);
    Battleboard Hide(tbd);
    Battleboard Traps(tbd);
    Battleboard Pass(tbd);
    Battleboard LetAiAct(tbd);
    Battleboard StopCombat(Battleboard battleboard);
}

public class BattleboardCombatLogic : IBattleboardCombatLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;
    private readonly IPersistenceService persistence;
    private readonly IDiceLogicDelegator dice;
    private readonly ICharacterLogicDelegator characters;

    public BattleboardCombatLogic(
        Snapshot snapshot,
        IPersistenceService persistence,
        IDiceLogicDelegator dice,
        ICharacterLogicDelegator characters)
    {
        this.snapshot = snapshot;
        this.persistence = persistence;
        this.dice = dice;
        this.characters = characters;
    }

    public Battleboard StartCombat(string battleboardId)
    {
        lock (_lock)
        {
            var board = snapshot.Battleboards.Find(s => s.Id == battleboardId)!;
            board.IsInCombat = true;

            ApplyTacticalRoll(board);

            var combatants = board.GetAllCharacters()
                .OrderByDescending(s => s.Sheet.Assets.Actions)
                .ToList();

            combatants.ForEach(s => s.Status.Gameplay.IsLocked = true);
            board.BattleOrder = combatants.Select(s => s.Identity.Id).ToList();

            return board;
        }
    }

    #region private methods
    private void ApplyTacticalRoll(Battleboard board)
    {
        var goodGuyPartyLead = board.GoodGuys.First(s => s.Identity.Id == board.GoodGuyPartyLead).Status.Name;

        var highestRollerGoodGuy = board.GoodGuys.OrderByDescending(s => s.Sheet.Skills.Tactics).First()!;
        var highestRollerBadGuy = board.BadGuys.OrderByDescending(s => s.Sheet.Skills.Tactics).First()!;

        var goodRoll = dice.Roll_game_dice(true, CharactersLore.Skills.Tactics, highestRollerGoodGuy);
        var badRoll = dice.Roll_game_dice(true, CharactersLore.Skills.Tactics, highestRollerBadGuy);

        var result = goodRoll - badRoll;

        if (result <= -30)
        {
            board.LastActionResult = $"Disastrous tactical disadvantage for {goodGuyPartyLead}'s party.";
            RemoveSomeItems(board.GoodGuys);
            ReduceResolve(board.GoodGuys);
            ReduceActions(board.GoodGuys);
        }
        else if (result <= -10 && result < -5)
        {
            board.LastActionResult = $"Unfavourable tactical disadvantage for {goodGuyPartyLead}'s party.";
            ReduceResolve(board.GoodGuys);
            ReduceActions(board.GoodGuys);
        }
        else if (result <= -5)
        {
            board.LastActionResult = $"Small tactical disadvantage for {goodGuyPartyLead}'s party.";
            ReduceActions(board.GoodGuys);
        }
        else if (result >= 5 && result < 10)
        {
            board.LastActionResult = $"Slight tactical advantage for {goodGuyPartyLead}'s party.";
            ReduceActions(board.BadGuys);
        }
        else if (result >= 10 && result < 30)
        {
            board.LastActionResult = $"Favourable tactical advantage for {goodGuyPartyLead}'s party.";
            ReduceResolve(board.BadGuys);
            ReduceActions(board.BadGuys);
        }
        else if (result >= 30)
        {
            board.LastActionResult = $"Masterly tactical disadvantage for {goodGuyPartyLead}'s party.";
            RemoveSomeItems(board.BadGuys);
            ReduceResolve(board.BadGuys);
            ReduceActions(board.BadGuys);
        }
        else
        {
            board.LastActionResult = $"Inconclusive tactical roll, the fight is even.";
        }

        void RemoveSomeItems(List<Character> party)
        {
            foreach (var character in party)
            {
                var unequip = new CharacterEquip
                {
                    CharacterIdentity = character.Identity,
                };

                var roll = dice.Roll_d6_noReroll();

                if (roll <= 1)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Body;
                    unequip.ItemId = character.Inventory.Body!.Identity.Id;
                    characters.UnequipCharacterItem(unequip);
                }
                if (roll <= 2)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Head;
                    unequip.ItemId = character.Inventory.Head!.Identity.Id;
                    characters.UnequipCharacterItem(unequip);
                }
                if (roll <= 3)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Mainhand;
                    unequip.ItemId = character.Inventory.Mainhand!.Identity.Id;
                    characters.UnequipCharacterItem(unequip);
                }
                if (roll <= 4)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Offhand;
                    unequip.ItemId = character.Inventory.Offhand!.Identity.Id;
                    characters.UnequipCharacterItem(unequip);
                }
                if (roll <= 5)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Ranged;
                    unequip.ItemId = character.Inventory.Ranged!.Identity.Id;
                    characters.UnequipCharacterItem(unequip);
                }
                if (roll <= 6)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Heraldry;
                    unequip.ItemId = character.Inventory.Heraldry!.First().Identity.Id;
                    characters.UnequipCharacterItem(unequip);
                }
            }
        }

        void ReduceResolve(List<Character> party)
        {
            foreach (var character in party)
            {
                character.Sheet.Assets.ResolveLeft = (int)(character.Sheet.Assets.Resolve * 0.5);
            }
        }

        void ReduceActions(List<Character> party)
        {
            foreach (var character in party)
            {
                character.Sheet.Assets.ActionsLeft = (int)(character.Sheet.Assets.Actions * 0.5);
            }
        }

    }



    #endregion
}
