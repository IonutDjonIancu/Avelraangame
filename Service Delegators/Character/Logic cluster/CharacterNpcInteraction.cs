using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterNpcInteraction
{
    Character HireMercenary(CharacterHireMercenary hireMercenary);
}

public class CharacterNpcInteraction : ICharacterNpcInteraction
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;

    public CharacterNpcInteraction(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public Character HireMercenary(CharacterHireMercenary hireMercenary)
    {
        lock (_lock)
        {
            var character = ServicesUtils.GetPlayerCharacter(hireMercenary.CharacterIdentity, snapshot);
            var location = snapshot.Locations.Find(s => s.FullName == ServicesUtils.GetLocationFullNameFromPosition(character.Status.Position))!;
            var merc = location.Mercenaries.Find(s => s.Identity.Id == hireMercenary.MercenaryId)!;

            character.Status.Wealth -= merc.Status.Worth;
            merc.Identity.PlayerId = character.Identity.PlayerId;

            character.Mercenaries.Add(merc);
            location.Mercenaries.Remove(merc);

            return character;
        }
    }
}
