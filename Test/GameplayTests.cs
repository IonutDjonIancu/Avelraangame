namespace Tests;

[Collection("GameplayTests")]
[Trait("Category", "GameplayServiceTests")]
public class GameplayTests : TestBase
{
    [Fact(DisplayName = "Generate location on visit should exist in snapshot")]
    public void GenerateLocationTest()
    {
        var location = _gameplay.GetOrGenerateLocation(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.Position);

        _snapshot.Locations.Count.Should().Be(1);
        location.Position.Should().Be(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.Position);
        location.TravelCostFromArada.Should().BeGreaterThanOrEqualTo(1);
        location.Market.Count.Should().BeGreaterThanOrEqualTo(1);
        location.Mercenaries.Count.Should().BeGreaterThanOrEqualTo(1);
        location.Effort.Should().BeLessThanOrEqualTo(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.Effort);
        //location.PossibleQuests.Count.Should().BeGreaterThanOrEqualTo(1);
        location.Description.Should().NotBeNullOrWhiteSpace();
        location.FullName.Should().Be(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.FullName);
        location.LastTimeVisited.Should().Be(DateTime.Now.ToShortDateString());
    }

    [Fact(DisplayName = "Get ladder should return characters ordered by wealth or worth")]
    public void GetLadderTest()
    {
        var char1name = "Char1";
        var player1name = "Player1";
        var char2name = "Char2";
        var player2name = "Player2";
        var char3name = "Char3";
        var player3name = "Player3";
        var char4name = "Char4";
        var player4name = "Player4";

        var char1 = TestUtils.CreateAndGetCharacter(player1name, _players, _characters, _snapshot);
        char1.Status.Name = char1name;
        char1.Status.Wealth = 100;
        char1.Status.Worth = 100;
        
        var char2 = TestUtils.CreateAndGetCharacter(player2name, _players, _characters, _snapshot);
        char2.Status.Name = char2name;
        char2.Status.Wealth = 200;
        char2.Status.Worth = 200;
        
        var char3 = TestUtils.CreateAndGetCharacter(player3name, _players, _characters, _snapshot);
        char3.Status.Name = char3name;
        char3.Status.Wealth = 300;
        char3.Status.Worth = 300;
        
        var char4 = TestUtils.CreateAndGetCharacter(player4name, _players, _characters, _snapshot);
        char4.Status.Name = char4name;
        char4.Status.Wealth = 400;
        char4.Status.Worth = 400;

        var ladder = _gameplay.GetLadder();

        ladder.CharactersByWealth.Count.Should().Be(4);
        ladder.CharactersByWorth.Count.Should().Be(4);

        ladder.CharactersByWealth[0].CharacterName.Should().Be(char4name);
        ladder.CharactersByWealth[0].PlayerName.Should().Be(player4name);
        ladder.CharactersByWealth[0].Wealth.Should().Be(400);
        ladder.CharactersByWealth[0].Worth.Should().Be(400);

        ladder.CharactersByWealth[1].CharacterName.Should().Be(char3name);
        ladder.CharactersByWealth[2].CharacterName.Should().Be(char2name);
        ladder.CharactersByWealth[3].CharacterName.Should().Be(char1name);
    }


}
