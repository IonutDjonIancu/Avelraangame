using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

public interface IBattleboardCombatLogic
{
    Battleboard StartCombat(string battleboardId);
    Battleboard Attack(BattleboardActor actor);
    Battleboard Cast(BattleboardActor actor);
    Battleboard Mend(BattleboardActor actor);
    Battleboard Hide(BattleboardActor actor);
    Battleboard Traps(BattleboardActor actor);
    Battleboard Rest(BattleboardActor actor);
    Battleboard LetAiAct(BattleboardActor actor);
    Battleboard EndRound(BattleboardActor actor);
    Battleboard EndCombat(BattleboardActor actor);
}

public class BattleboardCombatLogic : IBattleboardCombatLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;
    private readonly IDiceLogicDelegator dice;
    private readonly ICharacterLogicDelegator characters;

    public BattleboardCombatLogic(
        Snapshot snapshot,
        IPersistenceService persistence,
        IDiceLogicDelegator dice,
        ICharacterLogicDelegator characters)
    {
        this.snapshot = snapshot;
        this.dice = dice;
        this.characters = characters;
    }

    public Battleboard StartCombat(string battleboardId)
    {
        lock (_lock)
        {
            var board = snapshot.Battleboards.Find(s => s.Id == battleboardId)!;
            board.IsInCombat = true;
            board.CanLvlUp = true;
            board.RoundNr = 1;

            ApplyTacticalRoll(board);

            var combatants = board.GetAllCharacters()
                .OrderByDescending(s => s.Sheet.Assets.Actions)
                .ToList();

            combatants.ForEach(s => s.Status.Gameplay.IsLocked = true);
            board.BattleOrder = combatants.Select(s => s.Identity.Id).ToList();
            
            return board;
        }
    }

    public Battleboard Attack(BattleboardActor actor)
    {
        lock (_lock)
        {
            var (attacker, board, defender) = GetAttackerBoardDefender(actor, snapshot);

            return RunAttackLogic(attacker, board, defender);
        }
    }

    public Battleboard Cast(BattleboardActor actor)
    {
        lock (_lock)
        {
            var (attacker, board, defender) = GetAttackerBoardDefender(actor, snapshot);

            return RunCastLogic(attacker, board, defender);
        }
    }

    public Battleboard Mend(BattleboardActor actor)
    {
        lock (_lock)
        {
            var (attacker, board, defender) = GetAttackerBoardDefender(actor, snapshot);

            var attackerRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Skills.Apothecary, attacker);

            attacker.Status.Gameplay.IsHidden = false;

            if (attacker.Sheet.Assets.ResolveLeft >= 150)
            {
                attacker.Sheet.Assets.ResolveLeft -= 5;
            }
            else if (attacker.Sheet.Assets.ResolveLeft < 150 && attacker.Sheet.Assets.ResolveLeft >= 100)
            {
                attackerRoll *= (int)0.75;
            }
            else if (attacker.Sheet.Assets.ResolveLeft < 100 && attacker.Sheet.Assets.ResolveLeft >= 50)
            {
                attackerRoll *= (int)0.5;
            }
            else
            {
                attackerRoll *= (int)0.25;
            }

            defender.Sheet.Assets.ResolveLeft += (int)(attackerRoll * 0.3);
            board.LastActionResult = $"{attacker.Status.Name} healed {defender.Status.Name} for {attackerRoll}.";

            MoveOrRemoveFromBattleOrder(attacker, board);

            return board;
        }
    }

    public Battleboard Hide(BattleboardActor actor)
    {
        lock (_lock)
        {
            var (attacker, board) = GetAttackerBoard(actor, snapshot);

            var attackerRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Skills.Hide, attacker);
            var spotters = 0;

            if (attacker.Status.Gameplay.IsGoodGuy)
            {
                attackerRoll += board.BadGuys.Count;
                attackerRoll -= board.GoodGuys.Select(s => s.Status.Gameplay.IsHidden).ToList().Count;

                board.BadGuys.ForEach(def =>
                {
                    var defenderRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Assets.Spot, def);

                    if (defenderRoll > attackerRoll) spotters++;
                });
            }
            else
            {
                attackerRoll += board.GoodGuys.Count;
                attackerRoll -= board.BadGuys.Select(s => s.Status.Gameplay.IsHidden).ToList().Count;

                board.GoodGuys.ForEach(def =>
                {
                    var defenderRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Assets.Spot, def);

                    if (defenderRoll > attackerRoll) spotters++;
                });
            }

            attacker.Status.Gameplay.IsHidden = spotters == 0;
            board.LastActionResult = attacker.Status.Gameplay.IsHidden ? $"{attacker.Status.Name} is now hidden." : "Hide failed, you have been spotted.";

            MoveOrRemoveFromBattleOrder(attacker, board);

            return board;
        }
    }

    public Battleboard Traps(BattleboardActor actor)
    {
        lock (_lock)
        {
            var (attacker, board) = GetAttackerBoard(actor, snapshot);

            return RunTrapsLogic(attacker, board);
        }
    }

    public Battleboard Rest(BattleboardActor actor)
    {
        lock (_lock)
        {
            var (attacker, board) = GetAttackerBoard(actor, snapshot);

            var resolveToHeal = (int)((attacker.Sheet.Assets.Resolve - attacker.Sheet.Assets.ResolveLeft) * 0.1);
            attacker.Sheet.Assets.ResolveLeft += resolveToHeal;

            var manaToHeal = (int)((attacker.Sheet.Assets.Mana - attacker.Sheet.Assets.ManaLeft) * 0.2);
            attacker.Sheet.Assets.ManaLeft += manaToHeal;

            board.LastActionResult = $"You regain {resolveToHeal} Resolve, and {manaToHeal} Mana.";

            MoveOrRemoveFromBattleOrder(attacker, board);

            return board;
        }
    }

    public Battleboard LetAiAct(BattleboardActor actor)
    {
        lock (_lock)
        {
            var (attacker, board) = GetAttackerBoard(actor, snapshot);

            var npc = board.GetAllCharacters().Find(s => s.Identity.Id == board.BattleOrder.First())!;

            var skillToUse = AiDecideOnHighestSkill(npc);

            Character target;

            if (npc.Status.Gameplay.IsGoodGuy)
            {
                var enemyIndex = dice.Roll_1_to_n(board.BadGuys.Count) - 1;
                target = board.BadGuys[enemyIndex];
            }
            else
            {
                var enemyIndex = dice.Roll_1_to_n(board.GoodGuys.Count) - 1;
                target = board.GoodGuys[enemyIndex];
            }

            Battleboard resultBoard = skillToUse switch
            {
                CharactersLore.Skills.Melee => RunAttackLogic(npc, board, target),
                CharactersLore.Skills.Traps => RunTrapsLogic(npc, board),
                _ => RunCastLogic(npc, board, target),
            };

            var aiResultMessage = $"<< Ai >> {board.LastActionResult}";

            resultBoard.LastActionResult = aiResultMessage;

            MoveOrRemoveFromBattleOrder(npc, board);

            return resultBoard;
        }
    }

    public Battleboard EndRound(BattleboardActor actor)
    {
        lock (_lock)
        {
            var (attacker, board) = GetAttackerBoard(actor, snapshot);

            var combatants = board.GetAllCharacters().OrderByDescending(s => s.Sheet.Assets.Actions).ToList();

            foreach (var combatant in combatants)
            {
                combatant.Sheet.Assets.ActionsLeft = combatant.Sheet.Assets.Actions;
                combatant.Status.Gameplay.IsHidden = false;

                var resolveToHeal = (int)((combatant.Sheet.Assets.Resolve - combatant.Sheet.Assets.ResolveLeft) * 0.2);
                combatant.Sheet.Assets.ResolveLeft += resolveToHeal;

                var manaToHeal = (int)((combatant.Sheet.Assets.Mana - combatant.Sheet.Assets.ManaLeft) * 0.5);
                combatant.Sheet.Assets.Mana += manaToHeal;
            }

            board.BattleOrder = combatants.Select(s => s.Identity.Id).ToList();
            board.LastActionResult = "New round begins...";
            board.RoundNr++;

            if (board.RoundNr > board.EffortLvl / 10)
            {
                board.CanLvlUp = false;
                return board;
            }
            
            return board;
        }
    }

    public Battleboard EndCombat(BattleboardActor actor)
    {
        lock (_lock)
        {
            var (attacker, board) = GetAttackerBoard(actor, snapshot);

            ChooseNewPartyLeadIfPartyLeadDead(attacker, board);

            if (board.GoodGuyPartyLead == string.Empty && board.BadGuyPartyLead == string.Empty)
            {
                ReturnNpcsHomeIfAllPlayersAreDead(attacker, board, snapshot);
                return null;
            }

            if (attacker.Status.Gameplay.IsGoodGuy)
            {
                foreach (var goodGuy in board.GoodGuys)
                {
                    goodGuy.Status.Gameplay.IsLocked = false;
                    goodGuy.Status.Gameplay.IsHidden = false;

                    if (!goodGuy.Status.Gameplay.IsAlive)
                    {
                        LootDeadMan(goodGuy, attacker);
                    }
                }

                foreach (var badGuy in board.BadGuys)
                {
                    LootDeadMan(badGuy, attacker);
                }
            }
            else
            {
                foreach (var badguy in board.BadGuys)
                {
                    badguy.Status.Gameplay.IsLocked = false;
                    badguy.Status.Gameplay.IsHidden = false;

                    if (!badguy.Status.Gameplay.IsAlive)
                    {
                        LootDeadMan(badguy, attacker);
                    }
                }

                foreach (var goodGuy in board.GoodGuys)
                {
                    LootDeadMan(goodGuy, attacker);
                }
            }

            LootWealth(attacker, board);
            RemoveDeadCharacters(board);

            return board;
        }
    }




    #region private methods
    private void ReturnNpcsHomeIfAllPlayersAreDead(Character attacker, Battleboard board, Snapshot snapshot)
    {
        var surviors = board.GetAllCharacters().Where(s =>
            s.Status.Gameplay.IsAlive
            && s.Status.Gameplay.IsNpc
            && s.Identity.Id != Guid.Empty.ToString()).ToList();

        var location = snapshot.Locations.FirstOrDefault(s => s.FullName == attacker.Status.Position.GetPositionFullName())!;

        surviors.ForEach(s =>
        { 
            s.Status.Gameplay.IsGoodGuy = false;
            s.Status.Gameplay.IsHidden = false;
            s.Status.Gameplay.IsLocked = false;
            s.Status.Gameplay.BattleboardId = string.Empty;

            s.Sheet.Assets.ResolveLeft = s.Sheet.Assets.Resolve;
            s.Sheet.Assets.ManaLeft = s.Sheet.Assets.Mana;
            s.Sheet.Assets.ActionsLeft = s.Sheet.Assets.Actions;

            location.Mercenaries.Add(s);
        });
    }

    private void RemoveDeadCharacters(Battleboard board)
    {
        board.GoodGuys.Where(s => !s.Status.Gameplay.IsAlive).ToList().ForEach(s => board.GoodGuys.Remove(s));
        board.BadGuys.Where(s => !s.Status.Gameplay.IsAlive).ToList().ForEach(s => board.BadGuys.Remove(s));
    }

    private void ChooseNewPartyLeadIfPartyLeadDead(Character partyLead, Battleboard board)
    {
        if (partyLead.Status.Gameplay.IsAlive) return;

        if (partyLead.Status.Gameplay.IsGoodGuy)
        {
            var anySurvivor = board.GoodGuys.Where(s => s.Status.Gameplay.IsAlive && !s.Status.Gameplay.IsNpc).OrderByDescending(s => s.Status.Worth).FirstOrDefault();
            board.GoodGuyPartyLead = anySurvivor != null ? anySurvivor.Identity.Id : string.Empty;
        }
        else
        {
            var anySurvivor = board.BadGuys.Where(s => s.Status.Gameplay.IsAlive && !s.Status.Gameplay.IsNpc).OrderByDescending(s => s.Status.Worth).FirstOrDefault();
            board.BadGuyPartyLead = anySurvivor != null ? anySurvivor.Identity.Id : string.Empty;
        }
    }

    private void LootWealth(Character partyLead, Battleboard board)
    {
        var totalLootWealth = board.GetAllCharacters().Where(s => !s.Status.Gameplay.IsAlive).ToList().Sum(s => s.Status.Wealth);

        var partyLeadShare = (int)(totalLootWealth * 0.4);
        var partyShare = totalLootWealth - partyLeadShare;

        partyLead.Status.Wealth += partyLeadShare;

        var survivors = board.GetAllCharacters().Where(s => !s.Status.Gameplay.IsAlive && s.Identity.Id != partyLead.Identity.Id).ToList();

        foreach (var survivor in survivors)
        {
            survivor.Status.Wealth += partyShare / survivors.Count;
        }

    }

    private void LootDeadMan(Character deadMan, Character partyLead)
    {
        deadMan.Inventory.GetAllEquipedItems().ForEach(s => partyLead.Inventory.Supplies.Add(s));
        deadMan.Inventory.Supplies.ForEach(s => partyLead.Inventory.Supplies.Add(s));
    }

    private string AiDecideOnHighestSkill(Character npc)
    {
        var dictOfSkills = new Dictionary<string, int>
        {
            { CharactersLore.Skills.Melee, npc.Sheet.Skills.Melee },
            { CharactersLore.Skills.Arcane, npc.Sheet.Skills.Arcane },
            { CharactersLore.Skills.Psionics, npc.Sheet.Skills.Psionics },
            { CharactersLore.Skills.Traps, npc.Sheet.Skills.Traps },
        };

        return dictOfSkills.OrderByDescending(s => s.Value).First().Key;
    }

    private (Character attacker, Battleboard board) GetAttackerBoard(BattleboardActor actor, Snapshot snapshot)
    {
        var attacker = BattleboardHelpers.GetCharacter(actor, snapshot);
        var board = BattleboardHelpers.GetBattleboard(attacker.Status.Gameplay.BattleboardId, snapshot);

        return (attacker, board);
    }

    private (Character attacker, Battleboard board, Character defender) GetAttackerBoardDefender(BattleboardActor actor, Snapshot snapshot)
    {
        var (attacker, board) = GetAttackerBoard(actor, snapshot);  
        var defender = board.GetAllCharacters().Find(s => s.Identity.Id == actor.TargetId)!;

        return (attacker, board, defender); 
    }

    private void CheckDefenderIsDead(Character defender, Battleboard board)
    {
        if (defender.Sheet.Assets.ResolveLeft <= 0)
        {
            defender.Status.Gameplay.IsAlive = false;
            board.BattleOrder.Remove(defender.Identity.Id);
        }
    }

    private void MoveOrRemoveFromBattleOrder(Character attacker, Battleboard board)
    {
        board.BattleOrder.RemoveAt(0);
        if (attacker.Sheet.Assets.ActionsLeft > 0)
        {
            board.BattleOrder.Add(attacker.Identity.Id);
        }
    }

    private Battleboard RunTrapsLogic(Character attacker, Battleboard board)
    {
        var attackerRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Skills.Traps, attacker);
        var spotters = 0;

        if (attacker.Sheet.Assets.ResolveLeft >= 150)
        {
            attacker.Sheet.Assets.ResolveLeft -= 1;
        }
        else
        {
            attackerRoll *= (int)0.5;
        }

        if (attacker.Status.Gameplay.IsGoodGuy)
        {
            board.BadGuys.ForEach(def =>
            {
                var defenderRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Assets.Spot, def);

                if (defenderRoll > attackerRoll) spotters++;
            });

            if (spotters == 0)
            {
                var targetIndex = dice.Roll_1_to_n(board.BadGuys.Count) - 1;

                var target = board.BadGuys[targetIndex];
                target.Sheet.Assets.ResolveLeft -= attackerRoll;

                CheckDefenderIsDead(target, board);

                board.LastActionResult = $"{attacker.Status.Name} trapped {target.Status.Name} for {attackerRoll} dmg.";
            }
            else
            {
                attacker.Status.Gameplay.IsHidden = false;
                board.LastActionResult = $"Traps failed, {attacker.Status.Name} is now revealed.";
            }
        }
        else
        {
            board.GoodGuys.ForEach(def =>
            {
                var defenderRoll = dice.Roll_game_dice(true, CharactersLore.Assets.Spot, def);

                if (defenderRoll > attackerRoll) spotters++;
            });

            if (spotters == 0)
            {
                var targetIndex = dice.Roll_1_to_n(board.GoodGuys.Count) - 1;

                var target = board.GoodGuys[targetIndex];
                target.Sheet.Assets.ResolveLeft -= attackerRoll;

                CheckDefenderIsDead(target, board);

                board.LastActionResult = $"{attacker.Status.Name} trapped {target.Status.Name} for {attackerRoll} dmg.";
            }
            else
            {
                attacker.Status.Gameplay.IsHidden = false;
                board.LastActionResult = $"Traps failed for {attacker.Status.Name}, you are now revealed.";
            }
        }

        MoveOrRemoveFromBattleOrder(attacker, board);

        return board;
    }

    private Battleboard RunCastLogic(Character attacker, Battleboard board, Character defender)
    {
        if (defender.Sheet.Assets.Purge >= 100)
        {
            board.LastActionResult = "Your target is immune to arcane and psionic effects.";
            return board;
        }

        var isPsionicsHigher = attacker.Sheet.Skills.Psionics >= attacker.Sheet.Skills.Arcane;

        var attackerRoll = 0;

        attacker.Status.Gameplay.IsHidden = false;

        if (isPsionicsHigher)
        {
            attackerRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Skills.Psionics, attacker);
        }
        else
        {
            attackerRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Skills.Arcane, attacker);
        }

        if (attacker.Sheet.Assets.ResolveLeft >= 100)
        {
            attacker.Sheet.Assets.ResolveLeft -= 5;
        }
        else
        {
            attackerRoll *= (int)0.5;
        }

        attacker.Sheet.Assets.ManaLeft -= attackerRoll;

        var goodGuys = board.GoodGuys.Select(s => s.Identity.Id).ToList();
        var badGuys = board.BadGuys.Select(s => s.Identity.Id).ToList();
        var sameTeam = attacker.Status.Gameplay.IsGoodGuy && defender.Status.Gameplay.IsGoodGuy
            || !attacker.Status.Gameplay.IsGoodGuy && !defender.Status.Gameplay.IsGoodGuy;

        if (sameTeam)
        {
            var effect = isPsionicsHigher
                ? Energysurge(attackerRoll)
                : Spellcast(attackerRoll);

            var amountToHeal = (int)(effect * defender.Sheet.Assets.Purge / 100);
            defender.Sheet.Assets.ResolveLeft += amountToHeal;

            board.LastActionResult = $"{attacker.Status.Name} healed {defender.Status.Name} for {amountToHeal}.";
        }
        else
        {
            var defenderRoll = 0;

            if (defender.Status.Traits.Class == CharactersLore.Classes.Mage
                || defender.Status.Traits.Class == CharactersLore.Classes.Sorcerer)
            {
                defenderRoll = dice.Roll_game_dice(true, CharactersLore.Skills.Arcane, defender);
            }
            // add psionist classes
            //else if (defender.Status.Traits.Class == CharactersLore.Classes.Mage)
            //{

            //}
            else
            {
                defenderRoll = dice.Roll_game_dice(true, CharactersLore.Stats.Willpower, defender);
            }

            var attackDiff = attackerRoll - defenderRoll;

            if (attackDiff <= 0) 
            {
                board.LastActionResult = $"Your target has saved vs. your {(isPsionicsHigher ? "energy surge." : "spellcast.")}";
                return board;
            }

            var effect = isPsionicsHigher
                ? Energysurge(attackDiff)
                : Spellcast(attackDiff);

            var amountToDmg = defender.Sheet.Assets.Purge < 100 ? (int)(effect - effect * defender.Sheet.Assets.Purge / 100) : 0;

            defender.Sheet.Assets.ResolveLeft -= amountToDmg;

            board.LastActionResult = isPsionicsHigher ? $"{attacker.Status.Name}'s energy surge did {amountToDmg} dmg." : $"{attacker.Status.Name}'s spellcraft did {amountToDmg} dmg.";
        }

        int Energysurge(int roll)
        {
            return (int)(roll * attacker.Sheet.Assets.Purge / 100);
        }

        int Spellcast(int roll)
        {
            return attacker.Sheet.Stats.Abstract * attacker.Status.EntityLevel + (int)(roll * 0.1);
        }

        CheckDefenderIsDead(defender, board);
        MoveOrRemoveFromBattleOrder(attacker, board);

        return board;
    }

    private Battleboard RunAttackLogic(Character attacker, Battleboard board, Character defender)
    {
        var attackerRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Skills.Melee, attacker);
        var defenderRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Skills.Melee, defender);

        attacker.Status.Gameplay.IsHidden = false;

        if (attacker.Sheet.Assets.ResolveLeft >= 50)
        {
            attacker.Sheet.Assets.ResolveLeft -= 10;
        }
        else
        {
            attackerRoll *= (int)0.5;
        }

        if (defender.Sheet.Assets.ResolveLeft >= 30)
        {
            defender.Sheet.Assets.ResolveLeft -= 3;
        }
        else
        {
            defenderRoll *= (int)0.5;
        }

        var result = attackerRoll - defenderRoll;

        if (result <= 0)
        {
            board.LastActionResult = $"{attacker.Status.Name} missed.";
            return board;
        }

        var harmPercentage = result * 0.2;
        var totalHarm = (int)(attacker.Sheet.Assets.Harm * harmPercentage);
        var harmAfterDefense = (int)(totalHarm * defender.Sheet.Assets.DefenseFinal / 100);

        defender.Sheet.Assets.ResolveLeft -= harmAfterDefense;
        board.LastActionResult = $"{attacker.Status.Name} did {harmAfterDefense} dmg in melee.";

        CheckDefenderIsDead(defender, board);
        MoveOrRemoveFromBattleOrder(attacker, board);

        return board;
    }

    private void ApplyTacticalRoll(Battleboard board)
    {
        var goodGuyPartyLead = board.GoodGuys.First(s => s.Identity.Id == board.GoodGuyPartyLead).Status.Name;

        var highestRollerGoodGuy = board.GoodGuys.OrderByDescending(s => s.Sheet.Skills.Tactics).First()!;
        var highestRollerBadGuy = board.BadGuys.OrderByDescending(s => s.Sheet.Skills.Tactics).First()!;

        var goodRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Skills.Tactics, highestRollerGoodGuy);
        var badRoll = dice.Roll_game_dice(board.CanLvlUp, CharactersLore.Skills.Tactics, highestRollerBadGuy);

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
                    if (character.Inventory.Body != null)
                    {
                        unequip.ItemId = character.Inventory.Body!.Identity.Id;
                        characters.UnequipCharacterItem(unequip);
                    }
                }
                if (roll <= 2)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Head;
                    if (character.Inventory.Head != null)
                    {
                        unequip.ItemId = character.Inventory.Head!.Identity.Id;
                        characters.UnequipCharacterItem(unequip);
                    }
                }
                if (roll <= 3)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Mainhand;
                    if (character.Inventory.Mainhand != null)
                    {
                        unequip.ItemId = character.Inventory.Mainhand!.Identity.Id;
                        characters.UnequipCharacterItem(unequip);
                    }
                }
                if (roll <= 4)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Offhand;
                    if (character.Inventory.Offhand != null)
                    {
                        unequip.ItemId = character.Inventory.Offhand!.Identity.Id;
                        characters.UnequipCharacterItem(unequip);
                    }
                }
                if (roll <= 5)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Ranged;
                    if (character.Inventory.Ranged != null)
                    {
                        unequip.ItemId = character.Inventory.Ranged!.Identity.Id;
                        characters.UnequipCharacterItem(unequip);
                    }
                }
                if (roll <= 6)
                {
                    unequip.InventoryLocation = ItemsLore.InventoryLocation.Heraldry;
                    if (character.Inventory.Heraldry!.First() != null)
                    {
                        unequip.ItemId = character.Inventory.Heraldry!.First()!.Identity.Id;
                        characters.UnequipCharacterItem(unequip);
                    }
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
