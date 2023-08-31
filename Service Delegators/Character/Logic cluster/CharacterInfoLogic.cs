using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterInfoLogic
{
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
        var character = GetPlayerCharacter(charIdentity);

        character.Status!.Name = name;

        return character;
    }

    internal Character AddFame(string fame, CharacterIdentity charIdentity)
    {
        var (storedChar, player) = GetPlayerCharacter(charIdentity);

        storedChar.Status!.Fame = string.Concat(storedChar.Status.Fame, $"\n{fame}");

        dbs.PersistPlayer(player.Identity.Id);

        return storedChar;
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
        var (storedChar, player) = GetPlayerCharacter(charIdentity);

        player.Characters.Remove(storedChar!);

        dbs.PersistPlayer(player.Identity.Id);
    }

    #region private methods
    private Character GetPlayerCharacter(CharacterIdentity charIdentity)
    {
        lock (_lock)
        {
            var player = snapshot.Players.Find(p => p.Identity.Id == charIdentity.PlayerId)!;
            var character = player.Characters.Find(c => c.Identity.Id == charIdentity.Id)!;

            return character;
        }
    }
    #endregion
}
