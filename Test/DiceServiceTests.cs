namespace Tests;

public class DiceServiceTests : TestBase
{
    [Fact(DisplayName = "Roll a number between 1 and 20.")]
    public void Roll_simple_d20_test()
    {
        var roll = diceService.Roll_20_noReroll();

        roll.Should().BeLessThan(21);
    }

    [Fact(DisplayName = "Roll a number between 1 and 20, with a reroll.")]
    public void Roll_complex_d20_test()
    {
        var roll = diceService.Roll_20_withReroll();

        roll.Should().BeLessThan(101);
    }

    [Fact(DisplayName = "Roll a number between 1 and 100.")]
    public void Roll_simple_d100_test()
    {
        var roll = diceService.Roll_100_noReroll();

        roll.Should().BeLessThan(101);
    }

    [Fact(DisplayName = "Roll a number between 1 and 100, with a reroll.")]
    public void Roll_complex_d100_test()
    {
        var roll = diceService.Roll_100_withReroll();

        roll.Should().BeLessThan(1000);
    }

    [Fact(DisplayName = "Roll gameplay dice for martial.")]
    public void Roll_martial_gameplay_dice_test()
    {
        var (grade, crits) = diceService.Roll_gameplay_dice(GameplayLore.Tradition.Martial, 100);

        grade.Should().BeGreaterThanOrEqualTo(1);
        crits.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact(DisplayName = "Roll gameplay dice for common.")]
    public void Roll_common_gameplay_dice_test()
    {
        var (grade, crits) = diceService.Roll_gameplay_dice(GameplayLore.Tradition.Common, 100);

        grade.Should().BeGreaterThanOrEqualTo(1);
        crits.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact(DisplayName = "Roll 1 to n.")]
    public void Roll_1_to_n_test()
    {
        var roll = diceService.Roll_1_to_n(100);

        roll.Should().BeLessThanOrEqualTo(100);
    }

    [Fact(DisplayName = "Roll n to n.")]
    public void Roll_n_to_n_test()
    {
        var roll = diceService.Roll_n_to_n(50, 100);

        roll.Should().BeGreaterThanOrEqualTo(50);
        roll.Should().BeLessThanOrEqualTo(100);
    }
}
