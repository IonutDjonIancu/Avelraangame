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

    internal void HireMercenary(CharacterHireMercenary hireMercenary)
    {
        var character = Utils.GetPlayerCharacter(dbs.Snapshot, hireMercenary.CharacterIdentity);
        var location = dbs.Snapshot.Locations.Find(s => s.FullName == Utils.GetLocationFullNameFromPosition(character.Status.Position))!;
        var merc = location.Mercenaries.Find(s => s.Identity.Id == hireMercenary.MercenaryId)!;

        character.Status.Wealth -= merc.Status.Worth;
        merc.Identity.PlayerId = character.Identity.PlayerId;

        character.Mercenaries.Add(merc);
        location.Mercenaries.Remove(merc);

        dbs.PersistPlayer(hireMercenary.CharacterIdentity.PlayerId);
    }
}
