namespace Data_Mapping_Containers.Dtos;

public class QuestTemplate
{
    public string QuestType { get; set; }

    public string Fame { get; set; }
    public string Description { get; set; }
    public string Result { get; set; }

    public bool IsRepeatable { get; set; }
    public int MaxEffortLvl { get; set; }
}
