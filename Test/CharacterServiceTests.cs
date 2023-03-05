#pragma warning disable CS8602 // Dereference of a possibly null reference.

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
            Tradition = CharactersLore.Traditions.Ravanon,
            Class = CharactersLore.Classes.Warrior
        };

        var character = charService.SaveCharacterStub(origins, playerId);

        dbm.Snapshot.CharacterStubs.Count.Should().Be(0);
        character.Should().NotBeNull();

        character.Identity.Should().NotBeNull();
        character.Identity.Id.Should().NotBeNullOrWhiteSpace();
        character.Identity.PlayerId.Should().Be(playerId);

        character.Info.Should().NotBeNull();
        character.Info.Name.Should().NotBeNullOrWhiteSpace();
        character.Info.EntityLevel.Should().BeGreaterThanOrEqualTo(1);
        character.Info.Race.Should().Be(origins.Race);
        character.Info.Culture.Should().Be(origins.Culture);
        character.Info.Tradition.Should().Be(origins.Tradition);
        character.Info.Class.Should().Be(origins.Class);
        character.Info.Fame.Should().NotBeNullOrWhiteSpace();
        character.Info.Wealth.Should().BeGreaterThanOrEqualTo(1);
        character.Info.DateOfBirth.Should().Be(DateTime.Now.ToShortDateString());

        character.LevelUp.Should().NotBeNull();
        character.LevelUp.StatPoints.Should().Be(0);
        character.LevelUp.SkillPoints.Should().Be(0);

        character.Doll.Should().NotBeNull();
        character.Doll.Strength.Should().BeGreaterThanOrEqualTo(1);

        character.Traits.Should().NotBeNull();

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
            Tradition = CharactersLore.Traditions.Ravanon,
            Class = CharactersLore.Classes.Warrior
        };

        var character = charService.SaveCharacterStub(origins, playerId);
        character = charService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Name = newCharName
        }, playerId);

        character.Info.Name.Should().Be(newCharName);
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
            Tradition = CharactersLore.Traditions.Ravanon,
            Class = CharactersLore.Classes.Warrior
        };

        var character = charService.SaveCharacterStub(origins, playerId);

        charService.DeleteCharacter(character.Identity.Id, playerId);

        var characters = charService.GetCharacters(playerName);

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
            Tradition = CharactersLore.Traditions.Ravanon,
            Class = CharactersLore.Classes.Warrior
        };

        var character = charService.SaveCharacterStub(origins, playerId);

        var currentStr = character.Doll.Strength;

        dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId).Characters.Find(c => c.Identity.Id == character.Identity.Id).LevelUp.StatPoints = 1;

        character = charService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Stat = CharactersLore.Stats.Strength
        }, playerId);

        character.Doll.Strength.Should().Be(currentStr + 1);
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
            CharactersLore.Traditions.Ravanon,
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
            Tradition = CharactersLore.Traditions.Ravanon,
            Class = CharactersLore.Classes.Warrior
        };

        var character = charService.SaveCharacterStub(origins, playerId);

        var currentCombat = character.Doll.Combat;

        dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId).Characters.Find(c => c.Identity.Id == character.Identity.Id).LevelUp.SkillPoints = 1;

        character = charService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Skill = CharactersLore.Skills.Combat
        }, playerId);

        character.Doll.Combat.Should().Be(currentCombat + 1);
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
            CharactersLore.Traditions.Ravanon,
            CharactersLore.Classes.Warrior,
            playerId);

        Assert.Throws<Exception>(() => character = charService.UpdateCharacter(new CharacterUpdate
        {
            CharacterId = character.Identity.Id,
            Skill = CharactersLore.Skills.Combat
        }, playerId));
    }

    #region privates
    private string CreatePlayer()
    {
        dbm.Snapshot.Players!.Clear();
        playerService.CreatePlayer(playerName);

        return dbm.Snapshot.Players!.Find(p => p.Identity.Name == playerName)!.Identity.Id;
    }

    private Character CreateCharacter(string race, string culture, string tradition, string classes, string playerId)
    {
        dbm.Snapshot.CharacterStubs.Clear();

        charService.CreateCharacterStub(playerId);

        var origins = new CharacterOrigins
        {
            Race = race,
            Culture = culture,
            Tradition = tradition,
            Class = classes
        };

        return charService.SaveCharacterStub(origins, playerId);
    }
    #endregion
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
