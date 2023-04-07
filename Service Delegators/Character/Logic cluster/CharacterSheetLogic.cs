using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterSheetLogic
{
    private readonly IDiceRollService dice;

    internal CharacterSheetLogic(IDiceRollService dice)
    {
        this.dice = dice;
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
        var lvl = (int)info.EntityLevel!;

        var charsheet = new CharacterSheet
        {
            Strength = 5 * lvl,
            Constitution = 5 * lvl,
            Agility = 5 * lvl,
            Willpower = 5 * lvl,
            Perception = 5 * lvl,
            Abstract = 5 * lvl
        };

        if (info.Culture == CharactersLore.Cultures.Human.Danarian)
        {
            charsheet.Combat += 20;
            charsheet.Travel += 10;
            charsheet.Hide -= 10;
            charsheet.Sail -= 30;

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
            Strength = 2 * lvl,
            Constitution = 7 * lvl,
            Agility = 15 * lvl,
            Willpower = 6 * lvl,
            Perception = 10 * lvl,
            Abstract = 10 * lvl
        };

        if (info.Culture == CharactersLore.Cultures.Elf.Highborn)
        {
            charsheet.Arcane += 40;
            charsheet.Mana += 50;
            charsheet.Willpower += 10;
            charsheet.Travel -= 100;
        }

        charsheet = DistributeClassStatsAndSkills(charsheet, info.Class, statPoints, skillPoints);

        return charsheet;
    }

    internal CharacterSheet SetCharacterSheetForDwarf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var charsheet = new CharacterSheet
        {
            Strength = 15 * lvl,
            Constitution = 10 * lvl,
            Agility = 2 * lvl,
            Willpower = 10 * lvl,
            Perception = 2 * lvl,
            Abstract = 10 * lvl
        };

        if (info.Culture == CharactersLore.Cultures.Dwarf.Undermountain)
        {
            charsheet.Combat += 30;
            charsheet.Armour += 10;
            charsheet.Purge += 10;
            charsheet.Harm += 20;
            charsheet.Hide -= 40;
            charsheet.Social -= 20;
            charsheet.Travel -= 50;
            charsheet.Sail -= 200;
        }

        charsheet = DistributeClassStatsAndSkills(charsheet, info.Class, statPoints, skillPoints);

        return charsheet;
    }

    internal CharacterSheet DistributeClassStatsAndSkills(CharacterSheet sheet, string classes, int statPoints, int skillPoints)
    {
        if (classes == CharactersLore.Classes.Warrior) return SetClassForWarrior(sheet, statPoints, skillPoints);
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
                sheet.Agility++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Perception)
            {
                sheet.Perception++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Constitution)
            {
                sheet.Constitution++;
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
                sheet.Hide++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Traps)
            {
                sheet.Traps++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Combat)
            {
                sheet.Combat++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Travel)
            {
                sheet.Travel++;
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
                sheet.Abstract++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Constitution)
            {
                sheet.Constitution++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Willpower)
            {
                sheet.Willpower++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Agility)
            {
                sheet.Agility++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Perception)
            {
                sheet.Perception++;
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
                sheet.Arcane++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Social)
            {
                sheet.Social++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Apothecary)
            {
                sheet.Apothecary++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Combat)
            {
                sheet.Combat++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Tactics)
            {
                sheet.Tactics++;
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
                sheet.Strength++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Constitution)
            {
                sheet.Constitution++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Agility)
            {
                sheet.Agility++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Willpower)
            {
                sheet.Willpower++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Perception)
            {
                sheet.Perception++;
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
                sheet.Combat++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Tactics)
            {
                sheet.Tactics++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Travel)
            {
                sheet.Travel++;
                skillPoints--;
            }
        }

        return sheet;
    }
}
