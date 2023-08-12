namespace Tests;

public class GameplayServiceTests : TestBase
{
    [Fact(DisplayName = "Generate location on visit")]
    public void Visit_location_must_generate_content_test()
    {
        var locFullName = GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.FullName;

        gameplayService.GetLocation(Utils.GetPositionByFullName(locFullName));

        dbs.Snapshot.Locations.Count.Should().Be(1);
        dbs.Snapshot.Locations.First().FullName.Should().Be(locFullName);
    }

}