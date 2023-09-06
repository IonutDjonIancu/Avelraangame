namespace Tests;

public class PlayersTests : TestBase
{
    public PlayersTests() : base()
    {
    }

    [Fact]
    public void CreatePlayerTest()
    {
        // Use _playerLogicDelegator in your test
        var result = _playerLogicDelegator.CreatePlayer("John");

        // Assertions and test logic
        result.Should().NotBeNull();
    }
}
