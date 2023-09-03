using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;

namespace Service_Delegators;

public interface INpcCreateLogic
{
    Character GenerateNpcCharacter(string locationName, bool isGood);
}

public class NpcCreateLogic : INpcCreateLogic
{
    private readonly IDiceLogicDelegator dice;
    private readonly IItemsLogicDelegator items;

    public NpcCreateLogic(
        IDiceLogicDelegator dice,
        IItemsLogicDelegator items)
    {
        this.dice = dice;
        this.items = items;
    }

    public Character GenerateNpcCharacter(string locationName, bool isGood)
    {
        var location = Utils.GetLocationByLocationName(locationName);
        var character = new Character();

        SetIdentity(character);
        SetStatus(character, isGood, location);
        SetSheet(character, location);
        SetInventory(character);
        SetWorth(character, location);

        return character;
    }

    #region private methods

    #region Identity
    private static void SetIdentity(Character character)
    {
        character.Identity.Id = Guid.NewGuid().ToString();
        character.Identity.PlayerId = Guid.Empty.ToString();
    }
    #endregion

    #region Status
    private void SetStatus(Character character, bool isGood, Location location)
    {
        character.Status.EntityLevel = DecideEntityLevel();
        character.Status.DateOfBirth = DateTime.Now.ToShortDateString();
        character.Status.IsNpc = true;
        character.Status.IsAlive = true;
        character.Status.IsLockedToModify = false;
        character.Status.Traits = DecideTraits(isGood, location);
        character.Status.Gameplay = new CharacterGameplay();
        character.Status.Position = Utils.GetPositionByLocationFullName(location.FullName);
        character.Status.Worth = location.Effort;
        character.Status.Wealth = dice.Roll_d100_noReroll();
        character.Status.Fame = isGood ? $"This person is known around {location.Position.Location}." : "You've heard nothing of this person.";
        character.Status.NrOfQuestsFinished = 0;

        character.Status.Name = ResolveName(character.Status.Traits.Race, isGood);
    }

    private static string ResolveName(string race, bool isGood)
    {
        string GoodName(string race)
        {
            return $"{race}_mercenary_{DateTime.Now.Millisecond}";
        }

        string BadName(string race)
        {
            return race switch
            {
                CharactersLore.Races.Playable.Human => $"Brigand_{DateTime.Now.Millisecond}",
                CharactersLore.Races.Playable.Elf => $"Darkelf_{DateTime.Now.Millisecond}",
                CharactersLore.Races.Playable.Dwarf => $"Outcast_dwarf_{DateTime.Now.Millisecond}",
                CharactersLore.Races.Playable.Orc => $"Orc_{DateTime.Now.Millisecond}",
                CharactersLore.Races.NonPlayable.Animal => $"Wild_animal_{DateTime.Now.Millisecond}",
                CharactersLore.Races.NonPlayable.Undead => $"Undead_{DateTime.Now.Millisecond}",

                _ => $"Npc_{DateTime.Now.Millisecond}",
            };
        }

        return isGood ? GoodName(race) : BadName(race);
    }

    private CharacterTraits DecideTraits(bool isGood, Location location)
    {
        var race = DecideRace(isGood);
        var culture = DecideCulture(race);
        var tradition = DecideTradition(location);
        var chosenClass = DecideClass();

        return new CharacterTraits
        {
            Race = race,
            Culture = culture,
            Tradition = tradition,
            Class = chosenClass
        };
    }

    private string DecideClass()
    {
        var roll = dice.Roll_d100_noReroll();

        return roll <= 80 ? CharactersLore.Classes.Warrior : CharactersLore.Classes.Mage;
    }

    private static string DecideTradition(Location location)
    {
        return location.FullName.Contains(GameplayLore.Locations.Dragonmaw.RegionName) ? GameplayLore.Tradition.Martial : GameplayLore.Tradition.Common;
    }

