#pragma warning disable CS8629 // Nullable value type may be null.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterSheetLogic
{
    private readonly IDiceRollService dice;
    private readonly IDatabaseManager dbm;

    internal CharacterSheetLogic(
        IDatabaseManager databaseManager,
        IDiceRollService dice)
    {
        this.dice = dice;
        dbm = databaseManager;
    }

    internal CharacterSheet SetCharacterSheet(CharacterInfo info, int statPoints, int skillPoints)
    {
        if      (info.Race == CharactersLore.Races.Human) return SetCharacterSheetForHuman(info, statPoints, skillPoints);
        else if (info.Race == CharactersLore.Races.Elf) return SetCharacterSheetForElf(info, statPoints, skillPoints);
        else if (info.Race == CharactersLore.Races.Dwarf) return SetCharacterSheetForDwarf(info, statPoints, skillPoints);
        else throw new NotImplementedException();
    }

    #region private methods
    private CharacterSheet SetCharacterSheetForHuman(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel;

        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = lvl * dbm.Snapshot.Rulebook.Races.Human.Strength,
                Constitution= lvl * dbm.Snapshot.Rulebook.Races.Human.Constitution,
                Agility     = lvl * dbm.Snapshot.Rulebook.Races.Human.Agility,
                Willpower   = lvl * dbm.Snapshot.Rulebook.Races.Human.Willpower,
                Perception  = lvl * dbm.Snapshot.Rulebook.Races.Human.Perception,
                Abstract    = lvl * dbm.Snapshot.Rulebook.Races.Human.Abstract
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Human.Danarian) SetSheetForDanarian(charsheet);
        else throw new Exception("Character culture not found.");

        DistributeClassAttributes(charsheet, info.Class, statPoints, skillPoints);

        return charsheet;
    }

    private CharacterSheet SetCharacterSheetForElf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = lvl * dbm.Snapshot.Rulebook.Races.Elf.Strength,
                Constitution= lvl * dbm.Snapshot.Rulebook.Races.Elf.Constitution,
                Agility     = lvl * dbm.Snapshot.Rulebook.Races.Elf.Agility,
                Willpower   = lvl * dbm.Snapshot.Rulebook.Races.Elf.Willpower,
                Perception  = lvl * dbm.Snapshot.Rulebook.Races.Elf.Perception,
                Abstract    = lvl * dbm.Snapshot.Rulebook.Races.Elf.Abstract
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Elf.Highborn) SetSheetForHighborn(charsheet);
        else throw new Exception("Character culture not found.");

        DistributeClassAttributes(charsheet, info.Class, statPoints, skillPoints);

        return charsheet;
    }

    private CharacterSheet SetCharacterSheetForDwarf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Strength,
                Constitution= lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Constitution,
                Agility     = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Agility,
                Willpower   = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Willpower,
                Perception  = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Perception,
                Abstract    = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Abstract
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Dwarf.Undermountain) SetSheetForUndermoutain(charsheet);
        else throw new Exception("Character culture not found.");

        DistributeClassAttributes(charsheet, info.Class, statPoints, skillPoints);

        return charsheet;
    }

    private void SetSheetForDanarian(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Strength;
        charsheet.Stats.Constitution+= dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Constitution;
        charsheet.Stats.Agility     += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Agility;
        charsheet.Stats.Willpower   += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Willpower;
        charsheet.Stats.Perception  += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Perception;
        charsheet.Stats.Abstract    += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Stats.Abstract;
        //assets
        charsheet.Assets.Resolve    += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Resolve;
        charsheet.Assets.Harm       += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Harm;
        charsheet.Assets.Spot       += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Spot;
        charsheet.Assets.Defense    += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Defense;
        charsheet.Assets.Purge      += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Purge;
        charsheet.Assets.Mana       += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Assets.Mana;
        //skills
        charsheet.Skills.Combat     += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Combat;
        charsheet.Skills.Arcane     += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Arcane;
        charsheet.Skills.Psionics   += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Psionics;
        charsheet.Skills.Hide       += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Hide;
        charsheet.Skills.Traps      += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Traps;
        charsheet.Skills.Tactics    += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Tactics;
        charsheet.Skills.Social     += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Social;
        charsheet.Skills.Apothecary += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Apothecary;
        charsheet.Skills.Travel     += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Travel;
        charsheet.Skills.Sail       += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Skills.Sail;

        // danarias should start with a weapon, armour and helm
    }

    private void SetSheetForHighborn(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Strength;
        charsheet.Stats.Constitution+= dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Constitution;
        charsheet.Stats.Agility     += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Agility;
        charsheet.Stats.Willpower   += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Willpower;
        charsheet.Stats.Perception  += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Perception;
        charsheet.Stats.Abstract    += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Stats.Abstract;
        //assets
        charsheet.Assets.Resolve    += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Resolve;
        charsheet.Assets.Harm       += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Harm;
        charsheet.Assets.Spot       += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Spot;
        charsheet.Assets.Defense    += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Defense;
        charsheet.Assets.Purge      += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Purge;
        charsheet.Assets.Mana       += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Assets.Mana;
        //skills
        charsheet.Skills.Combat     += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Combat;
        charsheet.Skills.Arcane     += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Arcane;
        charsheet.Skills.Psionics   += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Psionics;
        charsheet.Skills.Hide       += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Hide;
        charsheet.Skills.Traps      += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Traps;
        charsheet.Skills.Tactics    += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Tactics;
        charsheet.Skills.Social     += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Social;
        charsheet.Skills.Apothecary += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Apothecary;
        charsheet.Skills.Travel     += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Travel;
        charsheet.Skills.Sail       += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Skills.Sail;

        // highborn should have a HT with no fear rolls
    }

    private void SetSheetForUndermoutain(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Strength;
        charsheet.Stats.Constitution+= dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Constitution;
        charsheet.Stats.Agility     += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Agility;
        charsheet.Stats.Willpower   += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Willpower;
        charsheet.Stats.Perception  += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Perception;
        charsheet.Stats.Abstract    += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Stats.Abstract;
        //assets
        charsheet.Assets.Resolve    += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Resolve;
        charsheet.Assets.Harm       += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Harm;
        charsheet.Assets.Spot       += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Spot;
        charsheet.Assets.Defense    += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Defense;
        charsheet.Assets.Purge      += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Purge;
        charsheet.Assets.Mana       += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Assets.Mana;
        //skills
        charsheet.Skills.Combat     += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Combat;
        charsheet.Skills.Arcane     += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Arcane;
        charsheet.Skills.Psionics   += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Psionics;
        charsheet.Skills.Hide       += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Hide;
        charsheet.Skills.Traps      += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Traps;
        charsheet.Skills.Tactics    += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Tactics;
        charsheet.Skills.Social     += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Social;
        charsheet.Skills.Apothecary += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Apothecary;
        charsheet.Skills.Travel     += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Travel;
        charsheet.Skills.Sail       += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Skills.Sail;

        // should start with heavy armour
    }

    private void DistributeClassAttributes(CharacterSheet sheet, string classes, int statPoints, int skillPoints)
    {
        if      (classes == CharactersLore.Classes.Warrior) SetClassForWarrior(sheet, statPoints, skillPoints);
        else if (classes == CharactersLore.Classes.Mage) SetClassForMage(sheet, statPoints, skillPoints);
        else if (classes == CharactersLore.Classes.Hunter) SetClassForHunter(sheet, statPoints, skillPoints);
        else if (classes == CharactersLore.Classes.Swashbuckler) SetClassForSwashbuckler(sheet, statPoints, skillPoints);
        else if (classes == CharactersLore.Classes.Sorcerer) SetClassForSorcerer(sheet, statPoints, skillPoints);
        else throw new NotImplementedException();
    }

    private CharacterSheet SetClassForSorcerer(CharacterSheet sheet, int statPoints, int skillPoints)
    {
        var likelyStats = dbm.Snapshot.Rulebook.Classes.Sorcerer.LikelyStats;
        var unlikelyStats = dbm.Snapshot.Rulebook.Classes.Sorcerer.UnlikelyStats;
        var likeySkills = dbm.Snapshot.Rulebook.Classes.Sorcerer.LikelySkills;
        var unlikelySkills = dbm.Snapshot.Rulebook.Classes.Sorcerer.UnlikelySkills;

        IncreaseStats(sheet, statPoints, likelyStats, unlikelyStats);
        IncreaseSkills(sheet, skillPoints, likeySkills, unlikelySkills);

        return sheet;
    }

    private CharacterSheet SetClassForSwashbuckler(CharacterSheet sheet, int statPoints, int skillPoints)
    {
        var likelyStats = dbm.Snapshot.Rulebook.Classes.Swashbuckler.LikelyStats;
        var unlikelyStats = dbm.Snapshot.Rulebook.Classes.Swashbuckler.UnlikelyStats;
        var likeySkills = dbm.Snapshot.Rulebook.Classes.Swashbuckler.LikelySkills;
        var unlikelySkills = dbm.Snapshot.Rulebook.Classes.Swashbuckler.UnlikelySkills;

        IncreaseStats(sheet, statPoints, likelyStats, unlikelyStats);
        IncreaseSkills(sheet, skillPoints, likeySkills, unlikelySkills);

        return sheet;
    }

    private CharacterSheet SetClassForHunter(CharacterSheet sheet, int statPoints, int skillPoints)
    {
        var likelyStats = dbm.Snapshot.Rulebook.Classes.Hunter.LikelyStats;
        var unlikelyStats = dbm.Snapshot.Rulebook.Classes.Hunter.UnlikelyStats;
        var likeySkills = dbm.Snapshot.Rulebook.Classes.Hunter.LikelySkills;
        var unlikelySkills = dbm.Snapshot.Rulebook.Classes.Hunter.UnlikelySkills;

        IncreaseStats(sheet, statPoints, likelyStats, unlikelyStats);
        IncreaseSkills(sheet, skillPoints, likeySkills, unlikelySkills);

        return sheet;
    }

    private CharacterSheet SetClassForMage(CharacterSheet sheet, int statPoints, int skillPoints)
    {
        var likelyStats = dbm.Snapshot.Rulebook.Classes.Mage.LikelyStats;
        var unlikelyStats = dbm.Snapshot.Rulebook.Classes.Mage.UnlikelyStats;
        var likeySkills = dbm.Snapshot.Rulebook.Classes.Mage.LikelySkills;
        var unlikelySkills = dbm.Snapshot.Rulebook.Classes.Mage.UnlikelySkills;

        IncreaseStats(sheet, statPoints, likelyStats, unlikelyStats);
        IncreaseSkills(sheet, skillPoints, likeySkills, unlikelySkills);

        return sheet;
    }

    private CharacterSheet SetClassForWarrior(CharacterSheet sheet, int statPoints, int skillPoints)
    {
        var likelyStats = dbm.Snapshot.Rulebook.Classes.Warrior.LikelyStats;
        var unlikelyStats = dbm.Snapshot.Rulebook.Classes.Warrior.UnlikelyStats;
        var likeySkills = dbm.Snapshot.Rulebook.Classes.Warrior.LikelySkills;
        var unlikelySkills = dbm.Snapshot.Rulebook.Classes.Warrior.UnlikelySkills;

        IncreaseStats(sheet, statPoints, likelyStats, unlikelyStats);
        IncreaseSkills(sheet, skillPoints, likeySkills, unlikelySkills);

        return sheet;
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

            if      (chosenStat == dbm.Snapshot.Rulebook.Acronyms.Stats.Strength) sheet.Stats.Strength++;
            else if (chosenStat == dbm.Snapshot.Rulebook.Acronyms.Stats.Constitution) sheet.Stats.Constitution++;
            else if (chosenStat == dbm.Snapshot.Rulebook.Acronyms.Stats.Agility) sheet.Stats.Agility++;
            else if (chosenStat == dbm.Snapshot.Rulebook.Acronyms.Stats.Willpower) sheet.Stats.Willpower++;
            else if (chosenStat == dbm.Snapshot.Rulebook.Acronyms.Stats.Perception) sheet.Stats.Perception++;
            else if (chosenStat == dbm.Snapshot.Rulebook.Acronyms.Stats.Abstract) sheet.Stats.Abstract++;
            else throw new Exception("Unable to increase stat, wrong stat string.");

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

            if      (chosenSkill == dbm.Snapshot.Rulebook.Acronyms.Skills.Combat) sheet.Skills.Combat++;
            else if (chosenSkill == dbm.Snapshot.Rulebook.Acronyms.Skills.Arcane) sheet.Skills.Arcane++;
            else if (chosenSkill == dbm.Snapshot.Rulebook.Acronyms.Skills.Psionics) sheet.Skills.Psionics++;
            else if (chosenSkill == dbm.Snapshot.Rulebook.Acronyms.Skills.Hide) sheet.Skills.Hide++;
            else if (chosenSkill == dbm.Snapshot.Rulebook.Acronyms.Skills.Traps) sheet.Skills.Traps++;
            else if (chosenSkill == dbm.Snapshot.Rulebook.Acronyms.Skills.Tactics) sheet.Skills.Tactics++;
            else if (chosenSkill == dbm.Snapshot.Rulebook.Acronyms.Skills.Social) sheet.Skills.Social++;
            else if (chosenSkill == dbm.Snapshot.Rulebook.Acronyms.Skills.Apothecary) sheet.Skills.Apothecary++;
            else if (chosenSkill == dbm.Snapshot.Rulebook.Acronyms.Skills.Travel) sheet.Skills.Travel++;
            else if (chosenSkill == dbm.Snapshot.Rulebook.Acronyms.Skills.Sail) sheet.Skills.Sail++;
            else throw new Exception("Unable to increase skill, wrong skill string.");

            skillPoints--;
        }
    }
    #endregion
}

#pragma warning restore CS8629 // Nullable value type may be null.