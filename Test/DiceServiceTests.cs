namespace Tests;

public class DiceServiceTests : TestBase
{
    [Theory]
    [Description("Roll 20 should return a DiceRoll object")]
    public void Roll20_diceRoll_object_should_have_properties()
    {
        var roll = diceService.Roll_d20_NoHeritage();

        roll.Should().NotBeNull();
        roll.Roll.Should().BeGreaterThan(0);
        roll.Dice?.Count.Should().BeGreaterThan(0);
        roll.Grade.Should().BeGreaterThan(0);
    }

    [Theory]
    [Description("Danarian roll should have a list of dice smaller than 21")]
    public void Roll20_Danarian_test()
    {
        var roll = diceService.Roll_d20_Traditional();

        roll.Dice.Should().NotBeNull();

        foreach (var item in roll.Dice!)
        {
            item.Should().BeLessOrEqualTo(20);
        }
    }

    [Theory]
    [Description("Calvinian roll should have a list of dice smaller than 101")]
    public void Roll20_Calvinian_test()
    {
        var roll = diceService.Roll_d20_Martial();

        roll.Dice.Should().NotBeNull();

        foreach (var item in roll.Dice!)
        {
            item.Should().BeLessOrEqualTo(100);
        }
    }
}
