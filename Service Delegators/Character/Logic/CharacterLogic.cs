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

            HasLevelUp = false,
            LevelUp = new CharacterLevelUp(),

            Doll = SetPaperDoll(info, stub.StatPoints, stub.SkillPoints),
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

    private CharacterPaperDoll SetPaperDoll(CharacterInfo info, int statPoints, int skillPoints)
    {
        if      (info.Race == CharactersLore.Races.Human) return SetDollForHuman(info, statPoints, skillPoints);
        else if (info.Race == CharactersLore.Races.Elf) return SetDollForElf(info);
        else if (info.Race == CharactersLore.Races.Dwarf) return SetDollForDwarf(info);
        else    throw new NotImplementedException();
    }

    private CharacterPaperDoll SetDollForHuman(CharacterInfo info, int statPoints, int skillPoints)
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

        humanPaperDoll = DistributeClassStatsAndSkills(humanPaperDoll, info.Class, statPoints, skillPoints);

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

    private CharacterPaperDoll DistributeClassStatsAndSkills(CharacterPaperDoll doll, string classes, int statPoints, int skillPoints)
    {
        if (classes == CharactersLore.Classes.Warrior) return SetClassForWarrior(doll, statPoints, skillPoints);

        return doll;
    }

    private CharacterPaperDoll SetClassForWarrior(CharacterPaperDoll doll, int statPoints, int skillPoints)
    {
        doll = ArrangeStats(doll, statPoints);
        doll = ArrangeSkills(doll, skillPoints);

        return doll;
    }

    private CharacterPaperDoll ArrangeSkills(CharacterPaperDoll doll, int skillPoints)
    {
        var mainSkills = new List<string>
        {
            CharactersLore.Skills.Combat,
        };

        var secondarySkills = new List<string>
        {
            CharactersLore.Skills.Tactics,
            CharactersLore.Skills.Travel
        };

        while (skillPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenSkill;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_dX(mainSkills.Count);
                chosenSkill = mainSkills[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(secondarySkills.Count);
                chosenSkill = secondarySkills[rollForStat - 1];
            }

            if (chosenSkill == CharactersLore.Skills.Combat)
            {
                doll.Combat++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Tactics)
            {
                doll.Tactics++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Travel)
            {
                doll.Travel++;
                skillPoints--;
            }
        }

        return doll;
    }

    private CharacterPaperDoll ArrangeStats(CharacterPaperDoll doll, int statPoints)
    {
        var mainStats = new List<string>
        {
            CharactersLore.Stats.Strength,
            CharactersLore.Stats.Constitution,
            CharactersLore.Stats.Agility
        };

        var secondaryStats = new List<string>
        {
            CharactersLore.Stats.Willpower,
            CharactersLore.Stats.Perception
        };

        while (statPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenStat;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_dX(mainStats.Count);
                chosenStat = mainStats[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(secondaryStats.Count);
                chosenStat = secondaryStats[rollForStat - 1];
            }

            if (chosenStat == CharactersLore.Stats.Strength) 
            {
                doll.Strength++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Constitution)
            {
                doll.Constitution++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Agility)
            {
                doll.Agility++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Willpower)
            {
                doll.Willpower++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Perception)
            {
                doll.Perception++;
                statPoints--;
            }
        }

        return doll;
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


