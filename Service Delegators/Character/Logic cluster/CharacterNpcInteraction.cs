using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterNpcInteraction
{
    private readonly IDatabaseService dbs;

    private CharacterNpcInteraction() { }
    internal CharacterNpcInteraction(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

    internal void MercenaryHire(CharacterHireMercenary hireMercenary)
    {
        var character = Utils.GetPlayerCharacter(dbs, hireMercenary.CharacterIdentity);
        var location = dbs.Snapshot.Locations.Find(s => s.FullName == Utils.GetLocationFullName(character.Position))!;
        var merc = location.Mercenaries.Find(s => s.Identity.Id == hireMercenary.MercenaryId)!;

        character.Status.Wealth -= merc.Worth;
        merc.Identity.PlayerId = character.Identity.PlayerId;

        character.Mercenaries.Add(merc);
        location.Mercenaries.Remove(merc);

        dbs.PersistPlayer(hireMercenary.CharacterIdentity.PlayerId);
    }
}
