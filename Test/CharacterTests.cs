namespace Tests;

[Collection("CharacterTests")]
[Trait("Category", "CharacterServiceTests")]
public class CharacterTests : TestBase
{
    private const string PlayerName = "JoeDoe";

    [Fact(DisplayName = "Create character stub should exist in snapshot")]
    public void CreateCharacterStub()
    {
        // Arrange
        var playerId = TestUtils.CreatePlayer(PlayerName, _players, _snapshot);

        // Act
        _characters.CreateCharacterStub(playerId);

        // Assert
        _snapshot.Stubs.Count.Should().Be(1);
    }

    [Fact(DisplayName = "Save character stub should exist as new character in player")]
    public void RecreateCharacterStub()
    {
        var player = TestUtils.CreateAndGetPlayer(PlayerName, _players, _snapshot);

        _characters.CreateCharacterStub(player.Identity.Id);

        var charTraits = new CharacterRacialTraits
        {
            Race = CharactersLore.Races.Playable.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Class = CharactersLore.Classes.Warrior,
            Tradition = GameplayLore.Tradition.Martial
        };

        _characters.SaveCharacterStub(charTraits, player.Identity.Id);

        player.Characters.Should().NotBeNull();
        player.Characters.Count.Should().Be(1);

        var character = player.Characters.First()!;

        character.Identity.Id.Should().NotBeNullOrWhiteSpace();
        character.Identity.PlayerId.Should().Be(player.Identity.Id);

        character.Status.Traits.Race.Should().Be(CharactersLore.Races.Playable.Human);
        character.Status.Traits.Class.Should().Be(CharactersLore.Classes.Warrior);
        character.Status.Name.Should().NotBeNullOrWhiteSpace();
        character.Status.EntityLevel.Should().BeGreaterThanOrEqualTo(1);
        character.Status.Fame.Should().NotBeNullOrWhiteSpace();
        character.Status.Wealth.Should().BeGreaterThanOrEqualTo(1);
        character.Status.Worth.Should().BeGreaterThanOrEqualTo(1);
        character.Status.DateOfBirth.Should().Be(DateTime.Now.ToShortDateString());
        character.Status.IsAlive.Should().BeTrue();
        character.Status.IsLockedToModify.Should().BeFalse();

        character.LevelUp.DeedPoints.Should().Be(0);
        character.LevelUp.StatPoints.Should().Be(0);
        character.LevelUp.AssetPoints.Should().Be(0);
        character.LevelUp.SkillPoints.Should().Be(0);

        character.Sheet.Stats.Strength.Should().BeGreaterThanOrEqualTo(1);
        character.Sheet.Assets.Harm.Should().BeGreaterThanOrEqualTo(1);
        character.Sheet.Skills.Combat.Should().BeGreaterThanOrEqualTo(1);

        character.Inventory.Supplies.Count.Should().BeGreaterThanOrEqualTo(1);

        GameplayLore.Locations.All.Select(s => s.LocationName).Should().Contain(character.Status.Position.Location);
    }

    [Fact(DisplayName = "Modifing character name should have the new name")]
    public void CharacterUpdateNameTest()
    {
        var character = CreateCharacter();

        var newName = "new name";

        _characters.UpdateCharacterName(newName, TestUtils.GetCharacterIdentity(character));

        character.Status.Name.Should().Be(newName);
    }

    [Fact(DisplayName = "Wrong character name should throw")]
    public void CharacterUpdateWrongNameTest()
    {
        var character = CreateCharacter();

        var newName = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        Assert.Throws<Exception>(() => _characters.UpdateCharacterName(newName, TestUtils.GetCharacterIdentity(character)));
    }

    [Fact(DisplayName = "Deleting a character should remove it from player on snapshot")]
    public void DeleteCharacterTest()
    {
        var character = TestUtils.CreateAndGetCharacter(PlayerName, _players, _characters, _snapshot);

        _characters.DeleteCharacter(TestUtils.GetCharacterIdentity(character));

        var player = TestUtils.GetPlayer(character.Identity.PlayerId, _snapshot);

        player.Characters.Count.Should().Be(0);
    }

