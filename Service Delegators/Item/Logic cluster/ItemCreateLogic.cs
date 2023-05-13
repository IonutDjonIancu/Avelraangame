#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class ItemCreateLogic
{
    private readonly IDiceRollService dice;

    private ItemCreateLogic() { }
    internal ItemCreateLogic(IDiceRollService dice)
    {
        this.dice = dice;
    }

    internal Item CreateItem()
    {
        return new()
        {
            Identity = new ItemIdentity
            {
                Id = Guid.NewGuid().ToString(),
                CharacterId = "",
            },
            Sheet = new CharacterSheet
            {
                Stats = new CharacterStats(),
                Assets = new CharacterAssets(),
                Skills = new CharacterSkills()
            }
        };
    }

    internal void NameItem(Item item)
    {
        item.Name = item.Level >= 5 ? item.Quality : $"{item.Quality} {item.Category.ToLowerInvariant()}";
    }
}

#pragma warning restore CA1822 // Mark members as static