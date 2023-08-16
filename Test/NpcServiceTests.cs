namespace Tests;

public class NpcServiceTests : TestBase
{
    [Fact(DisplayName = "Create good guy npc")]
    public void Generate_good_guy_npc_test()
    {
        var location = GameplayLore.Locations.All.First();

        var position = Utils.GetPositionByLocationFullName(location.FullName);
        var npc = npcService.GenerateGoodGuyNpc(position.Location);

        Assert.NotNull(npc);
        npc.Status.Worth.Should().BeGreaterThan(0);
        npc.Status.IsNpc.Should().BeTrue();
        npc.Status.IsAlive.Should().BeTrue();
        CharactersLore.Races.Playable.All.Should().Contain(npc.Status.Traits.Race);
        CharactersLore.Races.NonPlayable.All.Should().NotContain(npc.Status.Traits.Race);
    }

    [Fact(DisplayName = "Create bad guy npc")]
    public void Generate_bad_guy_npc_test()
    {
        var location = GameplayLore.Locations.All.First();

        var position = Utils.GetPositionByLocationFullName(location.FullName);
        var npc = npcService.GenerateBadGuyNpc(position.Location);

        Assert.NotNull(npc);
        CharactersLore.Races.NonPlayable.All.Should().Contain(npc.Status.Traits.Race);
        CharactersLore.Races.Playable.All.Should().NotContain(npc.Status.Traits.Race);
    }
}
