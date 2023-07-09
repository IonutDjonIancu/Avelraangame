namespace Data_Mapping_Containers.Dtos;

public class Character : CharacterBase
{
    public List<NpcCharacter> Henchmen { get; set; } = new();
    public List<Character> PartyMembers { get; set; } = new();
}
