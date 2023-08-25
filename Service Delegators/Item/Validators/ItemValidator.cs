using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class ItemValidator : ValidatorBase
{
    internal ItemValidator(SnapshotOld snapshot) 
        : base(snapshot)
    { }

    internal void ValidateTypeAndSubtypeOnGenerate(string type, string subtype)
    {
        ValidateString(type);
        ValidateString(subtype);

        if (!ItemsLore.Types.All.Contains(type)) throw new Exception("No such type found for item generate.");

        if (!(ItemsLore.Subtypes.Weapons.All.Contains(subtype)
            || ItemsLore.Subtypes.Protections.All.Contains(subtype)
            || ItemsLore.Subtypes.Wealth.All.Contains(subtype))) throw new Exception("No such subtype found for item generate.");
    }
}
