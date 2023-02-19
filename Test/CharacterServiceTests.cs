//namespace Tests;

//public class CharacterServiceTests : TestBase
//{
//    private static readonly string email = "player@gmail.com";
//    private static readonly string token = "1234";
//    public Player Player { get; init; }


//    public CharacterServiceTests()
//    {
//        Player = new Player()
//        {
//            Email = email,
//            Token = token
//        };

//        playerService.AuthorizePlayer(new Request()
//        {
//            Email = email,
//            Token = token
//        });
//    }

//    [Theory]
//    [Description("Creating a character template with no player id should throw.")]
//    public void Create_a_character_template_with_no_playerId_should_throw_test()
//    {
//        var player = playerService.GetPlayerByEmail(email);

//        Request request = new() 
//        { 
//            Token = token 
//        };

//        Assert.Throws<Exception>(() => charService.CreateTemplate(request.Email));
//    }

//    [Theory]
//    [Description("Test to create a character template.")]
//    public void Create_a_character_template_test()
//    {
//        var template = charService.CreateTemplate(email);

//        template.Should().NotBeNull();
//        template.PlayerId.Should().NotBeNullOrWhiteSpace();
//        template.Id.Should().NotBeNullOrWhiteSpace();
//        template.DateOfBirth.Should().NotBeNullOrWhiteSpace();
//        template.EntityLevel.Should().BeGreaterThan(0);
//        template.LevelUp.StatPoints.Should().BeGreaterThan(0);
//        template.LevelUp.SkillPoints.Should().BeGreaterThan(0);
//    }

//    [Theory]
//    [Description("Test to save a character template.")]
//    public void Save_a_character_template_test()
//    {
//        var player = playerService.GetPlayerByEmail(email);

//        player.Characters = new();

//        Character character = new()
//        {
//            PlayerId = player.Id,
//            Name = player.Name,
//        };

//        var template = charService.CreateTemplate(character);

//        player.Characters.Count.Should().Be(0);

//        template.Tradition = CharactersLore.Traditions.Ravanon;
//        template.Race = CharactersLore.Races.Human;
//        template.Culture = CharactersLore.Cultures.Human.Danarian;

//        charService.SaveTemplate(template);
//        player.Characters.Count.Should().Be(1);
//    }

//    [Theory]
//    [Description("Saving a character template with a wrong race-culture combination should throw.")]
//    public void Save_a_character_template_with_wrong_race_culture_should_throw_test()
//    {
//        var player = playerService.GetPlayerByEmail(email);

//        Character character = new()
//        {
//            PlayerId = player.Id,
//            Name = player.Name,
//        };

//        var template = charService.CreateTemplate(character);

//        template.Tradition = CharactersLore.Traditions.Ravanon;
//        template.Race = CharactersLore.Races.Human;
//        template.Culture = CharactersLore.Cultures.Elf.Highborn;

//        Assert.Throws<Exception>(() => charService.SaveTemplate(template));
//    }

//    [Theory]
//    [Description("Updating a character should update player's character list.")]
//    public void Update_character_test()
//    {
//        var charNewName = "new name";

//        var character = CreateHumanDanarian();
//        character.Name = charNewName;

//        charService.UpdateCharacter(character);

//        dbm.Readmetadata.Character_getCharacterByPlayerId(character.Id, character.PlayerId).Name.Should().Be(charNewName);
//    }

//    [Theory]
//    [Description("Deleting a character should remove it from the player owner.")]
//    public void Delete_character_test()
//    {
//        var character = CreateHumanDanarian();

//        charService.DeleteCharacter(character);

//        dbm.Readmetadata.Character_getCharacterByPlayerId(character.Id, character.PlayerId).Should().BeNull();
//    }

//    [Theory]
//    [Description("Updating stat points on character doll.")]
//    public void Update_character_stats_test()
//    {
//        var character = CreateHumanDanarian();
//        var amount = character.Doll.Strength + (int)character.StatPoints!;

//        character.Doll.Strength = amount;
//        character.StatPoints = 0;

//        charService.UpdateCharacter(character);

//        dbm.Readmetadata.Character_getCharacterByPlayerId(character.Id, character.PlayerId)
//            .Doll.Strength.Should().Be(amount);

