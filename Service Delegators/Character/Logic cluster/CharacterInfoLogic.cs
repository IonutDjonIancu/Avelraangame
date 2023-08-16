using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterInfoLogic
{
    private readonly IDatabaseService dbs;

    private CharacterInfoLogic() { }
    internal CharacterInfoLogic(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

    internal Character ChangeName(string name, CharacterIdentity charIdentity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(charIdentity);

        storedChar.Status!.Name = name;

        dbs.PersistPlayer(player.Identity.Id);

        return storedChar;
    }

    internal Character AddFame(string fame, CharacterIdentity charIdentity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(charIdentity);

        storedChar.Status!.Fame = string.Concat(storedChar.Status.Fame, $"\n{fame}");

        dbs.PersistPlayer(player.Identity.Id);

        return storedChar;
    }

    internal Character AddWealth(int wealth, CharacterIdentity charIdentity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(charIdentity);

        storedChar.Status!.Wealth += wealth;

        dbs.PersistPlayer(player.Identity.Id);

        return storedChar;
    }

    internal void KillChar(CharacterIdentity charIdentity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(charIdentity);

        storedChar.Status!.IsAlive = false;

        dbs.PersistPlayer(player.Identity.Id);
    }

    internal void DeleteChar(CharacterIdentity charIdentity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(charIdentity);

        player.Characters.Remove(storedChar!);

        dbs.PersistPlayer(player.Identity.Id);
    }

    #region private methods
    private (Character, Player) GetStoredCharacterAndPlayer(CharacterIdentity charIdentity)
    {
        var player = dbs.Snapshot.Players.Find(p => p.Identity.Id == charIdentity.PlayerId)!;
        var character = player.Characters.Find(c => c.Identity.Id == charIdentity.Id)!;

        return (character, player);
    }
    #endregion
}
