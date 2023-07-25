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

    public NpcCharacter GenerateBadGuyNpc(Position position, int effortUpper)
    {
        validator.ValidatePosition(position);
        return logic.GenerateBadGuyNpcCharacter(position, effortUpper);
    }

    public NpcCharacter GenerateGoodGuyNpc(Position position, int effortUpper)
    {
        validator.ValidatePosition(position);
        return logic.GenerateGoodGuyNpcCharacter(position, effortUpper);
    }
}
