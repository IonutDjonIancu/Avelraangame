namespace Tests;

public class GameplayTests : TestBase
{
    [Fact(DisplayName = "Generate location on visit should exist in snapshot")]
    public void GenerateLocation()
    {
        var location = _gameplay.GetLocation(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.Position);

        _snapshot.Locations.Count.Should().Be(1);
        location.Position.Should().Be(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.Position);
        location.TravelCostFromArada.Should().BeGreaterThanOrEqualTo(1);
        location.Market.Count.Should().BeGreaterThanOrEqualTo(1);
        location.Mercenaries.Count.Should().BeGreaterThanOrEqualTo(1);
        location.Effort.Should().BeLessThanOrEqualTo(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.Effort);
        location.PossibleQuests.Count.Should().BeGreaterThanOrEqualTo(1);
        location.Description.Should().NotBeNullOrWhiteSpace();
        location.FullName.Should().Be(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.FullName);
        location.LastTimeVisited.Should().Be(DateTime.Now.ToShortDateString());
    }


}
