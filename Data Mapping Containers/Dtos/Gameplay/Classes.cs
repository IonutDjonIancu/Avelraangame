namespace Data_Mapping_Containers.Dtos;

public class Classes
{
    public ClassesLikelyAttributes Warrior { get; set; }
    public ClassesLikelyAttributes Mage { get; set; }
    public ClassesLikelyAttributes Hunter { get; set; }
    public ClassesLikelyAttributes Swashbuckler { get; set; }
    public ClassesLikelyAttributes Sorcerer { get; set; }

    public List<ClassesLikelyAttributes> All()
    {
        return new List<ClassesLikelyAttributes>()
        {
            Warrior,
            Mage,
            Hunter,
            Swashbuckler,
            Sorcerer
        };
    }
}
