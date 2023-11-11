namespace Data_Mapping_Containers.Dtos;

public class CharacterTrade
{
    public CharacterIdentity CharacterIdentity { get; set; }
    public string ItemId { get; set; }
    public bool IsToBuy { get; set; }
    public int Amount { get; set; }
    public CharacterIdentity TargetIdentity { get; set; }
}
