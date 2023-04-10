#pragma warning disable CS8629 // Nullable value type may be null.

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
        var lvl = (int)info.EntityLevel;

        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength = 5 * lvl,
                Constitution = 5 * lvl,
                Agility = 5 * lvl,
                Willpower = 5 * lvl,
                Perception = 5 * lvl,
                Abstract = 5 * lvl
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Human.Danarian)
        {
            charsheet.Skills.Combat += 20;
            charsheet.Skills.Travel += 10;
            charsheet.Skills.Hide -= 10;
            charsheet.Skills.Sail -= 30;

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
                Strength = 2 * lvl,
                Constitution = 7 * lvl,
                Agility = 15 * lvl,
                Willpower = 7 * lvl,
                Perception = 10 * lvl,
                Abstract = 10 * lvl
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Elf.Highborn)
        {
            charsheet.Stats.Willpower += 10;
            charsheet.Assets.Spot += 50;
            charsheet.Assets.Mana += 50;
            charsheet.Skills.Arcane += 40;
            charsheet.Skills.Travel -= 100;
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
                Strength = 15 * lvl,
                Constitution = 10 * lvl,
                Agility = 2 * lvl,
                Willpower = 10 * lvl,
                Perception = 2 * lvl,
                Abstract = 10 * lvl
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Dwarf.Undermountain)
        {
            charsheet.Stats.Strength += 10;
            charsheet.Assets.Defense += 10;
            charsheet.Assets.Purge += 10;
            charsheet.Assets.Harm += 20;
            charsheet.Skills.Combat += 30;
            charsheet.Skills.Hide -= 40;
            charsheet.Skills.Social -= 20;
            charsheet.Skills.Travel -= 50;
            charsheet.Skills.Sail -= 200;
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