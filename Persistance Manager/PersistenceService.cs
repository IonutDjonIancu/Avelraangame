using Data_Mapping_Containers.Dtos;

namespace Persistance_Manager;

public interface IPersistenceService
{
}

public class PersistenceService : IPersistenceService
{
    private readonly AppSettings appSettings;

    public PersistenceService(AppSettings appSettings)
    {
        this.appSettings = appSettings;
    }




}
