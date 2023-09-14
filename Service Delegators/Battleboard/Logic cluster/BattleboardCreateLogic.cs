using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IBattleboardCreateLogic
{
    Battleboard CreateBattleboard(CharacterIdentity partyLeadIdentity);
    Battleboard JoinBattleboard(BattleboardCharacter battleboardCharacter, bool isGood);
    void LeaveBattleboard(BattleboardCharacter battleboardCharacter);
}

public class BattleboardCreateLogic : IBattleboardCreateLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;

    public BattleboardCreateLogic(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public Battleboard CreateBattleboard(CharacterIdentity partyLeadIdentity)
    {
        lock (_lock)
        {
            var battleboard = new Battleboard
            {
                Id = Guid.NewGuid().ToString(),
                IsLocked = false
            };

            var character = snapshot.Players.Find(s => s.Identity.Id == partyLeadIdentity.PlayerId)!.Characters.Find(s => s.Identity.Id == partyLeadIdentity.Id)!;
            character.Status.Gameplay.BattleboardId = battleboard.Id;

            battleboard.GoodGuys.PartyLeadId = character.Identity.PlayerId;
            battleboard.GoodGuys.Characters.Add(character);

            character.Mercenaries.ForEach(s => battleboard.GoodGuys.Characters.Add(s));

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

            if (isGood)
            {
                var partyLeadCharacter = battleboard.GoodGuys.Characters.Find(s => s.Identity.Id == battleboard.GoodGuys.PartyLeadId)!;
                if (character.Status.Worth > partyLeadCharacter.Status.Worth) battleboard.GoodGuys.PartyLeadId = character.Identity.Id;

                battleboard.GoodGuys.Characters.Add(character);
                character.Mercenaries.ForEach(s => battleboard.GoodGuys.Characters.Add(s));
            }
            else
            {
                var partyLeadCharacter = battleboard.BadGuys.Characters.Find(s => s.Identity.Id == battleboard.BadGuys.PartyLeadId)!;
                if (character.Status.Worth > partyLeadCharacter.Status.Worth) battleboard.BadGuys.PartyLeadId = character.Identity.Id;

                battleboard.BadGuys.Characters.Add(character);
                character.Mercenaries.ForEach(s => battleboard.BadGuys.Characters.Add(s));
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

            bool isGoodGuy = battleboard.GoodGuys.Characters.Exists(s => s.Identity.Id == character.Identity.Id);

            if (isGoodGuy)
            {
                battleboard.GoodGuys.Characters.Remove(character);
                battleboard.GoodGuys.BattleFormation.Remove(character.Identity.Id);

                if (battleboard.GoodGuys.PartyLeadId == character.Identity.Id)
                    battleboard.GoodGuys.PartyLeadId = battleboard.GoodGuys.Characters.OrderByDescending(s => s.Status.Worth).First().Identity.Id;
            }
            else
            {
                battleboard.BadGuys.Characters.Remove(character);
                battleboard.BadGuys.BattleFormation.Remove(character.Identity.Id);

                if (battleboard.BadGuys.PartyLeadId == character.Identity.Id)
                    battleboard.BadGuys.PartyLeadId = battleboard.BadGuys.Characters.OrderByDescending(s => s.Status.Worth).First().Identity.Id;
            }

            battleboard.BattleOrder.Remove(character.Identity.Id);
        }
    }
}
