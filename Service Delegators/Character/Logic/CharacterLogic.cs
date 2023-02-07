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

            HasLevelUp = true,
            LevelUp = new CharacterLevelUp
            {
                StatPoints = stub.StatPoints,
                SkillPoints = stub.SkillPoints
            },

            Doll = SetPaperDoll(info),
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

    internal Character UpdateCharacter(CharacterUpdate charUpdate, string playerId)
    {
        var oldChar = metadata.GetCharacter(charUpdate.CharacterId, playerId);

        // name
        oldChar.Info.Name = charUpdate.Name;

        // stats
        var newCharStats = CharacterOperations.SumStats(charUpdate.Doll);
        var oldCharStats = CharacterOperations.SumStats(oldChar.Doll);
        var statPointsLeft = oldCharStats + oldChar.LevelUp.StatPoints - newCharStats;
        
        oldChar.LevelUp.StatPoints = statPointsLeft;
        oldChar.Doll.Strength       = charUpdate.Doll.Strength;
        oldChar.Doll.Constitution   = charUpdate.Doll.Constitution;
        oldChar.Doll.Agility        = charUpdate.Doll.Agility;
        oldChar.Doll.Willpower      = charUpdate.Doll.Willpower;
        oldChar.Doll.Perception     = charUpdate.Doll.Perception;
        oldChar.Doll.Abstract       = charUpdate.Doll.Abstract;

        // skills
        var newCharSkills = CharacterOperations.SumSkills(charUpdate.Doll);
        var oldCharSkills = CharacterOperations.SumSkills(oldChar.Doll);
        var skillPointsLeft = oldCharSkills + oldChar.LevelUp.SkillPoints - newCharSkills;

        oldChar.LevelUp.SkillPoints = skillPointsLeft;
        oldChar.Doll.Combat     = charUpdate.Doll.Combat;
        oldChar.Doll.Arcane     = charUpdate.Doll.Arcane;
        oldChar.Doll.Psionics   = charUpdate.Doll.Psionics;
        oldChar.Doll.Hide       = charUpdate.Doll.Hide;
        oldChar.Doll.Traps      = charUpdate.Doll.Traps;
        oldChar.Doll.Tactics    = charUpdate.Doll.Tactics;
        oldChar.Doll.Social     = charUpdate.Doll.Social;
        oldChar.Doll.Apothecary = charUpdate.Doll.Apothecary;
        oldChar.Doll.Travel     = charUpdate.Doll.Travel;
        oldChar.Doll.Sail       = charUpdate.Doll.Sail;
        
        if (statPointsLeft == 0 && skillPointsLeft == 0)
        {
            oldChar.HasLevelUp = false;
        }

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

    private static CharacterPaperDoll SetPaperDoll(CharacterInfo info)
    {
        if      (info.Race == CharactersLore.Races.Human) return SetDollForHuman(info);
        else if (info.Race == CharactersLore.Races.Elf) return SetDollForElf(info);
        else if (info.Race == CharactersLore.Races.Dwarf) return SetDollForDwarf(info);
        else    throw new NotImplementedException();
    }

    private static CharacterPaperDoll SetDollForHuman(CharacterInfo info)
    {
        var lvl = (int)info.EntityLevel!;

        var humanPaperDoll = new CharacterPaperDoll
        {
            Strength        = 5 * lvl,
            Constitution    = 5 * lvl,
            Agility         = 5 * lvl,
            Willpower       = 5 * lvl,
            Perception      = 5 * lvl,
            Abstract        = 5 * lvl
        };

        if (info.Culture == CharactersLore.Cultures.Human.Danarian)
        {
            humanPaperDoll.Combat += 20;
            humanPaperDoll.Travel += 10;
            humanPaperDoll.Hide -= 10;
            humanPaperDoll.Sail -= 30;

            //var itemsRoll = dice.Roll_dX(6);

            //for (int i = 0; i < itemsRoll; i++)
            //{
            //    var item = itemService.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Armour);
            //    character.Supplies.Add(item);
            //}
        }

        return humanPaperDoll;
    }

    private static CharacterPaperDoll SetDollForElf(CharacterInfo info)
    {
        var lvl = (int)info.EntityLevel!;

        var elfPaperDoll = new CharacterPaperDoll
        {
            Strength        = 2 * lvl,
            Constitution    = 7 * lvl,
            Agility         = 15 * lvl,
            Willpower       = 6 * lvl,
            Perception      = 10 * lvl,
            Abstract        = 10 * lvl
        };

        if (info.Culture == CharactersLore.Cultures.Elf.Highborn)
        {
            elfPaperDoll.Arcane += 40;
            elfPaperDoll.Mana += 50;
            elfPaperDoll.Willpower += 10;
            elfPaperDoll.Travel -= 100;
        }

        return elfPaperDoll;
    }

    private static CharacterPaperDoll SetDollForDwarf(CharacterInfo info)
    {
        var lvl = (int)info.EntityLevel!;

        var dwarfPaperDoll = new CharacterPaperDoll
        {
            Strength     = 15 * lvl,
            Constitution = 10 * lvl,
            Agility      = 2 * lvl,
            Willpower    = 10 * lvl,
            Perception   = 2 * lvl,
            Abstract     = 10 * lvl
        };

        if (info.Culture == CharactersLore.Cultures.Dwarf.Undermountain)
        {
            dwarfPaperDoll.Combat += 30;
            dwarfPaperDoll.Armour += 10;
            dwarfPaperDoll.Purge += 10;
            dwarfPaperDoll.Harm += 20;
            dwarfPaperDoll.Hide -= 40;
            dwarfPaperDoll.Social -= 20;
            dwarfPaperDoll.Travel -= 50;
            dwarfPaperDoll.Sail -= 200;
        }

        return dwarfPaperDoll;
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

            DateOfBirth = DateTime.Now.ToShortDateString(),

            Fame = SetFame(origins.Culture),
        };
    }

    #endregion
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.


