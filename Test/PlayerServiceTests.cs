namespace Tests;

public class PlayerServiceTests : TestBase
{
    [Theory]
    [Description("Create player should exist in db.")]
    public void Create_player_test()
    {
        dbs.Snapshot.Players.Clear();
        
        var playerName = "john";

        var playerAuth = playerService.CreatePlayer(playerName);

        playerAuth.Should().NotBeNull();
        playerAuth.SetupCode.Length.Should().BeGreaterThan(0);
        playerAuth.SetupImage.Length.Should().BeGreaterThan(0);

        dbs.Snapshot.Players.Count.Should().Be(1);
    }

    [Theory]
    [Description("Wrong player name should throw.")]
    public void Create_player_with_wrong_name_test()
    {
        dbs.Snapshot.Players.Clear();

        var playerNameEmpty = " ";
        var playerNameTooLong = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        Assert.Throws<Exception>(() => playerService.CreatePlayer(playerNameEmpty));
        Assert.Throws<Exception>(() => playerService.CreatePlayer(playerNameTooLong));
        dbs.Snapshot.Players.Count.Should().Be(0);
    }

    [Theory]
    [Description("Same player name should throw.")]
    public void Create_player_with_same_name_test()
    {
        dbs.Snapshot.Players.Clear();

        var playerName = "aaa";

        playerService.CreatePlayer(playerName);
        dbs.Snapshot.Players.Count.Should().Be(1);
        Assert.Throws<Exception>(() => playerService.CreatePlayer(playerName));
    }

    [Theory]
    [Description("Creating more players than the limit should throw.")]
    public void Create_too_many_players_test()
    {
        dbs.Snapshot.Players.Clear();

        var index = 1;

        for (int i = 0; i < 20; i++)
        {
            var playerName = $"aaa{index}";
            playerService.CreatePlayer(playerName);
            index++;
        }

        Assert.Throws<Exception>(() => playerService.CreatePlayer("bbb"));
    }
}
