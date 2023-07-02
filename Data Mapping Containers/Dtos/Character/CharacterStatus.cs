namespace Data_Mapping_Containers.Dtos;

public class CharacterStatus
{
    public bool HasAttributesLocked { get; set; }
    public bool HasInventoryLocked { get; set; }

    public bool IsInParty { get; set; }
    public string PartyId { get; set; }

    public int NrOfQuestsFinished { get; set; }
    public List<string> QuestsFinished { get; set; } = new();
}
