#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterLogic
{
    private readonly IDatabaseManager dbm;
    private readonly IDiceRollService dice;
    private readonly IItemService itemService;
    private readonly CharacterMetadata charMetadata;
    private readonly CharacterDollOperations dollOperations;

    private CharacterLogic() { }

    internal CharacterLogic(
        IDatabaseManager databaseManager,
        IDiceRollService diceRollService,
        IItemService itemService,
        CharacterMetadata charMetadata)
    {
        dbm = databaseManager;
        dice = diceRollService;
        this.itemService = itemService;
        this.charMetadata = charMetadata;
        dollOperations = new CharacterDollOperations(dice);
    }

    internal CharacterStub CreateStub(string playerId)
    {
        dbm.Snapshot.CharacterStubs?.RemoveAll(s => s.PlayerId == playerId);

        var entityLevel = RandomizeEntityLevel();

        var stub = new CharacterStub
        {
            PlayerId = playerId,
            EntityLevel = entityLevel,
            StatPoints = RandomizeStatPoints(entityLevel),
            SkillPoints = RandomizeSkillPoints(entityLevel),
        };

        dbm.Snapshot.CharacterStubs?.Add(stub);

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
            },

            Info = info,

            LevelUp = new CharacterLevelUp(),

            Doll = dollOperations.SetPaperDoll(info, stub.StatPoints, stub.SkillPoints),
            Traits = new List<CharacterTrait>(),
            Inventory = new CharacterInventory(),
            Supplies = SetSupplies(),

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

    internal Character ChangeName(CharacterUpdate charUpdate, string playerId)
    {
        var oldChar = charMetadata.GetCharacter(charUpdate.CharacterId, playerId);

        oldChar.Info.Name = charUpdate.Name;

        var player = dbm.Metadata.GetPlayerById(playerId);
        
        dbm.PersistPlayer(player);

        return oldChar;
    }

    internal void DeleteCharacter(string characterId, string playerId)
    {
        var player = dbm.Metadata.GetPlayerById(playerId);
        var character = player.Characters.Find(c => c.Identity.Id == characterId);

        player.Characters.Remove(character);
        
        dbm.PersistPlayer(player);
    }

    internal Character IncreaseStats(CharacterUpdate charUpdate, string playerId)
    {
        var storedChar = charMetadata.GetCharacter(charUpdate.CharacterId, playerId);

        if (charUpdate.Stat == CharactersLore.Stats.Strength)
        {
            storedChar.LevelUp.StatPoints--;
            storedChar.Doll.Strength++;
        }
        else if (charUpdate.Stat == CharactersLore.Stats.Constitution)
        {
            storedChar.LevelUp.StatPoints--;
            storedChar.Doll.Constitution++;
        }
        else if (charUpdate.Stat == CharactersLore.Stats.Agility)
        {
            storedChar.LevelUp.StatPoints--;
            storedChar.Doll.Agility++;
        }
        else if (charUpdate.Stat == CharactersLore.Stats.Willpower)
        {
            storedChar.LevelUp.StatPoints--;
            storedChar.Doll.Willpower++;
        }
        else if (charUpdate.Stat == CharactersLore.Stats.Perception)
        {
            storedChar.LevelUp.StatPoints--;
            storedChar.Doll.Perception++;
        }
        else if (charUpdate.Stat == CharactersLore.Stats.Abstract)
        {
            storedChar.LevelUp.StatPoints--;
            storedChar.Doll.Abstract++;
        }
        else
        {
            throw new Exception("Unrecognized stat.");
        }

        var player = dbm.Metadata.GetPlayerById(playerId);

        Thread.Sleep(100);
        dbm.PersistPlayer(player);

        return storedChar;
    }

    #region privates
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
            Name = $"The {origins.Culture.ToLower()}",
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
#pragma warning restore CS8604 // Possible null reference argument.


