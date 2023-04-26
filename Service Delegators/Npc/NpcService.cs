#pragma warning disable CS8604 // Possible null reference argument.

using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;
using Persistance_Manager;
using System;

namespace Service_Delegators;

public class NpcService : INpcService
{
    private readonly IDatabaseManager dbm;
    private readonly IItemService itemService;
    private readonly ICharacterService characterService;
    private readonly IDiceRollService dice;

    public NpcService(
        IDatabaseManager databaseManager,
        IDiceRollService diceRollService,
        IItemService itemService,
        ICharacterService characterService)
    {
        dbm = databaseManager;
        this.itemService = itemService;
        this.characterService = characterService;
        dice = diceRollService;
    }

    public NpcPaperdoll GenerateNpc(NpcInfo info)
    {
        // must be moved to validator
        if (!QuestsLore.Difficulty.All.Contains(info.Difficulty)) throw new Exception("Wrong difficulty level.");

        var npcCharacter = CreateNpcCharacter();
        var npc = new NpcPaperdoll();

        SetNpcStats(info, npcCharacter.Sheet);
        SetNpcAssets(info, npcCharacter.Sheet);
        SetNpcSkills(info, npcCharacter.Sheet);
        SetNpcInventory(info, npcCharacter.Inventory);
        SetNpcMoney(npc);

        npc.Paperdoll = characterService.CalculateCharacterPaperdollForNpc(npcCharacter);
        npc.Items = npcCharacter.Inventory.GetAllEquipedItems();

        SetDifficultyFactor(info, npc);

        return npc;
    }

    #region private methods

    private static void SetDifficultyFactor(NpcInfo info, NpcPaperdoll npc)
    {
        var factor = 1.0m;
        if (IsEasy(info.Difficulty)) factor = 0.25m;
        if (IsMedi(info.Difficulty)) factor = 0.5m;
        if (IsHard(info.Difficulty)) factor = 2.0m;

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
        npc.Paperdoll.Assets.Defense = (int)Math.Floor(npc.Paperdoll.Assets.Defense * factor);
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
        npc.Money = (int)Math.Floor(npc.Money * factor);
    }

    private void SetNpcInventory(NpcInfo info, CharacterInventory inventory)
    {
        inventory.Head = itemService.GenerateSpecificItem("protection", "helm");
        inventory.Body = itemService.GenerateSpecificItem("protection", "armour");
        inventory.Shield = itemService.GenerateSpecificItem("protection", "shield");
        inventory.Mainhand = itemService.GenerateSpecificItem("weapon", "sword");
        inventory.Ranged = itemService.GenerateSpecificItem("weapon", "bow");
    }

    private void SetNpcSkills(NpcInfo info, CharacterSheet sheet)
    {
        var skillsFactor = dbm.Snapshot.Rulebook.Npcs.SkillsDifficultyFactor;

        var comMin = info.SkillsMin.Combat - skillsFactor < 0 ? 10 : info.SkillsMin.Combat - skillsFactor;
        var comMax = info.SkillsMax.Combat + skillsFactor;
        sheet.Skills.Combat = dice.Roll_dXY(comMin, comMax);

        var arcMin = info.SkillsMin.Arcane - skillsFactor < 0 ? 10 : info.SkillsMin.Arcane - skillsFactor;
        var arcMax = info.SkillsMax.Arcane + skillsFactor;
        sheet.Skills.Arcane = dice.Roll_dXY(arcMin, arcMax);

        var psiMin = info.SkillsMin.Psionics - skillsFactor < 0 ? 10 : info.SkillsMin.Psionics - skillsFactor;
        var psiMax = info.SkillsMax.Psionics + skillsFactor;
        sheet.Skills.Psionics = dice.Roll_dXY(psiMin, psiMax);

        var hidMin = info.SkillsMin.Hide - skillsFactor < 0 ? 10 : info.SkillsMin.Hide - skillsFactor;
        var hidMax = info.SkillsMax.Hide + skillsFactor;
        sheet.Skills.Hide = dice.Roll_dXY(hidMin, hidMax);

        var traMin = info.SkillsMin.Traps - skillsFactor < 0 ? 10 : info.SkillsMin.Traps - skillsFactor;
        var traMax = info.SkillsMax.Traps + skillsFactor;
        sheet.Skills.Traps = dice.Roll_dXY(traMin, traMax);

        var tacMin = info.SkillsMin.Tactics - skillsFactor < 0 ? 10 : info.SkillsMin.Tactics - skillsFactor;
        var tacMax = info.SkillsMax.Tactics + skillsFactor;
        sheet.Skills.Tactics = dice.Roll_dXY(tacMin, tacMax);

        var socMin = info.SkillsMin.Social - skillsFactor < 0 ? 10 : info.SkillsMin.Social - skillsFactor;
        var socMax = info.SkillsMax.Social + skillsFactor;
        sheet.Skills.Social = dice.Roll_dXY(socMin, socMax);

        var apoMin = info.SkillsMin.Apothecary - skillsFactor < 0 ? 10 : info.SkillsMin.Apothecary - skillsFactor;
        var apoMax = info.SkillsMax.Apothecary + skillsFactor;
        sheet.Skills.Apothecary = dice.Roll_dXY(apoMin, apoMax);

        var trvMin = info.SkillsMin.Travel - skillsFactor < 0 ? 10 : info.SkillsMin.Travel - skillsFactor;
        var trvMax = info.SkillsMax.Travel + skillsFactor;
        sheet.Skills.Travel = dice.Roll_dXY(trvMin, trvMax);

        var saiMin = info.SkillsMin.Sail - skillsFactor < 0 ? 10 : info.SkillsMin.Sail - skillsFactor;
        var saiMax = info.SkillsMax.Sail + skillsFactor;
        sheet.Skills.Sail = dice.Roll_dXY(saiMin, saiMax);
    }

