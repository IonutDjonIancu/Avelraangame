namespace Tests;

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




    #region private methods
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
