#pragma warning disable IDE0017 // Simplify object initialization

using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;

namespace Service_Delegators;

internal class NpcLogicDelegator
{
    private readonly NpcAttributesLogic attributesLogic;
    private readonly NpcBelongingsLogic belongingsLogic;
    private readonly NpcPaperdollLogic paperdollLogic;

    private readonly IDiceRollService dice;
    private readonly IItemService items;

    private NpcLogicDelegator() { }
    internal NpcLogicDelegator(
        IDiceRollService diceService,
        IItemService itemService,
        ICharacterService characterService)
    {
        attributesLogic = new NpcAttributesLogic(diceService);
        belongingsLogic = new NpcBelongingsLogic(diceService, itemService);
        paperdollLogic = new NpcPaperdollLogic(characterService);

        dice = diceService;
        items = itemService;
    }

    internal NpcPaperdoll GenerateNpcPaperdoll(NpcInfo npcInfo)
    {
        var npcCharacter = new Character();
            
        attributesLogic.SetNpcCharacterSheet(npcInfo, npcCharacter);
        belongingsLogic.SetNpcInventory(npcCharacter);

        var npcPaperdoll = paperdollLogic.CalculateNpcPaperdoll(npcInfo, npcCharacter);

        return npcPaperdoll;
    }

    internal NpcCharacter GenerateNpcCharacter(Position position, int effortUpper)
    {
        var chr = new NpcCharacter();

        var race = SetRace();
        var culture = SetCulture(race);
        var npcClass = SetClass();
        var entityLvl = SetEntityLevel();

        chr.Worth = SetWorth(entityLvl, effortUpper);

        chr.Identity = new CharacterIdentity()
        {
            Id = Guid.NewGuid().ToString(),
            PlayerId = Guid.Empty.ToString()
        };

        chr.Info = new CharacterInfo()
        {
            Name = $"{race}_{DateTime.Now.Millisecond}",
            EntityLevel = entityLvl,
            DateOfBirth = DateTime.Now.ToShortDateString(),
            IsAlive = true,
            Fame = $"This person is quite well known around {position.Location}.",
            Wealth = 0,
            Origins = new CharacterOrigins()
            {
                Race = race,
                Culture = culture,
                Tradition = CharactersLore.Tradition.Martial,
                Class = npcClass
            }
        };

        chr.Status = new CharacterStatus()
        {
            HasAttributesLocked = true,
            HasInventoryLocked = true,
            IsInParty = false,
            QuestId = string.Empty,
            NrOfQuestsFinished = 0,
            QuestsFinished = new List<string>()
        };

        chr.Position = position;

        chr.LevelUp = new CharacterLevelUp();

        chr.Sheet = SetCharacterSheet(race, effortUpper);

        chr.Inventory = SetInventory(race, entityLvl);

        return chr;
    }

