namespace Data_Mapping_Containers.Dtos;

public class NpcInfo
{
    public string Difficulty { get; set; }

    public CharacterStats StatsMin { get; set; }
    public CharacterStats StatsMax { get; set; }

    public CharacterAssets AssetsMin { get; set; }
    public CharacterAssets AssetsMax { get; set; }
    
    public CharacterSkills SkillsMin { get; set; }
    public CharacterSkills SkillsMax { get; set; }
}