    [Fact(DisplayName = "Increasing the attributes from a character should save them accordingly")]
    public void IncreaseAttributesTest()
    {
        var character = CreateCharacter();
        var oldStr = character.Sheet.Stats.Strength;
        var oldHar = character.Sheet.Assets.Harm;
        var oldCom = character.Sheet.Skills.Combat;

        character.LevelUp.StatPoints = 100;
        character.LevelUp.AssetPoints = 100;
        character.LevelUp.SkillPoints = 100;

        _characters.IncreaseCharacterStats(CharactersLore.Stats.Strength, GetCharIdentity(character));
        _characters.IncreaseCharacterAssets(CharactersLore.Assets.Harm, GetCharIdentity(character));
        _characters.IncreaseCharacterSkills(CharactersLore.Skills.Combat, GetCharIdentity(character));

        character.Sheet.Stats.Strength.Should().BeGreaterThan(oldStr);
        character.Sheet.Assets.Harm.Should().BeGreaterThan(oldHar);
        character.Sheet.Skills.Combat.Should().BeGreaterThan(oldCom);
    }

    [Fact(DisplayName = "Increasing the attributes from a character with no attributes points should throw")]
    public void IncreaseAttributesNoPointsTest()
    {
        var character = CreateCharacter();

        Assert.Throws<Exception>(() => _characters.IncreaseCharacterStats(CharactersLore.Stats.Strength, GetCharIdentity(character)));
        Assert.Throws<Exception>(() => _characters.IncreaseCharacterAssets(CharactersLore.Assets.Harm, GetCharIdentity(character)));
        Assert.Throws<Exception>(() => _characters.IncreaseCharacterSkills(CharactersLore.Skills.Combat, GetCharIdentity(character)));
    }

    [Fact(DisplayName = "Increasing the attributes from a character with wrong attribute name should throw")]
    public void IncreaseAttributesWrongNameTest()
    {
        var character = CreateCharacter();

        character.LevelUp.StatPoints = 100;

        Assert.Throws<Exception>(() => _characters.IncreaseCharacterStats("WrongAttributeName", GetCharIdentity(character)));
    }

    [Fact(DisplayName = "Equipping an item in character inventory should display on character and bonuses should be on character sheet")]
    public void EquipItemTest()
    {
        var character = CreateCharacter();
        var oldStr = character.Sheet.Stats.Strength;
        var oldHar = character.Sheet.Assets.Harm;
        var oldCom = character.Sheet.Skills.Combat;

        var strValue = 100;
        var harValue = 100;
        var comValue = 100;

        var item = _items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword);
        item.Sheet.Stats.Strength = strValue;
        item.Sheet.Assets.Harm = harValue;
        item.Sheet.Skills.Combat = comValue;

        character.Inventory.Supplies.Clear();
        character.Inventory.Supplies.Add(item);

        var equip = new CharacterEquip
        {
            CharacterIdentity = GetCharIdentity(character),
            InventoryLocation = ItemsLore.InventoryLocation.Mainhand,
            ItemId = item.Identity.Id
        };

        _characters.EquipCharacterItem(equip);

