using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterUpdateLogic
{
    Character AddFame(string fame, CharacterIdentity charIdentity);
    Character AddWealth(int wealth, CharacterIdentity charIdentity);
    Character ChangeName(CharacterData characterData);
}

public class CharacterUpdateLogic : ICharacterUpdateLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;

    public CharacterUpdateLogic(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public Character ChangeName(CharacterData characterData)
    {
        lock (_lock)
        {
            var character = ServicesUtils.GetPlayerCharacter(new CharacterIdentity
            {
                Id = characterData.CharacterId,
                PlayerId = characterData.PlayerId!,
            }, snapshot);

            character.Status!.Name = characterData.CharacterName;

            return character;
        }
    }

    public Character AddFame(string fame, CharacterIdentity charIdentity)
    {
        lock ( _lock)
        {
            var character = ServicesUtils.GetPlayerCharacter(charIdentity, snapshot);
            character.Status!.Fame = string.Concat(character.Status.Fame, $"\n{fame}");

            return character;
        }
    }

    public Character AddWealth(int wealth, CharacterIdentity charIdentity)
    {
        lock (_lock)
        {
            var character = ServicesUtils.GetPlayerCharacter(charIdentity, snapshot);
            character.Status!.Wealth += wealth;

            return character;
        }
    }
}
