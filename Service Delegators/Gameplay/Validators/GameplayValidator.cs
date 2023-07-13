using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayValidator : ValidatorBase
{
    private readonly Snapshot snapshot;

    internal GameplayValidator(Snapshot snapshot)
        : base(snapshot)
    {
        this.snapshot = snapshot;
    }

    internal void ValidateBeforeTravel(PositionTravel positionTravel)
    {
        ValidateCharacterPlayerCombination(positionTravel.CharacterIdentity);
        var character = GetCharacter(positionTravel.CharacterIdentity);

        if (character.Status.IsInQuest) throw new Exception("Unable to travel during quest.");
        if (character.Status.IsInArena) throw new Exception("Unable to travel during arena.");
        if (character.Status.IsInQuest) throw new Exception("Unable to travel during quest.");
        if (!character.Info.IsAlive) throw new Exception("Unable to travel, your character is dead.");

        var totalProvisions = character.Inventory.Provisions
            + character.Henchmen.Select(s => s.Inventory.Provisions).Sum();

        if (totalProvisions == 0) throw new Exception("Not enough provisions to travel.");

        var destinationFullName = Utils.GetLocationFullName(positionTravel.Destination);
        if (!GameplayLore.Map.All.Select(s => s.FullName).ToList().Contains(destinationFullName)) throw new Exception("No such destination is known.");
    }
}
