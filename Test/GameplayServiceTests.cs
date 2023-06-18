namespace Tests;

public class GameplayServiceTests : TestBase
{
    #region happy path
    [Theory]
    [Description("Create party.")]
    public void Create_party_test()
    {
        var party = CreateParty();

        party.Should().NotBeNull();
        party.IsAdventuring.Should().BeFalse();
        party.Identity.PartyLeadId.Should().BeEmpty();
        party.Food.Should().Be(1);

        dbs.Snapshot.Parties.Count.Should().Be(1);
    }

    [Theory]
    [Description("Join party.")]
    public void Join_party_test()
    {
        var party = CreateParty();
        var chr = CreateHumanCharacter("Jax");

        gameplayService.JoinParty(party.Identity.Id, GetCharacterIdentity(chr));

        party.Characters.Count.Should().Be(1);
        party.Identity.PartyLeadId.Should().Be(chr.Identity.Id);
    }

    [Theory]
    [Description("Leave party.")]
    public void Leave_party_test()
    {
        var party = CreateParty();
        var chr = CreateHumanCharacter("Jax");
        var charIdentity = GetCharacterIdentity(chr);

        gameplayService.JoinParty(party.Identity.Id, charIdentity);
        gameplayService.LeaveParty(party.Identity.Id, charIdentity);

        chr.Info.IsInParty.Should().BeFalse();
        dbs.Snapshot.Parties.Count.Should().Be(0);
        party.Identity.PartyLeadId.Should().BeEmpty();
    }
    #endregion

    #region validations
    [Theory]
    [Description("When creating a new party, older empty parties should be deleted.")]
    public void Create_party_should_delete_older_parties_test()
    {
        var party = CreateParty();

        var canParseDate = DateTime.TryParse(dbs.Snapshot.Parties.First().CreationDate, out var justNowDate);
        canParseDate.Should().BeTrue();

        var olderDate = justNowDate - DateTime.Now.AddDays(-70);
        party.CreationDate = (DateTime.Now - olderDate).ToString();

        var newParty = gameplayService.CreateParty();

        dbs.Snapshot.Parties.Count.Should().Be(1);
        dbs.Snapshot.Parties.First().Identity.Id.Should().Be(newParty.Identity.Id);   
    }

    [Theory]
    [Description("Character in another party should throw on join.")]
    public void Character_in_another_party_should_throw_test()
    {
        var party1 = CreateParty();
        var party2 = gameplayService.CreateParty();
        var chr = CreateHumanCharacter("Jax");
        var charIdentity = GetCharacterIdentity(chr);

        gameplayService.JoinParty(party1.Identity.Id, charIdentity);

        Assert.Throws<Exception>(() => gameplayService.JoinParty(party2.Identity.Id, charIdentity));
    }

    [Theory]
    [Description("Character leaving wrong party should throw.")]
    public void Character_leave_wrong_party_should_throw_test()
    {
        var party1 = CreateParty();
        var party2 = gameplayService.CreateParty();
        var chr = CreateHumanCharacter("Jax");
        var charIdentity = GetCharacterIdentity(chr);

        gameplayService.JoinParty(party1.Identity.Id, charIdentity);

        Assert.Throws<Exception>(() => gameplayService.LeaveParty(party2.Identity.Id, charIdentity));
    }
    #endregion

    #region private methods
    private Party CreateParty()
    {
        dbs.Snapshot.Parties.Clear();
        dbs.PersistDatabase();

        return gameplayService.CreateParty();
    }

    private static CharacterIdentity GetCharacterIdentity(Character character)
    {
        return new CharacterIdentity() { Id = character.Identity.Id, PlayerId = character.Identity.PlayerId };
    }
    #endregion
}