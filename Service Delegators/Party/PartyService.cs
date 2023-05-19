using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class PartyService
{
    private readonly IDatabaseService dbs;
    private readonly ICharacterService charService;

    public PartyService(
        IDatabaseService databaseService,
        ICharacterService characterService)
    {
        dbs = databaseService;
        charService = characterService;
    }

    public Party CreateParty()
    {
        return new Party()
        {
            Id = Guid.NewGuid().ToString()
        };
    }


    



    
}