    private string DecideRace(bool isGood)
    {
        string GetGoodRace()
        {
            var roll = dice.Roll_d100_noReroll();

            if (roll <= 70) return CharactersLore.Races.Playable.Human;
            else if (roll <= 95) return CharactersLore.Races.Playable.Elf;
            else return CharactersLore.Races.Playable.Dwarf;
        }

        string GetBadRace()
        {
            var roll = dice.Roll_d100_noReroll();

            if (roll <= 70)
            {
                var roll2nd = dice.Roll_d100_noReroll();

                if (roll2nd <= 30) return CharactersLore.Races.Playable.Human;
                else if (roll2nd <= 55) return CharactersLore.Races.Playable.Orc;
                else return CharactersLore.Races.NonPlayable.Animal;
            }
            else
            {
                var roll2nd = dice.Roll_d100_noReroll();

                if (roll2nd <= 75) return CharactersLore.Races.Playable.Orc;
                else return CharactersLore.Races.NonPlayable.Undead;
            }
        }

        return isGood ? GetGoodRace() : GetBadRace();
    }

    private string DecideCulture(string race)
    {
        if (race == CharactersLore.Races.Playable.Human)
        {
            var count = CharactersLore.Cultures.Human.All.Count;
            var index = dice.Roll_1_to_n(count) - 1;
            return CharactersLore.Cultures.Human.All[index];
        }
        else if (race == CharactersLore.Races.Playable.Elf)
        {
            var count = CharactersLore.Cultures.Elf.All.Count;
            var index = dice.Roll_1_to_n(count) - 1;
            return CharactersLore.Cultures.Elf.All[index];
        }
        else if (race == CharactersLore.Races.Playable.Dwarf)
        {
            var count = CharactersLore.Cultures.Dwarf.All.Count;
            var index = dice.Roll_1_to_n(count) - 1;
            return CharactersLore.Cultures.Dwarf.All[index];
        }
        else if (race == CharactersLore.Races.Playable.Orc)
        {
            var count = CharactersLore.Cultures.Orc.All.Count;
            var index = dice.Roll_1_to_n(count) - 1;
            return CharactersLore.Cultures.Orc.All[index];
        }
        else if (race == CharactersLore.Races.NonPlayable.Animal)
        {
            var count = CharactersLore.Cultures.Animal.All.Count;
            var index = dice.Roll_1_to_n(count) - 1;
            return CharactersLore.Cultures.Animal.All[index];
        }
        else if (race == CharactersLore.Races.NonPlayable.Undead)
        {
            var count = CharactersLore.Cultures.Undead.All.Count;
            var index = dice.Roll_1_to_n(count) - 1;
            return CharactersLore.Cultures.Undead.All[index];
        }
        else throw new Exception("Wrong race to match with culture.");
    }

    private int DecideEntityLevel()
    {
        var roll = dice.Roll_d20_withReroll();
        var level = roll / 20;

        return ++level;

    }
    #endregion

    #region Sheet
    private void SetSheet(Character character, Location location)
    {
        character.Sheet.Stats = DecideStats(character.Status.Traits.Race, location.Effort);
        character.Sheet.Assets = DecideAssets(character.Sheet.Stats, location.Effort);
        character.Sheet.Skills = DecideSkills(character.Sheet.Stats, location.Effort);
    }

    private CharacterSkills DecideSkills(CharacterStats stats, int effortUpper)
    {
        return new CharacterSkills
        {
            Combat = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateCombat(stats),
            Arcane = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateArcane(stats),
            Psionics = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculatePsionics(stats),
            Hide = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateHide(stats),
            Traps = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateTraps(stats),
            Tactics = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateTactics(stats),
            Social = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateSocial(stats),
            Apothecary = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateApothecary(stats),
            Travel = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateTravel(stats),
            Sail = Randomize(effortUpper) + RulebookLore.Formulae.Skills.CalculateSail(stats)
        };
    }

