namespace Data_Mapping_Containers.Dtos;

public class Character : CharacterBase, ICharacter
{
    public List<NpcCharacter> Mercenaries { get; set; } = new();
}
