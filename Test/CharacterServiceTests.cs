#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Tests;

public class CharacterServiceTests : TestBase
{
    private readonly string playerName = "test player";

    public CharacterServiceTests()
    {
        var listOfPlayers = dbm.Snapshot.Players.ToList();

        foreach (var player in listOfPlayers)
        {
            playerService.DeletePlayer(player.Identity.Id);
        }
    }

    [Theory]
    [Description("Create character stub.")]
    public void Create_character_stub_test()
    {
        var playerId = CreatePlayer();

        var stub = charService.CreateCharacterStub(playerId);

        dbm.Snapshot.CharacterStubs.Count.Should().BeGreaterThanOrEqualTo(1);
        stub.Should().NotBeNull();
        stub.EntityLevel.Should().BeGreaterThanOrEqualTo(1);
        stub.StatPoints.Should().BeGreaterThanOrEqualTo(1);
        stub.SkillPoints.Should().BeGreaterThanOrEqualTo(1);
    }

    [Theory]
    [Description("Save character stub.")]
    public void Save_character_stub_test()
    {
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        charService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = charService.SaveCharacterStub(origins, playerId);

        dbm.Snapshot.CharacterStubs.Count.Should().Be(0);
        character.Should().NotBeNull();

        character.Identity.Should().NotBeNull();
        character.Identity.Id.Should().NotBeNullOrWhiteSpace();
        character.Identity.PlayerId.Should().Be(playerId);
        character.Identity.Name.Should().NotBeNullOrWhiteSpace();

        character.Info.Should().NotBeNull();
        character.Info.EntityLevel.Should().BeGreaterThanOrEqualTo(1);
        character.Info.Race.Should().Be(origins.Race);
        character.Info.Culture.Should().Be(origins.Culture);
        character.Info.Heritage.Should().Be(origins.Heritage);
        character.Info.Class.Should().Be(origins.Class);
        character.Info.Fame.Should().NotBeNullOrWhiteSpace();
        character.Info.Wealth.Should().BeGreaterThanOrEqualTo(1);
        character.Info.DateOfBirth.Should().Be(DateTime.Now.ToShortDateString());

        character.LevelUp.Should().NotBeNull();
        character.LevelUp.StatPoints.Should().Be(0);
        character.LevelUp.SkillPoints.Should().Be(0);

        character.Sheet.Should().NotBeNull();
        character.Sheet.Stats.Strength.Should().BeGreaterThanOrEqualTo(1);
        character.Sheet.Skills.Combat.Should().BeGreaterThanOrEqualTo(1);

        character.Inventory.Should().NotBeNull();

        character.Supplies.Should().NotBeNull();
        character.Supplies.Count.Should().BeGreaterThanOrEqualTo(1);

        character.IsAlive.Should().BeTrue();
    }

