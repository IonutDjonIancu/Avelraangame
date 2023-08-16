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

    internal void SetCharacterSheet(int statPoints, int skillPoints, Character character)
    {
        if      (character.Status.Traits.Race == CharactersLore.Races.Playable.Human) SetCharacterSheetForHuman(statPoints, skillPoints, character);
        else if (character.Status.Traits.Race == CharactersLore.Races.Playable.Elf)   SetCharacterSheetForElf(statPoints, skillPoints, character);
        else if (character.Status.Traits.Race == CharactersLore.Races.Playable.Dwarf) SetCharacterSheetForDwarf(statPoints, skillPoints, character);
        else if (character.Status.Traits.Race == CharactersLore.Races.Playable.Orc)   SetCharacterSheetForOrc(statPoints, skillPoints, character);
        throw new NotImplementedException();
    }

    #region private methods
    private void SetCharacterSheetForOrc(int statPoints, int skillPoints, Character character)
    {
        character.Sheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength     = RulebookLore.Races.Playable.Orc.Strength * character.Status.EntityLevel,
                Constitution = RulebookLore.Races.Playable.Orc.Constitution * character.Status.EntityLevel,
                Agility      = RulebookLore.Races.Playable.Orc.Agility * character.Status.EntityLevel,
                Willpower    = RulebookLore.Races.Playable.Orc.Willpower * character.Status.EntityLevel,
                Perception   = RulebookLore.Races.Playable.Orc.Perception * character.Status.EntityLevel,
                Abstract     = RulebookLore.Races.Playable.Orc.Abstract * character.Status.EntityLevel
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (character.Status.Traits.Culture == CharactersLore.Cultures.Orc.Greenskin) ModifySheetForGreenskin(character.Sheet);

        DistributeClassAttributes(character.Sheet, character.Status.Traits.Class!, statPoints, skillPoints);
        CalculateAssets(character.Sheet);
        CalculateSkills(character.Sheet);
    }

    private void SetCharacterSheetForHuman(int statPoints, int skillPoints, Character character)
    {
        character.Sheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength     = RulebookLore.Races.Playable.Human.Strength * character.Status.EntityLevel,
                Constitution = RulebookLore.Races.Playable.Human.Constitution * character.Status.EntityLevel,
                Agility      = RulebookLore.Races.Playable.Human.Agility * character.Status.EntityLevel,
                Willpower    = RulebookLore.Races.Playable.Human.Willpower * character.Status.EntityLevel,
                Perception   = RulebookLore.Races.Playable.Human.Perception * character.Status.EntityLevel,
                Abstract     = RulebookLore.Races.Playable.Human.Abstract * character.Status.EntityLevel
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (character.Status.Traits.Culture == CharactersLore.Cultures.Human.Danarian) ModifySheetForDanarian(character.Sheet);

        DistributeClassAttributes(character.Sheet, character.Status.Traits.Class!, statPoints, skillPoints);
        CalculateAssets(character.Sheet);
        CalculateSkills(character.Sheet);
    }

    private void SetCharacterSheetForElf(int statPoints, int skillPoints, Character character)
    {
        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength     = RulebookLore.Races.Playable.Elf.Strength * character.Status.EntityLevel,
                Constitution = RulebookLore.Races.Playable.Elf.Constitution * character.Status.EntityLevel,
                Agility      = RulebookLore.Races.Playable.Elf.Agility * character.Status.EntityLevel,
                Willpower    = RulebookLore.Races.Playable.Elf.Willpower * character.Status.EntityLevel,
                Perception   = RulebookLore.Races.Playable.Elf.Perception * character.Status.EntityLevel,
                Abstract     = RulebookLore.Races.Playable.Elf.Abstract * character.Status.EntityLevel
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (character.Status.Traits.Culture == CharactersLore.Cultures.Elf.Highborn) ModifySheetForHighborn(charsheet);

        DistributeClassAttributes(character.Sheet, character.Status.Traits.Class!, statPoints, skillPoints);
        CalculateAssets(character.Sheet);
        CalculateSkills(character.Sheet);
    }

    private void SetCharacterSheetForDwarf(int statPoints, int skillPoints, Character character)
    {
        var charsheet = new CharacterSheet
        {
            Stats = new CharacterStats
            {
                Strength     = RulebookLore.Races.Playable.Dwarf.Strength * character.Status.EntityLevel,
                Constitution = RulebookLore.Races.Playable.Dwarf.Constitution * character.Status.EntityLevel,
                Agility      = RulebookLore.Races.Playable.Dwarf.Agility * character.Status.EntityLevel,
                Willpower    = RulebookLore.Races.Playable.Dwarf.Willpower * character.Status.EntityLevel,
                Perception   = RulebookLore.Races.Playable.Dwarf.Perception * character.Status.EntityLevel,
                Abstract     = RulebookLore.Races.Playable.Dwarf.Abstract * character.Status.EntityLevel
            },
            Assets = new CharacterAssets(),
            Skills = new CharacterSkills()
        };

        if (character.Status.Traits.Culture == CharactersLore.Cultures.Dwarf.Undermountain) ModifySheetForUndermountain(charsheet);

        DistributeClassAttributes(character.Sheet, character.Status.Traits.Class!, statPoints, skillPoints);
        CalculateAssets(character.Sheet);
        CalculateSkills(character.Sheet);
    }

    private static void ModifySheetForGreenskin(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += RulebookLore.Cultures.Orcs.Greenskin.Strength;
        charsheet.Stats.Constitution+= RulebookLore.Cultures.Orcs.Greenskin.Constitution;
        charsheet.Stats.Agility     += RulebookLore.Cultures.Orcs.Greenskin.Agility;
        charsheet.Stats.Willpower   += RulebookLore.Cultures.Orcs.Greenskin.Willpower;
        charsheet.Stats.Perception  += RulebookLore.Cultures.Orcs.Greenskin.Perception;
        charsheet.Stats.Abstract    += RulebookLore.Cultures.Orcs.Greenskin.Abstract;
        //assets
        charsheet.Assets.Resolve    += RulebookLore.Cultures.Orcs.Greenskin.Resolve;
        charsheet.Assets.Harm       += RulebookLore.Cultures.Orcs.Greenskin.Harm;
        charsheet.Assets.Spot       += RulebookLore.Cultures.Orcs.Greenskin.Spot;
        charsheet.Assets.Defense    += RulebookLore.Cultures.Orcs.Greenskin.Defense;
        charsheet.Assets.Purge      += RulebookLore.Cultures.Orcs.Greenskin.Purge;
        charsheet.Assets.Mana       += RulebookLore.Cultures.Orcs.Greenskin.Mana;
        charsheet.Assets.Actions    += RulebookLore.Cultures.Orcs.Greenskin.Actions;
        //skills
        charsheet.Skills.Combat     += RulebookLore.Cultures.Orcs.Greenskin.Combat;
        charsheet.Skills.Arcane     += RulebookLore.Cultures.Orcs.Greenskin.Arcane;
        charsheet.Skills.Psionics   += RulebookLore.Cultures.Orcs.Greenskin.Psionics;
        charsheet.Skills.Hide       += RulebookLore.Cultures.Orcs.Greenskin.Hide;
        charsheet.Skills.Traps      += RulebookLore.Cultures.Orcs.Greenskin.Traps;
        charsheet.Skills.Tactics    += RulebookLore.Cultures.Orcs.Greenskin.Tactics;
        charsheet.Skills.Social     += RulebookLore.Cultures.Orcs.Greenskin.Social;
        charsheet.Skills.Apothecary += RulebookLore.Cultures.Orcs.Greenskin.Apothecary;
        charsheet.Skills.Travel     += RulebookLore.Cultures.Orcs.Greenskin.Travel;
        charsheet.Skills.Sail       += RulebookLore.Cultures.Orcs.Greenskin.Sail;

        // TODO: apply culture bonuses
    }

    private static void ModifySheetForDanarian(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += RulebookLore.Cultures.Humans.Danarian.Strength;
        charsheet.Stats.Constitution+= RulebookLore.Cultures.Humans.Danarian.Constitution;
        charsheet.Stats.Agility     += RulebookLore.Cultures.Humans.Danarian.Agility;
        charsheet.Stats.Willpower   += RulebookLore.Cultures.Humans.Danarian.Willpower;
        charsheet.Stats.Perception  += RulebookLore.Cultures.Humans.Danarian.Perception;
        charsheet.Stats.Abstract    += RulebookLore.Cultures.Humans.Danarian.Abstract;
        //assets
        charsheet.Assets.Resolve    += RulebookLore.Cultures.Humans.Danarian.Resolve;
        charsheet.Assets.Harm       += RulebookLore.Cultures.Humans.Danarian.Harm;
        charsheet.Assets.Spot       += RulebookLore.Cultures.Humans.Danarian.Spot;
        charsheet.Assets.Defense    += RulebookLore.Cultures.Humans.Danarian.Defence;
        charsheet.Assets.Purge      += RulebookLore.Cultures.Humans.Danarian.Purge;
        charsheet.Assets.Mana       += RulebookLore.Cultures.Humans.Danarian.Mana;
        charsheet.Assets.Actions    += RulebookLore.Cultures.Humans.Danarian.Actions;
        //skills
        charsheet.Skills.Combat     += RulebookLore.Cultures.Humans.Danarian.Combat;
        charsheet.Skills.Arcane     += RulebookLore.Cultures.Humans.Danarian.Arcane;
        charsheet.Skills.Psionics   += RulebookLore.Cultures.Humans.Danarian.Psionics;
        charsheet.Skills.Hide       += RulebookLore.Cultures.Humans.Danarian.Hide;
        charsheet.Skills.Traps      += RulebookLore.Cultures.Humans.Danarian.Traps;
        charsheet.Skills.Tactics    += RulebookLore.Cultures.Humans.Danarian.Tactics;
        charsheet.Skills.Social     += RulebookLore.Cultures.Humans.Danarian.Social;
        charsheet.Skills.Apothecary += RulebookLore.Cultures.Humans.Danarian.Apothecary;
        charsheet.Skills.Travel     += RulebookLore.Cultures.Humans.Danarian.Travel;
        charsheet.Skills.Sail       += RulebookLore.Cultures.Humans.Danarian.Sail;

        // TODO: apply culture bonuses
    }

    private static void ModifySheetForHighborn(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += RulebookLore.Cultures.Elves.Highborn.Strength;
        charsheet.Stats.Constitution+= RulebookLore.Cultures.Elves.Highborn.Constitution;
        charsheet.Stats.Agility     += RulebookLore.Cultures.Elves.Highborn.Agility;
        charsheet.Stats.Willpower   += RulebookLore.Cultures.Elves.Highborn.Willpower;
        charsheet.Stats.Perception  += RulebookLore.Cultures.Elves.Highborn.Perception;
        charsheet.Stats.Abstract    += RulebookLore.Cultures.Elves.Highborn.Abstract;
        //assets
        charsheet.Assets.Resolve    += RulebookLore.Cultures.Elves.Highborn.Resolve;
        charsheet.Assets.Harm       += RulebookLore.Cultures.Elves.Highborn.Harm;
        charsheet.Assets.Spot       += RulebookLore.Cultures.Elves.Highborn.Spot;
        charsheet.Assets.Defense    += RulebookLore.Cultures.Elves.Highborn.Defence;
        charsheet.Assets.Purge      += RulebookLore.Cultures.Elves.Highborn.Purge;
        charsheet.Assets.Mana       += RulebookLore.Cultures.Elves.Highborn.Mana;
        //skills
        charsheet.Skills.Combat     += RulebookLore.Cultures.Elves.Highborn.Combat;
        charsheet.Skills.Arcane     += RulebookLore.Cultures.Elves.Highborn.Arcane;
        charsheet.Skills.Psionics   += RulebookLore.Cultures.Elves.Highborn.Psionics;
        charsheet.Skills.Hide       += RulebookLore.Cultures.Elves.Highborn.Hide;
        charsheet.Skills.Traps      += RulebookLore.Cultures.Elves.Highborn.Traps;
        charsheet.Skills.Tactics    += RulebookLore.Cultures.Elves.Highborn.Tactics;
        charsheet.Skills.Social     += RulebookLore.Cultures.Elves.Highborn.Social;
        charsheet.Skills.Apothecary += RulebookLore.Cultures.Elves.Highborn.Apothecary;
        charsheet.Skills.Travel     += RulebookLore.Cultures.Elves.Highborn.Travel;
        charsheet.Skills.Sail       += RulebookLore.Cultures.Elves.Highborn.Sail;

        // TODO: apply culture bonuses
    }

    private static void ModifySheetForUndermountain(CharacterSheet charsheet)
    {
        //stats
        charsheet.Stats.Strength    += RulebookLore.Cultures.Dwarves.Undermountain.Strength;
        charsheet.Stats.Constitution+= RulebookLore.Cultures.Dwarves.Undermountain.Constitution;
        charsheet.Stats.Agility     += RulebookLore.Cultures.Dwarves.Undermountain.Agility;
        charsheet.Stats.Willpower   += RulebookLore.Cultures.Dwarves.Undermountain.Willpower;
        charsheet.Stats.Perception  += RulebookLore.Cultures.Dwarves.Undermountain.Perception;
        charsheet.Stats.Abstract    += RulebookLore.Cultures.Dwarves.Undermountain.Abstract;
        //assets                                             
        charsheet.Assets.Resolve    += RulebookLore.Cultures.Dwarves.Undermountain.Resolve;
        charsheet.Assets.Harm       += RulebookLore.Cultures.Dwarves.Undermountain.Harm;
        charsheet.Assets.Spot       += RulebookLore.Cultures.Dwarves.Undermountain.Spot;
        charsheet.Assets.Defense    += RulebookLore.Cultures.Dwarves.Undermountain.Defense;
        charsheet.Assets.Purge      += RulebookLore.Cultures.Dwarves.Undermountain.Purge;
        charsheet.Assets.Mana       += RulebookLore.Cultures.Dwarves.Undermountain.Mana;
        //skills                                             
        charsheet.Skills.Combat     += RulebookLore.Cultures.Dwarves.Undermountain.Combat;
        charsheet.Skills.Arcane     += RulebookLore.Cultures.Dwarves.Undermountain.Arcane;
        charsheet.Skills.Psionics   += RulebookLore.Cultures.Dwarves.Undermountain.Psionics;
        charsheet.Skills.Hide       += RulebookLore.Cultures.Dwarves.Undermountain.Hide;
        charsheet.Skills.Traps      += RulebookLore.Cultures.Dwarves.Undermountain.Traps;
        charsheet.Skills.Tactics    += RulebookLore.Cultures.Dwarves.Undermountain.Tactics;
        charsheet.Skills.Social     += RulebookLore.Cultures.Dwarves.Undermountain.Social;
        charsheet.Skills.Apothecary += RulebookLore.Cultures.Dwarves.Undermountain.Apothecary;
        charsheet.Skills.Travel     += RulebookLore.Cultures.Dwarves.Undermountain.Travel;
        charsheet.Skills.Sail       += RulebookLore.Cultures.Dwarves.Undermountain.Sail;

        // TODO: apply culture bonuses
    }

    private void DistributeClassAttributes(CharacterSheet sheet, string classes, int statPoints, int skillPoints)
    {
        if (classes == CharactersLore.Classes.Warrior)
        {
            DistributeStatpoints(sheet, statPoints, RulebookLore.Classes.Warrior.LikelyStats, RulebookLore.Classes.Warrior.UnlikelyStats);
            DistributeSkillpoints(sheet, skillPoints, RulebookLore.Classes.Warrior.LikelySkills, RulebookLore.Classes.Warrior.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Mage)
        {
            DistributeStatpoints(sheet, statPoints, RulebookLore.Classes.Mage.LikelyStats, RulebookLore.Classes.Mage.UnlikelyStats);
            DistributeSkillpoints(sheet, skillPoints, RulebookLore.Classes.Mage.LikelySkills, RulebookLore.Classes.Mage.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Hunter)
        {
            DistributeStatpoints(sheet, statPoints, RulebookLore.Classes.Hunter.LikelyStats, RulebookLore.Classes.Hunter.UnlikelyStats);
            DistributeSkillpoints(sheet, skillPoints, RulebookLore.Classes.Hunter.LikelySkills, RulebookLore.Classes.Hunter.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Swashbuckler)
        {
            DistributeStatpoints(sheet, statPoints, RulebookLore.Classes.Swashbuckler.LikelyStats, RulebookLore.Classes.Swashbuckler.UnlikelyStats);
            DistributeSkillpoints(sheet, skillPoints, RulebookLore.Classes.Swashbuckler.LikelySkills, RulebookLore.Classes.Swashbuckler.UnlikelySkills);
        }
        else if (classes == CharactersLore.Classes.Sorcerer)
        {
            DistributeStatpoints(sheet, statPoints, RulebookLore.Classes.Sorcerer.LikelyStats, RulebookLore.Classes.Sorcerer.UnlikelyStats);
            DistributeSkillpoints(sheet, skillPoints, RulebookLore.Classes.Sorcerer.LikelySkills, RulebookLore.Classes.Sorcerer.UnlikelySkills);
        }
    }

    private void DistributeStatpoints(CharacterSheet sheet, int statPoints, List<string> likelyStats, List<string> unlikelyStats)
    {
        while (statPoints != 0)
        {
            var roll = dice.Roll_100_noReroll();
            string chosenStat;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_1_to_n(likelyStats.Count);
                chosenStat = likelyStats[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_1_to_n(unlikelyStats.Count);
                chosenStat = unlikelyStats[rollForStat - 1];
            }

            if      (chosenStat == CharactersLore.Stats.Strength) sheet.Stats.Strength++;
            else if (chosenStat == CharactersLore.Stats.Constitution) sheet.Stats.Constitution++;
            else if (chosenStat == CharactersLore.Stats.Agility) sheet.Stats.Agility++;
            else if (chosenStat == CharactersLore.Stats.Willpower) sheet.Stats.Willpower++;
            else if (chosenStat == CharactersLore.Stats.Perception) sheet.Stats.Perception++;
            else if (chosenStat == CharactersLore.Stats.Abstract) sheet.Stats.Abstract++;

            statPoints--;
        }
    }

    private void DistributeSkillpoints(CharacterSheet sheet, int skillPoints, List<string> likeySkills, List<string> unlikeySkills)
    {
        while (skillPoints != 0)
        {
            var roll = dice.Roll_100_noReroll();
            string chosenSkill;

            if (roll <= 70)
            {
                var rollForStat = dice.Roll_1_to_n(likeySkills.Count);
                chosenSkill = likeySkills[rollForStat - 1];
            }
            else
            {
                var rollForStat = dice.Roll_1_to_n(unlikeySkills.Count);
                chosenSkill = unlikeySkills[rollForStat - 1];
            }

            if      (chosenSkill == CharactersLore.Skills.Combat) sheet.Skills.Combat++;
            else if (chosenSkill == CharactersLore.Skills.Arcane) sheet.Skills.Arcane++;
            else if (chosenSkill == CharactersLore.Skills.Psionics) sheet.Skills.Psionics++;
            else if (chosenSkill == CharactersLore.Skills.Hide) sheet.Skills.Hide++;
            else if (chosenSkill == CharactersLore.Skills.Traps) sheet.Skills.Traps++;
            else if (chosenSkill == CharactersLore.Skills.Tactics) sheet.Skills.Tactics++;
            else if (chosenSkill == CharactersLore.Skills.Social) sheet.Skills.Social++;
            else if (chosenSkill == CharactersLore.Skills.Apothecary) sheet.Skills.Apothecary++;
            else if (chosenSkill == CharactersLore.Skills.Travel) sheet.Skills.Travel++;
            else if (chosenSkill == CharactersLore.Skills.Sail) sheet.Skills.Sail++;

            skillPoints--;
        }
    }

    private static void CalculateAssets(CharacterSheet sheet)
    {
        sheet.Assets.Harm    += RulebookLore.Formulae.Assets.CalculateHarm(sheet.Stats);
        sheet.Assets.Spot    += RulebookLore.Formulae.Assets.CalculateSpot(sheet.Stats);
        sheet.Assets.Defense += RulebookLore.Formulae.Assets.CalculateDefense(sheet.Stats);
        sheet.Assets.Purge   += RulebookLore.Formulae.Assets.CalculatePurge(sheet.Stats);

        var resolve = RulebookLore.Formulae.Assets.CalculateResolve(sheet.Stats);
        sheet.Assets.Resolve     += resolve;
        sheet.Assets.ResolveLeft += resolve;

        var mana = RulebookLore.Formulae.Assets.CalculateMana(sheet.Stats);
        sheet.Assets.Mana     += mana;
        sheet.Assets.ManaLeft += mana;

        var actions = RulebookLore.Formulae.Assets.CalculateActions(sheet.Stats);
        sheet.Assets.Actions     += actions;
        sheet.Assets.ActionsLeft += actions;
    }

    private static void CalculateSkills(CharacterSheet sheet)
    {
        sheet.Skills.Combat     += RulebookLore.Formulae.Skills.CalculateCombat(sheet.Stats);
        sheet.Skills.Arcane     += RulebookLore.Formulae.Skills.CalculateArcane(sheet.Stats);
        sheet.Skills.Psionics   += RulebookLore.Formulae.Skills.CalculatePsionics(sheet.Stats);
        sheet.Skills.Hide       += RulebookLore.Formulae.Skills.CalculateHide(sheet.Stats);
        sheet.Skills.Traps      += RulebookLore.Formulae.Skills.CalculateTraps(sheet.Stats);
        sheet.Skills.Tactics    += RulebookLore.Formulae.Skills.CalculateTactics(sheet.Stats);
        sheet.Skills.Social     += RulebookLore.Formulae.Skills.CalculateSocial(sheet.Stats);
        sheet.Skills.Apothecary += RulebookLore.Formulae.Skills.CalculateApothecary(sheet.Stats);
        sheet.Skills.Travel     += RulebookLore.Formulae.Skills.CalculateTravel(sheet.Stats);
        sheet.Skills.Sail       += RulebookLore.Formulae.Skills.CalculateSail(sheet.Stats);
    }
    #endregion
}
