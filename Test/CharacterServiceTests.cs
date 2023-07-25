using Service_Delegators;
using Xunit;

namespace Tests;

public class CharacterServiceTests : TestBase
{
    private readonly string playerName = "test player";

    public CharacterServiceTests()
    {
        dbs.Snapshot.Players.Clear();
    }

    [Fact(DisplayName = "Create character stub")]
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

    [Fact(DisplayName = "Save character stub")]
    public void Save_character_stub_test()
    {
        var playerId = CreatePlayer(playerName);
        dbs.Snapshot.CharacterStubs.Clear();

        characterService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = CharactersLore.Races.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Tradition = CharactersLore.Tradition.Common,
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
        character.Info.Origins.Tradition.Should().Be(origins.Tradition);
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
        character.Status.IsLockedForModify.Should().BeFalse();

        GameplayLore.Map.All.Select(s => s.LocationName).Should().Contain(character.Position.Location);
    }

    [Fact(DisplayName = "Modifing character name should have the new name")]
    public void Rename_character_test()
    {
        var newCharName = "Jax";

        var chr = CreateHumanCharacter("Jax");
        chr = characterService.UpdateCharacterName(newCharName, CreateCharIdentity(chr));

        chr.Info.Name.Should().Be(newCharName);
    }

    [Fact(DisplayName = "Deleting a character should remove it from db")]
    public void Delete_character_test()
    {
        var chr = CreateHumanCharacter("Jax");

        characterService.DeleteCharacter(CreateCharIdentity(chr));

        var characters = characterService.GetPlayerCharacters(chr.Identity.PlayerId);

        characters.CharactersList.Should().NotContain(chr);
        characters.Count.Should().Be(0);
    }

    [Fact(DisplayName = "Increasing the stats from a character should save it to db")]
    public void Increase_stats_for_character_test()
    {
        var chr = CreateHumanCharacter("Jax");

        var currentStr = chr.Sheet.Stats.Strength;

        dbs.Snapshot.Players.Find(p => p.Identity.Id == chr.Identity.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Identity.Id)!.LevelUp.StatPoints = 1;

        chr = characterService.UpdateCharacterStats(CharactersLore.Stats.Strength, CreateCharIdentity(chr));

        chr.Sheet.Stats.Strength.Should().Be(currentStr + 1);
    }

    [Fact(DisplayName = "Increasing the stats from a character with no stat points should throw")]
    public void Increase_stats_with_no_points_for_character_should_throw_test()
    {
        var chr = CreateHumanCharacter("Jax");

        Assert.Throws<Exception>(() => characterService.UpdateCharacterStats(CharactersLore.Stats.Strength, CreateCharIdentity(chr)));
    }

    [Fact(DisplayName = "Increasing the skills from a character should save it to db")]
    public void Increase_skills_for_character_test()
    {
        var chr = CreateHumanCharacter("Jax");
        var currentCombat = chr.Sheet.Skills.Combat;

        dbs.Snapshot.Players.Find(p => p.Identity.Id == chr.Identity.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Identity.Id)!.LevelUp.SkillPoints = 1;

        chr = characterService.UpdateCharacterSkills(CharactersLore.Skills.Combat, CreateCharIdentity(chr));

        chr.Sheet.Skills.Combat.Should().Be(currentCombat + 1);
    }

    [Fact(DisplayName = "Increasing the skills from a character with no skill points should throw")]
    public void Increase_skills_with_no_points_for_character_should_throw_test()
    {
        var chr = CreateHumanCharacter("Jax");

        Assert.Throws<Exception>(() => characterService.UpdateCharacterSkills(CharactersLore.Skills.Combat, CreateCharIdentity(chr)));
    }

