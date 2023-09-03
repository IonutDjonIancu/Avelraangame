namespace Tests;

public class CharacterServiceTests : TestBase
{
    //private readonly string playerName = "test player";

    //public CharacterServiceTests()
    //{
    //    dbs.Snapshot.Players.Clear();
    //}

    //[Fact(DisplayName = "Create character stub")]
    //public void Create_character_stub_test()
    //{
    //    var playerId = CreatePlayer(playerName);

    //    var stub = charService.CreateCharacterStub(playerId);

    //    dbs.Snapshot.CharacterStubs.Count.Should().BeGreaterThanOrEqualTo(1);
    //    stub.Should().NotBeNull();
    //    stub.EntityLevel.Should().BeGreaterThanOrEqualTo(1);
    //    stub.StatPoints.Should().BeGreaterThanOrEqualTo(1);
    //    stub.SkillPoints.Should().BeGreaterThanOrEqualTo(1);
    //}

    //[Fact(DisplayName = "Save character stub")]
    //public void Save_character_stub_test()
    //{
    //    var playerId = CreatePlayer(playerName);
    //    dbs.Snapshot.CharacterStubs.Clear();

    //    charService.CreateCharacterStub(playerId);

    //    var origins = new CharacterTraits
    //    {
    //        Race = CharactersLore.Races.Playable.Human,
    //        Culture = CharactersLore.Cultures.Human.Danarian,
    //        Tradition = CharactersLore.Tradition.Common,
    //        Class = CharactersLore.Classes.Warrior
    //    };

    //    var character = charService.SaveCharacterStub(origins, playerId);

    //    dbs.Snapshot.CharacterStubs.Count.Should().Be(0);
    //    character.Should().NotBeNull();

    //    character.Identity.Should().NotBeNull();
    //    character.Identity.Id.Should().NotBeNullOrWhiteSpace();
    //    character.Identity.PlayerId.Should().Be(playerId);
    //    character.Status.Name.Should().NotBeNullOrWhiteSpace();

    //    character.Status.Should().NotBeNull();
    //    character.Status.EntityLevel.Should().BeGreaterThanOrEqualTo(1);
    //    character.Status.Traits.Race.Should().Be(origins.Race);
    //    character.Status.Traits.Culture.Should().Be(origins.Culture);
    //    character.Status.Traits.Tradition.Should().Be(origins.Tradition);
    //    character.Status.Traits.Class.Should().Be(origins.Class);
    //    character.Status.Fame.Should().NotBeNullOrWhiteSpace();
    //    character.Status.Wealth.Should().BeGreaterThanOrEqualTo(1);
    //    character.Status.DateOfBirth.Should().Be(DateTime.Now.ToShortDateString());

    //    character.LevelUp.Should().NotBeNull();
    //    character.LevelUp.StatPoints.Should().Be(0);
    //    character.LevelUp.SkillPoints.Should().Be(0);

    //    character.Sheet.Should().NotBeNull();
    //    character.Sheet.Stats.Strength.Should().BeGreaterThanOrEqualTo(1);
    //    character.Sheet.Skills.Combat.Should().BeGreaterThanOrEqualTo(1);

    //    character.Inventory.Should().NotBeNull();

    //    character.Inventory.Supplies.Should().NotBeNull();
    //    character.Inventory.Supplies.Count.Should().BeGreaterThanOrEqualTo(1);

    //    character.Status.IsAlive.Should().BeTrue();
    //    character.Status.IsLockedToModify.Should().BeFalse();

    //    GameplayLore.Locations.All.Select(s => s.LocationName).Should().Contain(character.Status.Position.Location);
    //}

    //[Fact(DisplayName = "Modifing character name should have the new name")]
    //public void Rename_character_test()
    //{
    //    var newCharName = "Jax";

    //    var chr = CreateHumanCharacter("Jax");
    //    chr = charService.UpdateCharacterName(newCharName, CreateCharIdentity(chr));

    //    chr.Status.Name.Should().Be(newCharName);
    //}

    //[Fact(DisplayName = "Deleting a character should remove it from db")]
    //public void Delete_character_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");

    //    charService.DeleteCharacter(CreateCharIdentity(chr));
        
    //    var characters = charService.GetPlayerCharacters(chr.Identity.PlayerId);

    //    characters.CharactersList.Should().NotContain(chr);
    //    characters.Count.Should().Be(0);
    //}

    //[Fact(DisplayName = "Increasing the stats from a character should save it to db")]
    //public void Increase_stats_for_character_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");

    //    var currentStr = chr.Sheet.Stats.Strength;

    //    dbs.Snapshot.Players.Find(p => p.Identity.Id == chr.Identity.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Identity.Id)!.LevelUp.StatPoints = 1;