    private CharacterInventory SetInventory(string race, int entityLevel)
    {
        var inventory = new CharacterInventory();

        if (dice.FlipCoin()) inventory.Head = items.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Helm);
        if (dice.FlipCoin()) inventory.Body = items.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Armour);
        if (dice.FlipCoin()) inventory.Shield = items.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Shield);
        if (dice.FlipCoin()) inventory.Offhand = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Dagger);

        if (dice.FlipCoin()) 
        { 
            inventory.Mainhand = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Spear);
        }
        else
        {
            inventory.Mainhand = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Mace);
        }

        if (race == CharactersLore.Races.Elf)
        {
            inventory.Ranged = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Bow);
        }
        else
        {
            if (dice.FlipCoin()) inventory.Ranged = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Crossbow);
        }

        if (entityLevel >= 2)
        {
            var rollHeraldry = Randomize(6);

            for (int i = 0; i < rollHeraldry; i++)
            {
                if (dice.FlipCoin()) inventory.Heraldry!.Add(items.GenerateSpecificItem(ItemsLore.Types.Wealth, ItemsLore.Subtypes.Wealth.Trinket));
            }
        }

        return inventory;
    }


    private CharacterSheet SetCharacterSheet(string race, int effortUpper)
    {
        var charSheet = new CharacterSheet();

        charSheet.Stats = SetStatsByRace(race, effortUpper);
        charSheet.Assets = SetAssets(charSheet.Stats, effortUpper);
        charSheet.Skills = SetSkills(charSheet.Stats, effortUpper);

        return charSheet;
    }

    private CharacterSkills SetSkills(CharacterStats stats, int effortUpper)
    {
        var charSkills = new CharacterSkills();

        charSkills.Combat = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateCombat(stats);

        charSkills.Arcane = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateArcane(stats);

        charSkills.Psionics = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculatePsionics(stats);

        charSkills.Hide = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateHide(stats);

        charSkills.Traps = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateTraps(stats);

        charSkills.Tactics = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateTactics(stats);

        charSkills.Social = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateSocial(stats);

        charSkills.Apothecary = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateApothecary(stats);

        charSkills.Travel = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateTravel(stats);

        charSkills.Sail = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateSail(stats);

        return charSkills;
    }

    private CharacterAssets SetAssets(CharacterStats stats, int effortUpper)
    {
        var charAssets = new CharacterAssets();

        charAssets.Resolve = Randomize(effortUpper) + RulebookLore.Formulae.Assets.CalculateResolve(stats);

        charAssets.Harm = Randomize(effortUpper) + RulebookLore.Formulae.Assets.CalculateHarm(stats);

        charAssets.Spot = Randomize(effortUpper) + RulebookLore.Formulae.Assets.CalculateSpot(stats);

        charAssets.Defense = Randomize(effortUpper) + RulebookLore.Formulae.Assets.CalculateDefense(stats);

        charAssets.Purge = Randomize(effortUpper) + RulebookLore.Formulae.Assets.CalculatePurge(stats);

        charAssets.Mana = Randomize(effortUpper) + RulebookLore.Formulae.Assets.CalculateMana(stats);

        return charAssets;
    }

    private CharacterStats SetStatsByRace(string race, int effortUpper)
    {
        var charStats = new CharacterStats();

        if (race == CharactersLore.Races.Human)
        {
            charStats.Strength      = Randomize(effortUpper) + RulebookLore.Races.Human.Str;
            charStats.Constitution  = Randomize(effortUpper) + RulebookLore.Races.Human.Con;
            charStats.Agility       = Randomize(effortUpper) + RulebookLore.Races.Human.Agi;
            charStats.Willpower     = Randomize(effortUpper) + RulebookLore.Races.Human.Wil;
            charStats.Perception    = Randomize(effortUpper) + RulebookLore.Races.Human.Per;
            charStats.Abstract      = Randomize(effortUpper) + RulebookLore.Races.Human.Abs;
        } 
        else if (race == CharactersLore.Races.Elf)
        {
            charStats.Strength      = Randomize(effortUpper) + RulebookLore.Races.Elf.Str;
            charStats.Constitution  = Randomize(effortUpper) + RulebookLore.Races.Elf.Con;
            charStats.Agility       = Randomize(effortUpper) + RulebookLore.Races.Elf.Agi;
            charStats.Willpower     = Randomize(effortUpper) + RulebookLore.Races.Elf.Wil;
            charStats.Perception    = Randomize(effortUpper) + RulebookLore.Races.Elf.Per;
            charStats.Abstract      = Randomize(effortUpper) + RulebookLore.Races.Elf.Abs;
        }
        else if (race == CharactersLore.Races.Dwarf)
        {
            charStats.Strength      = Randomize(effortUpper) + RulebookLore.Races.Dwarf.Str;
            charStats.Constitution  = Randomize(effortUpper) + RulebookLore.Races.Dwarf.Con;
            charStats.Agility       = Randomize(effortUpper) + RulebookLore.Races.Dwarf.Agi;
            charStats.Willpower     = Randomize(effortUpper) + RulebookLore.Races.Dwarf.Wil;
            charStats.Perception    = Randomize(effortUpper) + RulebookLore.Races.Dwarf.Per;
            charStats.Abstract      = Randomize(effortUpper) + RulebookLore.Races.Dwarf.Abs;
        }


        return charStats;
    }

    private int SetWorth(int entityLvl, int effortUpper)
    {
        var roll = Randomize(effortUpper);

        return entityLvl * 100 + roll;
    }

    private int SetEntityLevel()
    {
        var roll = dice.Roll_d20(true);

        return roll switch
        {
            <= 20 => 1,
            <= 40 => 2,
            <= 60 => 3,
            <= 80 => 4,
            <= 100 => 5,
            _ => 6,
        };
    }

    private string SetClass() 
    {
        var warrior = CharactersLore.Classes.Warrior;
        var mage = CharactersLore.Classes.Mage;
        var hunter = CharactersLore.Classes.Hunter;

        var roll = Randomize(100);

        return roll switch
        {
            <= 80 => warrior,
            <= 95 => hunter,
            <= 100 => mage,
            _ => throw new NotImplementedException(),
        };
    }

    private string SetRace()
    {
        var roll = Randomize(3);

        return roll switch
        {
            1 => CharactersLore.Races.Human,
            2 => CharactersLore.Races.Elf,
            3 => CharactersLore.Races.Dwarf,
            _ => throw new NotImplementedException(),
        };
    }

    private static string SetCulture(string race)
    {
        return race switch
        {
            "Human" => CharactersLore.Cultures.Human.Danarian,
            "Elf" => CharactersLore.Cultures.Elf.Highborn,
            "Dwarf" => CharactersLore.Cultures.Dwarf.Undermountain,
            _ => throw new NotImplementedException(),
        };
    }

    private int Randomize(int max)
    {
        return dice.Roll_1dX(max);
    }
}

#pragma warning restore IDE0017 // Simplify object initialization