    [Fact(DisplayName = "Equipping an item in character inventory")]
    public void Equip_item_on_character_inventory_test()
    {
        var chr = CreateHumanCharacter("Jax");

        var item = chr.Supplies.First();
        item.Should().NotBeNull();

        var location = item.InventoryLocations.First();

        var equip = new CharacterEquip
        {
            CharacterIdentity = new CharacterIdentity 
            { 
                Id = chr.Identity.Id,
                PlayerId = chr.Identity.PlayerId,
            },
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

    [Fact(DisplayName = "Unequipping an item from character inventory")]
    public void Unequip_item_from_character_inventory_test()
    {
        var chr = CreateHumanCharacter("Jax");

        var item = chr.Supplies.First();
        item.Should().NotBeNull();

        var location = item.InventoryLocations.First();

        var equip = new CharacterEquip
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = chr.Identity.Id,
                PlayerId = chr.Identity.PlayerId,
            },
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

    [Fact(DisplayName = "Learning a common bonus heroic trait")]
    public void Learn_common_bonus_heroic_trait_test()
    {
        var chr = CreateHumanCharacter("Jax");
        chr.LevelUp.DeedsPoints = 100;

        var swordsman = TraitsLore.All.Find(t => t.Identity.Name == TraitsLore.BonusTraits.swordsman.Identity.Name)!;

        var trait = new CharacterHeroicTrait
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = chr.Identity.Id,
                PlayerId = chr.Identity.PlayerId,
            },
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

    [Fact(DisplayName = "Learning a unique heroic trait twice throws error")]
    public void Learn_unique_heroic_trait_throws_test()
    {
        var chr = CreateHumanCharacter("Jax");
        chr.LevelUp.DeedsPoints = 1000;

        var metachaos = TraitsLore.All.Find(t => t.Identity.Name == TraitsLore.ActivateTraits.metachaosDaemonology.Identity.Name)!;

        var trait = new CharacterHeroicTrait
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = chr.Identity.Id,
                PlayerId = chr.Identity.PlayerId,
            },
            HeroicTraitId = metachaos.Identity.Id,
        };

        characterService.LearnHeroicTrait(trait);

        Assert.Throws<Exception>(() => characterService.LearnHeroicTrait(trait));
    }

    [Fact(DisplayName = "Create character paperdoll")]
    public void Calculate_character_paperdoll_test()
    {
        var chr = CreateHumanCharacter("Jax");
        chr.LevelUp.DeedsPoints = 1000;

        var candlelight = TraitsLore.All.Find(t => t.Identity.Name == TraitsLore.PassiveTraits.candlelight.Identity.Name)!;
        var metachaos = TraitsLore.All.Find(t => t.Identity.Name == TraitsLore.ActivateTraits.metachaosDaemonology.Identity.Name)!;

        var candlelightTrait = new CharacterHeroicTrait
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = chr.Identity.Id,
                PlayerId = chr.Identity.PlayerId,
            },
            HeroicTraitId = candlelight.Identity.Id,
        };
        var metachaosTrait = new CharacterHeroicTrait
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = chr.Identity.Id,
                PlayerId = chr.Identity.PlayerId,
            },
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

        var calculatedActionTokens = RulebookLore.Formulae.Misc.CalculateActionTokens(paperdoll.Assets.Resolve);
        var actionTokens = calculatedActionTokens <= 1 ? 1 : calculatedActionTokens;

        paperdoll.ActionTokens.Should().Be(actionTokens);
    }

    [Theory(DisplayName = "Character travel should move to new position")]
    [InlineData("Dragonmaw_Farlindor_Danar_Belfordshire")]
    [InlineData("Dragonmaw_Farlindor_Danar_Arada")]
    public void Character_travel_test(string locationFullName)
    {
        var chr = CreateHumanCharacter("Jax");
        chr.Inventory.Provisions.Should().BeGreaterThan(0);
        chr.Inventory.Provisions.Should().BeLessThanOrEqualTo(100);

        var travelToPosition = new CharacterTravel
        {
            CharacterIdentity = CreateCharIdentity(chr),
            Destination = Utils.GetLocationPosition(locationFullName)
        };

        var initialProvisions = chr.Inventory.Provisions;

        characterService.TravelToLocation(travelToPosition);

        chr.Inventory.Provisions.Should().BeLessThan(initialProvisions);
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
    #endregion
}
