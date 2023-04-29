using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;
using Persistance_Manager;

namespace Service_Delegators;

internal class NpcAttributesLogic
{
    private readonly IDatabaseManager dbm;
    private readonly IDiceRollService dice;

    internal NpcAttributesLogic(
        IDatabaseManager databaseManager,
        IDiceRollService dice)
    {
        dbm = databaseManager;
        this.dice = dice;
    }

    internal void SetNpcCharacterSheet(NpcInfo npcInfo, Character npcChar)
    {
        // will have to account for all other races and cultures as well
        npcChar.IsAlive = true;

        npcChar.Identity = new CharacterIdentity
        {
            Id = Guid.NewGuid().ToString(),
            Name = $"Npc-Humanoid-{DateTime.Now.Millisecond}",
            PlayerId = Guid.Empty.ToString()
        };

        npcChar.Info = new CharacterInfo
        {
            EntityLevel = RandomizeEntityLevel(dice),

            Race = npcInfo.NpcType,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Class = CharactersLore.Classes.Warrior,
            Heritage = npcInfo.Heritage
        };

        npcChar.Sheet = new CharacterSheet()
        {
            Stats = CalculateNpcStats(npcInfo),
            Assets = CalculateNpcAssets(npcInfo),
            Skills = CalculateNpcSkills(npcInfo),
        };

        npcChar.Inventory = new CharacterInventory();
    }

    #region private methods
    private CharacterSkills CalculateNpcSkills(NpcInfo npcInfo)
    {
        var skills = new CharacterSkills();
        var skillsFactor = dbm.Snapshot.Rulebook.Npcs.SkillsDifficultyFactor;

        var comMin = npcInfo.SkillsMin.Combat - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Combat - skillsFactor;
        var comMax = npcInfo.SkillsMax.Combat + skillsFactor;
        skills.Combat = dice.Roll_dXY(comMin, comMax);

        var arcMin = npcInfo.SkillsMin.Arcane - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Arcane - skillsFactor;
        var arcMax = npcInfo.SkillsMax.Arcane + skillsFactor;
        skills.Arcane = dice.Roll_dXY(arcMin, arcMax);

        var psiMin = npcInfo.SkillsMin.Psionics - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Psionics - skillsFactor;
        var psiMax = npcInfo.SkillsMax.Psionics + skillsFactor;
        skills.Psionics = dice.Roll_dXY(psiMin, psiMax);

        var hidMin = npcInfo.SkillsMin.Hide - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Hide - skillsFactor;
        var hidMax = npcInfo.SkillsMax.Hide + skillsFactor;
        skills.Hide = dice.Roll_dXY(hidMin, hidMax);

        var traMin = npcInfo.SkillsMin.Traps - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Traps - skillsFactor;
        var traMax = npcInfo.SkillsMax.Traps + skillsFactor;
        skills.Traps = dice.Roll_dXY(traMin, traMax);

        var tacMin = npcInfo.SkillsMin.Tactics - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Tactics - skillsFactor;
        var tacMax = npcInfo.SkillsMax.Tactics + skillsFactor;
        skills.Tactics = dice.Roll_dXY(tacMin, tacMax);

        var socMin = npcInfo.SkillsMin.Social - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Social - skillsFactor;
        var socMax = npcInfo.SkillsMax.Social + skillsFactor;
        skills.Social = dice.Roll_dXY(socMin, socMax);

        var apoMin = npcInfo.SkillsMin.Apothecary - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Apothecary - skillsFactor;
        var apoMax = npcInfo.SkillsMax.Apothecary + skillsFactor;
        skills.Apothecary = dice.Roll_dXY(apoMin, apoMax);

        var trvMin = npcInfo.SkillsMin.Travel - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Travel - skillsFactor;
        var trvMax = npcInfo.SkillsMax.Travel + skillsFactor;
        skills.Travel = dice.Roll_dXY(trvMin, trvMax);

        var saiMin = npcInfo.SkillsMin.Sail - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Sail - skillsFactor;
        var saiMax = npcInfo.SkillsMax.Sail + skillsFactor;
        skills.Sail = dice.Roll_dXY(saiMin, saiMax);

        return skills;
    }

