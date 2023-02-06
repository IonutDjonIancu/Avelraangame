namespace Data_Mapping_Containers.Dtos;

public class CharacterUpdate
{
    public string CharacterId { get; set; }
    public bool HasChangesToName { get; set; }
    public string Name { get; set; }

    public bool HasChangesToStats { get; set; }
    public bool HasChangesToSkills { get; set; }
    public CharacterPaperDoll? Doll { get; set; }
}
