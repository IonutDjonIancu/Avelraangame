using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class DiceRollValidator : ValidatorBase
{
    private readonly Snapshot snapshot;

    internal DiceRollValidator(Snapshot snapshot)
        : base(snapshot)
    {
        this.snapshot = snapshot;
    }

    internal void ValidateCharacterBeforeRoll(Character character, string skill)
    {
        ValidateObject(character);

        if (!character.Status.IsAlive) throw new Exception("Character is dead or dying.");
        
        ValidateString(character.Status.Traits.Tradition);
        if (!GameplayLore.Tradition.All.Contains(character.Status.Traits.Tradition)) throw new Exception("Unrecognized tradition.");

        ValidateString(skill);
        if (!CharactersLore.Skills.All.Contains(skill)) throw new Exception("Skill does not exist.");
    }
}
