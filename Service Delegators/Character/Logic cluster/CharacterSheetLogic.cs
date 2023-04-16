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

    internal CharacterSheet SetCharacterSheetForHuman(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel;

        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength        = lvl * dbm.Snapshot.Rulebook.Races.Human.Str,
                Constitution    = lvl * dbm.Snapshot.Rulebook.Races.Human.Con,
                Agility         = lvl * dbm.Snapshot.Rulebook.Races.Human.Agl,
                Willpower       = lvl * dbm.Snapshot.Rulebook.Races.Human.Wil,
                Perception      = lvl * dbm.Snapshot.Rulebook.Races.Human.Per,
                Abstract        = lvl * dbm.Snapshot.Rulebook.Races.Human.Abs
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Human.Danarian)
        {
            //stats
            charsheet.Stats.Strength    += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Str;
            charsheet.Stats.Constitution+= dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Con;
            charsheet.Stats.Agility     += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Agi;
            charsheet.Stats.Willpower   += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Wil;
            charsheet.Stats.Perception  += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Per;
            charsheet.Stats.Abstract    += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Abs;
            //assets
            charsheet.Assets.Resolve    += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Res;
            charsheet.Assets.Harm       += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Har;
            charsheet.Assets.Spot       += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Spo;
            charsheet.Assets.Defense    += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Def;
            charsheet.Assets.Purge      += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Pur;
            charsheet.Assets.Mana       += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Man;
            //skills
            charsheet.Skills.Combat     += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Com;
            charsheet.Skills.Arcane     += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Arc;
            charsheet.Skills.Psionics   += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Psi;
            charsheet.Skills.Hide       += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Hid;
            charsheet.Skills.Traps      += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Tra;
            charsheet.Skills.Tactics    += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Tac;
            charsheet.Skills.Social     += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Soc;
            charsheet.Skills.Apothecary += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Apo;
            charsheet.Skills.Travel     += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Tra;
            charsheet.Skills.Sail       += dbm.Snapshot.Rulebook.Cultures.Humans.Danarian.Sai;

            //var itemsRoll = dice.Roll_dX(6);

            //for (int i = 0; i < itemsRoll; i++)
            //{
            //    var item = itemService.GenerateSpecificItem(ItemsLore.Types.Protection, ItemsLore.Subtypes.Protections.Armour);
            //    character.Supplies.Add(item);
            //}
        }

        charsheet = DistributeClassStatsAndSkills(charsheet, info.Class, statPoints, skillPoints);

        return charsheet;
    }

    internal CharacterSheet SetCharacterSheetForElf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength        = lvl * dbm.Snapshot.Rulebook.Races.Elf.Str,
                Constitution    = lvl * dbm.Snapshot.Rulebook.Races.Elf.Con,
                Agility         = lvl * dbm.Snapshot.Rulebook.Races.Elf.Agl,
                Willpower       = lvl * dbm.Snapshot.Rulebook.Races.Elf.Wil,
                Perception      = lvl * dbm.Snapshot.Rulebook.Races.Elf.Per,
                Abstract        = lvl * dbm.Snapshot.Rulebook.Races.Elf.Abs
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Elf.Highborn)
        {
            //stats
            charsheet.Stats.Strength    += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Str;
            charsheet.Stats.Constitution+= dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Con;
            charsheet.Stats.Agility     += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Agi;
            charsheet.Stats.Willpower   += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Wil;
            charsheet.Stats.Perception  += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Per;
            charsheet.Stats.Abstract    += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Abs;
            //assets
            charsheet.Assets.Resolve    += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Res;
            charsheet.Assets.Harm       += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Har;
            charsheet.Assets.Spot       += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Spo;
            charsheet.Assets.Defense    += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Def;
            charsheet.Assets.Purge      += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Pur;
            charsheet.Assets.Mana       += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Man;
            //skills
            charsheet.Skills.Combat     += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Com;
            charsheet.Skills.Arcane     += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Arc;
            charsheet.Skills.Psionics   += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Psi;
            charsheet.Skills.Hide       += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Hid;
            charsheet.Skills.Traps      += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Tra;
            charsheet.Skills.Tactics    += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Tac;
            charsheet.Skills.Social     += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Soc;
            charsheet.Skills.Apothecary += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Apo;
            charsheet.Skills.Travel     += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Tra;
            charsheet.Skills.Sail       += dbm.Snapshot.Rulebook.Cultures.Elves.Highborn.Sai;
        }

        charsheet = DistributeClassStatsAndSkills(charsheet, info.Class, statPoints, skillPoints);

        return charsheet;
    }

    internal CharacterSheet SetCharacterSheetForDwarf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength        = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Str,
                Constitution    = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Con,
                Agility         = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Agl,
                Willpower       = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Wil,
                Perception      = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Per,
                Abstract        = lvl * dbm.Snapshot.Rulebook.Races.Dwarf.Abs
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Dwarf.Undermountain)
        {
            //stats
            charsheet.Stats.Strength    += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Str;
            charsheet.Stats.Constitution+= dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Con;
            charsheet.Stats.Agility     += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Agi;
            charsheet.Stats.Willpower   += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Wil;
            charsheet.Stats.Perception  += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Per;
            charsheet.Stats.Abstract    += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Abs;
            //assets
            charsheet.Assets.Resolve  += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Res;
            charsheet.Assets.Harm       += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Har;
            charsheet.Assets.Spot       += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Spo;
            charsheet.Assets.Defense    += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Def;
            charsheet.Assets.Purge      += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Pur;
            charsheet.Assets.Mana       += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Man;
            //skills
            charsheet.Skills.Combat     += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Com;
            charsheet.Skills.Arcane     += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Arc;
            charsheet.Skills.Psionics   += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Psi;
            charsheet.Skills.Hide       += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Hid;
            charsheet.Skills.Traps      += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Tra;
            charsheet.Skills.Tactics    += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Tac;
            charsheet.Skills.Social     += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Soc;
            charsheet.Skills.Apothecary += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Apo;
            charsheet.Skills.Travel     += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Tra;
            charsheet.Skills.Sail       += dbm.Snapshot.Rulebook.Cultures.Dwarves.Undermountain.Sai;
        }

        charsheet = DistributeClassStatsAndSkills(charsheet, info.Class, statPoints, skillPoints);

        return charsheet;
    }

    internal CharacterSheet DistributeClassStatsAndSkills(CharacterSheet sheet, string classes, int statPoints, int skillPoints)
    {
        if      (classes == CharactersLore.Classes.Warrior) return SetClassForWarrior(sheet, statPoints, skillPoints);
        else if (classes == CharactersLore.Classes.Spellcaster) return SetClassForSpellcaster(sheet, statPoints, skillPoints);
        else if (classes == CharactersLore.Classes.Hunter) return SetClassForHunter(sheet, statPoints, skillPoints);
        else throw new NotImplementedException();
    }

    internal CharacterSheet SetClassForHunter(CharacterSheet sheet, int statPoints, int skillPoints)
    {
        var primaryStats = new List<string>
        {
            CharactersLore.Stats.Agility,
            CharactersLore.Stats.Perception
        };
        var unlikelyStats = new List<string>
        {
            CharactersLore.Stats.Agility,
            CharactersLore.Stats.Constitution
        };
        var primarySkills = new List<string>
        {
            CharactersLore.Skills.Hide,
            CharactersLore.Skills.Traps
        };
        var unlikelySkills = new List<string>
        {
            CharactersLore.Skills.Combat,
            CharactersLore.Skills.Travel
        };

        // stats
        while (statPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenStat;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_dX(primaryStats.Count);
                chosenStat = primaryStats[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(unlikelyStats.Count);
                chosenStat = unlikelyStats[rollForStat - 1];
            }

            if (chosenStat == CharactersLore.Stats.Agility)
            {
                sheet.Stats.Agility++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Perception)
            {
                sheet.Stats.Perception++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Constitution)
            {
                sheet.Stats.Constitution++;
                statPoints--;
            }
        }
        // skills
        while (skillPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenSkill;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_dX(primarySkills.Count);
                chosenSkill = primarySkills[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(unlikelySkills.Count);
                chosenSkill = unlikelySkills[rollForStat - 1];
            }

            if (chosenSkill == CharactersLore.Skills.Hide)
            {
                sheet.Skills.Hide++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Traps)
            {
                sheet.Skills.Traps++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Combat)
            {
                sheet.Skills.Combat++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Travel)
            {
                sheet.Skills.Travel++;
                skillPoints--;
            }
        }

        return sheet;
    }

    internal CharacterSheet SetClassForSpellcaster(CharacterSheet sheet, int statPoints, int skillPoints)
    {
        var primaryStats = new List<string>
        {
            CharactersLore.Stats.Abstract,
            CharactersLore.Stats.Constitution,
        };
        var unlikelyStats = new List<string>
        {
            CharactersLore.Stats.Willpower,
            CharactersLore.Stats.Agility,
            CharactersLore.Stats.Perception
        };
        var primarySkills = new List<string>
        {
            CharactersLore.Skills.Arcane,
        };
        var unlikelySkills = new List<string>
        {
            CharactersLore.Skills.Social,
            CharactersLore.Skills.Combat,
            CharactersLore.Skills.Apothecary,
            CharactersLore.Skills.Tactics
        };

        // stats
        while (statPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenStat;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_dX(primaryStats.Count);
                chosenStat = primaryStats[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(unlikelyStats.Count);
                chosenStat = unlikelyStats[rollForStat - 1];
            }

            if (chosenStat == CharactersLore.Stats.Abstract)
            {
                sheet.Stats.Abstract++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Constitution)
            {
                sheet.Stats.Constitution++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Willpower)
            {
                sheet.Stats.Willpower++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Agility)
            {
                sheet.Stats.Agility++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Perception)
            {
                sheet.Stats.Perception++;
                statPoints--;
            }
        }
        // skills
        while (skillPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenSkill;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_dX(primarySkills.Count);
                chosenSkill = primarySkills[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(unlikelySkills.Count);
                chosenSkill = unlikelySkills[rollForStat - 1];
            }

            if (chosenSkill == CharactersLore.Skills.Arcane)
            {
                sheet.Skills.Arcane++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Social)
            {
                sheet.Skills.Social++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Apothecary)
            {
                sheet.Skills.Apothecary++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Combat)
            {
                sheet.Skills.Combat++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Tactics)
            {
                sheet.Skills.Tactics++;
                skillPoints--;
            }
        }

        return sheet;
    }

    internal CharacterSheet SetClassForWarrior(CharacterSheet sheet, int statPoints, int skillPoints)
    {
        var primaryStats = new List<string>
        {
            CharactersLore.Stats.Strength,
            CharactersLore.Stats.Constitution,
            CharactersLore.Stats.Agility
        };
        var unlikelyStats = new List<string>
        {
            CharactersLore.Stats.Willpower,
            CharactersLore.Stats.Perception
        };
        var primarySkills = new List<string>
        {
            CharactersLore.Skills.Combat,
        };
        var unlikelySkills = new List<string>
        {
            CharactersLore.Skills.Tactics,
            CharactersLore.Skills.Travel
        };

        // stats
        while (statPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenStat;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_dX(primaryStats.Count);
                chosenStat = primaryStats[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(unlikelyStats.Count);
                chosenStat = unlikelyStats[rollForStat - 1];
            }

            if (chosenStat == CharactersLore.Stats.Strength)
            {
                sheet.Stats.Strength++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Constitution)
            {
                sheet.Stats.Constitution++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Agility)
            {
                sheet.Stats.Agility++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Willpower)
            {
                sheet.Stats.Willpower++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Perception)
            {
                sheet.Stats.Perception++;
                statPoints--;
            }
        }
        // skills
        while (skillPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenSkill;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_dX(primarySkills.Count);
                chosenSkill = primarySkills[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(unlikelySkills.Count);
                chosenSkill = unlikelySkills[rollForStat - 1];
            }

            if (chosenSkill == CharactersLore.Skills.Combat)
            {
                sheet.Skills.Combat++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Tactics)
            {
                sheet.Skills.Tactics++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Travel)
            {
                sheet.Skills.Travel++;
                skillPoints--;
            }
        }

        return sheet;
    }
}

#pragma warning restore CS8629 // Nullable value type may be null.