namespace Data_Mapping_Containers.Dtos;

public class Classes
{
    public Subclass Warrior { get; set; }
    public Subclass Mage { get; set; }
    public Subclass Hunter { get; set; }
    public Subclass Swashbuckler { get; set; }
    public Subclass Sorcerer { get; set; }

    public List<Subclass> All()
    {
        return new List<Subclass>()
        {
            Warrior,
            Mage,
            Hunter,
            Swashbuckler,
            Sorcerer
        };
    }
}