        character.Inventory.Supplies.Count.Should().Be(0);
        character.Inventory.Mainhand.Should().Be(item);
        character.Sheet.Stats.Strength.Should().Be(oldStr + strValue);
        character.Sheet.Assets.Harm.Should().Be(oldHar + harValue);
        character.Sheet.Skills.Combat.Should().Be(oldCom + comValue);
    }

    [Fact(DisplayName = "Equipping an item in the wrong slot should throw")]
    public void EquipItemWrongSlotTest()
    {
        var character = new Character();
        var item = _items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword);

        var equip = new CharacterEquip
        {
            CharacterIdentity = GetCharIdentity(character),
            InventoryLocation = ItemsLore.InventoryLocation.Body,
            ItemId = item.Identity.Id
        };

        Assert.Throws<Exception>(() => _characters.EquipCharacterItem(equip));
    }

    [Fact(DisplayName = "Unequipping an item from character inventory should display on character supplies and bonuses should be removed from character sheet")]
    public void UnequipItemTest()
    {
        var character = CreateCharacter();
        var oldStr = character.Sheet.Stats.Strength;
        var oldHar = character.Sheet.Assets.Harm;
        var oldCom = character.Sheet.Skills.Combat;

        var strValue = 100;
        var harValue = 100;
        var comValue = 100;

        var item = _items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword);
        item.Sheet.Stats.Strength = strValue;
        item.Sheet.Assets.Harm = harValue;
        item.Sheet.Skills.Combat = comValue;

        character.Inventory.Supplies.Clear();
        character.Inventory.Supplies.Add(item);

        var equip = new CharacterEquip
        {
            CharacterIdentity = GetCharIdentity(character),
            InventoryLocation = ItemsLore.InventoryLocation.Mainhand,
            ItemId = item.Identity.Id
        };

        _characters.EquipCharacterItem(equip);
        _characters.UnequipCharacterItem(equip);

        character.Inventory.Supplies.Count.Should().Be(1);
        character.Inventory.Mainhand.Should().BeNull();
        character.Sheet.Stats.Strength.Should().Be(oldStr);
        character.Sheet.Assets.Harm.Should().Be(oldHar);
        character.Sheet.Skills.Combat.Should().Be(oldCom);
    }

    [Fact(DisplayName = "Swap character item should display new item, old item goes to supplies")]
    public void SwapItemTest()
    {
        var character = CreateCharacter();

        var item1 = _items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword);
        var item2 = _items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword);

        character.Inventory.Supplies.Clear();
        character.Inventory.Supplies.Add(item1);
        character.Inventory.Supplies.Add(item2);

        var equip1 = new CharacterEquip
        {
            CharacterIdentity = GetCharIdentity(character),
            InventoryLocation = ItemsLore.InventoryLocation.Mainhand,
            ItemId = item1.Identity.Id
        };

        var equip2 = new CharacterEquip
        {
            CharacterIdentity = GetCharIdentity(character),
            InventoryLocation = ItemsLore.InventoryLocation.Mainhand,
            ItemId = item2.Identity.Id
        };

        _characters.EquipCharacterItem(equip1);

        character.Inventory.Mainhand.Should().Be(item1);

        _characters.EquipCharacterItem(equip2);

        character.Inventory.Mainhand.Should().Be(item2);
        character.Inventory.Supplies.First().Should().Be(item1);
    }

    [Fact(DisplayName = "Learning a special skill should display on character")]
    public void LearnSpecialSkillTest()
    {
        var character = CreateCharacter();
        var deedPtsInitialValue = 1000;
        character.LevelUp.DeedPoints = deedPtsInitialValue;

        var specialSkillAdd = new CharacterAddSpecialSkill
        {
            CharacterIdentity = GetCharIdentity(character),
            SpecialSkillId = SpecialSkillsLore.ActiveSpecialSkills.MetachaosDaemonology.Identity.Id,
            AppliesToSkill = string.Empty
        };

        _characters.LearnCharacterSpecialSkill(specialSkillAdd);

        character.Sheet.SpecialSkills.Count.Should().Be(1);
        character.Sheet.SpecialSkills.First().Identity.Name.Should().Be(SpecialSkillsLore.ActiveSpecialSkills.MetachaosDaemonology.Identity.Name);
        character.LevelUp.DeedPoints.Should().Be(deedPtsInitialValue - SpecialSkillsLore.ActiveSpecialSkills.MetachaosDaemonology.DeedsCost);
    }

    [Fact(DisplayName = "Learning a special skill with no deedpoints throws error")]
    public void LearnSpecialSkillNoPtsTest()
    {
        var character = CreateCharacter();

        var specialSkillAdd = new CharacterAddSpecialSkill
        {
            CharacterIdentity = GetCharIdentity(character),
            SpecialSkillId = SpecialSkillsLore.ActiveSpecialSkills.MetachaosDaemonology.Identity.Id,
            AppliesToSkill = string.Empty
        };

        Assert.Throws<Exception>(() => _characters.LearnCharacterSpecialSkill(specialSkillAdd));
    }

    [Fact(DisplayName = "Learning a unique spsk twice throws error")]
    public void LearnUniqueSpecialSkillTwiceTest()
    {
        var character = CreateCharacter();
        var deedPtsInitialValue = 1000;
        character.LevelUp.DeedPoints = deedPtsInitialValue;

        var specialSkillAdd = new CharacterAddSpecialSkill
        {
            CharacterIdentity = GetCharIdentity(character),
            SpecialSkillId = SpecialSkillsLore.ActiveSpecialSkills.MetachaosDaemonology.Identity.Id,
            AppliesToSkill = string.Empty
        };

        _characters.LearnCharacterSpecialSkill(specialSkillAdd);
        Assert.Throws<Exception>(() => _characters.LearnCharacterSpecialSkill(specialSkillAdd));
    }


    [Fact(DisplayName = "Hiring a mercenary should remove it from location and add it to player")]
    public void HireMercenaryTest()
    {
        var character = CreateCharacter();
        character.Status.Wealth = 1000;

        var location = _gameplay.GetOrGenerateLocation(character.Status.Position);

        var merc = location.Mercenaries.First();

        var hireMerc = new CharacterHireMercenary
        {
            CharacterIdentity = GetCharIdentity(character),
            MercenaryId = merc.Identity.Id
        };

        _characters.HireMercenaryForCharacter(hireMerc);

        character.Mercenaries.Count.Should().Be(1);
        character.Mercenaries.First().Should().Be(merc);
        location.Mercenaries.Exists(s => s.Identity.Id == merc.Identity.Id).Should().BeFalse();
    }

    [Fact(DisplayName = "Kill characters should display on character status")] 
    public void KillCharacterTest()
    {
        var character = CreateCharacter();

        _characters.KillCharacter(GetCharIdentity(character));

        character.Status.IsAlive.Should().BeFalse();
    }

    [Fact(DisplayName = "Locked character cannot be modified")]
    public void LockedCharacterTest()
    {
        var character = CreateCharacter();
        var charIdentity = GetCharIdentity(character);
        character.LevelUp.DeedPoints = 100;
        character.LevelUp.StatPoints = 100;
        character.LevelUp.AssetPoints = 100;
        character.LevelUp.SkillPoints = 100;

        var item = _items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword);
        character.Inventory.Supplies.Clear();
        character.Inventory.Supplies.Add(item);
        var equip = new CharacterEquip
        {
            CharacterIdentity = GetCharIdentity(character),
            InventoryLocation = ItemsLore.InventoryLocation.Mainhand,
            ItemId = item.Identity.Id
        };

        var location = _gameplay.GetOrGenerateLocation(character.Status.Position);
        var merc = location.Mercenaries.First();
        var hireMerc = new CharacterHireMercenary
        {
            CharacterIdentity = GetCharIdentity(character),
            MercenaryId = merc.Identity.Id
        };

        var specialSkillAdd = new CharacterAddSpecialSkill
        {
            CharacterIdentity = GetCharIdentity(character),
            SpecialSkillId = SpecialSkillsLore.ActiveSpecialSkills.MetachaosDaemonology.Identity.Id,
            AppliesToSkill = string.Empty
        };

        var charTravel = new CharacterTravel
        {
            CharacterIdentity = GetCharIdentity(character),
            Destination = Utils.GetPositionByLocationFullName(GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Belfordshire.FullName),
        };

        character.Status.IsLockedToModify = true;

        // not allowed actions
        Assert.Throws<Exception>(() => _characters.DeleteCharacter(charIdentity));
        Assert.Throws<Exception>(() => _characters.EquipCharacterItem(equip));
        Assert.Throws<Exception>(() => _characters.UnequipCharacterItem(equip));
        Assert.Throws<Exception>(() => _characters.UpdateCharacterName("newName", charIdentity));
        Assert.Throws<Exception>(() => _characters.AddCharacterWealth(100, charIdentity));
        Assert.Throws<Exception>(() => _characters.IncreaseCharacterStats(CharactersLore.Stats.Strength, charIdentity));
        Assert.Throws<Exception>(() => _characters.IncreaseCharacterAssets(CharactersLore.Assets.Harm, charIdentity));
        Assert.Throws<Exception>(() => _characters.IncreaseCharacterSkills(CharactersLore.Skills.Combat, charIdentity));
        Assert.Throws<Exception>(() => _characters.HireMercenaryForCharacter(hireMerc));
        Assert.Throws<Exception>(() => _characters.LearnCharacterSpecialSkill(specialSkillAdd));
        Assert.Throws<Exception>(() => _characters.TravelCharacterToLocation(charTravel));

        // excepted action
        _characters.KillCharacter(charIdentity);
        _characters.AddCharacterFame("This is the new fame.", charIdentity);
    }

    [Theory(DisplayName = "Character travel should move to new position")]
    [InlineData("Dragonmaw_Farlindor_Danar_Belfordshire")]
    [InlineData("Dragonmaw_Farlindor_Danar_Arada")]
    public void CharacterTravelTest(string locationFullName)
    {
        var character = CreateCharacter();
        var provInitialValue = 100;
        character.Inventory.Provisions = provInitialValue;

        var charTravel = new CharacterTravel
        {
            CharacterIdentity = GetCharIdentity(character),
            Destination = Utils.GetPositionByLocationFullName(locationFullName),
        };

        _characters.TravelCharacterToLocation(charTravel);

        character.Inventory.Provisions.Should().BeLessThan(provInitialValue);
    }

    [Theory(DisplayName = "Wrong character traits should throw")]
    [InlineData(CharactersLore.Races.Playable.Human, CharactersLore.Cultures.Elf.Highborn, CharactersLore.Classes.Warrior, GameplayLore.Tradition.Martial)]
    [InlineData("wrong race input", CharactersLore.Cultures.Elf.Highborn, CharactersLore.Classes.Warrior, GameplayLore.Tradition.Martial)]
    [InlineData(CharactersLore.Races.Playable.Human, "wrong culture input", CharactersLore.Classes.Warrior, GameplayLore.Tradition.Martial)]
    [InlineData(CharactersLore.Races.Playable.Human, CharactersLore.Cultures.Elf.Highborn, "wrong class input", GameplayLore.Tradition.Martial)]
    [InlineData(CharactersLore.Races.Playable.Human, CharactersLore.Cultures.Elf.Highborn, CharactersLore.Classes.Warrior, "wrong tradition input")]
    public void WrongTraitsTest(string race, string culture, string classes, string tradition)
    {
        var playerId = TestUtils.CreatePlayer(PlayerName, _players, _snapshot);

        _characters.CreateCharacterStub(playerId);

        var charTraits = new CharacterRacialTraits
        {
            Race = race,
            Culture = culture,
            Class = classes,
            Tradition = tradition
        };

        Assert.Throws<Exception>(() => _characters.SaveCharacterStub(charTraits, playerId));
    }

    #region private methods
    private Character CreateCharacter()
    {
        return TestUtils.CreateAndGetCharacter(PlayerName, _players, _characters, _snapshot);
    }

    private static CharacterIdentity GetCharIdentity(Character character)
    {
        return TestUtils.GetCharacterIdentity(character);
    }
    #endregion
}
