using Data_Mapping_Containers.Dtos;
using Service_Delegators.Validators;

namespace Service_Delegators;

public class NpcService : INpcService
{
    private readonly NpcValidator validator;
    private readonly NpcLogicDelegator logic;

    public NpcService(
        IDatabaseService dataBaseService,
        IDiceRollService diceRollService,
        IItemService itemService,
        ICharacterService characterService)
    {
        validator = new NpcValidator(dataBaseService.Snapshot);
        logic = new NpcLogicDelegator(diceRollService, itemService, characterService);
    }

    public Character GenerateGoodGuyNpc(string location)
    {
        validator.ValidateLocation(location);
        return logic.GenerateNpcCharacter(location, true);
    }

    public Character GenerateBadGuyNpc(string location)
    {
        validator.ValidateLocation(location);
        return logic.GenerateNpcCharacter(location, false);
    }
}