    //    chr = charService.UpdateCharacterStats(CharactersLore.Stats.Strength, CreateCharIdentity(chr));

    //    chr.Sheet.Stats.Strength.Should().BeGreaterThan(currentStr);
    //}

    //[Fact(DisplayName = "Increasing the assets from a character should save it to db")]
    //public void Increase_assets_for_character_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");

    //    var currentResolve = chr.Sheet.Assets.Resolve;

    //    Utils.GetPlayerCharacter(dbs.Snapshot, CreateCharIdentity(chr)).LevelUp.AssetPoints = 1;

    //    chr = charService.UpdateCharacterAssets(CharactersLore.Assets.Resolve, CreateCharIdentity(chr));

    //    chr.Sheet.Assets.Resolve.Should().BeGreaterThan(currentResolve);
    //}

    //[Fact(DisplayName = "Increasing the stats from a character with no stat points should throw")]
    //public void Increase_stats_with_no_points_for_character_should_throw_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");

    //    Assert.Throws<Exception>(() => charService.UpdateCharacterStats(CharactersLore.Stats.Strength, CreateCharIdentity(chr)));
    //}

    //[Fact(DisplayName = "Increasing the skills from a character should save it to db")]
    //public void Increase_skills_for_character_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");
    //    var currentCombat = chr.Sheet.Skills.Combat;

    //    dbs.Snapshot.Players.Find(p => p.Identity.Id == chr.Identity.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Identity.Id)!.LevelUp.SkillPoints = 1;

    //    chr = charService.UpdateCharacterSkills(CharactersLore.Skills.Combat, CreateCharIdentity(chr));

    //    chr.Sheet.Skills.Combat.Should().BeGreaterThan(currentCombat);
    //}

    //[Fact(DisplayName = "Increasing the skills from a character with no skill points should throw")]
    //public void Increase_skills_with_no_points_for_character_should_throw_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");

    //    Assert.Throws<Exception>(() => charService.UpdateCharacterSkills(CharactersLore.Skills.Combat, CreateCharIdentity(chr)));
    //}

    //[Fact(DisplayName = "Equipping an item in character inventory")]
    //public void Equip_item_on_character_inventory_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");

    //    if (chr.Inventory.Supplies.First().Subtype == ItemsLore.Subtypes.Wealth.Goods)
    //    {
    //        chr.Inventory.Supplies.Clear();
    //        chr.Inventory.Supplies.Add(itemService.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword));
    //    }

    //    var item = chr.Inventory.Supplies.First();
    //    item.Should().NotBeNull();

    //    var location = item.InventoryLocations.First();

    //    var equip = new CharacterEquip
    //    {
    //        CharacterIdentity = new CharacterIdentity 
    //        { 
    //            Id = chr.Identity.Id,
    //            PlayerId = chr.Identity.PlayerId,
    //        },
    //        InventoryLocation = location,
    //        ItemId = chr.Inventory.Supplies.First().Identity.Id
    //    };

    //    charService.CharacterEquipItem(equip);

    //    var hasEquipedItem =
    //        chr.Inventory.Head != null
    //        || chr.Inventory.Ranged != null
    //        || chr.Inventory.Body != null
    //        || chr.Inventory.Mainhand != null
    //        || chr.Inventory.Offhand != null
    //        || chr.Inventory.Shield != null;

    //    if (chr.Inventory.Heraldry!.Count > 0)
    //    {
    //        hasEquipedItem = chr.Inventory.Heraldry.First() != null;
    //    }

    //    hasEquipedItem.Should().BeTrue();
    //}

    //[Fact(DisplayName = "Unequipping an item from character inventory")]
    //public void Unequip_item_from_character_inventory_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");

    //    if (chr.Inventory.Supplies.First().Subtype == ItemsLore.Subtypes.Wealth.Goods)
    //    {
    //        chr.Inventory.Supplies.Clear();
    //        chr.Inventory.Supplies.Add(itemService.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword));
    //    }

    //    var item = chr.Inventory.Supplies.First();
    //    item.Should().NotBeNull();

    //    var location = item.InventoryLocations.First();

    //    var equip = new CharacterEquip
    //    {
    //        CharacterIdentity = new CharacterIdentity
    //        {
    //            Id = chr.Identity.Id,
    //            PlayerId = chr.Identity.PlayerId,
    //        },
    //        InventoryLocation = location,
    //        ItemId = chr.Inventory.Supplies.First().Identity.Id
    //    };

    //    charService.CharacterEquipItem(equip);
    //    charService.CharacterUnequipItem(equip);

    //    var hasEquipedItem =
    //        chr.Inventory.Head != null
    //        || chr.Inventory.Ranged != null
    //        || chr.Inventory.Body != null
    //        || chr.Inventory.Mainhand != null
    //        || chr.Inventory.Offhand != null
    //        || chr.Inventory.Shield != null;

