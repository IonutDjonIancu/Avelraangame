using Data_Mapping_Containers.Dtos;

namespace Tests;

[Collection("PlayerTests")]
[Trait("Category", "PlayerServiceTests")]
public class PlayerTests : TestBase
{
    [Fact(DisplayName = "Create player should exist in snapshot")]
    public void CreatePlayerTest()
    {
        // Arrange
        var playerData = new PlayerData
        {
            Name = "Joe"
        };

        // Act
        var result = _players.CreatePlayer(playerData);

        // Assert
        result.Should().NotBeNull();
        result.SetupCode.Length.Should().BeGreaterThan(0);
        result.SetupImage.Length.Should().BeGreaterThan(0);
        _snapshot.Players.Should().HaveCount(1);
    }

    [Fact(DisplayName = "Creating player with existing name should throw")]
    public void DuplicatePlayerNameTest()
    {
        var playerData = new PlayerData
        {
            Name = "Joe"
        };

        _players.CreatePlayer(playerData);
        Assert.Throws<Exception>(() => _players.CreatePlayer(playerData));
    }

    [Fact(DisplayName = "Creating more players than the limit should throw")]
    public void MaxPlayersReachedTest()
    {
        for (int i = 0; i < 20; i++)
        {
            var playerData = new PlayerData
            {
                Name = $"aaa{i}"
            };
            var playerName = $"aaa{i}";
            _players.CreatePlayer(playerData);
        }

        Assert.Throws<Exception>(() => _players.CreatePlayer(new PlayerData { Name = "just another player" }));
    }

    [Theory(DisplayName = "Wrong player name should throw")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void WrongPlayerNameTest(string name)
    {
        var playerData = new PlayerData
        {
            Name = name
        };

        Assert.Throws<Exception>(() => _players.CreatePlayer(playerData));
    }
}
