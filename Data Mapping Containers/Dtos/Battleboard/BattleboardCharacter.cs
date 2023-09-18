namespace Data_Mapping_Containers.Dtos;

public class BattleboardCharacter
{
    public CharacterIdentity CharacterIdentity { get; set; }
    
    public string BattleboardIdToJoin { get; set; }
    public bool WantsToBeGood { get; set; }

    public string FirstTargetId { get; set; }
    public string SecondTargetId { get; set; }
}
