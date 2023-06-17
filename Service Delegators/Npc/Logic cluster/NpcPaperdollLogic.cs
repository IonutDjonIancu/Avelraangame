using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class NpcPaperdollLogic
{
    private readonly ICharacterService characterService;

    private NpcPaperdollLogic() { }
    internal NpcPaperdollLogic(ICharacterService characterService)
    {
        this.characterService = characterService;
    }

    internal NpcPaperdoll CalculateNpcPaperdoll(NpcInfo npcInfo, Character npcCharacter)
    {
        var npcPaperdoll = new NpcPaperdoll
        {
            Origins = npcCharacter.Info.Origins,
            Items = npcCharacter.Inventory.GetAllEquipedItems(),
            Paperdoll = characterService.CalculatePaperdoll(npcCharacter),
            Wealth = npcCharacter.Info.Wealth
        };

        ApplyDifficultyFactor(npcInfo, npcPaperdoll);

        return npcPaperdoll;
    }

    #region private methods
    private static void ApplyDifficultyFactor(NpcInfo info, NpcPaperdoll npc)
    {
        var factor = 1.0m; // set to Normal
        if (info.Difficulty == RulebookLore.Gameplay.Quests.Difficulty.Easy) factor = 0.25m;
        if (info.Difficulty == RulebookLore.Gameplay.Quests.Difficulty.Medium) factor = 0.5m;
        if (info.Difficulty == RulebookLore.Gameplay.Quests.Difficulty.Hard) factor = 2.0m;

        // stats
        npc.Paperdoll.Stats.Strength = (int)Math.Floor(npc.Paperdoll.Stats.Strength * factor);
        npc.Paperdoll.Stats.Constitution = (int)Math.Floor(npc.Paperdoll.Stats.Constitution * factor);
        npc.Paperdoll.Stats.Agility = (int)Math.Floor(npc.Paperdoll.Stats.Agility * factor);
        npc.Paperdoll.Stats.Willpower = (int)Math.Floor(npc.Paperdoll.Stats.Willpower * factor);
        npc.Paperdoll.Stats.Perception = (int)Math.Floor(npc.Paperdoll.Stats.Perception * factor);
        npc.Paperdoll.Stats.Abstract = (int)Math.Floor(npc.Paperdoll.Stats.Abstract * factor);

        // assets
        npc.Paperdoll.Assets.Resolve = (int)Math.Floor(npc.Paperdoll.Assets.Resolve * factor);
        npc.Paperdoll.Assets.Harm = (int)Math.Floor(npc.Paperdoll.Assets.Harm * factor);
        npc.Paperdoll.Assets.Spot = (int)Math.Floor(npc.Paperdoll.Assets.Spot * factor);
        npc.Paperdoll.Assets.Defense = (int)Math.Floor(npc.Paperdoll.Assets.Defense * factor) >= 90 ? 90 : (int)Math.Floor(npc.Paperdoll.Assets.Defense * factor);
        npc.Paperdoll.Assets.Purge = (int)Math.Floor(npc.Paperdoll.Assets.Purge * factor);
        npc.Paperdoll.Assets.Mana = (int)Math.Floor(npc.Paperdoll.Assets.Mana * factor);

        // skills
        npc.Paperdoll.Skills.Combat = (int)Math.Floor(npc.Paperdoll.Skills.Combat * factor);
        npc.Paperdoll.Skills.Arcane = (int)Math.Floor(npc.Paperdoll.Skills.Arcane * factor);
        npc.Paperdoll.Skills.Psionics = (int)Math.Floor(npc.Paperdoll.Skills.Psionics * factor);
        npc.Paperdoll.Skills.Hide = (int)Math.Floor(npc.Paperdoll.Skills.Hide * factor);
        npc.Paperdoll.Skills.Traps = (int)Math.Floor(npc.Paperdoll.Skills.Traps * factor);
        npc.Paperdoll.Skills.Tactics = (int)Math.Floor(npc.Paperdoll.Skills.Tactics * factor);
        npc.Paperdoll.Skills.Social = (int)Math.Floor(npc.Paperdoll.Skills.Social * factor);
        npc.Paperdoll.Skills.Apothecary = (int)Math.Floor(npc.Paperdoll.Skills.Apothecary * factor);
        npc.Paperdoll.Skills.Travel = (int)Math.Floor(npc.Paperdoll.Skills.Travel * factor);
        npc.Paperdoll.Skills.Sail = (int)Math.Floor(npc.Paperdoll.Skills.Sail * factor);

        // money
        npc.Wealth = (int)Math.Floor(npc.Wealth * factor);

        // action tokens
        npc.Paperdoll.ActionTokens = (int)Math.Floor(npc.Paperdoll.ActionTokens * factor) <= 1 ? 1 : (int)Math.Floor(npc.Paperdoll.ActionTokens * factor);
    }
    #endregion
}
