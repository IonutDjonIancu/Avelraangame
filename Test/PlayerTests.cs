namespace Tests;

[Collection("PlayerTests")]
[Trait("Category", "PlayerServiceTests")]
public class PlayerTests : TestBase
{
    [Fact(DisplayName = "Create player should exist in snapshot")]
    public void CreatePlayerTest()
    {
        // Arrange
        var playerName = "JoeDoe";

        // Act
        var result = _players.CreatePlayer(playerName);

        // Assert
        result.Should().NotBeNull();
        result.SetupCode.Length.Should().BeGreaterThan(0);
        result.SetupImage.Length.Should().BeGreaterThan(0);
        _snapshot.Players.Should().HaveCount(1);
    }

    [Fact(DisplayName = "Creating player with existing name should throw")]
    public void DuplicatePlayerNameTest()
    {
        _players.CreatePlayer("John");
        Assert.Throws<Exception>(() => _players.CreatePlayer("John"));
    }

    [Fact(DisplayName = "Creating more players than the limit should throw")]
    public void MaxPlayersReachedTest()
    {
        for (int i = 0; i < 20; i++)
        {
            var playerName = $"aaa{i}";
            _players.CreatePlayer(playerName);
        }

        Assert.Throws<Exception>(() => _players.CreatePlayer("John"));
    }

    [Theory(DisplayName = "Wrong player name should throw")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void WrongPlayerNameTest(string name)
    {
        Assert.Throws<Exception>(() => _players.CreatePlayer(name));
    }
}
