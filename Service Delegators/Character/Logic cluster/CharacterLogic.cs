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
    private readonly CharacterMetadata metadata;
    private readonly CharacterDollOperations dollOperations;

    private CharacterLogic() { }

    internal CharacterLogic(
        IDatabaseManager databaseManager,
        IDiceRollService diceRollService,
        IItemService itemService,
        CharacterMetadata metadata)
    {
        dbm = databaseManager;
        dice = diceRollService;
        this.itemService = itemService;
        this.metadata = metadata;
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

            HasLevelUp = false,
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

        dbm.Snapshot.Players!.Find(p => p.Identity.Id == playerId)!.Characters!.Add(character);
        dbm.Persist();

        return character;
    }

    internal Character ChangeName(CharacterUpdate charUpdate, string playerId)
    {
        var oldChar = metadata.GetCharacter(charUpdate.CharacterId, playerId);

        oldChar.Info.Name = charUpdate.Name;
        
        dbm.Persist();

        return oldChar;
    }

    internal void DeleteCharacter(string characterId, string playerId)
    {
        var player = dbm.Snapshot.Players.Find(p => p.Identity.Id == playerId);
        var character = player.Characters.Find(c => c.Identity.Id == characterId);

        player.Characters.Remove(character);

        dbm.Persist();
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

    private static string SetFame(string culture)
    {
        return $"Known as the {culture}";
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

            Fame = SetFame(origins.Culture),
        };
    }

    #endregion
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.


