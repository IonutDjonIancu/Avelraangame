namespace Data_Mapping_Containers.Dtos;

public class CharacterStatus
{
    public bool IsLockedForModify { get; set; }

    public bool IsInParty { get; set; }
    public string QuestId { get; set; }

    public int NrOfQuestsFinished { get; set; }
    public List<string> QuestsFinished { get; set; } = new();
}
