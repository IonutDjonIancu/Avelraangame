namespace Data_Mapping_Containers.Dtos;

public class Quest
{
    public string Id { get; set; }
    public CharacterIdentity PartyLead { get; set; }
    public List<CharacterIdentity> PartyMembers { get; set; } = new();

    public Position Position { get; set; } = new();
    public QuestDetails Details { get; set; } = new();

    public List<QuestEncounter> Encounters { get; set; } = new();
}
