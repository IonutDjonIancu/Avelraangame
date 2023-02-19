using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterDollOperations
{
    private readonly IDiceRollService dice;

    internal CharacterDollOperations(IDiceRollService dice)
    {
        this.dice = dice;
    }

    internal CharacterPaperDoll SetPaperDoll(CharacterInfo info, int statPoints, int skillPoints)
    {
        if      (info.Race == CharactersLore.Races.Human) return SetDollForHuman(info, statPoints, skillPoints);
        else if (info.Race == CharactersLore.Races.Elf) return SetDollForElf(info, statPoints, skillPoints);
        else if (info.Race == CharactersLore.Races.Dwarf) return SetDollForDwarf(info, statPoints, skillPoints);
        else throw new NotImplementedException();
    }

    internal CharacterPaperDoll SetDollForHuman(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var humanPaperDoll = new CharacterPaperDoll
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

    internal CharacterPaperDoll SetDollForElf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var elfPaperDoll = new CharacterPaperDoll
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
            elfPaperDoll.Arcane += 40;
            elfPaperDoll.Mana += 50;
            elfPaperDoll.Willpower += 10;
            elfPaperDoll.Travel -= 100;
        }

        elfPaperDoll = DistributeClassStatsAndSkills(elfPaperDoll, info.Class, statPoints, skillPoints);

        return elfPaperDoll;
    }

    internal CharacterPaperDoll SetDollForDwarf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var lvl = (int)info.EntityLevel!;

        var dwarfPaperDoll = new CharacterPaperDoll
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
            dwarfPaperDoll.Combat += 30;
            dwarfPaperDoll.Armour += 10;
            dwarfPaperDoll.Purge += 10;
            dwarfPaperDoll.Harm += 20;
            dwarfPaperDoll.Hide -= 40;
            dwarfPaperDoll.Social -= 20;
            dwarfPaperDoll.Travel -= 50;
            dwarfPaperDoll.Sail -= 200;
        }

        dwarfPaperDoll = DistributeClassStatsAndSkills(dwarfPaperDoll, info.Class, statPoints, skillPoints);

        return dwarfPaperDoll;
    }

    internal CharacterPaperDoll DistributeClassStatsAndSkills(CharacterPaperDoll doll, string classes, int statPoints, int skillPoints)
    {
        if (classes == CharactersLore.Classes.Warrior) return SetClassForWarrior(doll, statPoints, skillPoints);
        else if (classes == CharactersLore.Classes.Spellcaster) return SetClassForSpellcaster(doll, statPoints, skillPoints);
        else if (classes == CharactersLore.Classes.Hunter) return SetClassForHunter(doll, statPoints, skillPoints);
        else throw new NotImplementedException();
    }

    internal CharacterPaperDoll SetClassForHunter(CharacterPaperDoll doll, int statPoints, int skillPoints)
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
                doll.Agility++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Perception)
            {
                doll.Perception++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Constitution)
            {
                doll.Constitution++;
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
                doll.Hide++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Traps)
            {
                doll.Traps++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Combat)
            {
                doll.Combat++;
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

    internal CharacterPaperDoll SetClassForSpellcaster(CharacterPaperDoll doll, int statPoints, int skillPoints)
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
                doll.Abstract++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Constitution)
            {
                doll.Constitution++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Willpower)
            {
                doll.Willpower++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Agility)
            {
                doll.Agility++;
                statPoints--;
            }
            else if (chosenStat == CharactersLore.Stats.Perception)
            {
                doll.Perception++;
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
                doll.Arcane++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Social)
            {
                doll.Social++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Apothecary)
            {
                doll.Apothecary++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Combat)
            {
                doll.Combat++;
                skillPoints--;
            }
            else if (chosenSkill == CharactersLore.Skills.Tactics)
            {
                doll.Tactics++;
                skillPoints--;
            }
        }

        return doll;
    }


    internal CharacterPaperDoll SetClassForWarrior(CharacterPaperDoll doll, int statPoints, int skillPoints)
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
}
