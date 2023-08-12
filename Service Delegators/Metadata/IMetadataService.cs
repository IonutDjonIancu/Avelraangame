using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IMetadataService
{
    Players GetPlayers();

    List<string> GetRaces();
    List<string> GetCultures();
    List<string> GetClasses();
    List<SpecialSkill> GetHeroicTraits();
    
    List<string> GetAvelraanRegions();
}