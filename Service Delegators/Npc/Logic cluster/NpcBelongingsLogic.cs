using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;

namespace Service_Delegators;

internal class NpcBelongingsLogic
{
    private readonly IItemService itemService;

    internal NpcBelongingsLogic(IItemService itemService)
    {
        this.itemService = itemService;
    }

    internal void SetNpcInventory(NpcInfo npcInfo, CharacterInventory inventory)
    {
        //TODO: this needs to be refactored to make more sense, i.e: fiends should only have tainted items, but not necessarily enchanted items
        //TODO: monsters depending on the difficulty factor have from no up to 5 items, animals have no items, humanoids have items depending on location, demon have no weapons, elemental have no weapons - but think of other ways of getting loot from these

        if (npcInfo.NpcType == QuestsLore.NpcType.Animal) return;
        if (npcInfo.NpcType == QuestsLore.NpcType.Elemental) return;

        inventory.Mainhand = itemService.GenerateSpecificItem("weapon", "sword");
        if (npcInfo.NpcType == QuestsLore.NpcType.Monster) return;

        inventory.Head = itemService.GenerateSpecificItem("protection", "helm");
        inventory.Body = itemService.GenerateSpecificItem("protection", "armour");
        if (npcInfo.NpcType == QuestsLore.NpcType.Fiend) return;

        inventory.Shield = itemService.GenerateSpecificItem("protection", "shield");
        inventory.Ranged = itemService.GenerateSpecificItem("weapon", "bow");
    }
}
