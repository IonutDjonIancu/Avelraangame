//namespace Tests;

//public class PlayerServiceTests : TestBase
//{
//    public PlayerServiceTests()
//    {
//        dbm.Snapshot.Players.Clear();
//    }

//    [Theory]
//    [Description("Test to create two players, and then retrieve them from the db.")]
//    public void Created_players_should_be_in_db_test()
//    {
//        Request request_admin = new()
//        {
//            PlayerName = "iiancu85@gmail.com",
//            Token = "12345"
//        };
//        Request request_noAdmin = new()
//        {
//            PlayerName = "someEmail@gmail.com",
//            Token = "1234"
//        };

//        playerService.AuthorizePlayer(request_admin);
//        playerService.AuthorizePlayer(request_noAdmin);

//        var adminPlayer = dbm.Snapshot.Players.FirstOrDefault(p => p.Identity.Name == request_admin.PlayerName);
//        adminPlayer.Should().NotBeNull();
//        adminPlayer.Identity.Id.Should().NotBeNullOrWhiteSpace();
//        adminPlayer.LastAction.Should().NotBeNullOrWhiteSpace();
//        adminPlayer.Identity.Token.Should().NotBeNullOrWhiteSpace();
//        adminPlayer.IsAdmin.Should().BeTrue();

//        var normalPlayer = dbm.Snapshot.Players.FirstOrDefault(p => p.Identity.Name == request_noAdmin.PlayerName);
//        normalPlayer.Should().NotBeNull();
//        normalPlayer.Identity.Id.Should().NotBeNullOrWhiteSpace();
//        normalPlayer.LastAction.Should().NotBeNullOrWhiteSpace();
//        normalPlayer.Identity.Token.Should().NotBeNullOrWhiteSpace();
//        normalPlayer.IsAdmin.Should().BeFalse();
//    }

//    [Theory]
//    [Description("Validate player properties before authorization process.")]
//    public void Invalid_player_properties_should_throw_test()
//    {
//        Request request_noId =      new();
//        Request request_noEmail =   new() { Token = "1234" };
//        Request request_badEmail =  new() { PlayerName = "asdas", Token = "1234" };
//        Request request_noToken =   new() { PlayerName = "asddas@gmail.com" };

//#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
//        Assert.Throws<Exception>(() => playerService.AuthorizePlayer(null));
//#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
//        Assert.Throws<Exception>(() => playerService.AuthorizePlayer(request_noId));
//        Assert.Throws<Exception>(() => playerService.AuthorizePlayer(request_noEmail));
//        Assert.Throws<Exception>(() => playerService.AuthorizePlayer(request_badEmail));
//        Assert.Throws<Exception>(() => playerService.AuthorizePlayer(request_noToken));
//    }

//    [Theory]
//    [Description("Update player will modify the existing one in the db snapshot.")]
//    public void Update_player_modifies_the_existing_one_test()
//    {
//        var email = "test@gmail.com";
//        var token = "1234";
//        var newName = "new name";

//        Request request = new() { PlayerName = email, Token = token };

//        playerService.AuthorizePlayer(request);

//        PlayerUpdate playerUpdate = new()
//        {
//            Name = newName,
//        };

//        playerService.UpdatePlayerName(playerUpdate);

//        var player = dbm.Snapshot.Players.FirstOrDefault(p => p.Identity.Name == email);

//        player.Identity.Name.Should().Be(newName);
//    }

//    [Theory]
//    [Description("Deleting a player should remove the player.")]
//    public void Delete_player_should_remove_it_test()
//    {
//        var email = "test@gmail.com";
//        var token = "1234";

//        Request request = new() { PlayerName = email, Token = token };

//        playerService.AuthorizePlayer(request);

//        var numberOfPlayersBeforeDelete = dbm.Snapshot.Players.Count;

//        var player = dbm.Snapshot.Players.FirstOrDefault(p => p.Identity.Name == email);
//        dbm.Snapshot.Players.Remove(player);

//        var numberOfPlayersAfterDelete = dbm.Snapshot.Players.Count;

//        numberOfPlayersAfterDelete.Should().BeLessThan(numberOfPlayersBeforeDelete);
//    }
//}