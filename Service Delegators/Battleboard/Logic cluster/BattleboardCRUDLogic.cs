using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardCRUDLogic
{
    List<Battleboard> GetBattleboards();
    Battleboard FindBattleboard(string battleboardId);
    Battleboard GetBattleboard(BattleboardActor battleboardCharacter);

    Battleboard CreateBattleboard(BattleboardActor battleboardCharacter);
    Battleboard JoinBattleboard(BattleboardActor battleboardCharacter);
    Battleboard RemoveFromBattleboard(BattleboardActor battleboardCharacter);
}

public class BattleboardCRUDLogic : IBattleboardCRUDLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;

    public BattleboardCRUDLogic(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public List<Battleboard> GetBattleboards()
    {
        return snapshot.Battleboards;
    }

    public Battleboard FindBattleboard(string battleboardId)
    {
        return GetBattleboards().Find(s => s.Id == battleboardId)!;
    }

    public Battleboard GetBattleboard(BattleboardActor battleboardCharacter)
    {
        lock (_lock)
        {
            var character = BattleboardHelpers.GetCharacter(battleboardCharacter, snapshot);
            return snapshot.Battleboards.Find(s => s.Id == character.Status.Gameplay.BattleboardId)!;
        }
    }

    public Battleboard CreateBattleboard(BattleboardActor battleboardCharacter)
    {
        lock (_lock)
        {
            var battleboard = new Battleboard
            {
                Id = Guid.NewGuid().ToString(),
            };

            var character = BattleboardHelpers.GetCharacter(battleboardCharacter, snapshot);
            character.Status.Gameplay.BattleboardId = battleboard.Id;
            character.Status.Gameplay.IsGoodGuy = true;

            battleboard.GoodGuyPartyLead = character.Identity.Id;
            battleboard.GoodGuys.Add(character);

            character.Mercenaries.ForEach(s =>
            {
                s.Status.Gameplay.BattleboardId = battleboard.Id;
                s.Status.Gameplay.IsGoodGuy = true;

                battleboard.GoodGuys.Add(s);
            });

            battleboard.EffortLvl = snapshot.Locations.Find(s => s.FullName == character.Status.Position.GetPositionFullName())!.Effort;

            snapshot.Battleboards.Add(battleboard);

            return battleboard;
        }
    }

    public Battleboard JoinBattleboard(BattleboardActor battleboardCharacter)
    {
        lock (_lock)
        {
            var character = BattleboardHelpers.GetCharacter(battleboardCharacter, snapshot);
            var board = snapshot.Battleboards.Find(s => s.Id == battleboardCharacter.BattleboardIdToJoin)!;
            character.Status.Gameplay.BattleboardId = board.Id;
            character.Status.Gameplay.IsGoodGuy = battleboardCharacter.WantsToBeGood;

            if (battleboardCharacter.WantsToBeGood)
            {
                var partyLead = board.GoodGuys.Find(s => s.Identity.Id == board.GoodGuyPartyLead)!;
                if (character.Status.Worth > partyLead.Status.Worth) board.GoodGuyPartyLead = character.Identity.Id;

                board.GoodGuys.Add(character);

                character.Mercenaries.ForEach(s =>
                {
                    s.Status.Gameplay.BattleboardId = board.Id;
                    s.Status.Gameplay.IsGoodGuy = true;

                    board.GoodGuys.Add(s);
                });
            }
            else
            {
                var partyLead = board.BadGuys.Find(s => s.Identity.Id == board.BadGuyPartyLead)!;
                if (character.Status.Worth > partyLead.Status.Worth) board.BadGuyPartyLead = character.Identity.Id;

                board.BadGuys.Add(character);

                character.Mercenaries.ForEach(s =>
                {
                    s.Status.Gameplay.BattleboardId = board.Id;
                    s.Status.Gameplay.IsGoodGuy = false;

                    board.BadGuys.Add(s);
                });
            }

            return board;
        }
    }

    public Battleboard RemoveFromBattleboard(BattleboardActor battleboardCharacter)
    {
        lock (_lock)
        {
            Character charToRemove;
            Battleboard board;

            if (battleboardCharacter.TargetId == string.Empty)
            {
                charToRemove = BattleboardHelpers.GetCharacter(battleboardCharacter, snapshot);
                board = snapshot.Battleboards.Find(s => s.Id == charToRemove.Status.Gameplay.BattleboardId)!;
            } 
            else
            {
                var partyLead = BattleboardHelpers.GetCharacter(battleboardCharacter, snapshot);
                board = snapshot.Battleboards.Find(s => s.Id == partyLead.Status.Gameplay.BattleboardId)!;
                charToRemove = partyLead.Status.Gameplay.IsGoodGuy 
                    ? board.GoodGuys.Find(s => s.Identity.Id == battleboardCharacter.TargetId)! 
                    : board.BadGuys.Find(s => s.Identity.Id == battleboardCharacter.TargetId)!;
            }

            if (charToRemove.Status.Gameplay.IsGoodGuy)
            {
                if (board.GoodGuyPartyLead == charToRemove.Identity.Id)
                {
                    board.GoodGuyPartyLead = board.GoodGuys
                        .Where(s => !s.Status.Gameplay.IsNpc)
                        .OrderByDescending(s => s.Status.Worth)
                        .First().Identity.Id;
                }

                board.GoodGuys.Remove(charToRemove);

                charToRemove.Mercenaries.ForEach(s =>
                {
                    s.Status.Gameplay.BattleboardId = string.Empty;
                    s.Status.Gameplay.IsGoodGuy = false;
                    s.Status.Gameplay.IsHidden = false;

                    board.GoodGuys.Remove(s);
                    board.BattleOrder.Remove(s.Identity.Id);
                });
            }
            else
            {
                if (board.BadGuyPartyLead == charToRemove.Identity.Id)
                {
                    board.BadGuyPartyLead = board.BadGuys
                        .Where(s => !s.Status.Gameplay.IsNpc)
                        .OrderByDescending(s => s.Status.Worth)
                        .First().Identity.Id;
                }

                board.BadGuys.Remove(charToRemove);

                charToRemove.Mercenaries.ForEach(s =>
                {
                    s.Status.Gameplay.BattleboardId = string.Empty;
                    s.Status.Gameplay.IsGoodGuy = false;
                    s.Status.Gameplay.IsHidden = false;

                    board.BadGuys.Remove(s);
                    board.BattleOrder.Remove(s.Identity.Id);
                });
            }

            charToRemove.Status.Gameplay.BattleboardId = string.Empty;
            charToRemove.Status.Gameplay.IsGoodGuy = false;
            charToRemove.Status.Gameplay.IsHidden = false;
            board.BattleOrder.Remove(charToRemove.Identity.Id);

            if (board.GoodGuys.Count == 0
                && board.BadGuys.Count == 0)
            {
                snapshot.Battleboards.Remove(board);
            }

            return board;
        }
    }
}
