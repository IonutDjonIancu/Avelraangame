﻿namespace Tests;

[Collection("BattleboardTests")]
[Trait("Category", "BattleboardServiceTests")]
public class BattleboardTests : TestBase
{
    private const string PlayerName1 = "Test Player 1";

    [Fact(DisplayName = "Create battleboard should exist in snapshot.")]
    public void CreateBattleboard()
    {
        var actor = CreateBattleboardActor(PlayerName1);

        var board = _battleboard.CreateBattleboard(actor);

        board.Should().NotBeNull();
        board.GoodGuyPartyLead.Should().Be(actor.MainActor.Id);
        _snapshot.Battleboards.Count.Should().Be(1);
    }

    [Fact(DisplayName = "Getting the battleboards should return a list of all battleboards.")]
    public void GetBattleboardsTest()
    {
        _battleboard.CreateBattleboard(CreateBattleboardActor(PlayerName1));

        var boards = _battleboard.GetBattleboards();

        boards.Count.Should().Be(1);
    }

    [Fact(DisplayName = "Find battleboard should return the exact battleboard.")]
    public void FindBattleboardTest()
    {
        var actor = CreateBattleboardActor(PlayerName1);

        _battleboard.CreateBattleboard(actor);

        var character = TestUtils.GetCharacter(actor.MainActor.Id, actor.MainActor.PlayerId, _snapshot);

        var battleboard = _battleboard.FindBattleboard(character.Status.Gameplay.BattleboardId);

        battleboard.Should().NotBeNull();
    }

    [Fact(DisplayName = "Find character battleboard should return the exact battleboard.")]
    public void FindCharacterBattleboardTest()
    {
        var actor = CreateBattleboardActor(PlayerName1);

        _battleboard.CreateBattleboard(actor);

        var battleboard = _battleboard.FindCharacterBattleboard(actor);

        battleboard.Should().NotBeNull();
        battleboard.GetAllCharacters().Select(s => s.Identity.Id).ToList().Should().Contain(actor.MainActor.Id);
    }

    [Fact(DisplayName = "Joining a battleboard should be good or bad guy in battleboard.")]
    public void JoinBattleboardTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = true;
        actor2.BattleboardIdToJoin = board.Id;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        actor2char.Status.Worth = 1000;
        _battleboard.JoinBattleboard(actor2);

        var actor3 = CreateBattleboardActor("player 3");
        actor3.WantsToBeGood = false;
        actor3.BattleboardIdToJoin = board.Id;
        _battleboard.JoinBattleboard(actor3);

