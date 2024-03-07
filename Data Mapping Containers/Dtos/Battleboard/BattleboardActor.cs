namespace Data_Mapping_Containers.Dtos;

public class BattleboardActor
{
    public CharacterIdentity MainActor { get; set; }
    
    public bool? WantsToBeGood { get; set; }

    public string? BattleboardId { get; set; }
    public string? TargetId { get; set; }
    public string? QuestId { get; set; }
    public bool? IsForArena { get; set; }
}
