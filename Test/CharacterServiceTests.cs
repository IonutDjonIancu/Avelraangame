namespace Tests;

public class CharacterServiceTests : TestBase
{
    private readonly string playerName = "test player";

    public CharacterServiceTests()
    {
        var listOfPlayers = dbs.Snapshot.Players.ToList();

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

        dbs.Snapshot.CharacterStubs.Count.Should().BeGreaterThanOrEqualTo(1);
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
        dbs.Snapshot.CharacterStubs.Clear();

        characterService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        var character = characterService.SaveCharacterStub(origins, playerId);

        dbs.Snapshot.CharacterStubs.Count.Should().Be(0);
        character.Should().NotBeNull();

        character.Identity.Should().NotBeNull();
        character.Identity.Id.Should().NotBeNullOrWhiteSpace();
        character.Identity.PlayerId.Should().Be(playerId);
        character.Info.Name.Should().NotBeNullOrWhiteSpace();

        character.Info.Should().NotBeNull();
        character.Info.EntityLevel.Should().BeGreaterThanOrEqualTo(1);
        character.Info.Origins.Race.Should().Be(origins.Race);
        character.Info.Origins.Culture.Should().Be(origins.Culture);
        character.Info.Origins.Heritage.Should().Be(origins.Heritage);
        character.Info.Origins.Class.Should().Be(origins.Class);
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

        character.Info.IsAlive.Should().BeTrue();
    }

    [Theory]
    [Description("Modifing character name should have the new name.")]
    public void Rename_character_test()
    {
        var newCharName = "Jax";

        var chr = CreateHumanCharacter();
        chr = characterService.UpdateCharacterName(newCharName, CreateCharIdentity(chr));

        chr.Info.Name.Should().Be(newCharName);
    }

    [Theory]
    [Description("Deleting a character should remove it from db.")]
    public void Delete_character_test()
    {
        var chr = CreateHumanCharacter();

        characterService.DeleteCharacter(CreateCharIdentity(chr));

        var characters = characterService.GetCharactersByPlayerId(chr.Identity.PlayerId);

        characters.CharactersList.Should().NotContain(chr);
        characters.Count.Should().Be(0);
    }

    [Theory]
    [Description("Increasing the stats from a character should save it to db.")]
    public void Increase_stats_for_character_test()
    {
        var chr = CreateHumanCharacter();

        var currentStr = chr.Sheet.Stats.Strength;

        dbs.Snapshot.Players.Find(p => p.Identity.Id == chr.Identity.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Identity.Id)!.LevelUp.StatPoints = 1;

        chr = characterService.UpdateCharacterStats(CharactersLore.Stats.Strength, CreateCharIdentity(chr));

        chr.Sheet.Stats.Strength.Should().Be(currentStr + 1);
    }

    [Theory]
    [Description("Increasing the stats from a character with no stat points should throw.")]
    public void Increase_stats_with_no_points_for_character_should_throw_test()
    {
        var chr = CreateHumanCharacter();

        Assert.Throws<Exception>(() => characterService.UpdateCharacterStats(CharactersLore.Stats.Strength, CreateCharIdentity(chr)));
    }

    [Theory]
    [Description("Increasing the skills from a character should save it to db.")]
    public void Increase_skills_for_character_test()
    {
        var chr = CreateHumanCharacter();
        var currentCombat = chr.Sheet.Skills.Combat;

        dbs.Snapshot.Players.Find(p => p.Identity.Id == chr.Identity.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Identity.Id)!.LevelUp.SkillPoints = 1;

        chr = characterService.UpdateCharacterSkills(CharactersLore.Skills.Combat, CreateCharIdentity(chr));

        chr.Sheet.Skills.Combat.Should().Be(currentCombat + 1);
    }

    [Theory]
    [Description("Increasing the skills from a character with no skill points should throw.")]
    public void Increase_skills_with_no_points_for_character_should_throw_test()
    {
        var chr = CreateHumanCharacter();

        Assert.Throws<Exception>(() => characterService.UpdateCharacterSkills(CharactersLore.Skills.Combat, CreateCharIdentity(chr)));
    }

    [Theory]
    [Description("Equipping an item in character inventory.")]
    public void Equip_item_on_character_inventory_test()
    {
        var chr = CreateHumanCharacter();

        var item = chr.Supplies.First();
        item.Should().NotBeNull();

        var location = item.InventoryLocations.First();

        var equip = new CharacterEquip
        {
            PlayerId = chr.Identity.PlayerId,
            CharacterId = chr.Identity.Id,
            InventoryLocation = location,
            ItemId = chr.Supplies.First().Identity.Id
        };

        characterService.EquipCharacterItem(equip);

        var hasEquipedItem =
            chr.Inventory.Head != null
            || chr.Inventory.Ranged != null
            || chr.Inventory.Body != null
            || chr.Inventory.Mainhand != null
            || chr.Inventory.Offhand != null
            || chr.Inventory.Shield != null;

        if (chr.Inventory.Heraldry!.Count > 0)
        {
            hasEquipedItem = chr.Inventory.Heraldry.First() != null;
        }

        hasEquipedItem.Should().BeTrue();
    }

