﻿namespace Data_Mapping_Containers.Dtos;

public class CharacterEquip
{
    public CharacterIdentity CharacterIdentity { get; set; } = new();
    public string ItemId { get; set; }
    public string InventoryLocation { get; set; }
}
