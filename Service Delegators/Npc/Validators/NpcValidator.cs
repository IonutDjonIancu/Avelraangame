#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators.Validators;

internal class NpcValidator : ValidatorBase
{
    internal NpcValidator(Snapshot snapshot) 
        : base(snapshot)
    { }

    public void ValidateNpcOnGenerate(NpcInfo npcInfo)
    {
        ValidateObject(npcInfo);

        if (!GameplayLore.Quests.Difficulty.All.Contains(npcInfo.Difficulty)) throw new Exception("Difficulty not found or in wrong format.");
        if (!CharactersLore.Tradition.All.Contains(npcInfo.Tradition)) throw new Exception("Tradition not found or in wrong format.");
        if (!GameplayLore.Subregions.All.Contains(npcInfo.Subregion)) throw new Exception("Wrong npc subregion or in wrong format.");

        ValidateObject(npcInfo.StatsMin);
        ValidateObject(npcInfo.StatsMax);
        ValidateObject(npcInfo.AssetsMin);
        ValidateObject(npcInfo.AssetsMax);
        ValidateObject(npcInfo.SkillsMin);
        ValidateObject(npcInfo.SkillsMax);
    }
}

#pragma warning restore CA1822 // Mark members as static