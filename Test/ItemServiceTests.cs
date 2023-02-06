using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Dtos;

namespace Tests;

public class ItemServiceTests : TestBase
{
    [Theory]
    [Description("Test to create a random item.")]
    public void Create_a_random_item_test()
    {
        var item = itemService.GenerateRandomItem();

        item.Should().BeOfType<Item>();
    }

    [Theory]
    [Description("Test random item name and level name.")]
    public void Random_item_level_and_levelName_test() 
    {
        var levelNames = ItemsLore.LevelNames.LevelNamesAll;

        var items = CreateSomeItems(10);

        foreach (var item in items)
        {
            item.Level.Should().BeInRange(1, 6);
            levelNames.Should().Contain(item.LevelName);
        }
    }

    [Theory]
    [Description("Test random item type and subtype.")]
    public void Random_item_type_and_subtype_test()
    {
        var types = ItemsLore.Types.All;
        var subtypes = new List<string>();
        ItemsLore.Subtypes.Weapons.All.ForEach(e => subtypes.Add(e));
        ItemsLore.Subtypes.Wealth.All.ForEach(e => subtypes.Add(e));
        ItemsLore.Subtypes.Protections.All.ForEach(e => subtypes.Add(e));

        var items = CreateSomeItems(10);

        foreach (var item in items)
        {
            types.Should().Contain(item.Type);
            subtypes.Should().Contain(item.Subtype);
        }
    }

    [Theory]
    [Description("Test random item category and description.")]
    public void Random_item_category_and_description_test()
    {
        var categories = new Dictionary<string, Dictionary<string, string>>();
        ItemsLore.Categories.Weapons.ToList().ForEach(e => categories.Add(e.Key, e.Value));
        ItemsLore.Categories.Wealth.ToList().ForEach(e => categories.Add(e.Key, e.Value));
        ItemsLore.Categories.Protections.ToList().ForEach(e => categories.Add(e.Key, e.Value));

        var normalQualities = new List<string>();
        ItemsLore.Qualities.Common.ForEach(e => normalQualities.Add(e));
        ItemsLore.Qualities.Refined.ForEach(e => normalQualities.Add(e));
        ItemsLore.Qualities.Masterwork.ForEach(e => normalQualities.Add(e));

        var items = CreateSomeItems(10);

        foreach (var item in items)
        {
            categories.FirstOrDefault(s => s.Key.Equals(item.Subtype)).Value.Keys.Should().Contain(item.Category);
            categories.FirstOrDefault(s => s.Key.Equals(item.Subtype)).Value.Values.Should().Contain(item.Description);
        }
    }

    [Theory]
    [Description("Test random item quality and lore.")]
    public void Random_item_quality_and_lore_test()
    {
        var normalQualities = new List<string>();
        ItemsLore.Qualities.Common.ForEach(e => normalQualities.Add(e));
        ItemsLore.Qualities.Refined.ForEach(e => normalQualities.Add(e));
        ItemsLore.Qualities.Masterwork.ForEach(e => normalQualities.Add(e));

        var specialQualities = new List<string>();
        ItemsLore.Heirlooms.All.ForEach(e => specialQualities.Add(e.Quality));
        ItemsLore.Artifacts.All.ForEach(e => specialQualities.Add(e.Quality));
        ItemsLore.Relics.All.ForEach(e => specialQualities.Add(e.Quality));

        var lores = new List<string>();
        ItemsLore.Heirlooms.All.ForEach(e => lores.Add(e.Lore));
        ItemsLore.Artifacts.All.ForEach(e => lores.Add(e.Lore));
        ItemsLore.Relics.All.ForEach(e => lores.Add(e.Lore));

        var items = CreateSomeItems(10);

        foreach (var item in items)
        {
            if (item.Level <= 3)
            {
                normalQualities.Should().Contain(item.Quality);
                item.Lore.Should().Be("Not much can be said about this item.");
            }
            else
            {
                specialQualities.Should().Contain(item.Quality);
                lores.Should().Contain(item.Lore);
            }
        }
    }

    [Theory]
    [Description("Test random item taint.")] 
    public void Random_item_taint_test()
    {
        var items = CreateSomeItems(100);
        var areAllTainted = items.Count(s => s.HasTaint == true) == 100;
        var areAllNormal = items.Count(s => s.HasTaint == false) == 100;

        while (areAllTainted || areAllNormal)
        {
            items = CreateSomeItems(100);
        }

        items.FirstOrDefault(s => s.HasTaint).Should().NotBeNull();
        items.FirstOrDefault(s => !s.HasTaint).Should().NotBeNull();
    }

    [Theory]
    [Description("Test random item value.")]
    public void Random_item_value_test()
    {
        var item = CreateAnItem();

        item.Value.Should().BeGreaterThan(0);
    }

    [Theory]
    [Description("Create specific items.")]
    public void Create_specific_items_test()
    {
        var helmet = itemService.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Helmet);
        var sword = itemService.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword);

        ItemsLore.Protections.Helmets.Keys.Should().Contain(helmet.Category);
        ItemsLore.Protections.Helmets.Values.Should().Contain(helmet.Description);

        ItemsLore.Weapons.Swords.Keys.Should().Contain(sword.Category);
        ItemsLore.Weapons.Swords.Values.Should().Contain(sword.Description);
    }

    #region privates
    private List<Item> CreateSomeItems(int numberOfItems)
    {
        var items = new List<Item>();

        for (int i = 0; i < numberOfItems; i++)
        {
            items.Add(CreateAnItem());
        }

        return items;
    }

    private Item CreateAnItem()
    {
        return itemService.GenerateRandomItem();
    }
    #endregion
}
