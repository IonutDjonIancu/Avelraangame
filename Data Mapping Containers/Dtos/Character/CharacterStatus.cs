namespace Data_Mapping_Containers.Dtos;

public class CharacterStatus
{
    public bool IsLockedForChange { get; set; }

    public bool IsInParty { get; set; }
    public string PartyId { get; set; }

    public bool IsInQuest { get; set; }
    public string QuestId { get; set; }

    public bool IsInEncounter { get; set; }
    public string EncounterId { get; set; }
}
