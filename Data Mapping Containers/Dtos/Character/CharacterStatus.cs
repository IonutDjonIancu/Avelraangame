namespace Data_Mapping_Containers.Dtos;

public class CharacterStatus
{
    public bool IsLockedForModify { get; set; }

    public bool IsInQuest { get; set; }
    public string QuestId { get; set; }

    public bool IsInArena { get; set; }
    public string ArenaId { get; set; }

    public bool IsInStory { get; set; }
    public string StoryId { get; set; }
}
