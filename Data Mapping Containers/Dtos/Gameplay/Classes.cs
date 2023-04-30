namespace Data_Mapping_Containers.Dtos;

public class Classes
{
    public Subclasses Warrior { get; set; }
    public Subclasses Mage { get; set; }
    public Subclasses Hunter { get; set; }
    public Subclasses Swashbuckler { get; set; }
    public Subclasses Sorcerer { get; set; }

    public List<Subclasses> All()
    {
        return new List<Subclasses>()
        {
            Warrior,
            Mage,
            Hunter,
            Swashbuckler,
            Sorcerer
        };
    }
}
