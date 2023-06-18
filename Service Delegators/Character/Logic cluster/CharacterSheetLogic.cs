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
                Strength    = GameplayLore.Calculations.Races.Human.Str * info.EntityLevel,
                Constitution= GameplayLore.Calculations.Races.Human.Con * info.EntityLevel,
                Agility     = GameplayLore.Calculations.Races.Human.Agi * info.EntityLevel,
                Willpower   = GameplayLore.Calculations.Races.Human.Wil * info.EntityLevel,
                Perception  = GameplayLore.Calculations.Races.Human.Per * info.EntityLevel,
                Abstract    = GameplayLore.Calculations.Races.Human.Abs * info.EntityLevel
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
                Strength    = GameplayLore.Calculations.Races.Elf.Str * info.EntityLevel,
                Constitution= GameplayLore.Calculations.Races.Elf.Con * info.EntityLevel,
                Agility     = GameplayLore.Calculations.Races.Elf.Agi * info.EntityLevel,
                Willpower   = GameplayLore.Calculations.Races.Elf.Wil * info.EntityLevel,
                Perception  = GameplayLore.Calculations.Races.Elf.Per * info.EntityLevel,
                Abstract    = GameplayLore.Calculations.Races.Elf.Abs * info.EntityLevel
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
                Strength    = GameplayLore.Calculations.Races.Dwarf.Str * info.EntityLevel,
                Constitution= GameplayLore.Calculations.Races.Dwarf.Con * info.EntityLevel,
                Agility     = GameplayLore.Calculations.Races.Dwarf.Agi * info.EntityLevel,
                Willpower   = GameplayLore.Calculations.Races.Dwarf.Wil * info.EntityLevel,
                Perception  = GameplayLore.Calculations.Races.Dwarf.Per * info.EntityLevel,
                Abstract    = GameplayLore.Calculations.Races.Dwarf.Abs * info.EntityLevel
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
        charsheet.Stats.Strength    += GameplayLore.Calculations.Cultures.Humans.Danarian.Str;
        charsheet.Stats.Constitution+= GameplayLore.Calculations.Cultures.Humans.Danarian.Con;
        charsheet.Stats.Agility     += GameplayLore.Calculations.Cultures.Humans.Danarian.Agi;
        charsheet.Stats.Willpower   += GameplayLore.Calculations.Cultures.Humans.Danarian.Wil;
        charsheet.Stats.Perception  += GameplayLore.Calculations.Cultures.Humans.Danarian.Per;
        charsheet.Stats.Abstract    += GameplayLore.Calculations.Cultures.Humans.Danarian.Abs;
        //assets
        charsheet.Assets.Resolve    += GameplayLore.Calculations.Cultures.Humans.Danarian.Res;
        charsheet.Assets.Harm       += GameplayLore.Calculations.Cultures.Humans.Danarian.Har;
        charsheet.Assets.Spot       += GameplayLore.Calculations.Cultures.Humans.Danarian.Spo;
        charsheet.Assets.Defense    += GameplayLore.Calculations.Cultures.Humans.Danarian.Def;
        charsheet.Assets.Purge      += GameplayLore.Calculations.Cultures.Humans.Danarian.Pur;
        charsheet.Assets.Mana       += GameplayLore.Calculations.Cultures.Humans.Danarian.Man;
        //skills
        charsheet.Skills.Combat     += GameplayLore.Calculations.Cultures.Humans.Danarian.Com;
        charsheet.Skills.Arcane     += GameplayLore.Calculations.Cultures.Humans.Danarian.Arc;
        charsheet.Skills.Psionics   += GameplayLore.Calculations.Cultures.Humans.Danarian.Psi;
        charsheet.Skills.Hide       += GameplayLore.Calculations.Cultures.Humans.Danarian.Hid;
        charsheet.Skills.Traps      += GameplayLore.Calculations.Cultures.Humans.Danarian.Tra;
        charsheet.Skills.Tactics    += GameplayLore.Calculations.Cultures.Humans.Danarian.Tac;
        charsheet.Skills.Social     += GameplayLore.Calculations.Cultures.Humans.Danarian.Soc;
        charsheet.Skills.Apothecary += GameplayLore.Calculations.Cultures.Humans.Danarian.Apo;
        charsheet.Skills.Travel     += GameplayLore.Calculations.Cultures.Humans.Danarian.Trv;
        charsheet.Skills.Sail       += GameplayLore.Calculations.Cultures.Humans.Danarian.Sai;

        // danarias should start with a weapon, armour and helm
    }

    private static void SetSheetForHighborn(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += GameplayLore.Calculations.Cultures.Elves.Highborn.Str;
        charsheet.Stats.Constitution+= GameplayLore.Calculations.Cultures.Elves.Highborn.Con;
        charsheet.Stats.Agility     += GameplayLore.Calculations.Cultures.Elves.Highborn.Agi;
        charsheet.Stats.Willpower   += GameplayLore.Calculations.Cultures.Elves.Highborn.Wil;
        charsheet.Stats.Perception  += GameplayLore.Calculations.Cultures.Elves.Highborn.Per;
        charsheet.Stats.Abstract    += GameplayLore.Calculations.Cultures.Elves.Highborn.Abs;
        //assets
        charsheet.Assets.Resolve    += GameplayLore.Calculations.Cultures.Elves.Highborn.Res;
        charsheet.Assets.Harm       += GameplayLore.Calculations.Cultures.Elves.Highborn.Har;
        charsheet.Assets.Spot       += GameplayLore.Calculations.Cultures.Elves.Highborn.Spo;
        charsheet.Assets.Defense    += GameplayLore.Calculations.Cultures.Elves.Highborn.Def;
        charsheet.Assets.Purge      += GameplayLore.Calculations.Cultures.Elves.Highborn.Pur;
        charsheet.Assets.Mana       += GameplayLore.Calculations.Cultures.Elves.Highborn.Man;
        //skills
        charsheet.Skills.Combat     += GameplayLore.Calculations.Cultures.Elves.Highborn.Com;
        charsheet.Skills.Arcane     += GameplayLore.Calculations.Cultures.Elves.Highborn.Arc;
        charsheet.Skills.Psionics   += GameplayLore.Calculations.Cultures.Elves.Highborn.Psi;
        charsheet.Skills.Hide       += GameplayLore.Calculations.Cultures.Elves.Highborn.Hid;
        charsheet.Skills.Traps      += GameplayLore.Calculations.Cultures.Elves.Highborn.Tra;
        charsheet.Skills.Tactics    += GameplayLore.Calculations.Cultures.Elves.Highborn.Tac;
        charsheet.Skills.Social     += GameplayLore.Calculations.Cultures.Elves.Highborn.Soc;
        charsheet.Skills.Apothecary += GameplayLore.Calculations.Cultures.Elves.Highborn.Apo;
        charsheet.Skills.Travel     += GameplayLore.Calculations.Cultures.Elves.Highborn.Trv;
        charsheet.Skills.Sail       += GameplayLore.Calculations.Cultures.Elves.Highborn.Sai;

        // highborn should have a HT with no fear rolls
    }

    private static void SetSheetForUndermoutain(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Str;
        charsheet.Stats.Constitution+= GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Con;
        charsheet.Stats.Agility     += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Agi;
        charsheet.Stats.Willpower   += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Wil;
        charsheet.Stats.Perception  += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Per;
        charsheet.Stats.Abstract    += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Abs;
        //assets                                             
        charsheet.Assets.Resolve    += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Res;
        charsheet.Assets.Harm       += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Har;
        charsheet.Assets.Spot       += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Spo;
        charsheet.Assets.Defense    += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Def;
        charsheet.Assets.Purge      += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Pur;
        charsheet.Assets.Mana       += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Man;
        //skills                                             
        charsheet.Skills.Combat     += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Com;
        charsheet.Skills.Arcane     += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Arc;
        charsheet.Skills.Psionics   += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Psi;
        charsheet.Skills.Hide       += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Hid;
        charsheet.Skills.Traps      += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Tra;
        charsheet.Skills.Tactics    += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Tac;
        charsheet.Skills.Social     += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Soc;
        charsheet.Skills.Apothecary += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Apo;
        charsheet.Skills.Travel     += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Trv;
        charsheet.Skills.Sail       += GameplayLore.Calculations.Cultures.Dwarves.Undermountain.Sai;

        // should start with heavy armour
    }

    private void DistributeClassAttributes(CharacterSheet sheet, string classes, int statPoints, int skillPoints)
    {
        if (classes == CharactersLore.Classes.Warrior)
        {
            IncreaseStats(sheet, statPoints, GameplayLore.Calculations.Classes.Warrior.LikelyStats, GameplayLore.Calculations.Classes.Warrior.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, GameplayLore.Calculations.Classes.Warrior.LikelySkills, GameplayLore.Calculations.Classes.Warrior.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Mage)
        {
            IncreaseStats(sheet, statPoints, GameplayLore.Calculations.Classes.Mage.LikelyStats, GameplayLore.Calculations.Classes.Mage.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, GameplayLore.Calculations.Classes.Mage.LikelySkills, GameplayLore.Calculations.Classes.Mage.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Hunter)
        {
            IncreaseStats(sheet, statPoints, GameplayLore.Calculations.Classes.Hunter.LikelyStats, GameplayLore.Calculations.Classes.Hunter.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, GameplayLore.Calculations.Classes.Hunter.LikelySkills, GameplayLore.Calculations.Classes.Hunter.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Swashbuckler)
        {
            IncreaseStats(sheet, statPoints, GameplayLore.Calculations.Classes.Swashbuckler.LikelyStats, GameplayLore.Calculations.Classes.Swashbuckler.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, GameplayLore.Calculations.Classes.Swashbuckler.LikelySkills, GameplayLore.Calculations.Classes.Swashbuckler.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Sorcerer)
        {
            IncreaseStats(sheet, statPoints, GameplayLore.Calculations.Classes.Sorcerer.LikelyStats, GameplayLore.Calculations.Classes.Sorcerer.UnlikelyStats);
            IncreaseSkills(sheet, skillPoints, GameplayLore.Calculations.Classes.Sorcerer.LikelySkills, GameplayLore.Calculations.Classes.Sorcerer.UnlikelySkills);
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

            if      (chosenStat == GameplayLore.Calculations.Acronyms.Stats.Str) sheet.Stats.Strength++;
            else if (chosenStat == GameplayLore.Calculations.Acronyms.Stats.Con) sheet.Stats.Constitution++;
            else if (chosenStat == GameplayLore.Calculations.Acronyms.Stats.Agi) sheet.Stats.Agility++;
            else if (chosenStat == GameplayLore.Calculations.Acronyms.Stats.Wil) sheet.Stats.Willpower++;
            else if (chosenStat == GameplayLore.Calculations.Acronyms.Stats.Per) sheet.Stats.Perception++;
            else if (chosenStat == GameplayLore.Calculations.Acronyms.Stats.Abs) sheet.Stats.Abstract++;

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

            if      (chosenSkill == GameplayLore.Calculations.Acronyms.Skills.Com) sheet.Skills.Combat++;
            else if (chosenSkill == GameplayLore.Calculations.Acronyms.Skills.Arc) sheet.Skills.Arcane++;
            else if (chosenSkill == GameplayLore.Calculations.Acronyms.Skills.Psi) sheet.Skills.Psionics++;
            else if (chosenSkill == GameplayLore.Calculations.Acronyms.Skills.Hid) sheet.Skills.Hide++;
            else if (chosenSkill == GameplayLore.Calculations.Acronyms.Skills.Tra) sheet.Skills.Traps++;
            else if (chosenSkill == GameplayLore.Calculations.Acronyms.Skills.Tac) sheet.Skills.Tactics++;
            else if (chosenSkill == GameplayLore.Calculations.Acronyms.Skills.Soc) sheet.Skills.Social++;
            else if (chosenSkill == GameplayLore.Calculations.Acronyms.Skills.Apo) sheet.Skills.Apothecary++;
            else if (chosenSkill == GameplayLore.Calculations.Acronyms.Skills.Tra) sheet.Skills.Travel++;
            else if (chosenSkill == GameplayLore.Calculations.Acronyms.Skills.Sai) sheet.Skills.Sail++;

            skillPoints--;
        }
    }
    #endregion
}
