namespace Tests;

[Collection("DiceTests")]
[Trait("Category", "DiceServiceTests")]
public class DiceTests : TestBase
{
    [Fact(DisplayName = "Roll a number between 1 and 20")]
    public void RollD20Test()
    {
        var roll = _dice.Roll_d20_noReroll();

        roll.Should().BeGreaterThanOrEqualTo(1);
        roll.Should().BeLessThan(21);
    }

    [Fact(DisplayName = "Roll a number between 1 and 20 with reroll")]
    public void RollD20RerollTest()
    {
        var roll = _dice.Roll_d20_withReroll();

        roll.Should().BeGreaterThanOrEqualTo(1);
        roll.Should().BeLessThan(101);
    }

    [Fact(DisplayName = "Roll a number between 1 and 100")]
    public void RollD100Test()
    {
        var roll = _dice.Roll_d100_noReroll();

        roll.Should().BeGreaterThanOrEqualTo(1);
        roll.Should().BeLessThan(101);
    }

    [Fact(DisplayName = "Roll a number between 1 and 100 with reroll")]
    public void RollD100RerollTest()
    {
        var roll = _dice.Roll_d100_withReroll();

        roll.Should().BeGreaterThanOrEqualTo(1);
        roll.Should().BeLessThan(301);
    }

    [Fact(DisplayName = "Roll a number between 1 and a set limit")]
    public void Roll1ToNTest()
    {
        var limit = 50;
        var roll = _dice.Roll_1_to_n(limit);

        roll.Should().BeGreaterThanOrEqualTo(1);
        roll.Should().BeLessThan(limit + 1);
    }

    [Fact(DisplayName = "Roll a number between a lower limit and an upper limit")]
    public void RollNtoNTest()
    {
        var lowerLimit = 25;
        var upperLimit = 45;

        var roll = _dice.Roll_n_to_n(lowerLimit, upperLimit);

        roll.Should().BeGreaterThanOrEqualTo(lowerLimit);
        roll.Should().BeLessThan(upperLimit);
    }

    [Fact(DisplayName = "Roll character dice")]
    public void RollCharacterDiceTest()
    {
        var character = TestUtils.CreateAndGetCharacter("playerName", _players, _characters, _snapshot);

        var roll = _dice.Roll_character_gameplay_dice(true, CharactersLore.Skills.Melee, character);

        roll.Should().BeGreaterThanOrEqualTo(1);
    }
}
