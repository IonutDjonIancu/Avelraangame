using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;

namespace Service_Delegators;

internal class CharacterCreateLogic
{
    private readonly IDatabaseService dbs;
    private readonly IDiceRollService dice;
    private readonly IItemService items;

    private readonly CharacterSheetLogic sheetLogic;

    private CharacterCreateLogic() { }
    internal CharacterCreateLogic(
        IDatabaseService databaseService,
        IDiceRollService diceService,
        IItemService itemService,
        CharacterSheetLogic characterSheetLogic)
    {
        dbs = databaseService;
        dice = diceService;
        items = itemService;

        sheetLogic = characterSheetLogic;
    }

    internal CharacterStub CreateStub(string playerId)
    {
        dbs.Snapshot.CharacterStubs.RemoveAll(s => s.PlayerId == playerId);

        var entityLevel = RandomizeEntityLevel();

        var stub = new CharacterStub
        {
            PlayerId = playerId,
            EntityLevel = entityLevel,
            StatPoints = RandomizeStatPoints(entityLevel),
            SkillPoints = RandomizeSkillPoints(entityLevel),
        };

        dbs.Snapshot.CharacterStubs.Add(stub);

        return stub;
    }

    internal Character SaveStub(CharacterOrigins origins, string playerId)
    {
        var stub = dbs.Snapshot.CharacterStubs!.Find(s => s.PlayerId == playerId)!;
        var info = CreateCharacterInfo(origins, stub);

        Character character = new()
        {
            Identity = new CharacterIdentity
            {
                Id = Guid.NewGuid().ToString(),
                PlayerId = playerId,
            },

            Info = info,
            LevelUp = new CharacterLevelUp(),
            Status = new CharacterStatus()
            {
                HasAttributesLocked = false,
                HasInventoryLocked = false,
                IsInParty = false,
                PartyId = string.Empty,
                NrOfQuestsFinished = 0,
                QuestsFinished = new List<string>()
            },

            Sheet = sheetLogic.SetCharacterSheet(info, stub.StatPoints, stub.SkillPoints),

            Inventory = new CharacterInventory(),
            Supplies = SetSupplies(),

            HeroicTraits = new List<HeroicTrait>()
        };

        character.Info.Wealth = SetWealth();

        if (character.Info.Origins.Tradition == GameplayLore.Tradition.Martial)
        {
            character.Position = new Position
            {
                Region = GameplayLore.MapLocations.Dragonmaw.Name,
                Subregion = GameplayLore.MapLocations.Dragonmaw.Farlindor.Name,
                Land = GameplayLore.MapLocations.Dragonmaw.Farlindor.Danar.Name,
                Location = GameplayLore.MapLocations.Dragonmaw.Farlindor.Danar.Locations.Arada.Name
            };
        }
        else
        {
            // TODO: this will have to be changed eventually to incorporate Calvinia starting point
            character.Position = new Position
            {
                Region = GameplayLore.MapLocations.Dragonmaw.Name,
                Subregion = GameplayLore.MapLocations.Dragonmaw.Farlindor.Name,
                Land = GameplayLore.MapLocations.Dragonmaw.Farlindor.Danar.Name,
                Location = GameplayLore.MapLocations.Dragonmaw.Farlindor.Danar.Locations.Arada.Name
            };
        }

        // set cultural bonuses like Human Danarian gets extra armour pieces, etc, wood elves get a bow, etc

        dbs.Snapshot.CharacterStubs.RemoveAll(s => s.PlayerId == playerId);

        var player = dbs.Snapshot.Players.Find(p => p.Identity.Id == playerId)!;
        player.Characters!.Add(character);

        dbs.PersistPlayer(playerId);

        return character;
    }

    #region private methods
    private int RandomizeEntityLevel()
    {
        var roll = dice.Roll_d20(true);

        if      (roll >= 100)   return 6;
        else if (roll >= 80)    return 5;
        else if (roll >= 60)    return 4;
        else if (roll >= 40)    return 3;
        else if (roll >= 20)    return 2;
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
        var roll = dice.Roll_1dX(6);
        var supplies = new List<Item>();

        for (int i = 0; i < roll; i++)
        {
            var item = items.GenerateRandomItem();
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
        var rollTimes = dice.Roll_1dX(6);
        var total = 10;
        for (int i = 0; i < rollTimes; i++)
        {
            total += dice.Roll_1dX(100);
        }

        return total;
    }

    private static CharacterInfo CreateCharacterInfo(CharacterOrigins origins, CharacterStub stub)
    {
        return new CharacterInfo
        {
            Name = $"The {origins.Culture.ToLower()}",

            EntityLevel = stub!.EntityLevel,

            Origins = new CharacterOrigins
            {
                Race = origins.Race,
                Culture = origins.Culture,
                Tradition = origins.Tradition,
                Class = origins.Class,
            },

            DateOfBirth = DateTime.Now.ToShortDateString(),

            Fame = SetFame(origins.Culture, origins.Class),
            IsAlive = true,
        };
    }
    #endregion
}
