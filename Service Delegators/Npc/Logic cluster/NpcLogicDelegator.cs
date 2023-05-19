using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class NpcLogicDelegator
{
    private readonly NpcAttributesLogic attributesLogic;
    private readonly NpcBelongingsLogic belongingsLogic;
    private readonly NpcPaperdollLogic paperdollLogic;

    private NpcLogicDelegator() { }
    internal NpcLogicDelegator(
        IDatabaseService databaseService,
        IDiceRollService diceService,
        IItemService itemService,
        ICharacterService characterService)
    {
        attributesLogic = new NpcAttributesLogic(databaseService, diceService);
        belongingsLogic = new NpcBelongingsLogic(itemService);
        paperdollLogic = new NpcPaperdollLogic(diceService, characterService);
    }

    internal NpcPaperdoll GenerateNpc(NpcInfo npcInfo)
    {
        var npcCharacter = new Character();
            
        attributesLogic.SetNpcCharacterSheet(npcInfo, npcCharacter);
        belongingsLogic.SetNpcInventory(npcInfo, npcCharacter.Inventory);

        var npcPaperdoll = paperdollLogic.GetNpcPaperdoll(npcInfo, npcCharacter);

        return npcPaperdoll;
    }
}

