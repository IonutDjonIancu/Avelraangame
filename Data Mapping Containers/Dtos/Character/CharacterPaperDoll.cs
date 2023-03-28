namespace Data_Mapping_Containers.Dtos;

public class CharacterSheet
{
    // stats
    public int Strength { get; set; }
    public int Constitution { get; set; }
    public int Agility { get; set; }
    public int Willpower { get; set; }
    public int Perception { get; set; }
    public int Abstract { get; set; }

    // assets
    public int Stamina { get; set; }
    public int Harm { get; set; }
    public int Armour { get; set; }
    public int Purge { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }

    // skills
    public int Combat { get; set; }
    public int Arcane { get; set; }
    public int Psionics { get; set; }
    public int Hide { get; set; }
    public int Traps { get; set; }
    public int Tactics { get; set; }
    public int Social { get; set; }
    public int Apothecary { get; set; }
    public int Travel { get; set; }
    public int Sail { get; set; }
}