    private CharacterAssets DecideAssets(CharacterStats stats, int effort)
    {
        var defense = (Randomize(effort) + RulebookLore.Formulae.Assets.CalculateDefense(stats)) / 10;
        var purge = (Randomize(effort) + RulebookLore.Formulae.Assets.CalculatePurge(stats)) / 10;

        return new CharacterAssets
        {
            Resolve = Randomize(effort) + RulebookLore.Formulae.Assets.CalculateResolve(stats),
            Harm = Randomize(effort) + RulebookLore.Formulae.Assets.CalculateHarm(stats),
            Spot = Randomize(effort) + RulebookLore.Formulae.Assets.CalculateSpot(stats),
            Defense = defense >= 90 ? 90 : defense,
            Purge = purge >= 90 ? 90 : purge,
            Mana = Randomize(effort) + RulebookLore.Formulae.Assets.CalculateMana(stats),
            Actions = Randomize(2) + RulebookLore.Formulae.Assets.CalculateActions(stats),
        };
    }

    private CharacterStats DecideStats(string race, int effort)
    {
        var charStats = new CharacterStats();

        if (race == CharactersLore.Races.Playable.Human)
        {
            charStats.Strength = Randomize(effort) + RulebookLore.Races.Playable.Human.Strength;
            charStats.Constitution = Randomize(effort) + RulebookLore.Races.Playable.Human.Constitution;
            charStats.Agility = Randomize(effort) + RulebookLore.Races.Playable.Human.Agility;
            charStats.Willpower = Randomize(effort) + RulebookLore.Races.Playable.Human.Willpower;
            charStats.Perception = Randomize(effort) + RulebookLore.Races.Playable.Human.Perception;
            charStats.Abstract = Randomize(effort) + RulebookLore.Races.Playable.Human.Abstract;
        }
        else if (race == CharactersLore.Races.Playable.Elf)
        {
            charStats.Strength = Randomize(effort) + RulebookLore.Races.Playable.Elf.Strength;
            charStats.Constitution = Randomize(effort) + RulebookLore.Races.Playable.Elf.Constitution;
            charStats.Agility = Randomize(effort) + RulebookLore.Races.Playable.Elf.Agility;
            charStats.Willpower = Randomize(effort) + RulebookLore.Races.Playable.Elf.Willpower;
            charStats.Perception = Randomize(effort) + RulebookLore.Races.Playable.Elf.Perception;
            charStats.Abstract = Randomize(effort) + RulebookLore.Races.Playable.Elf.Abstract;
        }
        else if (race == CharactersLore.Races.Playable.Dwarf)
        {
            charStats.Strength = Randomize(effort) + RulebookLore.Races.Playable.Dwarf.Strength;
            charStats.Constitution = Randomize(effort) + RulebookLore.Races.Playable.Dwarf.Constitution;
            charStats.Agility = Randomize(effort) + RulebookLore.Races.Playable.Dwarf.Agility;
            charStats.Willpower = Randomize(effort) + RulebookLore.Races.Playable.Dwarf.Willpower;
            charStats.Perception = Randomize(effort) + RulebookLore.Races.Playable.Dwarf.Perception;
            charStats.Abstract = Randomize(effort) + RulebookLore.Races.Playable.Dwarf.Abstract;
        }
        else if (race == CharactersLore.Races.Playable.Orc)
        {
            charStats.Strength = Randomize(effort) + RulebookLore.Races.Playable.Orc.Strength;
            charStats.Constitution = Randomize(effort) + RulebookLore.Races.Playable.Orc.Constitution;
            charStats.Agility = Randomize(effort) + RulebookLore.Races.Playable.Orc.Agility;
            charStats.Willpower = Randomize(effort) + RulebookLore.Races.Playable.Orc.Willpower;
            charStats.Perception = Randomize(effort) + RulebookLore.Races.Playable.Orc.Perception;
            charStats.Abstract = Randomize(effort) + RulebookLore.Races.Playable.Orc.Abstract;
        }
        else if (race == CharactersLore.Races.NonPlayable.Animal)
        {
            charStats.Strength = Randomize(effort) + RulebookLore.Races.NonPlayable.Animal.Strength;
            charStats.Constitution = Randomize(effort) + RulebookLore.Races.NonPlayable.Animal.Constitution;
            charStats.Agility = Randomize(effort) + RulebookLore.Races.NonPlayable.Animal.Agility;
            charStats.Willpower = Randomize(effort) + RulebookLore.Races.NonPlayable.Animal.Willpower;
            charStats.Perception = Randomize(effort) + RulebookLore.Races.NonPlayable.Animal.Perception;
            charStats.Abstract = Randomize(effort) + RulebookLore.Races.NonPlayable.Animal.Abstract;
        }
        else if (race == CharactersLore.Races.NonPlayable.Animal)
        {
            charStats.Strength = Randomize(effort) + RulebookLore.Races.NonPlayable.Undead.Strength;
            charStats.Constitution = Randomize(effort) + RulebookLore.Races.NonPlayable.Undead.Constitution;
            charStats.Agility = Randomize(effort) + RulebookLore.Races.NonPlayable.Undead.Agility;
            charStats.Willpower = Randomize(effort) + RulebookLore.Races.NonPlayable.Undead.Willpower;
            charStats.Perception = Randomize(effort) + RulebookLore.Races.NonPlayable.Undead.Perception;
            charStats.Abstract = Randomize(effort) + RulebookLore.Races.NonPlayable.Undead.Abstract;
        }

        return charStats;
    }
    #endregion

