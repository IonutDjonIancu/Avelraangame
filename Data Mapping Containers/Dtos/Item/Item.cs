namespace Data_Mapping_Containers.Dtos;

public class Item
{
    public ItemIdentity Identity { get; set; }

    public string Name { get; set; }

    public int Level { get; set; }
    public string LevelName { get; set; }
    public string Type { get; set; }
    public string Subtype { get; set; }
    public string Quality { get; set; }
    public string Category { get; set; }
    public string Subcategory { get; set; }
    public List<string> InventoryLocations { get; set; } = new();
    public string Description { get; set; }
    public string Lore { get; set; }
    public bool HasTaint { get; set; }

    public CharacterSheet Sheet { get; set; } = new();

    public int Value { get; set; }
}
