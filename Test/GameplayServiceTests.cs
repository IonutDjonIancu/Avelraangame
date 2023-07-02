namespace Tests;

public class GameplayServiceTests : TestBase
{
    private static readonly bool isSinglePlayerParty = true;

    #region happy path
    [Fact(DisplayName = "Create single player party")]
    public void Create_party_test()
    {
        var party = CreateParty(isSinglePlayerParty);

        party.Should().NotBeNull();
        party.IsAdventuring.Should().BeFalse();
        party.Identity.PartyLeadId.Should().BeEmpty();
        party.Food.Should().Be(1);

        dbs.Snapshot.Parties.Count.Should().Be(1);
    }

    [Fact(DisplayName = "Join single player party")]
    public void Join_party_test()
    {
        throw new NotImplementedException();
        //var party = CreateParty(isSinglePlayerParty);
        //var chr = CreateHumanCharacter("Jax");

        //gameplayService.JoinParty(party.Identity.Id, isSinglePlayerParty, GetCharacterIdentity(chr));

        //party.Characters.Count.Should().Be(1);
        //party.Identity.PartyLeadId.Should().Be(chr.Identity.Id);
    }

    [Fact(DisplayName = "Leave party")]
    public void Leave_party_test()
    {
        throw new NotImplementedException();
        //var party = CreateParty(isSinglePlayerParty);
        //var chr = CreateHumanCharacter("Jax");
        //var charIdentity = GetCharacterIdentity(chr);

        //gameplayService.JoinParty(party.Identity.Id, isSinglePlayerParty, charIdentity);
        //gameplayService.LeaveParty(party.Identity.Id, charIdentity);

        //chr.Status.IsInParty.Should().BeFalse();
        //dbs.Snapshot.Parties.Count.Should().Be(0);
        //party.Identity.PartyLeadId.Should().BeEmpty();
    }
    #endregion

    #region validations
    [Fact(DisplayName = "When creating a new party, older empty parties should be deleted")]
    public void Create_party_should_delete_older_parties_test()
    {
        throw new NotImplementedException();
        //var party = CreateParty(isSinglePlayerParty);

        //var canParseDate = DateTime.TryParse(dbs.Snapshot.Parties.First().CreationDate, out var justNowDate);
        //canParseDate.Should().BeTrue();

        //var olderDate = justNowDate - DateTime.Now.AddDays(-70);
        //party.CreationDate = (DateTime.Now - olderDate).ToString();

        //var newParty = gameplayService.CreateParty(isSinglePlayerParty);

        //dbs.Snapshot.Parties.Count.Should().Be(1);
        //dbs.Snapshot.Parties.First().Identity.Id.Should().Be(newParty.Identity.Id);   
    }

    [Fact(DisplayName = "Character in another party should throw on join")]
    public void Character_in_another_party_should_throw_test()
    {
        throw new NotImplementedException();
        //var party1 = CreateParty(isSinglePlayerParty);
        //var party2 = gameplayService.CreateParty(isSinglePlayerParty);
        //var chr = CreateHumanCharacter("Jax");
        //var charIdentity = GetCharacterIdentity(chr);

        //gameplayService.JoinParty(party1.Identity.Id, isSinglePlayerParty, charIdentity);

        //Assert.Throws<Exception>(() => gameplayService.JoinParty(party2.Identity.Id, isSinglePlayerParty, charIdentity));
    }

    [Fact(DisplayName = "Character leaving wrong party should throw")]
    public void Character_leave_wrong_party_should_throw_test()
    {
        throw new NotImplementedException();
        //var party1 = CreateParty(isSinglePlayerParty);
        //var party2 = gameplayService.CreateParty(isSinglePlayerParty);
        //var chr = CreateHumanCharacter("Jax");
        //var charIdentity = GetCharacterIdentity(chr);

        //gameplayService.JoinParty(party1.Identity.Id, isSinglePlayerParty, charIdentity);

        //Assert.Throws<Exception>(() => gameplayService.LeaveParty(party2.Identity.Id, charIdentity));
    }
    #endregion

    #region private methods
    private Party CreateParty(bool isForSinglePlayer)
    {
        throw new NotImplementedException();
        //dbs.Snapshot.Parties.Clear();
        //dbs.PersistDatabase();

        //return gameplayService.CreateParty(isForSinglePlayer);
    }

    private static CharacterIdentity GetCharacterIdentity(Character character)
    {
        return new CharacterIdentity() { Id = character.Identity.Id, PlayerId = character.Identity.PlayerId };
    }
    #endregion
}