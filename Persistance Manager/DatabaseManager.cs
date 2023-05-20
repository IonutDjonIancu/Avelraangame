using Newtonsoft.Json;
using Data_Mapping_Containers.Dtos;

namespace Persistance_Manager;

public class DatabaseManager : IDatabaseManager
{
    #region private props
    private static readonly List<string> admins = new()
    {
        "djon"
    };
    private static readonly List<string> banned = new()
    {
        "janedoe"
    };
    private static readonly string currentDir = Directory.GetCurrentDirectory();
    #endregion

    public DatabaseManagerInfo Info { get; init; }
    public DatabaseManagerSnapshot Snapshot { get; set; }

    public DatabaseManager(DatabaseManagerConfig dbmconfig)
    {
        Info = SetUpInfo(dbmconfig);
        Snapshot = SetUpSnaphot(Info);

    }

    #region private methods
    private static DatabaseManagerInfo SetUpInfo(DatabaseManagerConfig config)
    {
        return new DatabaseManagerInfo
        {
            DbPath = $"{currentDir}{config.DbPath}",
            DbPlayersPath = $"{currentDir}{config.DbPlayersPath}",
            LogPath = $"{currentDir}{config.LogPath}",

            AvelraanEmail = config.AvelraanEmail,
            AvelraanPassword = config.AvelraanPassword,
        };
    }

    private static DatabaseManagerSnapshot SetUpSnaphot(DatabaseManagerInfo info)
    {
        var avDbDiskFile = File.ReadAllText(info.DbPath);
        var avDatabase = JsonConvert.DeserializeObject<Database>(avDbDiskFile);

        return new DatabaseManagerSnapshot
        {
            DbDate = avDatabase.DbDate,
            Admins = admins,
            Banned = banned,
            CharacterStubs = avDatabase.CharacterStubs,
            Parties = avDatabase.Parties,

            Players = ReadPlayerFiles(info.DbPlayersPath),
            Items = avDatabase.Items,
            Traits = LoadTraitsFromLore(),
        };
    }

    private static List<Player> ReadPlayerFiles(string playersPath)
    {
        var paths = Directory.GetFiles(playersPath);

        var list = new List<Player>();

        foreach (var path in paths)
        {
            var text = File.ReadAllText(path);
            var player = JsonConvert.DeserializeObject<Player>(text);
            list.Add(player);
        }

        return list;
    }

    private static List<HeroicTrait> LoadTraitsFromLore()
    {
        var listOfTraits = new List<HeroicTrait>();

        foreach (var item in TraitsLore.PassiveTraits.All)
        {
            listOfTraits.Add(item);
        }
        foreach (var item in TraitsLore.ActiveTraits.All)
        {
            listOfTraits.Add(item);
        }
        foreach (var item in TraitsLore.BonusTraits.All)
        {
            listOfTraits.Add(item);
        }

        return listOfTraits;
    }
    #endregion
}