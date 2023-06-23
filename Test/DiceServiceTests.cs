namespace Tests;

public class DiceServiceTests : TestBase
{
    [Fact(DisplayName = "Roll 20 should return a DiceRoll object")]
    public void Roll20_diceRoll_object_should_have_properties()
    {
        var roll = diceService.Roll_d20();

        roll.Should().BeGreaterThan(0);
    }

    [Fact(DisplayName = "Common roll should have a list of dice smaller than 101")]
    public void Roll20_Danarian_test()
    {
        var roll = diceService.Roll_d20_Common();

        roll.Dice.Should().NotBeNull();

        foreach (var item in roll.Dice!)
        {
            item.Should().BeLessOrEqualTo(100);
        }
    }

    [Fact(DisplayName = "Martial roll should have a list of dice smaller than 21")]
    public void Roll20_Calvinian_test()
    {
        var roll = diceService.Roll_d20_Martial();

        roll.Dice.Should().NotBeNull();

        foreach (var item in roll.Dice!)
        {
            item.Should().BeLessOrEqualTo(20);
        }
    }
}
