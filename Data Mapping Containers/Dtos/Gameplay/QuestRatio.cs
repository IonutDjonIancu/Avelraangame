namespace Data_Mapping_Containers.Dtos;

/// <summary>
/// Ratios should not exceed 100
/// </summary>
public class QuestRatio
{
    public int Diplomacy { get; set; }
    public int DiplomacyMultiplier { get; set; }

    public int Utilitarian { get; set; }
    public int UtilitarianMultiplier { get; set; }

    public int Overcome { get; set; }
    public int OvercomeMultiplier { get; set; }

    public int Fight { get; set; }

    public string RewardType { get; set; }
}
