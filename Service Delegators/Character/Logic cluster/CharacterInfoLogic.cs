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

    internal Character ChangeName(string name, CharacterIdentity identity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(identity);

        storedChar.Info!.Name = name;

        dbs.PersistPlayer(player.Identity.Id);

        return storedChar;
    }

    internal Character AddFame(string fame, CharacterIdentity identity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(identity);

        storedChar.Info!.Fame = string.Concat(storedChar.Info.Fame, $"\n{fame}");

        dbs.PersistPlayer(player.Identity.Id);

        return storedChar;
    }

    internal Character AddWealth(int wealth, CharacterIdentity identity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(identity);

        storedChar.Info!.Wealth += wealth;

        dbs.PersistPlayer(player.Identity.Id);

        return storedChar;
    }

    internal void KillChar(CharacterIdentity identity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(identity);

        storedChar.Info!.IsAlive = false;

        dbs.PersistPlayer(player.Identity.Id);
    }

    internal void DeleteChar(CharacterIdentity identity)
    {
        var (storedChar, player) = GetStoredCharacterAndPlayer(identity);

        player.Characters.Remove(storedChar!);

        dbs.PersistPlayer(player.Identity.Id);
    }

    #region private methods
    private (Character, Player) GetStoredCharacterAndPlayer(CharacterIdentity identity)
    {
        var player = dbs.Snapshot.Players.Find(p => p.Identity.Id == identity.PlayerId)!;
        var character = player.Characters.Find(p => p.Identity!.Id == identity.Id)!;

        return (character, player);
    }
    #endregion

}