    //    if (chr.Inventory.Heraldry!.Count > 0)
    //    {
    //        hasEquipedItem = chr.Inventory.Heraldry.First() != null;
    //    }

    //    hasEquipedItem.Should().BeFalse();
    //    chr.Inventory.Supplies.Count.Should().BeGreaterThanOrEqualTo(1);
    //}

    //[Fact(DisplayName = "Learning a common bonus heroic trait")]
    //public void Learn_common_bonus_heroic_trait_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");
    //    chr.LevelUp.DeedsPoints = 100;

    //    var swordsman = SpecialSkillsLore.All.Find(t => t.Identity.Name == SpecialSkillsLore.BonusSpecialSkills.Swordsman.Identity.Name)!;

    //    var trait = new CharacterAddSpecialSkill
    //    {
    //        CharacterIdentity = new CharacterIdentity
    //        {
    //            Id = chr.Identity.Id,
    //            PlayerId = chr.Identity.PlayerId,
    //        },
    //        SpecialSkillId = swordsman.Identity.Id,
    //    };

    //    var combatBeforeTrait = chr.Sheet.Skills.Combat;
    //    charService.CharacterLearnHeroicTrait(trait);
    //    var combatIncreasedOnce = chr.Sheet.Skills.Combat;

    //    combatBeforeTrait.Should().BeLessThan(combatIncreasedOnce);

    //    charService.CharacterLearnHeroicTrait(trait);
    //    var combatIncreasedTwice = chr.Sheet.Skills.Combat;

    //    combatIncreasedOnce.Should().BeLessThan(combatIncreasedTwice);
    //}

    //[Fact(DisplayName = "Learning a unique heroic trait twice throws error")]
    //public void Learn_unique_heroic_trait_throws_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");
    //    chr.LevelUp.DeedsPoints = 1000;

    //    var metachaos = SpecialSkillsLore.All.Find(t => t.Identity.Name == SpecialSkillsLore.ActivateSpecialSkills.MetachaosDaemonology.Identity.Name)!;

    //    var trait = new CharacterAddSpecialSkill
    //    {
    //        CharacterIdentity = new CharacterIdentity
    //        {
    //            Id = chr.Identity.Id,
    //            PlayerId = chr.Identity.PlayerId,
    //        },
    //        SpecialSkillId = metachaos.Identity.Id,
    //    };

    //    charService.CharacterLearnHeroicTrait(trait);

    //    Assert.Throws<Exception>(() => charService.CharacterLearnHeroicTrait(trait));
    //}

    //[Theory(DisplayName = "Character travel should move to new position")]
    //[InlineData("Dragonmaw_Farlindor_Danar_Belfordshire")]
    //[InlineData("Dragonmaw_Farlindor_Danar_Arada")]
    //public void Character_travel_test(string locationFullName)
    //{
    //    var chr = CreateHumanCharacter("Jax");
    //    chr.Inventory.Provisions.Should().BeGreaterThan(0);
    //    chr.Inventory.Provisions.Should().BeLessThanOrEqualTo(100);

    //    var travelToPosition = new CharacterTravel
    //    {
    //        CharacterIdentity = CreateCharIdentity(chr),
    //        Destination = Utils.GetPositionByLocationFullName(locationFullName)
    //    };

    //    var initialProvisions = chr.Inventory.Provisions;

    //    charService.CharacterTravelToLocation(travelToPosition);

    //    chr.Inventory.Provisions.Should().BeLessThan(initialProvisions);
    //}

    //[Fact(DisplayName = "Hiring a mercenary should remove it from location and add it to player")]
    //public void Hiring_mercenary_test()
    //{
    //    var chr = CreateHumanCharacter("Jax");
    //    chr.Status.Wealth = 10000;

    //    var location = gameplayService.GetLocation(chr.Status.Position);
    //    location.Mercenaries.Count.Should().BeGreaterThanOrEqualTo(1);

    //    var merc = location.Mercenaries.First();
    //    merc.Identity.Id.Should().NotBe(Guid.Empty.ToString());
    //    merc.Identity.PlayerId.Should().Be(Guid.Empty.ToString());

    //    charService.CharacterHireMercenary(new CharacterHireMercenary() { CharacterIdentity = CreateCharIdentity(chr), MercenaryId = location.Mercenaries.First().Identity.Id });

    //    chr.Mercenaries.Count.Should().Be(1);
    //    chr.Mercenaries.First().Identity.Id.Should().Be(merc.Identity.Id);
    //}


    //#region private methods
    //private static CharacterIdentity CreateCharIdentity(Character chr)
    //{
    //    return new CharacterIdentity
    //    {
    //        Id = chr.Identity.Id,
    //        PlayerId = chr.Identity.PlayerId
    //    };
    //}
    //#endregion
}
