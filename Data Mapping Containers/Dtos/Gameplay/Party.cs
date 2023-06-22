namespace Data_Mapping_Containers.Dtos;

public class Party
{
    public PartyIdentity Identity { get; set; }

    public string CreationDate { get; set; }
    public bool IsAdventuring { get; set; }
    public bool IsInCombat { get; set; }
    public int Food { get; set; }

    public Position Position { get; set; } = new();

    public List<CharacterIdentity> Characters { get; set; } = new();
    public List<CharacterPaperdoll> Paperdolls { get; set; } = new();

    // loot will be populated for every party member death
    public List<Item> Loot { get; set; } = new();
}
