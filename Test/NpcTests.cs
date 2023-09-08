namespace Tests;

[Collection("NpcTests")]
[Trait("Category", "NpcServiceTests")]
public class NpcTests: TestBase
{
    [Fact(DisplayName = "Generating good guy Npc race should belong to playable")]
    public void GenerateGoodGuyTest()
    {
        var goodGuy = _npcs.GenerateGoodGuy(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.LocationName);

        goodGuy.Identity.PlayerId.Should().Be(Guid.Empty.ToString());
        goodGuy.Status.IsNpc.Should().BeTrue();
        goodGuy.Status.IsLockedToModify.Should().BeFalse();
        goodGuy.Status.Position.Should().Be(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.Position);

        CharactersLore.Races.Playable.All.Should().Contain(goodGuy.Status.Traits.Race);
    }

    [Fact(DisplayName = "Generating bad guy Npc race should belong to non-playable")]
    public void GenerateBadGuyTest()
    {
        var badGuy = _npcs.GenerateBadGuy(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.LocationName);

        badGuy.Identity.PlayerId.Should().Be(Guid.Empty.ToString());
        badGuy.Status.IsNpc.Should().BeTrue();
        badGuy.Status.IsLockedToModify.Should().BeFalse();
        badGuy.Status.Position.Should().Be(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.Position);

        var listOfRaces = new List<string>();
        listOfRaces.AddRange(CharactersLore.Races.Playable.All);
        listOfRaces.AddRange(CharactersLore.Races.NonPlayable.All);

        listOfRaces.Should().Contain(badGuy.Status.Traits.Race);
    }
}
