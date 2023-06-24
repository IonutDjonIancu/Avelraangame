namespace Data_Mapping_Containers.Dtos;

public class Quest
{
    public string Id { get; set; }
    public string PartyId { get; set; }

    // these are hardcoded in the QuestsLore
    public string Difficulty { get; set; }
    public string Effort { get; set; }
    public Position Position { get; set; } = new();
    public string Description { get; set; }

    // these will be generated dynamically
    public int EffortRoll { get; set; }
    public int EncountersTotal { get; set; }
    public int EncountersLeft { get; set; }
    public Encounter CurrentEncounter { get; set; } = new();

    public Reward Reward { get; set; } = new();
}