        board.GoodGuys.Count.Should().Be(2);
        board.GoodGuyPartyLead.Should().Be(actor2.MainActor.Id);
        board.BadGuys.Count.Should().Be(1);
        board.BadGuyPartyLead.Should().Be(actor3.MainActor.Id);
    }

    [Fact(DisplayName = "Being kicked from a battleboard should remove character from battleboard.")]
    public void KickFromBattleboardTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = true;
        actor2.BattleboardIdToJoin = board.Id;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        actor2char.Status.Worth = 0;
        _battleboard.JoinBattleboard(actor2);

        board.GoodGuyPartyLead.Should().Be(actor1.MainActor.Id);

        actor1.TargetId = actor2.MainActor.Id;
        _battleboard.KickFromBattleboard(actor1);

        board.GoodGuys.Count.Should().Be(1);
        actor2char.Status.Gameplay.BattleboardId.Should().Be(string.Empty);
    }

    [Fact(DisplayName = "Trying to kick an enemy player from a battleboard should throw.")]
    public void KickEnemyFromBattleboardTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = false;
        actor2.BattleboardIdToJoin = board.Id;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        _battleboard.JoinBattleboard(actor2);

        actor1.TargetId = actor2.MainActor.Id;

        Assert.Throws<Exception>(() => _battleboard.KickFromBattleboard(actor1));
        actor2char.Status.Gameplay.BattleboardId.Should().Be(board.Id);
    }

    [Fact(DisplayName = "Kicking a player when you are both evil from a battleboard should remove character from battleboard.")]
    public void KickEvilFromBattleboardTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = false;
        actor2.BattleboardIdToJoin = board.Id;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        actor2char.Status.Worth = 1000;
        _battleboard.JoinBattleboard(actor2);

        var actor3 = CreateBattleboardActor("player 3");
        actor3.WantsToBeGood = false;
        actor3.BattleboardIdToJoin = board.Id;
        _battleboard.JoinBattleboard(actor3);

        actor2.TargetId = actor3.MainActor.Id;

        _battleboard.KickFromBattleboard(actor2);

        board.BadGuys.Count.Should().Be(1);
        board.BadGuys.First().Identity.Id.Should().Be(actor2.MainActor.Id);
    }

    [Fact(DisplayName = "Only party leads can perform these actions.")]
    public void ActionRestrictedToPartyLeadsTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var actor1char = TestUtils.GetCharacter(actor1.MainActor.Id, actor1.MainActor.PlayerId, _snapshot);
        actor1char.Status.Worth = 1000;
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = true;
        actor2.BattleboardIdToJoin = board.Id;
        _battleboard.JoinBattleboard(actor2);

        var actor3 = CreateBattleboardActor("player 3");
        var actor3char = TestUtils.GetCharacter(actor3.MainActor.Id, actor3.MainActor.PlayerId, _snapshot);
        actor3char.Status.Worth = 1000;
        actor3.WantsToBeGood = false;
        actor3.BattleboardIdToJoin = board.Id;
        _battleboard.JoinBattleboard(actor3);

        var actor4 = CreateBattleboardActor("player 4");
        actor4.WantsToBeGood = false;
        actor4.BattleboardIdToJoin = board.Id;
        _battleboard.JoinBattleboard(actor4);

        actor4.TargetId = actor3.MainActor.Id;
        Assert.Throws<Exception>(() => _battleboard.KickFromBattleboard(actor4));
    }

    [Fact(DisplayName = "Leaving a battleboard should remove character from battleboard.")]
    public void LeaveBattleboardTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var actor1char = TestUtils.GetCharacter(actor1.MainActor.Id, actor1.MainActor.PlayerId, _snapshot);
        actor1char.Status.Worth = 1000;
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = true;
        actor2.BattleboardIdToJoin = board.Id;
        _battleboard.JoinBattleboard(actor2);

        var actor3 = CreateBattleboardActor("player 3");
        var actor3char = TestUtils.GetCharacter(actor3.MainActor.Id, actor3.MainActor.PlayerId, _snapshot);
        actor3char.Status.Worth = 1000;
        actor3.WantsToBeGood = false;
        actor3.BattleboardIdToJoin = board.Id;
        _battleboard.JoinBattleboard(actor3);

        var actor4 = CreateBattleboardActor("player 4");
        actor4.WantsToBeGood = false;
        actor4.BattleboardIdToJoin = board.Id;
        _battleboard.JoinBattleboard(actor4);

        _battleboard.LeaveBattleboard(actor2);
        board.GoodGuys.Count.Should().Be(1);
        board.GoodGuys.Select(s => s.Identity.Id).Should().NotContain(actor2.MainActor.Id);

        _battleboard.LeaveBattleboard(actor4);
        board.BadGuys.Count.Should().Be(1);
        board.BadGuys.Select(s => s.Identity.Id).Should().NotContain(actor4.MainActor.Id);

        _battleboard.LeaveBattleboard(actor1);
        board.GoodGuys.Count.Should().Be(0);
        board.GoodGuys.Select(s => s.Identity.Id).Should().NotContain(actor2.MainActor.Id);

        _battleboard.LeaveBattleboard(actor3);
        board.BadGuys.Count.Should().Be(0);
        board.BadGuys.Select(s => s.Identity.Id).Should().NotContain(actor3.MainActor.Id);

        _snapshot.Battleboards.Count.Should().Be(0);
    }

    [Fact(DisplayName = "Joining a battleboard brings your mercs with you.")]
    public void JoinBattleboardWithMercsTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var actor1char = TestUtils.GetCharacter(actor1.MainActor.Id, actor1.MainActor.PlayerId, _snapshot);
        actor1char.Status.Worth = 10000;
        actor1char.Status.Wealth = 10000;
        
        var location = _snapshot.Locations.Find(s => s.FullName == actor1char.Status.Position.GetPositionFullName())!;

        var hire = new CharacterHireMercenary
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = actor1.MainActor.Id,
                PlayerId = actor1.MainActor.PlayerId,
            },
            MercenaryId = location.Mercenaries.First().Identity.Id
        };

        _characters.HireMercenaryForCharacter(hire);

        var board = _battleboard.CreateBattleboard(actor1);
        board.GoodGuys.Select(s => s.Identity.Id).Should().Contain(actor1char.Mercenaries.First().Identity.Id);
    }

    [Fact(DisplayName = "Leaving a battleboard takes your mercs with you.")]
    public void LeaveBattleboardWithMercsTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.BattleboardIdToJoin = board.Id;
        actor2.WantsToBeGood = true;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        actor2char.Status.Worth = 10000;
        actor2char.Status.Wealth = 10000;

        var location = _snapshot.Locations.Find(s => s.FullName == actor2char.Status.Position.GetPositionFullName())!;

        var hire = new CharacterHireMercenary
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = actor2.MainActor.Id,
                PlayerId = actor2.MainActor.PlayerId,
            },
            MercenaryId = location.Mercenaries.First().Identity.Id
        };
        _characters.HireMercenaryForCharacter(hire);

        _battleboard.JoinBattleboard(actor2);
        board.GoodGuys.Select(s => s.Identity.Id).Should().Contain(actor2char.Mercenaries.First().Identity.Id);

        _battleboard.LeaveBattleboard(actor2);
        board.GoodGuys.Select(s => s.Identity.Id).Should().NotContain(actor2char.Mercenaries.First().Identity.Id);
    }

    [Fact(DisplayName = "Starting combat should lock all characters on battleboard.")]
    public void BattleboardStartCombatTest()
    {
        var board = CreateBattleboardWithCombatants();
        var location = _snapshot.Locations.Find(s => s.Name == board.GoodGuys.First().Status.Position.Location)!;

        _battleboard.StartCombat(board.Id);

        board.GetAllCharacters().Count.Should().Be(6);
        board.IsInCombat.Should().BeTrue();
        board.CanLvlUp.Should().BeTrue();
        board.RoundNr.Should().Be(1);
        board.EffortLvl.Should().Be(location.Effort);
        board.GetAllCharacters().Select(s => s.Status.Gameplay.IsLocked).ToList().Should().NotContain(false);
        board.BattleOrder.Count.Should().Be(6);
        board.LastActionResult.Length.Should().BeGreaterThan(10);
    }

    [Fact(DisplayName = "Attacking in combat should display correctly.")]
    public void BattleboardAttackTest()
    {
        var board = CreateBattleboardWithCombatants();
        var location = _snapshot.Locations.Find(s => s.Name == board.GoodGuys.First().Status.Position.Location)!;
        
        _battleboard.StartCombat(board.Id);

        var attacker = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;
        Character defender;
        if (attacker.Status.Gameplay.IsGoodGuy)
        {
            defender = board.BadGuys.First();
        } 
        else
        {
            defender = board.GoodGuys.First();
        }

        attacker.Sheet.Skills.Melee = 1000;
        defender.Sheet.Skills.Melee = 10;
        var attackerInitialResolve = attacker.Sheet.Assets.ResolveLeft;
        var defenderInitialResolve = defender.Sheet.Assets.ResolveLeft;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = attacker.Identity.Id,
                PlayerId = attacker.Identity.PlayerId
            },
            TargetId = defender.Identity.Id
        };
        _battleboard.Attack(actor);

        attacker.Sheet.Assets.ResolveLeft.Should().BeLessThan(attackerInitialResolve);
        defender.Sheet.Assets.ResolveLeft.Should().BeLessThan(defenderInitialResolve);



    }











    #region private methods
    private Battleboard CreateBattleboardWithCombatants()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var actor1char = TestUtils.GetCharacter(actor1.MainActor.Id, actor1.MainActor.PlayerId, _snapshot);
        actor1char.Status.Worth = 10000;
        actor1char.Status.Wealth = 10000;

        var board = _battleboard.CreateBattleboard(actor1);

        var location = _snapshot.Locations.Find(s => s.FullName == actor1char.Status.Position.GetPositionFullName())!;
        var merc1 = _npcs.GenerateGoodGuy(location.Name);
        var merc2 = _npcs.GenerateGoodGuy(location.Name);
        location.Mercenaries.Clear();
        location.Mercenaries.Add(merc1);
        location.Mercenaries.Add(merc2);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.BattleboardIdToJoin = board.Id;
        actor2.WantsToBeGood = true;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        actor2char.Status.Wealth = 10000;

        var hireFor2 = new CharacterHireMercenary
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = actor2.MainActor.Id,
                PlayerId = actor2.MainActor.PlayerId,
            },
            MercenaryId = location.Mercenaries.First().Identity.Id
        };
        _characters.HireMercenaryForCharacter(hireFor2);
        _battleboard.JoinBattleboard(actor2);

        var actor3 = CreateBattleboardActor("player 3");
        actor3.BattleboardIdToJoin = board.Id;
        actor3.WantsToBeGood = false;
        var actor3char = TestUtils.GetCharacter(actor3.MainActor.Id, actor3.MainActor.PlayerId, _snapshot);
        actor3char.Status.Worth = 10000;
        actor3char.Status.Wealth = 10000;
        _battleboard.JoinBattleboard(actor3);

        var actor4 = CreateBattleboardActor("player 4");
        actor4.BattleboardIdToJoin = board.Id;
        actor4.WantsToBeGood = false;
        var actor4char = TestUtils.GetCharacter(actor4.MainActor.Id, actor4.MainActor.PlayerId, _snapshot);
        actor4char.Status.Wealth = 10000;

        var hireFor4 = new CharacterHireMercenary
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = actor4.MainActor.Id,
                PlayerId = actor4.MainActor.PlayerId,
            },
            MercenaryId = location.Mercenaries.First().Identity.Id
        };
        _characters.HireMercenaryForCharacter(hireFor4);
        _battleboard.JoinBattleboard(actor4);

        return board;
    }

    private BattleboardActor CreateBattleboardActor(string playerName)
    {
        var character = TestUtils.CreateAndGetCharacter(playerName, _players, _characters, _snapshot);

        return new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = character.Identity.Id,
                PlayerId = character.Identity.PlayerId,
            },
        };
    }
    #endregion
}
