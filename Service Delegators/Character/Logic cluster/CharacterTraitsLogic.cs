using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterTraitsLogic
{
    private readonly IDatabaseService dbs;
    private readonly CharacterPaperdollLogic paperdollLogic;

    private CharacterTraitsLogic() { }
    internal CharacterTraitsLogic(
        IDatabaseService databaseService,
        CharacterPaperdollLogic characterPaperdollLogic) 
    {
        dbs = databaseService;
        paperdollLogic = characterPaperdollLogic;
    }

    internal Character ApplyHeroicTrait(CharacterHeroicTrait trait)
    {
        var player = dbs.Snapshot.Players.Find(p => p.Identity.Id == trait.CharacterIdentity.PlayerId)!;
        var character = player.Characters.Find(c => c.Identity.Id == trait.CharacterIdentity.Id)!;
        var heroicTrait = TraitsLore.All.Find(t => t.Identity.Id == trait.HeroicTraitId)!;

        character.HeroicTraits.Add(heroicTrait);
        character.LevelUp.DeedsPoints -= heroicTrait.DeedsCost;

        if (heroicTrait.Type == TraitsLore.Type.bonus) ApplyBonusHeroicTraits(character, heroicTrait.Identity.Name, trait.Skill);

        dbs.PersistPlayer(player.Identity.Id);

        return character;
    }

    #region private methods
    private void ApplyBonusHeroicTraits(Character character, string heroicTraitName, string skill = "")
    {
        if      (heroicTraitName == TraitsLore.BonusTraits.swordsman.Identity.Name) RunSwordsmanLogic(character);
        else if (heroicTraitName == TraitsLore.BonusTraits.skillful.Identity.Name) RunSkillfulLogic(character, skill);
    }

    private void RunSwordsmanLogic(Character character)
    {
        var paperdollCombatValue = paperdollLogic.CalculateCharPaperdoll(character).Skills.Combat;
        var value = 5 + (int)Math.Floor(paperdollCombatValue * 0.01);
        character.Sheet.Skills.Combat += value;
    }

    private static void RunSkillfulLogic(Character character, string skill)
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
    }
    #endregion
}
