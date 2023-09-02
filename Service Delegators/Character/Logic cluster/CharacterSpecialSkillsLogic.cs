using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterSpecialSkillsLogic
{
    Character ApplySpecialSkill(CharacterSpecialSkillAdd spsk);
}

public class CharacterSpecialSkillsLogic : ICharacterSpecialSkillsLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;

    public CharacterSpecialSkillsLogic(Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public Character ApplySpecialSkill(CharacterSpecialSkillAdd spsk)
    {
        lock (_lock)
        {
            var character = Utils.GetPlayerCharacter(spsk.CharacterIdentity, snapshot);
            var specialSkill = SpecialSkillsLore.All.Find(t => t.Identity.Id == spsk.SpecialSkillId)!;

            character.Sheet.SpecialSkills.Add(specialSkill);
            character.LevelUp.DeedsPoints -= specialSkill.DeedsCost;

            if (specialSkill.Type == SpecialSkillsLore.Type.Bonus) ApplyBonusHeroicTraits(character, specialSkill.Identity.Name, spsk.Subskill);

            return character;
        }
    }

    #region private methods
    private static void ApplyBonusHeroicTraits(Character character, string heroicTraitName, string skill = "")
    {
        if (heroicTraitName == SpecialSkillsLore.BonusSpecialSkills.Swordsman.Identity.Name) RunSwordsmanLogic(character);
        else if (heroicTraitName == SpecialSkillsLore.BonusSpecialSkills.Skillful.Identity.Name) RunSkillfulLogic(character, skill);
    }

    private static void RunSwordsmanLogic(Character character)
    {
        var value = 5 + (int)Math.Floor(character.Sheet.Skills.Combat * 0.01);
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
