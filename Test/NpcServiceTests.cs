namespace Tests;

public class NpcServiceTests : TestBase
{
    [Fact(DisplayName = "Create good guy npc")]
    public void Generate_good_guy_npc_test()
    {
        var location = GameplayLore.Map.All.First();

        var position = Utils.GetLocationPosition(location.FullName);
        var npc = npcService.GenerateGoodGuyNpc(position, location.Effort);

        Assert.NotNull(npc);
        npc.Worth.Should().BeGreaterThan(0);
        npc.Info.IsNpc.Should().BeTrue();
        npc.Info.IsAlive.Should().BeTrue();
    }

    [Fact(DisplayName = "Create bad guy npc")]
    public void Generate_bad_guy_npc_test()
    {
        var location = GameplayLore.Map.All.First();

        var position = Utils.GetLocationPosition(location.FullName);
        var npc = npcService.GenerateBadGuyNpc(position, location.Effort);

        Assert.NotNull(npc);
        npc.Worth.Should().Be(0);
        npc.Info.Wealth.Should().BeGreaterThan(0);
        npc.Info.IsNpc.Should().BeTrue();
        npc.Info.IsAlive.Should().BeTrue();
    }
}
