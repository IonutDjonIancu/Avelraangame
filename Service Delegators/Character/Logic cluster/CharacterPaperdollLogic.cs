#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterPaperdollLogic
{
    private readonly IDatabaseService dbs;
    private readonly IDiceRollService dice;

    private CharacterPaperdollLogic() { }
    internal CharacterPaperdollLogic(
        IDatabaseService databaseService,
        IDiceRollService diceRollService)
    {
        dbs = databaseService;
        dice = diceRollService;
    }

    internal CharacterPaperdoll CalculatePaperdoll(CharacterIdentity charIdentity)
    {
        var character = dbs.Snapshot.Players.Find(p => p.Identity.Id == charIdentity.PlayerId)!.Characters.Find(c => c.Identity.Id == charIdentity.Id)!;

        return GetPaperdoll(character);
    }

    internal CharacterPaperdoll CalculatePaperdoll(CharacterIdentity characterIdentity, string npcId)
    {
        var character = dbs.Snapshot.Players.Find(p => p.Identity.Id == characterIdentity.PlayerId)!.Characters.Find(c => c.Identity.Id == characterIdentity.Id)!;
        var npc = character.Mercenaries.Find(s => s.Identity.Id == npcId)!;

        return GetPaperdoll(npc);
    }

    internal CharacterPaperdoll CalculatePaperdoll(ICharacter character)
    {
        return GetPaperdoll(character);
    }

    /// <summary>
    /// Generic gameplay roll. This is the official dice roll for characters and npcs. Includes a level up and persistence.
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="character"></param>
    /// <returns>The grade expressed as int.</returns>
    /// <exception cref="NotImplementedException"></exception>
    internal int PaperdollDiceRoll(string attribute, ICharacter character)
    {
        var paperdoll = GetPaperdoll(character);
        int grade;

        if      (attribute == CharactersLore.Stats.Strength)    grade = RollDice(paperdoll.Stats.Strength, character);
        else if (attribute == CharactersLore.Stats.Constitution)grade = RollDice(paperdoll.Stats.Constitution, character);
        else if (attribute == CharactersLore.Stats.Agility)     grade = RollDice(paperdoll.Stats.Agility, character);
        else if (attribute == CharactersLore.Stats.Willpower)   grade = RollDice(paperdoll.Stats.Willpower, character);
        else if (attribute == CharactersLore.Stats.Perception)  grade = RollDice(paperdoll.Stats.Perception, character);
        else if (attribute == CharactersLore.Stats.Abstract)    grade = RollDice(paperdoll.Stats.Abstract, character);

        else if (attribute == CharactersLore.Skills.Combat)     grade = RollDice(paperdoll.Skills.Combat, character);
        else if (attribute == CharactersLore.Skills.Arcane)     grade = RollDice(paperdoll.Skills.Arcane, character);
        else if (attribute == CharactersLore.Skills.Psionics)   grade = RollDice(paperdoll.Skills.Psionics, character);
        else if (attribute == CharactersLore.Skills.Hide)       grade = RollDice(paperdoll.Skills.Hide, character);
        else if (attribute == CharactersLore.Skills.Traps)      grade = RollDice(paperdoll.Skills.Traps, character);
        else if (attribute == CharactersLore.Skills.Tactics)    grade = RollDice(paperdoll.Skills.Tactics, character);
        else if (attribute == CharactersLore.Skills.Social)     grade = RollDice(paperdoll.Skills.Social, character);
        else if (attribute == CharactersLore.Skills.Apothecary) grade = RollDice(paperdoll.Skills.Apothecary, character);
        else if (attribute == CharactersLore.Skills.Travel)     grade = RollDice(paperdoll.Skills.Travel, character);
        else if (attribute == CharactersLore.Skills.Sail)       grade = RollDice(paperdoll.Skills.Sail, character);
        else throw new NotImplementedException();

        if (!string.IsNullOrEmpty(character.Identity.PlayerId)) dbs.PersistPlayer(character.Identity.PlayerId);

        return grade;
    }

    #region private methods
    private int RollDice(int attributeValue, ICharacter character)
    {
        var (grade, crits) = dice.Roll_gameplay_dice(character.Info.Origins.Tradition, attributeValue);

        LevelUpByCrits(crits, character);

        return grade;
    }

    private void LevelUpByCrits(int crits, ICharacter character)
    {
        for (var i = 0; i < crits; i++)
        {
            character.LevelUp.DeedsPoints += 1;
            character.LevelUp.StatPoints += 2;
            character.LevelUp.SkillPoints += 4;
        }
    }

    private CharacterPaperdoll GetPaperdoll(ICharacter character)
    {
        var items = character.Inventory.GetAllEquipedItems();
        var passiveTraits = character.HeroicTraits.Where(t => t.Type == TraitsLore.Type.passive).ToList();

        var paperdoll = new CharacterPaperdoll()
        {
            PlayerId = character.Identity.PlayerId,
            CharacterId = character.Identity.Id,
            Name = character.Info.Name,
            Race = character.Info.Origins.Race,
            Inventory = items
        };

        CalculatePaperdollStats(character, items, paperdoll);
        CalculatePaperdollAssets(character, items, paperdoll);
        CalculatePaperdollSkills(character, items, paperdoll);
        CalculatePaperdollSpecialSkills(character, items, paperdoll);
        CalculatePaperdollActionTokens(paperdoll);

        return paperdoll;
    }



    private static void CalculatePaperdollStats(ICharacter character, List<Item> items, CharacterPaperdoll paperdoll)
    {
        var itemStrBonus = 0;
        var itemConBonus = 0;
        var itemAgiBonus = 0;
        var itemWilBonus = 0;
        var itemPerBonus = 0;
        var itemAbsBonus = 0;

        if (items?.Count > 0)
        {
            itemStrBonus = items.Sum(i => i.Sheet.Stats.Strength);
            itemConBonus = items.Sum(i => i.Sheet.Stats.Constitution);
            itemAgiBonus = items.Sum(i => i.Sheet.Stats.Agility);
            itemWilBonus = items.Sum(i => i.Sheet.Stats.Willpower);
            itemPerBonus = items.Sum(i => i.Sheet.Stats.Perception);
            itemAbsBonus = items.Sum(i => i.Sheet.Stats.Abstract);
        }

        paperdoll.Stats = new CharacterStats
        {
            Strength    = character.Sheet.Stats.Strength + itemStrBonus,
            Constitution= character.Sheet.Stats.Constitution + itemConBonus,
            Agility     = character.Sheet.Stats.Agility + itemAgiBonus,
            Willpower   = character.Sheet.Stats.Willpower + itemWilBonus,
            Perception  = character.Sheet.Stats.Perception + itemPerBonus,
            Abstract    = character.Sheet.Stats.Abstract + itemAbsBonus
        };
    }

    private static void CalculatePaperdollAssets(ICharacter character, List<Item> items, CharacterPaperdoll paperdoll)
    {
        var itemResBonus = 0;
        var itemHarBonus = 0;
        var itemSpoBonus = 0;
        var itemDefBonus = 0;
        var itemPurBonus = 0;
        var itemManBonus = 0;

        if (items?.Count > 0)    
        {
            itemResBonus = items.Sum(i => i.Sheet.Assets.Resolve);
            itemHarBonus = items.Sum(i => i.Sheet.Assets.Harm);
            itemSpoBonus = items.Sum(i => i.Sheet.Assets.Spot);
            itemDefBonus = items.Sum(i => i.Sheet.Assets.Defense);
            itemPurBonus = items.Sum(i => i.Sheet.Assets.Purge);
            itemManBonus = items.Sum(i => i.Sheet.Assets.Mana);
        }

        paperdoll.Assets = new CharacterAssets
        {
            // RES is the only asset increased by entity level
            Resolve = (character.Sheet.Assets.Resolve + RulebookLore.Formulae.Assets.CalculateResolve(character.Sheet.Stats) + itemResBonus) * character.Info.EntityLevel,

            // DEF cannot be greater than 90, so that to avoid making characters immune to all dmg
            Defense = character.Sheet.Assets.Defense + RulebookLore.Formulae.Assets.CalculateDefense(character.Sheet.Stats) + itemDefBonus >= 90
                        ? 90
                        : character.Sheet.Assets.Defense + RulebookLore.Formulae.Assets.CalculateDefense(character.Sheet.Stats) + itemDefBonus,

            Harm    = character.Sheet.Assets.Harm + RulebookLore.Formulae.Assets.CalculateHarm(character.Sheet.Stats) + itemHarBonus,
            Spot    = character.Sheet.Assets.Spot + RulebookLore.Formulae.Assets.CalculateSpot(character.Sheet.Stats) + itemSpoBonus,
            Purge   = character.Sheet.Assets.Purge + RulebookLore.Formulae.Assets.CalculatePurge(character.Sheet.Stats) + itemPurBonus,
            Mana    = character.Sheet.Assets.Mana + RulebookLore.Formulae.Assets.CalculateMana(character.Sheet.Stats) + itemManBonus
        };
    }

    private static void CalculatePaperdollSkills(ICharacter character, List<Item> items, CharacterPaperdoll paperdoll)
    {
        var itemComBonus = 0;
        var itemArcBonus = 0;
        var itemPsiBonus = 0;
        var itemHidBonus = 0;
        var itemTraBonus = 0;
        var itemTacBonus = 0;
        var itemSocBonus = 0;
        var itemApoBonus = 0;
        var itemTrvBonus = 0;
        var itemSaiBonus = 0;

        if (items?.Count > 0)
        {
            itemComBonus = items.Sum(i => i.Sheet.Skills.Combat);
            itemArcBonus = items.Sum(i => i.Sheet.Skills.Arcane);
            itemPsiBonus = items.Sum(i => i.Sheet.Skills.Psionics);
            itemHidBonus = items.Sum(i => i.Sheet.Skills.Hide);
            itemTraBonus = items.Sum(i => i.Sheet.Skills.Traps);
            itemTacBonus = items.Sum(i => i.Sheet.Skills.Tactics);
            itemSocBonus = items.Sum(i => i.Sheet.Skills.Social);
            itemApoBonus = items.Sum(i => i.Sheet.Skills.Apothecary);
            itemTrvBonus = items.Sum(i => i.Sheet.Skills.Travel);
            itemSaiBonus = items.Sum(i => i.Sheet.Skills.Sail);
        }

        paperdoll.Skills = new CharacterSkills
        {
            Combat      = character.Sheet.Skills.Combat     + RulebookLore.Formulae.Skills.CalculateCombat(character.Sheet.Stats) + itemComBonus,
            Arcane      = character.Sheet.Skills.Arcane     + RulebookLore.Formulae.Skills.CalculateArcane(character.Sheet.Stats) + itemArcBonus,
            Psionics    = character.Sheet.Skills.Psionics   + RulebookLore.Formulae.Skills.CalculatePsionics(character.Sheet.Stats) + itemPsiBonus,
            Hide        = character.Sheet.Skills.Hide       + RulebookLore.Formulae.Skills.CalculateHide(character.Sheet.Stats) + itemHidBonus,
            Traps       = character.Sheet.Skills.Traps      + RulebookLore.Formulae.Skills.CalculateTraps(character.Sheet.Stats) + itemTraBonus,
            Tactics     = character.Sheet.Skills.Tactics    + RulebookLore.Formulae.Skills.CalculateTactics(character.Sheet.Stats) + itemTacBonus,
            Social      = character.Sheet.Skills.Social     + RulebookLore.Formulae.Skills.CalculateSocial(character.Sheet.Stats) + itemSocBonus,
            Apothecary  = character.Sheet.Skills.Apothecary + RulebookLore.Formulae.Skills.CalculateApothecary(character.Sheet.Stats) + itemApoBonus,
            Travel      = character.Sheet.Skills.Travel     + RulebookLore.Formulae.Skills.CalculateTravel(character.Sheet.Stats) + itemTrvBonus,
            Sail        = character.Sheet.Skills.Sail       + RulebookLore.Formulae.Skills.CalculateSail(character.Sheet.Stats) + itemSaiBonus
        };
    }

    private static void CalculatePaperdollSpecialSkills(ICharacter character, List<Item> items, CharacterPaperdoll paperdoll)
    {
        if (character.HeroicTraits == null) return;

        if (items.Count > 0)
        {
            // some items may contain heroic traits!!
            // TODO: cater for items heroic traits in the future
        }

        paperdoll.SpecialSkills = new List<HeroicTrait>();

        if (character.HeroicTraits?.Count < 0) return;
        paperdoll.SpecialSkills = character.HeroicTraits!.Where(ht => ht.Type == TraitsLore.Type.active).ToList();

        if (character.HeroicTraits!.Exists(t => t.Identity.Name == TraitsLore.PassiveTraits.theStrengthOfMany.Identity.Name))
        {
            paperdoll.Stats.Strength += (int)Math.Floor(paperdoll.Stats.Strength * 0.1);
        }

        if (character.HeroicTraits.Exists(t => t.Identity.Name == TraitsLore.PassiveTraits.lifeInThePits.Identity.Name))
        {
            paperdoll.Assets.Resolve += 50;
        }

        if (character.HeroicTraits.Exists(t => t.Identity.Name == TraitsLore.PassiveTraits.candlelight.Identity.Name))
        {
            paperdoll.Skills.Arcane += 20;
        }
    }

    private static void CalculatePaperdollActionTokens(CharacterPaperdoll paperdoll)
    {
        if (paperdoll.Assets.Resolve < 100) 
        {
            paperdoll.ActionTokens = 1;
        }
        else
        {
            paperdoll.ActionTokens = RulebookLore.Formulae.Misc.CalculateActionTokens(paperdoll.Assets.Resolve);
        }
    }
    #endregion
}

#pragma warning restore CA1822 // Mark members as static