    private void SetNpcAssets(NpcInfo info, CharacterSheet sheet)
    {
        var assetsFactor = dbm.Snapshot.Rulebook.Npcs.AssetsDifficultyFactor;
        
        var resMin = info.AssetsMin.Resolve - assetsFactor < 0 ? 50 : info.AssetsMin.Resolve - assetsFactor;
        var resMax = info.AssetsMax.Resolve + assetsFactor;
        sheet.Assets.Resolve = dice.Roll_dXY(resMin, resMax);

        var harMin = info.AssetsMin.Harm - assetsFactor < 0 ? 0 : info.AssetsMin.Harm - assetsFactor;
        var harMax = info.AssetsMax.Harm + assetsFactor;
        sheet.Assets.Harm = dice.Roll_dXY(harMin, harMax);

        var spoMin = info.AssetsMin.Spot - assetsFactor < 0 ? 0 : info.AssetsMin.Spot - assetsFactor;
        var spoMax = info.AssetsMax.Spot + assetsFactor;
        sheet.Assets.Spot = dice.Roll_dXY(spoMin, spoMax);

        var defMin = info.AssetsMin.Defense - assetsFactor < 0 ? 0 : info.AssetsMin.Defense - assetsFactor;
        var defMax = info.AssetsMax.Defense + assetsFactor > 90 ? 90 : info.AssetsMax.Defense + assetsFactor;
        sheet.Assets.Defense = dice.Roll_dXY(defMin, defMax);

        var purMin = info.AssetsMin.Purge - assetsFactor;
        var purMax = info.AssetsMax.Purge + assetsFactor > 90 ? 90 : info.AssetsMax.Purge + assetsFactor;
        sheet.Assets.Purge = dice.Roll_dXY(purMin, purMax);

        var manMin = info.AssetsMin.Mana - assetsFactor < 0 ? 0 : info.AssetsMin.Mana - assetsFactor;
        var manMax = info.AssetsMax.Mana + assetsFactor;
        sheet.Assets.Mana = dice.Roll_dXY(manMin, manMax);
    }

    private void SetNpcStats(NpcInfo info, CharacterSheet sheet)
    {
        var statsFactor = dbm.Snapshot.Rulebook.Npcs.StatsDifficultyFactor;

        var strMin = info.StatsMin.Strength - statsFactor < 0 ? 10 : info.StatsMin.Strength - statsFactor;
        var strMax = info.StatsMax.Strength + statsFactor;
        sheet.Stats.Strength = dice.Roll_dXY(strMin, strMax);

        var conMin = info.StatsMin.Constitution - statsFactor < 0 ? 10 : info.StatsMin.Constitution - statsFactor;
        var conMax = info.StatsMax.Constitution + statsFactor;
        sheet.Stats.Constitution = dice.Roll_dXY(conMin, conMax);

        var agiMin = info.StatsMin.Agility - statsFactor < 0 ? 10 : info.StatsMin.Agility - statsFactor;
        var agiMax = info.StatsMax.Agility + statsFactor;
        sheet.Stats.Agility = dice.Roll_dXY(agiMin, agiMax);

        var wilMin = info.StatsMin.Willpower - statsFactor < 0 ? 10 : info.StatsMin.Willpower - statsFactor;
        var wilMax = info.StatsMax.Willpower + statsFactor;
        sheet.Stats.Willpower = dice.Roll_dXY(wilMin, wilMax);

        var perMin = info.StatsMin.Perception - statsFactor < 0 ? 10 : info.StatsMin.Perception - statsFactor;
        var perMax = info.StatsMax.Perception + statsFactor;
        sheet.Stats.Perception = dice.Roll_dXY(perMin, perMax);

        var absMin = info.StatsMin.Abstract - statsFactor < 0 ? 10 : info.StatsMin.Abstract - statsFactor;
        var absMax = info.StatsMax.Abstract + statsFactor;
        sheet.Stats.Abstract = dice.Roll_dXY(absMin, absMax);
    }

    private void SetNpcMoney(NpcPaperdoll npc)
    {
        npc.Money =  dice.Roll_d100(true);
    }

    private static Character CreateNpcCharacter()
    {
        return new Character
        {
            Identity = new CharacterIdentity
            {
                Id = Guid.NewGuid().ToString(),
                Name = $"Npc-{DateTime.Now.Millisecond}",
                PlayerId = Guid.Empty.ToString()
            },
            Info = new CharacterInfo
            {
                EntityLevel = 1,

                Race = CharactersLore.Races.Human,
                Culture = CharactersLore.Cultures.Human.Danarian,
                Class = CharactersLore.Classes.Warrior,
                Heritage = CharactersLore.Heritage.Traditional
            },
            IsAlive = true,
            Sheet = new CharacterSheet()
            {
                Stats = new CharacterStats(),
                Assets = new CharacterAssets(),
                Skills = new CharacterSkills(),
            },
            Inventory = new CharacterInventory()
        };
    }

    private static bool IsEasy(string difficulty) => difficulty == QuestsLore.Difficulty.Easy;
    private static bool IsMedi(string difficulty) => difficulty == QuestsLore.Difficulty.Medium;
    private static bool IsNorm(string difficulty) => difficulty == QuestsLore.Difficulty.Normal;
    private static bool IsHard(string difficulty) => difficulty == QuestsLore.Difficulty.Hard;
    #endregion

}

#pragma warning restore CS8604 // Possible null reference argument.
