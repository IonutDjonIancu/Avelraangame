#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterPaperdollLogic
{
    private readonly IDatabaseManager dbm;

    internal CharacterPaperdollLogic(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
    }

    internal CharacterPaperdoll CalculatePaperdoll(string characterId, string playerId)
    {
        var character = dbm.Metadata.GetCharacterById(characterId, playerId);
        var items = character.Inventory.GetAllEquipedItems();
        var traits = character.HeroicTraits.Where(t => t.Type == TraitsLore.Type.passive).ToList();

        var paperdoll = new CharacterPaperdoll(dbm.Snapshot.Rulebook.Acronyms);
        CalculatePaperdollStats(character, items, traits, paperdoll);
        CalculatePaperdollAssets(character, items, traits, paperdoll);
        CalculatePaperdollSkills(character, items, traits, paperdoll);

        return paperdoll;
    }

    #region private methods
    private static void CalculatePaperdollStats(Character character, List<Item> items, List<HeroicTrait> traits, CharacterPaperdoll paperdoll)
    {
        var itemStrBonus = 0;
        var itemConBonus = 0;
        var itemAgiBonus = 0;
        var itemWilBonus = 0;
        var itemPerBonus = 0;
        var itemAbsBonus = 0;

        if (items.Count > 0)
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

        if (traits.Count > 0)
        {
            if (traits.Exists(t => t.Identity.Name == TraitsLore.PassiveTraits.theStrengthOfMany))
            {
                paperdoll.Stats.Strength += (int)Math.Floor(paperdoll.Stats.Strength * 0.1);
            }
        }
    }

    private void CalculatePaperdollAssets(Character character, List<Item> items, List<HeroicTrait> traits, CharacterPaperdoll paperdoll)
    {
        var itemResBonus = 0;
        var itemHarBonus = 0;
        var itemSpoBonus = 0;
        var itemDefBonus = 0;
        var itemPurBonus = 0;
        var itemManBonus = 0;

        if (items.Count > 0)
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
            Resolve = (character.Sheet.Assets.Resolve + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.AssetsFormulas.ResolveFormula) + itemResBonus) * character.Info.EntityLevel.GetValueOrDefault(),
            Harm    = character.Sheet.Assets.Harm + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.AssetsFormulas.HarmFormula) + itemHarBonus,
            Spot    = character.Sheet.Assets.Spot + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.AssetsFormulas.SpotFormula) + itemSpoBonus,
            Defense = character.Sheet.Assets.Defense + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.AssetsFormulas.DefenseFormula) + itemDefBonus,
            Purge   = character.Sheet.Assets.Purge + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.AssetsFormulas.PurgeFormula) + itemPurBonus,
            Mana    = character.Sheet.Assets.Mana + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.AssetsFormulas.ManaFormula) + itemManBonus
        };

        if (traits.Count > 0)
        {
            if (traits.Exists(t => t.Identity.Name == TraitsLore.PassiveTraits.lifeInThePits))
            {
                paperdoll.Assets.Resolve += 50;
            }
        }
    }

    private void CalculatePaperdollSkills(Character character, List<Item> items, List<HeroicTrait> traits, CharacterPaperdoll paperdoll)
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

        if (items.Count > 0)
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
            Combat      = character.Sheet.Skills.Combat + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.SkillsFormulas.Combat) + itemComBonus,
            Arcane      = character.Sheet.Skills.Arcane + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.SkillsFormulas.Arcane) + itemArcBonus,
            Psionics    = character.Sheet.Skills.Psionics + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.SkillsFormulas.Psionics) + itemPsiBonus,
            Hide        = character.Sheet.Skills.Hide + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.SkillsFormulas.Hide) + itemHidBonus,
            Traps       = character.Sheet.Skills.Traps + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.SkillsFormulas.Traps) + itemTraBonus,
            Tactics     = character.Sheet.Skills.Tactics + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.SkillsFormulas.Tactics) + itemTacBonus,
            Social      = character.Sheet.Skills.Social + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.SkillsFormulas.Social) + itemSocBonus,
            Apothecary  = character.Sheet.Skills.Apothecary + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.SkillsFormulas.Apothecary) + itemApoBonus,
            Travel      = character.Sheet.Skills.Travel + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.SkillsFormulas.Travel) + itemTraBonus,
            Sail        = character.Sheet.Skills.Sail + paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.SkillsFormulas.Sail) + itemSaiBonus
        };

        if (traits.Count > 0)
        {
            if (traits.Exists(t => t.Identity.Name == TraitsLore.PassiveTraits.candlelight))
            {
                paperdoll.Skills.Arcane += 20;
            }
        }
    }
    #endregion
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.