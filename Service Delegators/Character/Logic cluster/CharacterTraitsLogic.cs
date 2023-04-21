#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using System.Diagnostics;

namespace Service_Delegators;

internal class CharacterTraitsLogic
{
    private readonly IDatabaseManager dbm;
    private readonly CharacterPaperdollLogic paperdollLogic;

    public CharacterTraitsLogic(
        IDatabaseManager databaseManager,
        CharacterPaperdollLogic paperdollLogic) 
    {
        dbm = databaseManager;
        this.paperdollLogic = paperdollLogic;
    }

    internal Character ApplyHeroicTrait(CharacterHeroicTrait trait, string playerId)
    {
        var character = dbm.Metadata.GetCharacterById(trait.CharacterId, playerId);
        var heroicTrait = dbm.Snapshot.Traits.Find(t => t.Identity.Id == trait.HeroicTraitId);

        character.HeroicTraits.Add(heroicTrait);
        character.LevelUp.DeedsPoints -= heroicTrait.DeedsCost;

        if (heroicTrait.Type == TraitsLore.Type.bonus) ApplyBonusHeroicTraits(character, heroicTrait, trait);

        dbm.PersistPlayer(dbm.Metadata.GetPlayerById(playerId));

        return character;
    }

    #region private methods
    private void ApplyBonusHeroicTraits(Character character, HeroicTrait heroicTrait, CharacterHeroicTrait trait)
    {
        if      (heroicTrait.Identity.Name == TraitsLore.BonusTraits.swordsman) RunSwordsmanLogic(character);
        else if (heroicTrait.Identity.Name == TraitsLore.BonusTraits.skillful) RunSkillfulLogic(character, trait.Skill);
    }

    private void RunSwordsmanLogic(Character character)
    {
        var paperdollCombatValue = paperdollLogic.CalculatePaperdoll(character.Identity.Id, character.Identity.PlayerId).Skills.Combat;
        var value = 5 + (int)Math.Floor(paperdollCombatValue * 0.01);
        character.Sheet.Skills.Combat += value;
    }

    private void RunSkillfulLogic(Character character, string skill)
    {
        if (skill == CharactersLore.Skills.Combat)
        {
            var value = (int)Math.Floor(character.Sheet.Skills.Combat * 0.2);
            character.Sheet.Skills.Combat += value;
        }
        else if (skill == CharactersLore.Skills.Arcane)
        {
            var value = (int)Math.Floor(character.Sheet.Skills.Arcane * 0.2);
            character.Sheet.Skills.Arcane += value;
        }
        else if (skill == CharactersLore.Skills.Psionics)
        {
            var value = (int)Math.Floor(character.Sheet.Skills.Psionics * 0.2);
            character.Sheet.Skills.Psionics += value;
        }
        else if (skill == CharactersLore.Skills.Hide)
        {
            var value = (int)Math.Floor(character.Sheet.Skills.Hide * 0.2);
            character.Sheet.Skills.Hide += value;
        }
        else if (skill == CharactersLore.Skills.Traps)
        {
            var value = (int)Math.Floor(character.Sheet.Skills.Traps * 0.2);
            character.Sheet.Skills.Traps += value;
        }
        else if (skill == CharactersLore.Skills.Tactics)
        {
            var value = (int)Math.Floor(character.Sheet.Skills.Tactics * 0.2);
            character.Sheet.Skills.Tactics += value;
        }
        else if (skill == CharactersLore.Skills.Social)
        {
            var value = (int)Math.Floor(character.Sheet.Skills.Social * 0.2);
            character.Sheet.Skills.Social += value;
        }
        else if (skill == CharactersLore.Skills.Apothecary)
        {
            var value = (int)Math.Floor(character.Sheet.Skills.Apothecary * 0.2);
            character.Sheet.Skills.Apothecary += value;
        }
        else if (skill == CharactersLore.Skills.Travel)
        {
            var value = (int)Math.Floor(character.Sheet.Skills.Travel * 0.2);
            character.Sheet.Skills.Travel += value;
        }
        else if (skill == CharactersLore.Skills.Sail)
        {
            var value = (int)Math.Floor(character.Sheet.Skills.Sail * 0.2);
            character.Sheet.Skills.Sail += value;
        }
        else
        {
            throw new Exception("Unable to apply heroic trait to said skill, skill not found.");
        }
    }

    #endregion
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CA1822 // Mark members as static