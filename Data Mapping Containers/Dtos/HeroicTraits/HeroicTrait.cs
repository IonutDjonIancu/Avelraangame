namespace Data_Mapping_Containers.Dtos;

public class HeroicTrait
{
    public HeroicTraitIdentity Identity { get; set; }

    public string Description { get; set; }
    public string Lore { get; set; }
    
    public TraitsLore.Type Type { get; set; }
    public TraitsLore.Subtype Subtype { get; set; }

    public int DeedsCost { get; set; }
}
