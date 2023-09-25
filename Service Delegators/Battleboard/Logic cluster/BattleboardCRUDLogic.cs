using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardCRUDLogic
{
    List<Battleboard> GetBattleboards();
    Battleboard FindBattleboard(string battleboardId);
    Battleboard GetBattleboard(BattleboardCharacter battleboardCharacter);

    Battleboard CreateBattleboard(BattleboardCharacter battleboardCharacter);
    Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter);
    Battleboard KickFromBattleboard(BattleboardCharacter battleboardCharacter);
    void LeaveBattleboard(BattleboardCharacter battleboardCharacter);
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

    public Battleboard GetBattleboard(BattleboardCharacter battleboardCharacter)
    {
        lock (_lock)
        {
            var character = snapshot.Players.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.Id)!;
            return snapshot.Battleboards.Find(s => s.Id == character.Status.Gameplay.BattleboardId)!;
        }
    }

    public Battleboard CreateBattleboard(BattleboardCharacter battleboardCharacter)
    {
        lock (_lock)
        {
            var battleboard = new Battleboard
            {
                Id = Guid.NewGuid().ToString(),
            };

            var character = snapshot.Players.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.Id)!;
            character.Status.Gameplay.BattleboardId = battleboard.Id;
            character.Status.Gameplay.IsBattleboardGoodGuy = true;

            battleboard.GoodGuys.PartyLeadId = character.Identity.Id;
            battleboard.GoodGuys.Characters.Add(character);

            character.Mercenaries.ForEach(s =>
            {
                s.Status.Gameplay.BattleboardId = battleboard.Id;
                s.Status.Gameplay.IsBattleboardGoodGuy = true;

                battleboard.GoodGuys.Characters.Add(s);
            });

            snapshot.Battleboards.Add(battleboard);

            return battleboard;
        }
    }

    public Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter)
    {
        lock (_lock)
        {
            var character = snapshot.Players.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.Id)!;
            var battleboard = snapshot.Battleboards.Find(s => s.Id == battleboardCharacter.BattleboardIdToJoin)!;
            character.Status.Gameplay.BattleboardId = battleboard.Id;
            character.Status.Gameplay.IsBattleboardGoodGuy = battleboardCharacter.WantsToBeGood;

            if (battleboardCharacter.WantsToBeGood)
            {
                var partyLeadCharacter = battleboard.GoodGuys.Characters.Find(s => s.Identity.Id == battleboard.GoodGuys.PartyLeadId)!;
                if (character.Status.Worth > partyLeadCharacter.Status.Worth) battleboard.GoodGuys.PartyLeadId = character.Identity.Id;

                battleboard.GoodGuys.Characters.Add(character);

                character.Mercenaries.ForEach(s =>
                {
                    s.Status.Gameplay.BattleboardId = battleboard.Id;
                    s.Status.Gameplay.IsBattleboardGoodGuy = true;

                    battleboard.GoodGuys.Characters.Add(s);
                });
            }
            else
            {
                var partyLeadCharacter = battleboard.BadGuys.Characters.Find(s => s.Identity.Id == battleboard.BadGuys.PartyLeadId)!;
                if (character.Status.Worth > partyLeadCharacter.Status.Worth) battleboard.BadGuys.PartyLeadId = character.Identity.Id;

                battleboard.BadGuys.Characters.Add(character);

                character.Mercenaries.ForEach(s =>
                {
                    s.Status.Gameplay.BattleboardId = battleboard.Id;
                    s.Status.Gameplay.IsBattleboardGoodGuy = false;

                    battleboard.GoodGuys.Characters.Add(s);
                });
            }

            return battleboard;
        }
    }

    public Battleboard KickFromBattleboard(BattleboardCharacter battleboardCharacter)
    {
        lock (_lock)
        {
            var partyleadChar = snapshot.Players.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.Id)!;
            var battleboard = snapshot.Battleboards.Find(s => s.Id == partyleadChar.Status.Gameplay.BattleboardId)!;

            if (partyleadChar.Status.Gameplay.IsBattleboardGoodGuy)
            {
                var characterToBeKicked = battleboard.GoodGuys.Characters.Find(s => s.Identity.Id == battleboardCharacter.FirstTargetId)!;
                characterToBeKicked.Status.Gameplay.BattleboardId = string.Empty;
                battleboard.GoodGuys.Characters.Remove(characterToBeKicked);
                battleboard.GoodGuys.BattleFormation.Remove(characterToBeKicked.Identity.Id);

                characterToBeKicked.Mercenaries.ForEach(s =>
                {
                    battleboard.GoodGuys.Characters.Remove(s);
                    battleboard.GoodGuys.BattleFormation.Remove(s.Identity.Id);
                    battleboard.BattleOrder.Remove(s.Identity.Id);
                });
            }
            else
            {
                var characterToBeKicked = battleboard.BadGuys.Characters.Find(s => s.Identity.Id == battleboardCharacter.FirstTargetId)!;
                characterToBeKicked.Status.Gameplay.BattleboardId = string.Empty;
                battleboard.BadGuys.Characters.Remove(characterToBeKicked);
                battleboard.BadGuys.BattleFormation.Remove(characterToBeKicked.Identity.Id);

                characterToBeKicked.Mercenaries.ForEach(s =>
                {
                    battleboard.BadGuys.Characters.Remove(s);
                    battleboard.BadGuys.BattleFormation.Remove(s.Identity.Id);
                    battleboard.BattleOrder.Remove(s.Identity.Id);
                });
            }

            return battleboard;
        }
    }

    public void LeaveBattleboard(BattleboardCharacter battleboardCharacter)
    {
        lock (_lock)
        {
            var character = snapshot.Players.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.Id)!;
            var battleboard = snapshot.Battleboards.Find(s => s.Id == character.Status.Gameplay.BattleboardId)!;

            character.Status.Gameplay.BattleboardId = string.Empty;
            battleboard.BattleOrder.Remove(character.Identity.Id);

            if (character.Status.Gameplay.IsBattleboardGoodGuy)
            {
                battleboard.GoodGuys.Characters.Remove(character);
                battleboard.GoodGuys.BattleFormation.Remove(character.Identity.Id);

                if (battleboard.GoodGuys.PartyLeadId == character.Identity.Id)
                    battleboard.GoodGuys.PartyLeadId = battleboard.GoodGuys.Characters.OrderByDescending(s => s.Status.Worth).First().Identity.Id;

                character.Mercenaries.ForEach(s =>
                {
                    battleboard.GoodGuys.Characters.Remove(s);
                    battleboard.GoodGuys.BattleFormation.Remove(s.Identity.Id);
                    battleboard.BattleOrder.Remove(s.Identity.Id);
                });
            }
            else
            {
                battleboard.BadGuys.Characters.Remove(character);
                battleboard.BadGuys.BattleFormation.Remove(character.Identity.Id);

                if (battleboard.BadGuys.PartyLeadId == character.Identity.Id)
                    battleboard.BadGuys.PartyLeadId = battleboard.BadGuys.Characters.OrderByDescending(s => s.Status.Worth).First().Identity.Id;

                character.Mercenaries.ForEach(s =>
                {
                    battleboard.BadGuys.Characters.Remove(s);
                    battleboard.BadGuys.BattleFormation.Remove(s.Identity.Id);
                    battleboard.BattleOrder.Remove(s.Identity.Id);
                });
            }

            battleboard.BattleOrder.Remove(character.Identity.Id);
        }
    }
}
