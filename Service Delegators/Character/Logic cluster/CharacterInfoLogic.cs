using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterInfoLogic
{
    Character AddFame(string fame, CharacterIdentity charIdentity);
    Character ChangeName(string name, CharacterIdentity charIdentity);
}

public class CharacterInfoLogic : ICharacterInfoLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;

    public CharacterInfoLogic(Snapshot snapshot)
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

    internal Character AddWealth(int wealth, CharacterIdentity charIdentity)
    {
        var (storedChar, player) = GetPlayerCharacter(charIdentity);

        storedChar.Status!.Wealth += wealth;

        dbs.PersistPlayer(player.Identity.Id);

        return storedChar;
    }

    internal void KillChar(CharacterIdentity charIdentity)
    {
        var (storedChar, player) = GetPlayerCharacter(charIdentity);

        storedChar.Status!.IsAlive = false;

        dbs.PersistPlayer(player.Identity.Id);
    }

    internal void DeleteChar(CharacterIdentity charIdentity)
    {
        var character = GetPlayerCharacter(charIdentity);

        player.Characters.Remove(storedChar!);

        dbs.PersistPlayer(player.Identity.Id);
    }

}
