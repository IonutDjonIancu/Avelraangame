using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class NpcLogicDelegator
{
    private readonly NpcAttributesLogic attributesLogic;
    private readonly NpcBelongingsLogic belongingsLogic;
    private readonly NpcPaperdollLogic paperdollLogic;

    private NpcLogicDelegator() { }
    internal NpcLogicDelegator(
        IDiceRollService diceService,
        IItemService itemService,
        ICharacterService characterService)
    {
        attributesLogic = new NpcAttributesLogic(diceService);
        belongingsLogic = new NpcBelongingsLogic(diceService, itemService);
        paperdollLogic = new NpcPaperdollLogic(characterService);
    }

    internal NpcPaperdoll GenerateNpc(NpcInfo npcInfo)
    {
        var npcCharacter = new Character();
            
        attributesLogic.SetNpcCharacterSheet(npcInfo, npcCharacter);
        belongingsLogic.SetNpcInventory(npcCharacter);

        var npcPaperdoll = paperdollLogic.CalculateNpcPaperdoll(npcInfo, npcCharacter);

        return npcPaperdoll;
    }
}

