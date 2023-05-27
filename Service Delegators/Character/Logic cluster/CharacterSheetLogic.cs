using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterSheetLogic
{
    private readonly IDiceRollService dice;

    private CharacterSheetLogic() { }
    internal CharacterSheetLogic(IDiceRollService diceService)
    {
        dice = diceService;
    }

    internal CharacterSheet SetCharacterSheet(CharacterInfo info, int statPoints, int skillPoints)
    {
        if      (info.Origins.Race == CharactersLore.Races.Human) return SetCharacterSheetForHuman(info, statPoints, skillPoints);
        else if (info.Origins.Race == CharactersLore.Races.Elf) return SetCharacterSheetForElf(info, statPoints, skillPoints);
        else if (info.Origins.Race == CharactersLore.Races.Dwarf) return SetCharacterSheetForDwarf(info, statPoints, skillPoints);
        throw new NotImplementedException();
    }

    #region private methods
    private CharacterSheet SetCharacterSheetForHuman(CharacterInfo info, int statPoints, int skillPoints)
    {
        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = RulebookLore.Calculations.Races.Human.Str * info.EntityLevel,
                Constitution= RulebookLore.Calculations.Races.Human.Con * info.EntityLevel,
                Agility     = RulebookLore.Calculations.Races.Human.Agi * info.EntityLevel,
                Willpower   = RulebookLore.Calculations.Races.Human.Wil * info.EntityLevel,
                Perception  = RulebookLore.Calculations.Races.Human.Per * info.EntityLevel,
                Abstract    = RulebookLore.Calculations.Races.Human.Abs * info.EntityLevel
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Origins.Culture == CharactersLore.Cultures.Human.Danarian) SetSheetForDanarian(charsheet);

        DistributeClassAttributes(charsheet, info.Origins.Class!, statPoints, skillPoints);

        return charsheet;
    }

    private CharacterSheet SetCharacterSheetForElf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = RulebookLore.Calculations.Races.Elf.Str * info.EntityLevel,
                Constitution= RulebookLore.Calculations.Races.Elf.Con * info.EntityLevel,
                Agility     = RulebookLore.Calculations.Races.Elf.Agi * info.EntityLevel,
                Willpower   = RulebookLore.Calculations.Races.Elf.Wil * info.EntityLevel,
                Perception  = RulebookLore.Calculations.Races.Elf.Per * info.EntityLevel,
                Abstract    = RulebookLore.Calculations.Races.Elf.Abs * info.EntityLevel
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Origins.Culture == CharactersLore.Cultures.Elf.Highborn) SetSheetForHighborn(charsheet);
        else throw new Exception("Character culture not found.");

        DistributeClassAttributes(charsheet, info.Origins.Class!, statPoints, skillPoints);

        return charsheet;
    }

    private CharacterSheet SetCharacterSheetForDwarf(CharacterInfo info, int statPoints, int skillPoints)
    {
        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = RulebookLore.Calculations.Races.Dwarf.Str * info.EntityLevel,
                Constitution= RulebookLore.Calculations.Races.Dwarf.Con * info.EntityLevel,
                Agility     = RulebookLore.Calculations.Races.Dwarf.Agi * info.EntityLevel,
                Willpower   = RulebookLore.Calculations.Races.Dwarf.Wil * info.EntityLevel,
                Perception  = RulebookLore.Calculations.Races.Dwarf.Per * info.EntityLevel,
                Abstract    = RulebookLore.Calculations.Races.Dwarf.Abs * info.EntityLevel
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Origins.Culture == CharactersLore.Cultures.Dwarf.Undermountain) SetSheetForUndermoutain(charsheet);
        else throw new Exception("Character culture not found.");

        DistributeClassAttributes(charsheet, info.Origins.Class!, statPoints, skillPoints);

        return charsheet;
    }

    private static void SetSheetForDanarian(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += RulebookLore.Calculations.Cultures.Humans.Danarian.Str;
        charsheet.Stats.Constitution+= RulebookLore.Calculations.Cultures.Humans.Danarian.Con;
        charsheet.Stats.Agility     += RulebookLore.Calculations.Cultures.Humans.Danarian.Agi;
        charsheet.Stats.Willpower   += RulebookLore.Calculations.Cultures.Humans.Danarian.Wil;
        charsheet.Stats.Perception  += RulebookLore.Calculations.Cultures.Humans.Danarian.Per;
        charsheet.Stats.Abstract    += RulebookLore.Calculations.Cultures.Humans.Danarian.Abs;
        //assets
        charsheet.Assets.Resolve    += RulebookLore.Calculations.Cultures.Humans.Danarian.Res;
        charsheet.Assets.Harm       += RulebookLore.Calculations.Cultures.Humans.Danarian.Har;
        charsheet.Assets.Spot       += RulebookLore.Calculations.Cultures.Humans.Danarian.Spo;
        charsheet.Assets.Defense    += RulebookLore.Calculations.Cultures.Humans.Danarian.Def;
        charsheet.Assets.Purge      += RulebookLore.Calculations.Cultures.Humans.Danarian.Pur;
        charsheet.Assets.Mana       += RulebookLore.Calculations.Cultures.Humans.Danarian.Man;
        //skills
        charsheet.Skills.Combat     += RulebookLore.Calculations.Cultures.Humans.Danarian.Com;
        charsheet.Skills.Arcane     += RulebookLore.Calculations.Cultures.Humans.Danarian.Arc;
        charsheet.Skills.Psionics   += RulebookLore.Calculations.Cultures.Humans.Danarian.Psi;
        charsheet.Skills.Hide       += RulebookLore.Calculations.Cultures.Humans.Danarian.Hid;
        charsheet.Skills.Traps      += RulebookLore.Calculations.Cultures.Humans.Danarian.Tra;
        charsheet.Skills.Tactics    += RulebookLore.Calculations.Cultures.Humans.Danarian.Tac;
        charsheet.Skills.Social     += RulebookLore.Calculations.Cultures.Humans.Danarian.Soc;
        charsheet.Skills.Apothecary += RulebookLore.Calculations.Cultures.Humans.Danarian.Apo;
        charsheet.Skills.Travel     += RulebookLore.Calculations.Cultures.Humans.Danarian.Trv;
        charsheet.Skills.Sail       += RulebookLore.Calculations.Cultures.Humans.Danarian.Sai;

        // danarias should start with a weapon, armour and helm
    }

    private static void SetSheetForHighborn(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += RulebookLore.Calculations.Cultures.Elves.Highborn.Str;
        charsheet.Stats.Constitution+= RulebookLore.Calculations.Cultures.Elves.Highborn.Con;
        charsheet.Stats.Agility     += RulebookLore.Calculations.Cultures.Elves.Highborn.Agi;
        charsheet.Stats.Willpower   += RulebookLore.Calculations.Cultures.Elves.Highborn.Wil;
        charsheet.Stats.Perception  += RulebookLore.Calculations.Cultures.Elves.Highborn.Per;
        charsheet.Stats.Abstract    += RulebookLore.Calculations.Cultures.Elves.Highborn.Abs;
        //assets
        charsheet.Assets.Resolve    += RulebookLore.Calculations.Cultures.Elves.Highborn.Res;
        charsheet.Assets.Harm       += RulebookLore.Calculations.Cultures.Elves.Highborn.Har;
        charsheet.Assets.Spot       += RulebookLore.Calculations.Cultures.Elves.Highborn.Spo;
        charsheet.Assets.Defense    += RulebookLore.Calculations.Cultures.Elves.Highborn.Def;
        charsheet.Assets.Purge      += RulebookLore.Calculations.Cultures.Elves.Highborn.Pur;
        charsheet.Assets.Mana       += RulebookLore.Calculations.Cultures.Elves.Highborn.Man;
        //skills
        charsheet.Skills.Combat     += RulebookLore.Calculations.Cultures.Elves.Highborn.Com;
        charsheet.Skills.Arcane     += RulebookLore.Calculations.Cultures.Elves.Highborn.Arc;
        charsheet.Skills.Psionics   += RulebookLore.Calculations.Cultures.Elves.Highborn.Psi;
        charsheet.Skills.Hide       += RulebookLore.Calculations.Cultures.Elves.Highborn.Hid;
        charsheet.Skills.Traps      += RulebookLore.Calculations.Cultures.Elves.Highborn.Tra;
        charsheet.Skills.Tactics    += RulebookLore.Calculations.Cultures.Elves.Highborn.Tac;
        charsheet.Skills.Social     += RulebookLore.Calculations.Cultures.Elves.Highborn.Soc;
        charsheet.Skills.Apothecary += RulebookLore.Calculations.Cultures.Elves.Highborn.Apo;
        charsheet.Skills.Travel     += RulebookLore.Calculations.Cultures.Elves.Highborn.Trv;
        charsheet.Skills.Sail       += RulebookLore.Calculations.Cultures.Elves.Highborn.Sai;

        // highborn should have a HT with no fear rolls
    }

    private static void SetSheetForUndermoutain(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Str;
        charsheet.Stats.Constitution+= RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Con;
        charsheet.Stats.Agility     += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Agi;
        charsheet.Stats.Willpower   += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Wil;
        charsheet.Stats.Perception  += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Per;
        charsheet.Stats.Abstract    += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Abs;
        //assets                                             
        charsheet.Assets.Resolve    += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Res;
        charsheet.Assets.Harm       += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Har;
        charsheet.Assets.Spot       += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Spo;
        charsheet.Assets.Defense    += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Def;
        charsheet.Assets.Purge      += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Pur;
        charsheet.Assets.Mana       += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Man;
        //skills                                             
        charsheet.Skills.Combat     += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Com;
        charsheet.Skills.Arcane     += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Arc;
        charsheet.Skills.Psionics   += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Psi;
        charsheet.Skills.Hide       += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Hid;
        charsheet.Skills.Traps      += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Tra;
        charsheet.Skills.Tactics    += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Tac;
        charsheet.Skills.Social     += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Soc;
        charsheet.Skills.Apothecary += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Apo;
        charsheet.Skills.Travel     += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Trv;
        charsheet.Skills.Sail       += RulebookLore.Calculations.Cultures.Dwarves.Undermountain.Sai;

        // should start with heavy armour
    }

    private void DistributeClassAttributes(CharacterSheet sheet, string classes, int statPoints, int skillPoints)
    {
        if (classes == CharactersLore.Classes.Warrior)
        {
            IncreaseStats(sheet, statPoints, RulebookLore.Calculations.Classes.Warrior.LikelyStats, RulebookLore.Calculations.Classes.Warrior.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, RulebookLore.Calculations.Classes.Warrior.LikelySkills, RulebookLore.Calculations.Classes.Warrior.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Mage)
        {
            IncreaseStats(sheet, statPoints, RulebookLore.Calculations.Classes.Mage.LikelyStats, RulebookLore.Calculations.Classes.Mage.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, RulebookLore.Calculations.Classes.Mage.LikelySkills, RulebookLore.Calculations.Classes.Mage.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Hunter)
        {
            IncreaseStats(sheet, statPoints, RulebookLore.Calculations.Classes.Hunter.LikelyStats, RulebookLore.Calculations.Classes.Hunter.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, RulebookLore.Calculations.Classes.Hunter.LikelySkills, RulebookLore.Calculations.Classes.Hunter.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Swashbuckler)
        {
            IncreaseStats(sheet, statPoints, RulebookLore.Calculations.Classes.Swashbuckler.LikelyStats, RulebookLore.Calculations.Classes.Swashbuckler.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, RulebookLore.Calculations.Classes.Swashbuckler.LikelySkills, RulebookLore.Calculations.Classes.Swashbuckler.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Sorcerer)
        {
            IncreaseStats(sheet, statPoints, RulebookLore.Calculations.Classes.Sorcerer.LikelyStats, RulebookLore.Calculations.Classes.Sorcerer.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, RulebookLore.Calculations.Classes.Sorcerer.LikelySkills, RulebookLore.Calculations.Classes.Sorcerer.UnlikelySkills);
        }
    }

    private void IncreaseStats(CharacterSheet sheet, int statPoints, List<string> likelyStats, List<string> unlikelyStats)
    {
        while (statPoints != 0)
        {
            var roll = dice.Roll_d100();
            string chosenStat;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_1dX(likelyStats.Count);
                chosenStat = likelyStats[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_1dX(unlikelyStats.Count);
                chosenStat = unlikelyStats[rollForStat - 1];
            }

            if      (chosenStat == RulebookLore.Calculations.Acronyms.Stats.Str) sheet.Stats.Strength++;
            else if (chosenStat == RulebookLore.Calculations.Acronyms.Stats.Con) sheet.Stats.Constitution++;
            else if (chosenStat == RulebookLore.Calculations.Acronyms.Stats.Agi) sheet.Stats.Agility++;
            else if (chosenStat == RulebookLore.Calculations.Acronyms.Stats.Wil) sheet.Stats.Willpower++;
            else if (chosenStat == RulebookLore.Calculations.Acronyms.Stats.Per) sheet.Stats.Perception++;
            else if (chosenStat == RulebookLore.Calculations.Acronyms.Stats.Abs) sheet.Stats.Abstract++;

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
                var rollForStat = dice.Roll_1dX(likeySkills.Count);
                chosenSkill = likeySkills[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_1dX(unlikeySkills.Count);
                chosenSkill = unlikeySkills[rollForStat - 1];
            }

            if      (chosenSkill == RulebookLore.Calculations.Acronyms.Skills.Com) sheet.Skills.Combat++;
            else if (chosenSkill == RulebookLore.Calculations.Acronyms.Skills.Arc) sheet.Skills.Arcane++;
            else if (chosenSkill == RulebookLore.Calculations.Acronyms.Skills.Psi) sheet.Skills.Psionics++;
            else if (chosenSkill == RulebookLore.Calculations.Acronyms.Skills.Hid) sheet.Skills.Hide++;
            else if (chosenSkill == RulebookLore.Calculations.Acronyms.Skills.Tra) sheet.Skills.Traps++;
            else if (chosenSkill == RulebookLore.Calculations.Acronyms.Skills.Tac) sheet.Skills.Tactics++;
            else if (chosenSkill == RulebookLore.Calculations.Acronyms.Skills.Soc) sheet.Skills.Social++;
            else if (chosenSkill == RulebookLore.Calculations.Acronyms.Skills.Apo) sheet.Skills.Apothecary++;
            else if (chosenSkill == RulebookLore.Calculations.Acronyms.Skills.Tra) sheet.Skills.Travel++;
            else if (chosenSkill == RulebookLore.Calculations.Acronyms.Skills.Sai) sheet.Skills.Sail++;

            skillPoints--;
        }
    }
    #endregion
}