    #region Inventory
    private void SetInventory(Character character)
    {
        if (character.Status.Traits.Race == CharactersLore.Races.NonPlayable.Animal
            || character.Status.Traits.Race == CharactersLore.Races.NonPlayable.Undead)
        {
            return;
        }

        if (dice.Roll_true_false()) character.Inventory.Head = items.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Helm);
        if (dice.Roll_true_false()) character.Inventory.Body = items.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Armour);
        if (dice.Roll_true_false()) character.Inventory.Shield = items.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Shield);
        if (dice.Roll_true_false()) character.Inventory.Offhand = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Dagger);


        if (character.Status.Traits.Race == CharactersLore.Races.Playable.Human)
        {
            if (dice.Roll_true_false())
            {
                character.Inventory.Mainhand = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Sword);
            }
            else
            {
                character.Inventory.Mainhand = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Spear);
            }
        }
        else
        {
            if (dice.Roll_true_false())
            {
                character.Inventory.Mainhand = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Axe);
            }
            else
            {
                character.Inventory.Mainhand = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Spear);
            }
        }

        if (character.Status.Traits.Race == CharactersLore.Races.Playable.Elf)
        {
            character.Inventory.Ranged = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Bow);
        }
        else
        {
            if (dice.Roll_true_false()) character.Inventory.Ranged = items.GenerateSpecificItem(ItemsLore.Types.Weapon, ItemsLore.Subtypes.Weapons.Crossbow);
        }

        if (character.Status.EntityLevel >= 2)
        {
            var rollHeraldry = Randomize(6);

            for (int i = 0; i < rollHeraldry; i++)
            {
                if (dice.Roll_true_false()) character.Inventory.Heraldry!.Add(items.GenerateSpecificItem(ItemsLore.Types.Wealth, ItemsLore.Subtypes.Wealth.Trinket));
            }
        }

        character.Inventory.Provisions = dice.Roll_d100_withReroll() + 10;
    }
    #endregion

    private void SetWorth(Character character, Location location)
    {
        var skillsAverage = (character.Sheet.Skills.Combat
            + character.Sheet.Skills.Arcane
            + character.Sheet.Skills.Psionics
            + character.Sheet.Skills.Hide
            + character.Sheet.Skills.Traps
            + character.Sheet.Skills.Tactics
            + character.Sheet.Skills.Social
            + character.Sheet.Skills.Apothecary
            + character.Sheet.Skills.Travel
            + character.Sheet.Skills.Sail)
            / 10;

        var roll = Randomize(location.Effort);

        character.Status.Worth = character.Status.EntityLevel * 20
            + roll
            + skillsAverage;
    }

    private int Randomize(int max)
    {
        return dice.Roll_1_to_n(max);
    }
    #endregion
}