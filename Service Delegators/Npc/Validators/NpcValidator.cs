using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Validators;
using Persistance_Manager;

namespace Service_Delegators.Validators;

public class NpcValidator : ValidatorBase
{
    private readonly IDatabaseManager dbm;

    public NpcValidator(IDatabaseManager manager)
    {
        dbm = manager;
    }

    public void ValidateNpcOnGenerate(NpcInfo npcInfo)
    {
        if (!QuestsLore.Difficulty.All.Contains(npcInfo.Difficulty)) Throw("Difficulty not found or in wrong format.");
        if (!CharactersLore.Heritage.All.Contains(npcInfo.Heritage)) Throw("Heritage not found or in wrong format.");
        if (!QuestsLore.NpcType.All.Contains(npcInfo.NpcType)) Throw("Wrong npc type or in wrong format.");

        ValidateObject(npcInfo.StatsMin);
        ValidateObject(npcInfo.StatsMax);
        ValidateObject(npcInfo.AssetsMin);
        ValidateObject(npcInfo.AssetsMax);
        ValidateObject(npcInfo.SkillsMin);
        ValidateObject(npcInfo.SkillsMax);
    }
}
