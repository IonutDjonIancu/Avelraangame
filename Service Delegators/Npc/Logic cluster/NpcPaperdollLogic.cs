﻿#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class NpcPaperdollLogic
{
    private readonly IDiceRollService diceService;
    private readonly ICharacterService characterService;

    internal NpcPaperdollLogic(
        IDiceRollService diceService,
        ICharacterService characterService)
    {
        this.diceService = diceService;
        this.characterService = characterService;
    }

    internal NpcPaperdoll GetNpcPaperdoll(NpcInfo npcInfo, Character npcCharacter)
    {
        var npcPaperdoll = new NpcPaperdoll();

        _ = npcInfo.NpcType != QuestsLore.NpcType.Humanoid ? npcPaperdoll.Money = 0 : npcPaperdoll.Money = diceService.Roll_d100(true);

        npcPaperdoll.Items = npcCharacter.Inventory.GetAllEquipedItems();
        npcPaperdoll.Paperdoll = characterService.CalculateCharacterPaperdoll(npcCharacter);

        ApplyDifficultyFactor(npcInfo, npcPaperdoll);

        return npcPaperdoll;
    }

    #region private methods
    private static void ApplyDifficultyFactor(NpcInfo info, NpcPaperdoll npc)
    {
        var factor = 1.0m;
        if (info.Difficulty == QuestsLore.Difficulty.Easy) factor = 0.25m;
        if (info.Difficulty == QuestsLore.Difficulty.Medium) factor = 0.5m;
        if (info.Difficulty == QuestsLore.Difficulty.Hard) factor = 2.0m;

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
        npc.Paperdoll.Assets.Purge = (int)Math.Floor(npc.Paperdoll.Assets.Purge * factor) >= 100 ? 100 : npc.Paperdoll.Assets.Purge = (int)Math.Floor(npc.Paperdoll.Assets.Purge * factor);
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
        npc.Money = (int)Math.Floor(npc.Money * factor);
    }
    #endregion
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.