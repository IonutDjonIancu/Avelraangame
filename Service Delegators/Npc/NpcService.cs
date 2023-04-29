using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using Service_Delegators.Validators;

namespace Service_Delegators;

public class NpcService : INpcService
{
    private readonly NpcValidator validator;
    private readonly NpcLogicDelegator logic;

    public NpcService(
        IDatabaseManager databaseManager,
        IDiceRollService diceRollService,
        IItemService itemService,
        ICharacterService characterService)
    {
        validator = new NpcValidator(databaseManager);
        logic = new NpcLogicDelegator(databaseManager, diceRollService, itemService, characterService);
    }

    public NpcPaperdoll GenerateNpc(NpcInfo npcInfo)
    {
        validator.ValidateNpcOnGenerate(npcInfo);

        return logic.GenerateNpc(npcInfo);
    }
}