    private CharacterAssets CalculateNpcAssets(NpcInfo npcInfo)
    {
        var assets = new CharacterAssets();
        var assetsFactor = dbm.Snapshot.Rulebook.Npcs.AssetsDifficultyFactor;

        var resMin = npcInfo.AssetsMin.Resolve - assetsFactor < 0 ? 5 : npcInfo.AssetsMin.Resolve - assetsFactor;
        var resMax = npcInfo.AssetsMax.Resolve + assetsFactor;
        assets.Resolve = dice.Roll_dXY(resMin, resMax);

        var harMin = npcInfo.AssetsMin.Harm - assetsFactor < 0 ? 0 : npcInfo.AssetsMin.Harm - assetsFactor;
        var harMax = npcInfo.AssetsMax.Harm + assetsFactor;
        assets.Harm = dice.Roll_dXY(harMin, harMax);

        var spoMin = npcInfo.AssetsMin.Spot - assetsFactor < 0 ? 0 : npcInfo.AssetsMin.Spot - assetsFactor;
        var spoMax = npcInfo.AssetsMax.Spot + assetsFactor;
        assets.Spot = dice.Roll_dXY(spoMin, spoMax);

        var defMin = 0;
        var defMax = 10;
        assets.Defense = dice.Roll_dXY(defMin, defMax);

        var purMin = 0;
        var purMax = 25;
        assets.Purge = dice.Roll_dXY(purMin, purMax);

        var manMin = npcInfo.AssetsMin.Mana - assetsFactor < 0 ? 0 : npcInfo.AssetsMin.Mana - assetsFactor;
        var manMax = npcInfo.AssetsMax.Mana + assetsFactor;
        assets.Mana = dice.Roll_dXY(manMin, manMax);

        return assets;
    }

    private CharacterStats CalculateNpcStats(NpcInfo npcInfo)
    {
        var stats = new CharacterStats();
        var statsFactor = dbm.Snapshot.Rulebook.Npcs.StatsDifficultyFactor;

        var strMin = npcInfo.StatsMin.Strength - statsFactor < 0 ? 10 : npcInfo.StatsMin.Strength - statsFactor;
        var strMax = npcInfo.StatsMax.Strength + statsFactor;
        stats.Strength = dice.Roll_dXY(strMin, strMax);

        var conMin = npcInfo.StatsMin.Constitution - statsFactor < 0 ? 10 : npcInfo.StatsMin.Constitution - statsFactor;
        var conMax = npcInfo.StatsMax.Constitution + statsFactor;
        stats.Constitution = dice.Roll_dXY(conMin, conMax);

        var agiMin = npcInfo.StatsMin.Agility - statsFactor < 0 ? 10 : npcInfo.StatsMin.Agility - statsFactor;
        var agiMax = npcInfo.StatsMax.Agility + statsFactor;
        stats.Agility = dice.Roll_dXY(agiMin, agiMax);

        var wilMin = npcInfo.StatsMin.Willpower - statsFactor < 0 ? 10 : npcInfo.StatsMin.Willpower - statsFactor;
        var wilMax = npcInfo.StatsMax.Willpower + statsFactor;
        stats.Willpower = dice.Roll_dXY(wilMin, wilMax);

        var perMin = npcInfo.StatsMin.Perception - statsFactor < 0 ? 10 : npcInfo.StatsMin.Perception - statsFactor;
        var perMax = npcInfo.StatsMax.Perception + statsFactor;
        stats.Perception = dice.Roll_dXY(perMin, perMax);

        var absMin = npcInfo.StatsMin.Abstract - statsFactor < 0 ? 10 : npcInfo.StatsMin.Abstract - statsFactor;
        var absMax = npcInfo.StatsMax.Abstract + statsFactor;
        stats.Abstract = dice.Roll_dXY(absMin, absMax);

        return stats;
    }

    private static int RandomizeEntityLevel(IDiceRollService dice)
    {
        var roll = dice.Roll_d20(true);

        if      (roll >= 100)   return 6;
        else if (roll >= 80)    return 5;
        else if (roll >= 60)    return 4;
        else if (roll >= 40)    return 3;
        else if (roll >= 20)    return 2;
        else  /*(roll >= 1)*/   return 1;
    }
    #endregion

}
