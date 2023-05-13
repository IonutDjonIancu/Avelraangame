using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

public class PartyService
{
    private readonly IDatabaseManager dbm;
    private readonly ICharacterService characterService;

    public PartyService(
        IDatabaseManager databaseManager,
        ICharacterService characterService)
    {
        dbm = databaseManager;
        this.characterService = characterService;
    }

    public Party CreateParty(string characterId, string playerId)
    {
        // validate stuff

        characterService.UpdateCharacter();


        return new Party()
        {
            Id = Guid.NewGuid().ToString(),
            CharacterIds = new List<string> { characterId },
        };
    }


    



    
}
