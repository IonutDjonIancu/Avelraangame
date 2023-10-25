namespace Data_Mapping_Containers.Dtos;

public class BattleboardActor
{
    public CharacterIdentity MainActor { get; set; }
    
    public string BattleboardIdToJoin { get; set; }
    public bool WantsToBeGood { get; set; }

    public string TargetId { get; set; }
}
