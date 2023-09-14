using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterUpdateLogic
{
    Character AddFame(string fame, CharacterIdentity charIdentity);
    Character AddWealth(int wealth, CharacterIdentity charIdentity);
    Character ChangeName(string name, CharacterIdentity charIdentity);
}

public class CharacterUpdateLogic : ICharacterUpdateLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;

    public CharacterUpdateLogic(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public Character ChangeName(string name, CharacterIdentity charIdentity)
    {
        lock (_lock)
        {
            var character = Utils.GetPlayerCharacter(charIdentity, snapshot);
            character.Status!.Name = name;

            return character;
        }
    }

    public Character AddFame(string fame, CharacterIdentity charIdentity)
    {
        lock ( _lock)
        {
            var character = Utils.GetPlayerCharacter(charIdentity, snapshot);
            character.Status!.Fame = string.Concat(character.Status.Fame, $"\n{fame}");

            return character;
        }
    }

    public Character AddWealth(int wealth, CharacterIdentity charIdentity)
    {
        lock (_lock)
        {
            var character = Utils.GetPlayerCharacter(charIdentity, snapshot);
            character.Status!.Wealth += wealth;

            return character;
        }
    }
}
