using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardCreateLogic
{
    Battleboard GetBattleboard(BattleboardCharacter battleboardCharacter);

    Battleboard CreateBattleboard(BattleboardCharacter battleboardCharacter);
    Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter);
    Battleboard KickFromBattleboard(BattleboardCharacter battleboardCharacter);
    void LeaveBattleboard(BattleboardCharacter battleboardCharacter);
}

public class BattleboardCRUDLogic : IBattleboardCreateLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;

    public BattleboardCRUDLogic(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public Battleboard GetBattleboard(BattleboardCharacter battleboardCharacter)
    {
        lock (_lock)
        {
            var character = snapshot.Players.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.Id)!;
            return snapshot.Battleboards.Find(s => s.Id == character.Status.Gameplay.BattleboardId)!;
        }
    }

    public Battleboard CreateBattleboard(CharacterIdentity partyLeadIdentity)
    {
        lock (_lock)
        {
            var battleboard = new Battleboard
            {
                Id = Guid.NewGuid().ToString(),
            };

            var character = snapshot.Players.Find(s => s.Identity.Id == partyLeadIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == partyLeadIdentity.Id)!;
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

    public Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter, bool isGood)
    {
        lock (_lock)
        {
            var battleboard = snapshot.Battleboards.Find(s => s.Id == battleboardCharacter.BattleboardId)!;
            var character = snapshot.Players.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.Id)!;
            character.Status.Gameplay.BattleboardId = battleboard.Id;
            character.Status.Gameplay.IsBattleboardGoodGuy = isGood;

            if (isGood)
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




    public void LeaveBattleboard(BattleboardCharacter battleboardCharacter)
    {
        lock (_lock)
        {
            var battleboard = snapshot.Battleboards.Find(s => s.Id == battleboardCharacter.BattleboardId)!;

            var character = snapshot.Players.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == battleboardCharacter.CharacterIdentity.Id)!;
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
