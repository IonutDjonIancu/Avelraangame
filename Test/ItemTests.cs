namespace Tests;

[Collection("ItemTests")]
[Trait("Category", "ItemServiceTests")]
public class ItemTests : TestBase
{
    [Fact(DisplayName = "Generate random item returns the item")] 
    public void GenerateRandomItemTest()
    {
        Item item = _items.GenerateRandomItem();

        if (item.Subcategory == ItemsLore.Subcategories.Tradeable)
        {
            item = _items.GenerateRandomItem();
        }

        item.Should().NotBeNull();
        item.Identity.Should().NotBeNull();
        item.Name.Should().NotBeNullOrWhiteSpace();
        item.Level.Should().BeGreaterThanOrEqualTo(1);
        item.LevelName.Should().NotBeNullOrWhiteSpace();

        ItemsLore.Types.All.Should().Contain(item.Type);
        ItemsLore.Subtypes.All.Should().Contain(item.Subtype);
        ItemsLore.Subcategories.All.Should().Contain(item.Subcategory);
        item.Quality.Should().NotBeNullOrWhiteSpace();

        item.InventoryLocations.Count.Should().BeGreaterThanOrEqualTo(1);
        item.Description.Should().NotBeNullOrWhiteSpace();
        item.Lore.Should().NotBeNullOrWhiteSpace();
        item.Sheet.Should().NotBeNull();

        item.Value.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact(DisplayName = "Generate specific item returns the item correctly")]
    public void GenerateSpecificItem()
    {
        var item = _items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword);

        item.Type.Should().Be(ItemsLore.Types.Weapon);
        item.Subtype.Should().Be(ItemsLore.Subtypes.Weapons.Sword);
    }

    [Fact(DisplayName = "Generate specific item with wrong type subtype combination throws")]
    public void GenerateSpecificItemWrongCombinationTest()
    {
        Assert.Throws<KeyNotFoundException>(() => _items.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Weapons.Sword));
    }
}
