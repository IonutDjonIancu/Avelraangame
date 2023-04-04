#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterCreateLogic
{
    private readonly IDatabaseManager dbm;
    private readonly IDiceRollService dice;
    private readonly IItemService itemService;

    private readonly CharacterSheetLogic sheetOps;

    public CharacterCreateLogic(
        IDatabaseManager databaseManager,
        IDiceRollService rollService,
        IItemService itemService,
        CharacterSheetLogic characterSheetOperations)
    {
        dbm = databaseManager;
        dice = rollService;
        this.itemService = itemService;

        sheetOps = characterSheetOperations;
    }

    internal CharacterStub CreateStub(string playerId)
    {
        dbm.Snapshot.CharacterStubs.RemoveAll(s => s.PlayerId == playerId);

        var entityLevel = RandomizeEntityLevel();

        var stub = new CharacterStub
        {
            PlayerId = playerId,
            EntityLevel = entityLevel,
            StatPoints = RandomizeStatPoints(entityLevel),
            SkillPoints = RandomizeSkillPoints(entityLevel),
        };

        dbm.Snapshot.CharacterStubs.Add(stub);

        return stub;
    }

    internal Character SaveStub(CharacterOrigins origins, string playerId)
    {
        var stub = dbm.Snapshot.CharacterStubs!.Find(s => s.PlayerId == playerId)!;
        var info = CreateCharacterInfo(origins, stub);

        Character character = new()
        {
            Identity = new CharacterIdentity
            {
                Id = Guid.NewGuid().ToString(),
                PlayerId = playerId,
                Name = $"The {origins.Culture.ToLower()}"
            },

            Info = info,

            LevelUp = new CharacterLevelUp(),

            Sheet = sheetOps.SetCharacterSheet(info, stub.StatPoints, stub.SkillPoints),

            Inventory = new CharacterInventory(),
            Supplies = SetSupplies(),

            HeroicTraits = new List<HeroicTrait>(),
            IsAlive = true,
        };

        character.Info.Wealth = SetWealth();

        // set cultural bonuses like Human Danarian gets extra armour pieces, etc, wood elves get a bow, etc

        dbm.Snapshot.CharacterStubs.RemoveAll(s => s.PlayerId == playerId);

        var player = dbm.Metadata.GetPlayerById(playerId);
        player.Characters!.Add(character);

        dbm.PersistPlayer(player);

        return character;
    }

    #region private methods
    private int RandomizeEntityLevel()
    {
        var roll = dice.Roll_d20(true);

        if (roll >= 100) return 6;
        else if (roll >= 80) return 5;
        else if (roll >= 60) return 4;
        else if (roll >= 40) return 3;
        else if (roll >= 20) return 2;
        else  /*(roll >= 1)*/   return 1;
    }

    private int RandomizeStatPoints(int entityLevel)
    {
        var roll = dice.Roll_d20(true);
        return roll * entityLevel;
    }

    private int RandomizeSkillPoints(int entityLevel)
    {
        var roll = dice.Roll_d20(true);
        return roll * entityLevel;
    }

    private List<Item> SetSupplies()
    {
        var roll = dice.Roll_dX(6);
        var supplies = new List<Item>();

        for (int i = 0; i < roll; i++)
        {
            var item = itemService.GenerateRandomItem();
            supplies.Add(item);
        }

        return supplies;
    }

    private static string SetFame(string culture, string classes)
    {
        return $"Known as the {culture} {classes.ToLower()}";
    }

    private int SetWealth()
    {
        var rollTimes = dice.Roll_dX(6);
        var total = 10;
        for (int i = 0; i < rollTimes; i++)
        {
            total += dice.Roll_dX(100);
        }

        return total;
    }

    private static CharacterInfo CreateCharacterInfo(CharacterOrigins origins, CharacterStub stub)
    {
        return new CharacterInfo
        {
            EntityLevel = stub!.EntityLevel,

            Race = origins.Race,
            Culture = origins.Culture,
            Tradition = origins.Tradition,
            Class = origins.Class,

            DateOfBirth = DateTime.Now.ToShortDateString(),

            Fame = SetFame(origins.Culture, origins.Class),
        };
    }
    #endregion
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
