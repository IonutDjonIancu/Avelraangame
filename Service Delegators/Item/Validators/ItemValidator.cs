﻿#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class ItemValidator : ValidatorBase
{
    internal void ValidateTypeAndSubtypeOnGenerate(string type, string subtype)
    {
        ValidateString(type);
        ValidateString(subtype);

        if (!ItemsLore.Types.All.Contains(type)) Throw("No such type found for item generate.");

        if (!(ItemsLore.Subtypes.Weapons.All.Contains(subtype)
            || ItemsLore.Subtypes.Protections.All.Contains(subtype)
            || ItemsLore.Subtypes.Wealth.All.Contains(subtype))) Throw("No such subtype found for item generate.");
    }
}

#pragma warning restore CA1822 // Mark members as static