    [Theory]
    [Description("Modifing character name should have the new name.")]
    public void Rename_character_test()
    {
        var newCharName = "Jax";
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        charService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = charService.SaveCharacterStub(origins, playerId);
        character = charService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Name = newCharName
        }, playerId);

        character.Identity.Name.Should().Be(newCharName);
    }

    [Theory]
    [Description("Deleting a character should remove it from db.")]
    public void Delete_character_test()
    {
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        charService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = charService.SaveCharacterStub(origins, playerId);

        charService.DeleteCharacter(character.Identity.Id, playerId);

        var characters = charService.GetCharacters(playerId);

        characters.CharactersList.Should().NotContain(character);
        characters.Count.Should().Be(0);
    }

    [Theory]
    [Description("Increasing the stats from a character should save it to db.")]
    public void Increase_stats_for_character_test()
    {
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        charService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = charService.SaveCharacterStub(origins, playerId);

        var currentStr = character.Sheet.Stats.Strength;

        dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId).Characters.Find(c => c.Identity.Id == character.Identity.Id).LevelUp.StatPoints = 1;

        character = charService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Stat = CharactersLore.Stats.Strength
        }, playerId);

        character.Sheet.Stats.Strength.Should().Be(currentStr + 1);
    }

    [Theory]
    [Description("Increasing the stats from a character with no stat points should throw.")]
    public void Increase_stats_with_no_points_for_character_should_throw_test()
    {
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);

        Assert.Throws<Exception>(() => character = charService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Stat = CharactersLore.Stats.Strength
        }, playerId));
    }

    [Theory]
    [Description("Increasing the skills from a character should save it to db.")]
    public void Increase_skills_for_character_test()
    {
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        charService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = charService.SaveCharacterStub(origins, playerId);

        var currentCombat = character.Sheet.Skills.Combat;

        dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId).Characters.Find(c => c.Identity.Id == character.Identity.Id).LevelUp.SkillPoints = 1;

        character = charService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Skill = CharactersLore.Skills.Combat
        }, playerId);

        character.Sheet.Skills.Combat.Should().Be(currentCombat + 1);
    }

    [Theory]
    [Description("Increasing the skills from a character with no stat points should throw.")]
    public void Increase_skills_with_no_points_for_character_should_throw_test()
    {
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);

        Assert.Throws<Exception>(() => character = charService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Skill = CharactersLore.Skills.Combat
        }, playerId));
    }

    [Theory]
    [Description("Equipping an item in character inventory.")]
    public void Equip_item_on_character_inventory_test()
    {
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);

        var item = character.Supplies?.First();
        item.Should().NotBeNull();

        var location = GetItemLocation(character);

        var equip = new CharacterEquip
        {
            CharacterId = character.Identity.Id,
            InventoryLocation = location,
            ItemId = character.Supplies.First().Identity.Id
        };

        charService.EquipCharacterItem(equip, playerId);

        var hasEquipedItem =
            character.Inventory.Head != null
            || character.Inventory.Ranged != null
            || character.Inventory.Body != null
            || character.Inventory.Mainhand != null
            || character.Inventory.Offhand != null
            || character.Inventory.Shield != null;

        if (character.Inventory.Heraldry.Count > 0)
        {
            hasEquipedItem = character.Inventory.Heraldry.First() != null;
        }

        hasEquipedItem.Should().BeTrue();
    }

    [Theory]
    [Description("Unequipping an item from character inventory.")]
    public void Unequip_item_from_character_inventory_test()
    {
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);

        var item = character.Supplies?.First();
        item.Should().NotBeNull();

        var location = GetItemLocation(character);

        var equip = new CharacterEquip
        {
            CharacterId = character.Identity.Id,
            InventoryLocation = location,
            ItemId = character.Supplies.First().Identity.Id
        };

        charService.EquipCharacterItem(equip, playerId);
        charService.UnequipCharacterItem(equip, playerId);

        var hasEquipedItem =
            character.Inventory.Head != null
            || character.Inventory.Ranged != null
            || character.Inventory.Body != null
            || character.Inventory.Mainhand != null
            || character.Inventory.Offhand != null
            || character.Inventory.Shield != null;

        if (character.Inventory.Heraldry.Count > 0)
        {
            hasEquipedItem = character.Inventory.Heraldry.First() != null;
        }

        hasEquipedItem.Should().BeFalse();
        character.Supplies.Count.Should().Be(1);
    }

    [Theory]
    [Description("Learning a common bonus heroic trait.")]
    public void Learn_common_bonus_heroic_trait_test()
    {
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);
        character.LevelUp.DeedsPoints = 100;

        var listOfTraits = charService.GetHeroicTraits();

        var swordsman = listOfTraits.Find(t => t.Identity.Name == TraitsLore.BonusTraits.swordsman);

        var trait = new CharacterHeroicTrait
        {
            CharacterId = character.Identity.Id,
            HeroicTraitId = swordsman.Identity.Id,
        };

        var combatBeforeTrait = character.Sheet.Skills.Combat;
        charService.LearnHeroicTrait(trait, playerId);
        var combatIncreasedOnce = character.Sheet.Skills.Combat;

        combatBeforeTrait.Should().BeLessThan(combatIncreasedOnce);

        charService.LearnHeroicTrait(trait, playerId);
        var combatIncreasedTwice = character.Sheet.Skills.Combat;

        combatIncreasedOnce.Should().BeLessThan(combatIncreasedTwice);
    }

    [Theory]
    [Description("Learning a unique heroic trait twice throws error.")]
    public void Learn_unique_heroic_trait_throws_test()
    {
        var playerId = CreatePlayer();
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);
        character.LevelUp.DeedsPoints = 1000;

        var listOfTraits = charService.GetHeroicTraits();

        var metachaos = listOfTraits.Find(t => t.Identity.Name == TraitsLore.ActiveTraits.metachaosDaemonology);

        var trait = new CharacterHeroicTrait
        {
            CharacterId = character.Identity.Id,
            HeroicTraitId = metachaos.Identity.Id,
        };

        charService.LearnHeroicTrait(trait, playerId);

        Assert.Throws<Exception>(() => charService.LearnHeroicTrait(trait, playerId));
    }


    #region private methods
    private string GetItemLocation(Character character)
    {
        var item = itemService.GenerateRandomItem();
        if (item.Subtype == ItemsLore.Subtypes.Wealth.Goods)
        {
            return GetItemLocation(character);
        }

        item.Identity.CharacterId = character.Identity.Id;
        character.Supplies.Clear();
        character.Supplies.Add(item);

        string location = string.Empty;
        if (ItemsLore.Subtypes.Weapons.All.Contains(item.Subtype))
        {
            switch (item.Subtype)
            {
                case ItemsLore.Subtypes.Weapons.Axe:
                case ItemsLore.Subtypes.Weapons.Dagger:
                case ItemsLore.Subtypes.Weapons.Mace:
                case ItemsLore.Subtypes.Weapons.Pike:
                case ItemsLore.Subtypes.Weapons.Polearm:
                case ItemsLore.Subtypes.Weapons.Spear:
                case ItemsLore.Subtypes.Weapons.Sword:
                    location = ItemsLore.InventoryLocation.Mainhand;
                    break;
                case ItemsLore.Subtypes.Weapons.Bow:
                case ItemsLore.Subtypes.Weapons.Crossbow:
                case ItemsLore.Subtypes.Weapons.Sling:
                    location = ItemsLore.InventoryLocation.Ranged;
                    break;
                default:
                    break;
            }
        }
        else if (ItemsLore.Subtypes.Protections.All.Contains(item.Subtype))
        {
            switch (item.Subtype)
            {
                case ItemsLore.Subtypes.Protections.Helmet:
                    location = ItemsLore.InventoryLocation.Head;
                    break;
                case ItemsLore.Subtypes.Protections.Armour:
                    location = ItemsLore.InventoryLocation.Body;
                    break;
                case ItemsLore.Subtypes.Protections.Shield:
                    location = ItemsLore.InventoryLocation.Shield;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (item.Subtype)
            {
                case ItemsLore.Subtypes.Wealth.Gems:
                case ItemsLore.Subtypes.Wealth.Trinket:
                case ItemsLore.Subtypes.Wealth.Valuables:
                    location = ItemsLore.InventoryLocation.Heraldry;
                    break;
                default:
                    break;
            }
        }

        return location;
    }

    private string CreatePlayer()
    {
        dbm.Snapshot.Players!.Clear();
        playerService.CreatePlayer(playerName);

        return dbm.Snapshot.Players!.Find(p => p.Identity.Name == playerName)!.Identity.Id;
    }

    private Character CreateCharacter(string race, string culture, string heritage, string classes, string playerId)
    {
        dbm.Snapshot.CharacterStubs.Clear();

        charService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = race,
            Culture = culture,
            Heritage = heritage,
            Class = classes
        };

        return charService.SaveCharacterStub(origins, playerId);
    }
    #endregion
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.