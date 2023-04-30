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
        var playerId = CreatePlayer(playerName);

        var stub = characterService.CreateCharacterStub(playerId);

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
        var playerId = CreatePlayer(playerName);
        dbm.Snapshot.CharacterStubs.Clear();

        characterService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = characterService.SaveCharacterStub(origins, playerId);

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
        var playerId = CreatePlayer(playerName);
        dbm.Snapshot.CharacterStubs.Clear();

        characterService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = characterService.SaveCharacterStub(origins, playerId);
        character = characterService.UpdateCharacter(new CharacterUpdate
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
        var playerId = CreatePlayer(playerName);
        dbm.Snapshot.CharacterStubs.Clear();

        characterService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = characterService.SaveCharacterStub(origins, playerId);

        characterService.DeleteCharacter(character.Identity.Id, playerId);

        var characters = characterService.GetCharacters(playerId);

        characters.CharactersList.Should().NotContain(character);
        characters.Count.Should().Be(0);
    }

    [Theory]
    [Description("Increasing the stats from a character should save it to db.")]
    public void Increase_stats_for_character_test()
    {
        var playerId = CreatePlayer(playerName);
        dbm.Snapshot.CharacterStubs.Clear();

        characterService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = characterService.SaveCharacterStub(origins, playerId);

        var currentStr = character.Sheet.Stats.Strength;

        dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId).Characters.Find(c => c.Identity.Id == character.Identity.Id).LevelUp.StatPoints = 1;

        character = characterService.UpdateCharacter(new CharacterUpdate
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
        var playerId = CreatePlayer(playerName);
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);

        Assert.Throws<Exception>(() => character = characterService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Stat = CharactersLore.Stats.Strength
        }, playerId));
    }

    [Theory]
    [Description("Increasing the skills from a character should save it to db.")]
    public void Increase_skills_for_character_test()
    {
        var playerId = CreatePlayer(playerName);
        dbm.Snapshot.CharacterStubs.Clear();

        characterService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = characterService.SaveCharacterStub(origins, playerId);

        var currentCombat = character.Sheet.Skills.Combat;

        dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId).Characters.Find(c => c.Identity.Id == character.Identity.Id).LevelUp.SkillPoints = 1;

        character = characterService.UpdateCharacter(new CharacterUpdate
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
        var playerId = CreatePlayer(playerName);
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);

        Assert.Throws<Exception>(() => character = characterService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Skill = CharactersLore.Skills.Combat
        }, playerId));
    }

    [Theory]
    [Description("Equipping an item in character inventory.")]
    public void Equip_item_on_character_inventory_test()
    {
        var playerId = CreatePlayer(playerName);
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

        characterService.EquipCharacterItem(equip, playerId);

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
        var playerId = CreatePlayer(playerName);
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

        characterService.EquipCharacterItem(equip, playerId);
        characterService.UnequipCharacterItem(equip, playerId);

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
        var playerId = CreatePlayer(playerName);
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);
        character.LevelUp.DeedsPoints = 100;

        var listOfTraits = characterService.GetHeroicTraits();

        var swordsman = listOfTraits.Find(t => t.Identity.Name == TraitsLore.BonusTraits.swordsman);

        var trait = new CharacterHeroicTrait
        {
            CharacterId = character.Identity.Id,
            HeroicTraitId = swordsman.Identity.Id,
        };

        var combatBeforeTrait = character.Sheet.Skills.Combat;
        characterService.LearnHeroicTrait(trait, playerId);
        var combatIncreasedOnce = character.Sheet.Skills.Combat;

        combatBeforeTrait.Should().BeLessThan(combatIncreasedOnce);

        characterService.LearnHeroicTrait(trait, playerId);
        var combatIncreasedTwice = character.Sheet.Skills.Combat;

        combatIncreasedOnce.Should().BeLessThan(combatIncreasedTwice);
    }

    [Theory]
    [Description("Learning a unique heroic trait twice throws error.")]
    public void Learn_unique_heroic_trait_throws_test()
    {
        var playerId = CreatePlayer(playerName);
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);
        character.LevelUp.DeedsPoints = 1000;

        var listOfTraits = characterService.GetHeroicTraits();

        var metachaos = listOfTraits.Find(t => t.Identity.Name == TraitsLore.ActiveTraits.metachaosDaemonology);

        var trait = new CharacterHeroicTrait
        {
            CharacterId = character.Identity.Id,
            HeroicTraitId = metachaos.Identity.Id,
        };

        characterService.LearnHeroicTrait(trait, playerId);

        Assert.Throws<Exception>(() => characterService.LearnHeroicTrait(trait, playerId));
    }

    [Theory]
    [Description("Get character paperdoll.")]
    public void Generate_character_paperdoll_test()
    {
        var playerId = CreatePlayer(playerName);
        dbm.Snapshot.CharacterStubs.Clear();

        var character = CreateCharacter(
            CharactersLore.Races.Human,
            CharactersLore.Cultures.Human.Danarian,
            CharactersLore.Heritage.Traditional,
            CharactersLore.Classes.Warrior,
            playerId);
        character.LevelUp.DeedsPoints = 1000;

        var listOfTraits = characterService.GetHeroicTraits();

        var candlelight = listOfTraits.Find(t => t.Identity.Name == TraitsLore.PassiveTraits.candlelight);
        var metachaos = listOfTraits.Find(t => t.Identity.Name == TraitsLore.ActiveTraits.metachaosDaemonology);

        var candlelightTrait = new CharacterHeroicTrait
        {
            CharacterId = character.Identity.Id,
            HeroicTraitId = candlelight.Identity.Id,
        };
        var metachaosTrait = new CharacterHeroicTrait
        {
            CharacterId = character.Identity.Id,
            HeroicTraitId = metachaos.Identity.Id,
        };
        characterService.LearnHeroicTrait(candlelightTrait, playerId);
        characterService.LearnHeroicTrait(metachaosTrait, playerId);

        var paperdoll = characterService.CalculateCharacterPaperdoll(character.Identity.Id, playerId);

        paperdoll.Should().NotBeNull();
        paperdoll.Stats.Should().NotBeNull();
        paperdoll.Assets.Should().NotBeNull();
        paperdoll.Skills.Should().NotBeNull();
        paperdoll.SpecialSkills.Should().NotBeNull();

        paperdoll.Stats.Strength.Should().BeGreaterThanOrEqualTo(character.Sheet.Stats.Strength);
        paperdoll.Assets.Resolve.Should().BeGreaterThan(10);
        paperdoll.Skills.Arcane.Should().BeGreaterThan(20);
        paperdoll.SpecialSkills.Count.Should().Be(1);
        paperdoll.SpecialSkills.First().Identity.Name.Should().Be(metachaos.Identity.Name);
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
                case ItemsLore.Subtypes.Protections.Helm:
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

    private Character CreateCharacter(string race, string culture, string heritage, string classes, string playerId)
    {
        dbm.Snapshot.CharacterStubs.Clear();

        characterService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = race,
            Culture = culture,
            Heritage = heritage,
            Class = classes
        };

        return characterService.SaveCharacterStub(origins, playerId);
    }
    #endregion
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.