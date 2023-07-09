using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IMetadataService
{
    Players GetPlayers();

    List<string> GetRaces();
    List<string> GetCultures();
    List<string> GetClasses();
    List<HeroicTrait> GetHeroicTraits();
    
    List<string> GetAvelraanRegions();
}