#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators.Validators;

internal class NpcValidator : ValidatorBase
{
    public void ValidateNpcOnGenerate(NpcInfo npcInfo)
    {
        ValidateObject(npcInfo);

        if (!RulebookLore.Quests.Difficulty.All.Contains(npcInfo.Difficulty)) Throw("Difficulty not found or in wrong format.");
        if (!CharactersLore.Heritage.All.Contains(npcInfo.Heritage)) Throw("Heritage not found or in wrong format.");
        if (!RulebookLore.Regions.All.Contains(npcInfo.Region)) Throw("Wrong npc region or in wrong format.");

        ValidateObject(npcInfo.StatsMin);
        ValidateObject(npcInfo.StatsMax);
        ValidateObject(npcInfo.AssetsMin);
        ValidateObject(npcInfo.AssetsMax);
        ValidateObject(npcInfo.SkillsMin);
        ValidateObject(npcInfo.SkillsMax);
    }
}

#pragma warning restore CA1822 // Mark members as static