    [Theory]
    [Description("Unequipping an item from character inventory.")]
    public void Unequip_item_from_character_inventory_test()
    {
        var chr = CreateHumanCharacter();

        var item = chr.Supplies.First();
        item.Should().NotBeNull();

        var location = item.InventoryLocations.First();

        var equip = new CharacterEquip
        {
            PlayerId = chr.Identity.PlayerId,
            CharacterId = chr.Identity.Id,
            InventoryLocation = location,
            ItemId = chr.Supplies.First().Identity.Id
        };

        characterService.EquipCharacterItem(equip);
        characterService.UnequipCharacterItem(equip);

        var hasEquipedItem =
            chr.Inventory.Head != null
            || chr.Inventory.Ranged != null
            || chr.Inventory.Body != null
            || chr.Inventory.Mainhand != null
            || chr.Inventory.Offhand != null
            || chr.Inventory.Shield != null;

        if (chr.Inventory.Heraldry!.Count > 0)
        {
            hasEquipedItem = chr.Inventory.Heraldry.First() != null;
        }

        hasEquipedItem.Should().BeFalse();
        chr.Supplies.Count.Should().BeGreaterThanOrEqualTo(1);
    }

    [Theory]
    [Description("Learning a common bonus heroic trait.")]
    public void Learn_common_bonus_heroic_trait_test()
    {
        var chr = CreateHumanCharacter();
        chr.LevelUp.DeedsPoints = 100;

        var swordsman = dbs.Snapshot.Traits.Find(t => t.Identity.Name == TraitsLore.BonusTraits.swordsman)!;

        var trait = new CharacterHeroicTrait
        {
            PlayerId = chr.Identity.PlayerId,
            CharacterId = chr.Identity.Id,
            HeroicTraitId = swordsman.Identity.Id,
        };

        var combatBeforeTrait = chr.Sheet.Skills.Combat;
        characterService.LearnHeroicTrait(trait);
        var combatIncreasedOnce = chr.Sheet.Skills.Combat;

        combatBeforeTrait.Should().BeLessThan(combatIncreasedOnce);

        characterService.LearnHeroicTrait(trait);
        var combatIncreasedTwice = chr.Sheet.Skills.Combat;

        combatIncreasedOnce.Should().BeLessThan(combatIncreasedTwice);
    }

    [Theory]
    [Description("Learning a unique heroic trait twice throws error.")]
    public void Learn_unique_heroic_trait_throws_test()
    {
        var chr = CreateHumanCharacter();
        chr.LevelUp.DeedsPoints = 1000;

        var metachaos = dbs.Snapshot.Traits.Find(t => t.Identity.Name == TraitsLore.ActivateTraits.metachaosDaemonology)!;

        var trait = new CharacterHeroicTrait
        {
            PlayerId = chr.Identity.PlayerId,
            CharacterId = chr.Identity.Id,
            HeroicTraitId = metachaos.Identity.Id,
        };

        characterService.LearnHeroicTrait(trait);

        Assert.Throws<Exception>(() => characterService.LearnHeroicTrait(trait));
    }

    [Theory]
    [Description("Create character paperdoll.")]
    public void Generate_character_paperdoll_test()
    {
        var chr = CreateHumanCharacter();
        chr.LevelUp.DeedsPoints = 1000;

        var candlelight = dbs.Snapshot.Traits.Find(t => t.Identity.Name == TraitsLore.PassiveTraits.candlelight)!;
        var metachaos = dbs.Snapshot.Traits.Find(t => t.Identity.Name == TraitsLore.ActivateTraits.metachaosDaemonology)!;

        var candlelightTrait = new CharacterHeroicTrait
        {
            PlayerId = chr.Identity.PlayerId,
            CharacterId = chr.Identity.Id,
            HeroicTraitId = candlelight.Identity.Id,
        };
        var metachaosTrait = new CharacterHeroicTrait
        {
            PlayerId = chr.Identity.PlayerId,
            CharacterId = chr.Identity.Id,
            HeroicTraitId = metachaos.Identity.Id,
        };
        characterService.LearnHeroicTrait(candlelightTrait);
        characterService.LearnHeroicTrait(metachaosTrait);

        var charIdentity = new CharacterIdentity
        {
            Id = chr.Identity.Id,
            PlayerId = chr.Identity.PlayerId
        };

        var paperdoll = characterService.CalculatePaperdollForPlayerCharacter(charIdentity);

        paperdoll.Should().NotBeNull();
        paperdoll.Stats.Should().NotBeNull();
        paperdoll.Assets.Should().NotBeNull();
        paperdoll.Skills.Should().NotBeNull();
        paperdoll.SpecialSkills.Should().NotBeNull();

        paperdoll.Stats.Strength.Should().BeGreaterThanOrEqualTo(chr.Sheet.Stats.Strength);
        paperdoll.Assets.Resolve.Should().BeGreaterThan(10);
        paperdoll.Skills.Arcane.Should().BeGreaterThan(20);
        paperdoll.SpecialSkills.Count.Should().Be(1);
        paperdoll.SpecialSkills.First().Identity.Name.Should().Be(metachaos.Identity.Name);
    }

    #region private methods
    private static CharacterIdentity CreateCharIdentity(Character chr)
    {
        return new CharacterIdentity
        {
            Id = chr.Identity.Id,
            PlayerId = chr.Identity.PlayerId
        };
    }

    private Character CreateHumanCharacter()
    {
        var playerId = CreatePlayer(playerName);
        dbs.Snapshot.CharacterStubs.Clear();

        characterService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Heritage = CharactersLore.Heritage.Traditional,
            Class = CharactersLore.Classes.Warrior
        };

        return characterService.SaveCharacterStub(origins, playerId);
    }
    #endregion
}