//        dbm.Readmetadata.Character_getCharacterByPlayerId(character.Id, character.PlayerId)
//            .StatPoints.Should().Be(0);
//    }

//    [Theory]
//    [Description("Updating wrong stat points on character doll should throw.")]
//    public void Update_wrong_character_stats_should_throw_test()
//    {
//        var character = CreateHumanDanarian();
//        var amount = character.Doll.Strength + (int)character.StatPoints! + 10;

//        var newCharacter = new Character()
//        {
//            Id = character.Id,
//            PlayerId = character.PlayerId,
//            Name = character.Name,
//            Doll = new PaperDoll()
//            {
//                Strength = amount,
//            },
//            StatPoints = 0
//        };

//        Assert.Throws<Exception>(() => charService.UpdateCharacter(newCharacter));
//    }

//    [Theory]
//    [Description("Updating skill points on character doll.")]
//    public void Update_character_skills_test()
//    {
//        var character = CreateHumanDanarian();
//        var amount = character.Doll.Combat + (int)character.SkillPoints!;

//        character.Doll.Combat = amount;
//        character.SkillPoints = 0;

//        charService.UpdateCharacter(character);

//        dbm.Readmetadata.Character_getCharacterByPlayerId(character.Id, character.PlayerId)
//            .Doll.Combat.Should().Be(amount);

//        dbm.Readmetadata.Character_getCharacterByPlayerId(character.Id, character.PlayerId)
//            .SkillPoints.Should().Be(0);
//    }

//    [Theory]
//    [Description("Updating wrong skill points on character doll should throw.")]
//    public void Update_wrong_character_skills_should_throw_test()
//    {
//        var character = CreateHumanDanarian();
//        var amount = character.Doll.Combat + (int)character.SkillPoints! + 10;

//        var newCharacter = new Character()
//        {
//            Id = character.Id,
//            PlayerId = character.PlayerId,
//            Name = character.Name,
//            Doll = new PaperDoll()
//            {
//                Combat = amount,
//            },
//            SkillPoints = 0
//        };

//        Assert.Throws<Exception>(() => charService.UpdateCharacter(newCharacter));
//    }

//    [Theory]
//    [Description("Cheating stat points on character doll should throw.")]
//    public void Cheating_character_stats_should_throw_test()
//    {
//        var character = CreateHumanDanarian();
//        var amount = character.Doll.Strength + (int)character.StatPoints!;

//        var newCharacter = new Character()
//        {
//            Id = character.Id,
//            PlayerId = character.PlayerId,
//            Name = character.Name,
//            Doll = new PaperDoll()
//            {
//                Strength = amount,
//            },
//            StatPoints = 10
//        };

//        Assert.Throws<Exception>(() => charService.UpdateCharacter(newCharacter));
//    }

//    [Theory]
//    [Description("Cheating skill points on character doll should throw.")]
//    public void Cheating_character_skills_should_throw_test()
//    {
//        var character = CreateHumanDanarian();
//        var amount = character.Doll.Combat + (int)character.SkillPoints!;

//        var newCharacter = new Character()
//        {
//            Id = character.Id,
//            PlayerId = character.PlayerId,
//            Name = character.Name,
//            Doll = new PaperDoll()
//            {
//                Combat = amount,
//            },
//            SkillPoints = 10
//        };

//        Assert.Throws<Exception>(() => charService.UpdateCharacter(newCharacter));
//    }

//    #region privates
//    private Character CreateHumanDanarian()
//    {
//        var player = playerService.GetPlayerByEmail(email);
//        player.Characters = new();

//        Character newChar = new()
//        {
//            PlayerId = player.Id,
//            Name = "Danarian Human",
//        };

//        var template = charService.CreateTemplate(newChar);

//        template.Tradition = CharactersLore.Traditions.Ravanon;
//        template.Race = CharactersLore.Races.Human;
//        template.Culture = CharactersLore.Cultures.Human.Danarian;

//        charService.SaveTemplate(template);

//        var characterId = player.Characters.First().Id;

//        return dbm.Readmetadata.Character_getCharacterByPlayerId(characterId, player.Id);
//    }
//    #endregion
//}
