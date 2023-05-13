using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterSheetLogic
{
    private readonly IDatabaseService dbs;
    private readonly IDiceRollService dice;

    internal CharacterSheetLogic(
        IDatabaseService databaseService,
        IDiceRollService diceService)
    {
        dice = diceService;
        dbs = databaseService;
    }

    internal CharacterSheet SetCharacterSheet(CharacterInfo info, int statPoints, int skillPoints)
    {
        if      (info.Race == CharactersLore.Races.Human) return SetCharacterSheetForHuman(info, statPoints, skillPoints);
        else if (info.Race == CharactersLore.Races.Elf) return SetCharacterSheetForElf(info, statPoints, skillPoints);
        else if (info.Race == CharactersLore.Races.Dwarf) return SetCharacterSheetForDwarf(info, statPoints, skillPoints);
        throw new NotImplementedException();
    }

    #region private methods
    private CharacterSheet SetCharacterSheetForHuman(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = lvl * dbs.Snapshot.Rulebook.Races.Human.Strength,
                Constitution= lvl * dbs.Snapshot.Rulebook.Races.Human.Constitution,
                Agility     = lvl * dbs.Snapshot.Rulebook.Races.Human.Agility,
                Willpower   = lvl * dbs.Snapshot.Rulebook.Races.Human.Willpower,
                Perception  = lvl * dbs.Snapshot.Rulebook.Races.Human.Perception,
                Abstract    = lvl * dbs.Snapshot.Rulebook.Races.Human.Abstract
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Human.Danarian) SetSheetForDanarian(charsheet);

        DistributeClassAttributes(charsheet, info.Class!, statPoints, skillPoints);

        return charsheet;
    }

    private CharacterSheet SetCharacterSheetForElf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = lvl * dbs.Snapshot.Rulebook.Races.Elf.Strength,
                Constitution= lvl * dbs.Snapshot.Rulebook.Races.Elf.Constitution,
                Agility     = lvl * dbs.Snapshot.Rulebook.Races.Elf.Agility,
                Willpower   = lvl * dbs.Snapshot.Rulebook.Races.Elf.Willpower,
                Perception  = lvl * dbs.Snapshot.Rulebook.Races.Elf.Perception,
                Abstract    = lvl * dbs.Snapshot.Rulebook.Races.Elf.Abstract
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Elf.Highborn) SetSheetForHighborn(charsheet);
        else throw new Exception("Character culture not found.");

        DistributeClassAttributes(charsheet, info.Class!, statPoints, skillPoints);

        return charsheet;
    }

    private CharacterSheet SetCharacterSheetForDwarf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = lvl * dbs.Snapshot.Rulebook.Races.Dwarf.Strength,
                Constitution= lvl * dbs.Snapshot.Rulebook.Races.Dwarf.Constitution,
                Agility     = lvl * dbs.Snapshot.Rulebook.Races.Dwarf.Agility,
                Willpower   = lvl * dbs.Snapshot.Rulebook.Races.Dwarf.Willpower,
                Perception  = lvl * dbs.Snapshot.Rulebook.Races.Dwarf.Perception,
                Abstract    = lvl * dbs.Snapshot.Rulebook.Races.Dwarf.Abstract
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Dwarf.Undermountain) SetSheetForUndermoutain(charsheet);
        else throw new Exception("Character culture not found.");

        DistributeClassAttributes(charsheet, info.Class!, statPoints, skillPoints);

        return charsheet;
    }

    private void SetSheetForDanarian(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Strength;
        charsheet.Stats.Constitution+= dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Constitution;
        charsheet.Stats.Agility     += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Agility;
        charsheet.Stats.Willpower   += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Willpower;
        charsheet.Stats.Perception  += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Perception;
        charsheet.Stats.Abstract    += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Abstract;
        //assets
        charsheet.Assets.Resolve    += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Resolve;
        charsheet.Assets.Harm       += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Harm;
        charsheet.Assets.Spot       += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Spot;
        charsheet.Assets.Defense    += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Defense;
        charsheet.Assets.Purge      += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Purge;
        charsheet.Assets.Mana       += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Mana;
        //skills
        charsheet.Skills.Combat     += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Combat;
        charsheet.Skills.Arcane     += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Arcane;
        charsheet.Skills.Psionics   += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Psionics;
        charsheet.Skills.Hide       += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Hide;
        charsheet.Skills.Traps      += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Traps;
        charsheet.Skills.Tactics    += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Tactics;
        charsheet.Skills.Social     += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Social;
        charsheet.Skills.Apothecary += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Apothecary;
        charsheet.Skills.Travel     += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Travel;
        charsheet.Skills.Sail       += dbs.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Sail;

        // danarias should start with a weapon, armour and helm
    }

    private void SetSheetForHighborn(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Strength;
        charsheet.Stats.Constitution+= dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Constitution;
        charsheet.Stats.Agility     += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Agility;
        charsheet.Stats.Willpower   += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Willpower;
        charsheet.Stats.Perception  += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Perception;
        charsheet.Stats.Abstract    += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Abstract;
        //assets
        charsheet.Assets.Resolve    += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Resolve;
        charsheet.Assets.Harm       += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Harm;
        charsheet.Assets.Spot       += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Spot;
        charsheet.Assets.Defense    += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Defense;
        charsheet.Assets.Purge      += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Purge;
        charsheet.Assets.Mana       += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Mana;
        //skills
        charsheet.Skills.Combat     += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Combat;
        charsheet.Skills.Arcane     += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Arcane;
        charsheet.Skills.Psionics   += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Psionics;
        charsheet.Skills.Hide       += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Hide;
        charsheet.Skills.Traps      += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Traps;
        charsheet.Skills.Tactics    += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Tactics;
        charsheet.Skills.Social     += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Social;
        charsheet.Skills.Apothecary += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Apothecary;
        charsheet.Skills.Travel     += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Travel;
        charsheet.Skills.Sail       += dbs.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Sail;

        // highborn should have a HT with no fear rolls
    }

    private void SetSheetForUndermoutain(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Strength;
        charsheet.Stats.Constitution+= dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Constitution;
        charsheet.Stats.Agility     += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Agility;
        charsheet.Stats.Willpower   += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Willpower;
        charsheet.Stats.Perception  += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Perception;
        charsheet.Stats.Abstract    += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Abstract;
        //assets
        charsheet.Assets.Resolve    += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Resolve;
        charsheet.Assets.Harm       += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Harm;
        charsheet.Assets.Spot       += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Spot;
        charsheet.Assets.Defense    += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Defense;
        charsheet.Assets.Purge      += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Purge;
        charsheet.Assets.Mana       += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Mana;
        //skills
        charsheet.Skills.Combat     += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Combat;
        charsheet.Skills.Arcane     += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Arcane;
        charsheet.Skills.Psionics   += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Psionics;
        charsheet.Skills.Hide       += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Hide;
        charsheet.Skills.Traps      += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Traps;
        charsheet.Skills.Tactics    += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Tactics;
        charsheet.Skills.Social     += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Social;
        charsheet.Skills.Apothecary += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Apothecary;
        charsheet.Skills.Travel     += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Travel;
        charsheet.Skills.Sail       += dbs.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Sail;

        // should start with heavy armour
    }

    private void DistributeClassAttributes(CharacterSheet sheet, string classes, int statPoints, int skillPoints)
    {
        List<string> likelyStats = new();
        List<string> unlikelyStats = new();
        List<string> likeySkills = new();
        List<string> unlikelySkills = new();

        if (classes == CharactersLore.Classes.Warrior)
        {
            likelyStats = dbs.Snapshot.Rulebook.Classes.Warrior.LikelyStats;
            unlikelyStats = dbs.Snapshot.Rulebook.Classes.Warrior.UnlikelyStats;
            likeySkills = dbs.Snapshot.Rulebook.Classes.Warrior.LikelySkills;
            unlikelySkills = dbs.Snapshot.Rulebook.Classes.Warrior.UnlikelySkills;
        }
        else if (classes == CharactersLore.Classes.Mage)
        {
            likelyStats = dbs.Snapshot.Rulebook.Classes.Mage.LikelyStats;
            unlikelyStats = dbs.Snapshot.Rulebook.Classes.Mage.UnlikelyStats;
            likeySkills = dbs.Snapshot.Rulebook.Classes.Mage.LikelySkills;
            unlikelySkills = dbs.Snapshot.Rulebook.Classes.Mage.UnlikelySkills;
        }
        else if (classes == CharactersLore.Classes.Hunter)
        {
            likelyStats = dbs.Snapshot.Rulebook.Classes.Hunter.LikelyStats;
            unlikelyStats = dbs.Snapshot.Rulebook.Classes.Hunter.UnlikelyStats;
            likeySkills = dbs.Snapshot.Rulebook.Classes.Hunter.LikelySkills;
            unlikelySkills = dbs.Snapshot.Rulebook.Classes.Hunter.UnlikelySkills;
        }
        else if (classes == CharactersLore.Classes.Swashbuckler)
        {
            likelyStats = dbs.Snapshot.Rulebook.Classes.Swashbuckler.LikelyStats;
            unlikelyStats = dbs.Snapshot.Rulebook.Classes.Swashbuckler.UnlikelyStats;
            likeySkills = dbs.Snapshot.Rulebook.Classes.Swashbuckler.LikelySkills;
            unlikelySkills = dbs.Snapshot.Rulebook.Classes.Swashbuckler.UnlikelySkills;
        }
        else if (classes == CharactersLore.Classes.Sorcerer)
        {
            likelyStats = dbs.Snapshot.Rulebook.Classes.Sorcerer.LikelyStats;
            unlikelyStats = dbs.Snapshot.Rulebook.Classes.Sorcerer.UnlikelyStats;
            likeySkills = dbs.Snapshot.Rulebook.Classes.Sorcerer.LikelySkills;
            unlikelySkills = dbs.Snapshot.Rulebook.Classes.Sorcerer.UnlikelySkills;
        }

        IncreaseStats(sheet, statPoints, likelyStats, unlikelyStats);
        IncreaseSkills(sheet, skillPoints, likeySkills, unlikelySkills);
    }

    private void IncreaseStats(CharacterSheet sheet, int statPoints, List<string> likelyStats, List<string> unlikelyStats)
    {
        while (statPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenStat;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_dX(likelyStats.Count);
                chosenStat = likelyStats[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(unlikelyStats.Count);
                chosenStat = unlikelyStats[rollForStat - 1];
            }

            if      (chosenStat == dbs.Snapshot.Rulebook.Acronyms.Stats.Strength) sheet.Stats.Strength++;
            else if (chosenStat == dbs.Snapshot.Rulebook.Acronyms.Stats.Constitution) sheet.Stats.Constitution++;
            else if (chosenStat == dbs.Snapshot.Rulebook.Acronyms.Stats.Agility) sheet.Stats.Agility++;
            else if (chosenStat == dbs.Snapshot.Rulebook.Acronyms.Stats.Willpower) sheet.Stats.Willpower++;
            else if (chosenStat == dbs.Snapshot.Rulebook.Acronyms.Stats.Perception) sheet.Stats.Perception++;
            else if (chosenStat == dbs.Snapshot.Rulebook.Acronyms.Stats.Abstract) sheet.Stats.Abstract++;

            statPoints--;
        }
    }

    private void IncreaseSkills(CharacterSheet sheet, int skillPoints, List<string> likeySkills, List<string> unlikeySkills)
    {
        while (skillPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenSkill;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_dX(likeySkills.Count);
                chosenSkill = likeySkills[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(unlikeySkills.Count);
                chosenSkill = unlikeySkills[rollForStat - 1];
            }

            if      (chosenSkill == dbs.Snapshot.Rulebook.Acronyms.Skills.Combat) sheet.Skills.Combat++;
            else if (chosenSkill == dbs.Snapshot.Rulebook.Acronyms.Skills.Arcane) sheet.Skills.Arcane++;
            else if (chosenSkill == dbs.Snapshot.Rulebook.Acronyms.Skills.Psionics) sheet.Skills.Psionics++;
            else if (chosenSkill == dbs.Snapshot.Rulebook.Acronyms.Skills.Hide) sheet.Skills.Hide++;
            else if (chosenSkill == dbs.Snapshot.Rulebook.Acronyms.Skills.Traps) sheet.Skills.Traps++;
            else if (chosenSkill == dbs.Snapshot.Rulebook.Acronyms.Skills.Tactics) sheet.Skills.Tactics++;
            else if (chosenSkill == dbs.Snapshot.Rulebook.Acronyms.Skills.Social) sheet.Skills.Social++;
            else if (chosenSkill == dbs.Snapshot.Rulebook.Acronyms.Skills.Apothecary) sheet.Skills.Apothecary++;
            else if (chosenSkill == dbs.Snapshot.Rulebook.Acronyms.Skills.Travel) sheet.Skills.Travel++;
            else if (chosenSkill == dbs.Snapshot.Rulebook.Acronyms.Skills.Sail) sheet.Skills.Sail++;

            skillPoints--;
        }
    }
    #endregion
}
