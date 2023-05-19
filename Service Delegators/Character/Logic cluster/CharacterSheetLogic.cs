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
        if      (info.Race == CharactersLore.Races.Human) return SetCharacterSheetForHuman(info, statPoints, skillPoints);
        else if (info.Race == CharactersLore.Races.Elf) return SetCharacterSheetForElf(info, statPoints, skillPoints);
        else if (info.Race == CharactersLore.Races.Dwarf) return SetCharacterSheetForDwarf(info, statPoints, skillPoints);
        throw new NotImplementedException();
    }

    #region private methods
    private CharacterSheet SetCharacterSheetForHuman(CharacterInfo info, int statPoints, int skillPoints)
    {
        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = RulebookLore.Races.Human.Str * info.EntityLevel,
                Constitution= RulebookLore.Races.Human.Con * info.EntityLevel,
                Agility     = RulebookLore.Races.Human.Agi * info.EntityLevel,
                Willpower   = RulebookLore.Races.Human.Wil * info.EntityLevel,
                Perception  = RulebookLore.Races.Human.Per * info.EntityLevel,
                Abstract    = RulebookLore.Races.Human.Abs * info.EntityLevel
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
        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = RulebookLore.Races.Elf.Str * info.EntityLevel,
                Constitution= RulebookLore.Races.Elf.Con * info.EntityLevel,
                Agility     = RulebookLore.Races.Elf.Agi * info.EntityLevel,
                Willpower   = RulebookLore.Races.Elf.Wil * info.EntityLevel,
                Perception  = RulebookLore.Races.Elf.Per * info.EntityLevel,
                Abstract    = RulebookLore.Races.Elf.Abs * info.EntityLevel
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
        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength    = RulebookLore.Races.Dwarf.Str * info.EntityLevel,
                Constitution= RulebookLore.Races.Dwarf.Con * info.EntityLevel,
                Agility     = RulebookLore.Races.Dwarf.Agi * info.EntityLevel,
                Willpower   = RulebookLore.Races.Dwarf.Wil * info.EntityLevel,
                Perception  = RulebookLore.Races.Dwarf.Per * info.EntityLevel,
                Abstract    = RulebookLore.Races.Dwarf.Abs * info.EntityLevel
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (info.Culture == CharactersLore.Cultures.Dwarf.Undermountain) SetSheetForUndermoutain(charsheet);
        else throw new Exception("Character culture not found.");

        DistributeClassAttributes(charsheet, info.Class!, statPoints, skillPoints);

        return charsheet;
    }

    private static void SetSheetForDanarian(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += RulebookLore.Cultures.Humans.Danarian.Str;
        charsheet.Stats.Constitution+= RulebookLore.Cultures.Humans.Danarian.Con;
        charsheet.Stats.Agility     += RulebookLore.Cultures.Humans.Danarian.Agi;
        charsheet.Stats.Willpower   += RulebookLore.Cultures.Humans.Danarian.Wil;
        charsheet.Stats.Perception  += RulebookLore.Cultures.Humans.Danarian.Per;
        charsheet.Stats.Abstract    += RulebookLore.Cultures.Humans.Danarian.Abs;
        //assets
        charsheet.Assets.Resolve    += RulebookLore.Cultures.Humans.Danarian.Res;
        charsheet.Assets.Harm       += RulebookLore.Cultures.Humans.Danarian.Har;
        charsheet.Assets.Spot       += RulebookLore.Cultures.Humans.Danarian.Spo;
        charsheet.Assets.Defense    += RulebookLore.Cultures.Humans.Danarian.Def;
        charsheet.Assets.Purge      += RulebookLore.Cultures.Humans.Danarian.Pur;
        charsheet.Assets.Mana       += RulebookLore.Cultures.Humans.Danarian.Man;
        //skills
        charsheet.Skills.Combat     += RulebookLore.Cultures.Humans.Danarian.Com;
        charsheet.Skills.Arcane     += RulebookLore.Cultures.Humans.Danarian.Arc;
        charsheet.Skills.Psionics   += RulebookLore.Cultures.Humans.Danarian.Psi;
        charsheet.Skills.Hide       += RulebookLore.Cultures.Humans.Danarian.Hid;
        charsheet.Skills.Traps      += RulebookLore.Cultures.Humans.Danarian.Tra;
        charsheet.Skills.Tactics    += RulebookLore.Cultures.Humans.Danarian.Tac;
        charsheet.Skills.Social     += RulebookLore.Cultures.Humans.Danarian.Soc;
        charsheet.Skills.Apothecary += RulebookLore.Cultures.Humans.Danarian.Apo;
        charsheet.Skills.Travel     += RulebookLore.Cultures.Humans.Danarian.Trv;
        charsheet.Skills.Sail       += RulebookLore.Cultures.Humans.Danarian.Sai;

        // danarias should start with a weapon, armour and helm
    }

    private static void SetSheetForHighborn(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += RulebookLore.Cultures.Elves.Highborn.Str;
        charsheet.Stats.Constitution+= RulebookLore.Cultures.Elves.Highborn.Con;
        charsheet.Stats.Agility     += RulebookLore.Cultures.Elves.Highborn.Agi;
        charsheet.Stats.Willpower   += RulebookLore.Cultures.Elves.Highborn.Wil;
        charsheet.Stats.Perception  += RulebookLore.Cultures.Elves.Highborn.Per;
        charsheet.Stats.Abstract    += RulebookLore.Cultures.Elves.Highborn.Abs;
        //assets
        charsheet.Assets.Resolve    += RulebookLore.Cultures.Elves.Highborn.Res;
        charsheet.Assets.Harm       += RulebookLore.Cultures.Elves.Highborn.Har;
        charsheet.Assets.Spot       += RulebookLore.Cultures.Elves.Highborn.Spo;
        charsheet.Assets.Defense    += RulebookLore.Cultures.Elves.Highborn.Def;
        charsheet.Assets.Purge      += RulebookLore.Cultures.Elves.Highborn.Pur;
        charsheet.Assets.Mana       += RulebookLore.Cultures.Elves.Highborn.Man;
        //skills
        charsheet.Skills.Combat     += RulebookLore.Cultures.Elves.Highborn.Com;
        charsheet.Skills.Arcane     += RulebookLore.Cultures.Elves.Highborn.Arc;
        charsheet.Skills.Psionics   += RulebookLore.Cultures.Elves.Highborn.Psi;
        charsheet.Skills.Hide       += RulebookLore.Cultures.Elves.Highborn.Hid;
        charsheet.Skills.Traps      += RulebookLore.Cultures.Elves.Highborn.Tra;
        charsheet.Skills.Tactics    += RulebookLore.Cultures.Elves.Highborn.Tac;
        charsheet.Skills.Social     += RulebookLore.Cultures.Elves.Highborn.Soc;
        charsheet.Skills.Apothecary += RulebookLore.Cultures.Elves.Highborn.Apo;
        charsheet.Skills.Travel     += RulebookLore.Cultures.Elves.Highborn.Trv;
        charsheet.Skills.Sail       += RulebookLore.Cultures.Elves.Highborn.Sai;

        // highborn should have a HT with no fear rolls
    }

    private static void SetSheetForUndermoutain(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += RulebookLore.Cultures.Dwarves.Undermountain.Str;
        charsheet.Stats.Constitution+= RulebookLore.Cultures.Dwarves.Undermountain.Con;
        charsheet.Stats.Agility     += RulebookLore.Cultures.Dwarves.Undermountain.Agi;
        charsheet.Stats.Willpower   += RulebookLore.Cultures.Dwarves.Undermountain.Wil;
        charsheet.Stats.Perception  += RulebookLore.Cultures.Dwarves.Undermountain.Per;
        charsheet.Stats.Abstract    += RulebookLore.Cultures.Dwarves.Undermountain.Abs;
        //assets                                             
        charsheet.Assets.Resolve    += RulebookLore.Cultures.Dwarves.Undermountain.Res;
        charsheet.Assets.Harm       += RulebookLore.Cultures.Dwarves.Undermountain.Har;
        charsheet.Assets.Spot       += RulebookLore.Cultures.Dwarves.Undermountain.Spo;
        charsheet.Assets.Defense    += RulebookLore.Cultures.Dwarves.Undermountain.Def;
        charsheet.Assets.Purge      += RulebookLore.Cultures.Dwarves.Undermountain.Pur;
        charsheet.Assets.Mana       += RulebookLore.Cultures.Dwarves.Undermountain.Man;
        //skills                                             
        charsheet.Skills.Combat     += RulebookLore.Cultures.Dwarves.Undermountain.Com;
        charsheet.Skills.Arcane     += RulebookLore.Cultures.Dwarves.Undermountain.Arc;
        charsheet.Skills.Psionics   += RulebookLore.Cultures.Dwarves.Undermountain.Psi;
        charsheet.Skills.Hide       += RulebookLore.Cultures.Dwarves.Undermountain.Hid;
        charsheet.Skills.Traps      += RulebookLore.Cultures.Dwarves.Undermountain.Tra;
        charsheet.Skills.Tactics    += RulebookLore.Cultures.Dwarves.Undermountain.Tac;
        charsheet.Skills.Social     += RulebookLore.Cultures.Dwarves.Undermountain.Soc;
        charsheet.Skills.Apothecary += RulebookLore.Cultures.Dwarves.Undermountain.Apo;
        charsheet.Skills.Travel     += RulebookLore.Cultures.Dwarves.Undermountain.Trv;
        charsheet.Skills.Sail       += RulebookLore.Cultures.Dwarves.Undermountain.Sai;

        // should start with heavy armour
    }

    private void DistributeClassAttributes(CharacterSheet sheet, string classes, int statPoints, int skillPoints)
    {
        if (classes == CharactersLore.Classes.Warrior)
        {
            IncreaseStats(sheet, statPoints, RulebookLore.Classes.Warrior.LikelyStats, RulebookLore.Classes.Warrior.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, RulebookLore.Classes.Warrior.LikelySkills, RulebookLore.Classes.Warrior.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Mage)
        {
            IncreaseStats(sheet, statPoints, RulebookLore.Classes.Mage.LikelyStats, RulebookLore.Classes.Mage.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, RulebookLore.Classes.Mage.LikelySkills, RulebookLore.Classes.Mage.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Hunter)
        {
            IncreaseStats(sheet, statPoints, RulebookLore.Classes.Hunter.LikelyStats, RulebookLore.Classes.Hunter.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, RulebookLore.Classes.Hunter.LikelySkills, RulebookLore.Classes.Hunter.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Swashbuckler)
        {
            IncreaseStats(sheet, statPoints, RulebookLore.Classes.Swashbuckler.LikelyStats, RulebookLore.Classes.Swashbuckler.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, RulebookLore.Classes.Swashbuckler.LikelySkills, RulebookLore.Classes.Swashbuckler.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Sorcerer)
        {
            IncreaseStats(sheet, statPoints, RulebookLore.Classes.Sorcerer.LikelyStats, RulebookLore.Classes.Sorcerer.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, RulebookLore.Classes.Sorcerer.LikelySkills, RulebookLore.Classes.Sorcerer.UnlikelySkills);
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
                var rollForStat = dice.Roll_dX(likelyStats.Count);
                chosenStat = likelyStats[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_dX(unlikelyStats.Count);
                chosenStat = unlikelyStats[rollForStat - 1];
            }

            if      (chosenStat == RulebookLore.Acronyms.Stats.Str) sheet.Stats.Strength++;
            else if (chosenStat == RulebookLore.Acronyms.Stats.Con) sheet.Stats.Constitution++;
            else if (chosenStat == RulebookLore.Acronyms.Stats.Agi) sheet.Stats.Agility++;
            else if (chosenStat == RulebookLore.Acronyms.Stats.Wil) sheet.Stats.Willpower++;
            else if (chosenStat == RulebookLore.Acronyms.Stats.Per) sheet.Stats.Perception++;
            else if (chosenStat == RulebookLore.Acronyms.Stats.Abs) sheet.Stats.Abstract++;

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

            if      (chosenSkill == RulebookLore.Acronyms.Skills.Com) sheet.Skills.Combat++;
            else if (chosenSkill == RulebookLore.Acronyms.Skills.Arc) sheet.Skills.Arcane++;
            else if (chosenSkill == RulebookLore.Acronyms.Skills.Psi) sheet.Skills.Psionics++;
            else if (chosenSkill == RulebookLore.Acronyms.Skills.Hid) sheet.Skills.Hide++;
            else if (chosenSkill == RulebookLore.Acronyms.Skills.Tra) sheet.Skills.Traps++;
            else if (chosenSkill == RulebookLore.Acronyms.Skills.Tac) sheet.Skills.Tactics++;
            else if (chosenSkill == RulebookLore.Acronyms.Skills.Soc) sheet.Skills.Social++;
            else if (chosenSkill == RulebookLore.Acronyms.Skills.Apo) sheet.Skills.Apothecary++;
            else if (chosenSkill == RulebookLore.Acronyms.Skills.Tra) sheet.Skills.Travel++;
            else if (chosenSkill == RulebookLore.Acronyms.Skills.Sai) sheet.Skills.Sail++;

            skillPoints--;
        }
    }
    #endregion
}
