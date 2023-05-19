using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;

namespace Service_Delegators;

internal class NpcAttributesLogic
{
    private readonly IDatabaseService dbs;
    private readonly IDiceRollService dice;

    private NpcAttributesLogic() { }
    internal NpcAttributesLogic(
        IDatabaseService databaseService,
        IDiceRollService diceService)
    {
        dbs = databaseService;
        dice = diceService;
    }

    internal void SetNpcCharacterSheet(NpcInfo npcInfo, Character npcChar)
    {
        npcChar.Identity.Id = Guid.NewGuid().ToString();
        npcChar.Identity.PlayerId = Guid.Empty.ToString();

        SetNpcCharacterInfo(npcInfo, npcChar);

        npcChar.Sheet = new CharacterSheet()
        {
            Stats = CalculateNpcStats(npcInfo),
            Assets = CalculateNpcAssets(npcInfo),
            Skills = CalculateNpcSkills(npcInfo),
        };

        npcChar.Inventory = new CharacterInventory();
    }

    #region private methods
    private void SetNpcCharacterInfo(NpcInfo npcInfo, Character npcChar)
    {
        npcChar.Info.EntityLevel = RandomizeEntityLevel(dice);
        npcChar.Info.DateOfBirth = DateTime.Now.ToShortDateString();
        npcChar.Info.IsAlive = true;
        npcChar.Info.IsInParty = false;
        npcChar.Info.Name = $"Npc-Humanoid-{DateTime.Now.Millisecond}";
        npcChar.Info.Fame = "Not much is know about this one.";
        npcChar.Info.Wealth = dice.Roll_d100(true);
        npcChar.Info.Origins = GetOriginsByRegion(npcInfo);
    }

    private CharacterOrigins GetOriginsByRegion(NpcInfo info)
    {
        var origins = new CharacterOrigins
        {
            Culture = "npc culture",
            Heritage = RulebookLore.Regions.Eastern.Contains(info.Region) ? CharactersLore.Heritage.Traditional : CharactersLore.Heritage.Martial,
            Race = SetNpcRace(info)
        };

        origins.Class = SetNpcClass(origins.Race);

        return origins;
    }

    private string SetNpcRace(NpcInfo info)
    {
        var animalsFoundIn = new List<string>
        {
            RulebookLore.Regions.WestDragonmaw.Northeim,
            RulebookLore.Regions.WestDragonmaw.Midheim,
            RulebookLore.Regions.WestDragonmaw.Southeim,

            RulebookLore.Regions.EastDragonmaw.VargasStand,
            RulebookLore.Regions.EastDragonmaw.Longshore,
            RulebookLore.Regions.EastDragonmaw.Farlindor,
            
            RulebookLore.Regions.Hyperborea.FrozenWastes,
            RulebookLore.Regions.Hyperborea.Brimland,
            
            RulebookLore.Regions.ThreeSeas.Endar,
            RulebookLore.Regions.ThreeSeas.TwinVines,
            RulebookLore.Regions.ThreeSeas.Stormbork,
            RulebookLore.Regions.ThreeSeas.Calvinia
        };

        var monstersFoundIn = new List<string>
        {
            RulebookLore.Regions.WestDragonmaw.Northeim,
            RulebookLore.Regions.WestDragonmaw.Midheim,
            RulebookLore.Regions.WestDragonmaw.Southeim,

            RulebookLore.Regions.EastDragonmaw.VargasStand,
            RulebookLore.Regions.EastDragonmaw.Longshore,
            RulebookLore.Regions.EastDragonmaw.Farlindor,
            RulebookLore.Regions.EastDragonmaw.PelRavan,
            
            RulebookLore.Regions.Hyperborea.FrozenWastes,
            RulebookLore.Regions.Hyperborea.Brimland,
            RulebookLore.Regions.Hyperborea.Ryxos,
            
            RulebookLore.Regions.ThreeSeas.Endar,
            RulebookLore.Regions.ThreeSeas.TwinVines,
            RulebookLore.Regions.ThreeSeas.Stormbork,
            RulebookLore.Regions.ThreeSeas.Calvinia,

            RulebookLore.Regions.Eversun.AjJahra
        };

        var humanoidsFoundIn = new List<string>
        {
            RulebookLore.Regions.WestDragonmaw.Northeim,
            RulebookLore.Regions.WestDragonmaw.Midheim,
            RulebookLore.Regions.WestDragonmaw.Southeim,

            RulebookLore.Regions.EastDragonmaw.VargasStand,
            RulebookLore.Regions.EastDragonmaw.Longshore,
            RulebookLore.Regions.EastDragonmaw.Farlindor,
            RulebookLore.Regions.EastDragonmaw.PelRavan,

            RulebookLore.Regions.Hyperborea.FrozenWastes,
            RulebookLore.Regions.Hyperborea.Brimland,
            RulebookLore.Regions.Hyperborea.Ryxos,

            RulebookLore.Regions.ThreeSeas.Endar,
            RulebookLore.Regions.ThreeSeas.TwinVines,
            RulebookLore.Regions.ThreeSeas.Stormbork,
            RulebookLore.Regions.ThreeSeas.Calvinia,

            RulebookLore.Regions.Eversun.AjJahra,
            RulebookLore.Regions.Eversun.ShiftingPlanes,
        };

        var undeadsFoundIn = new List<string>
        {
            RulebookLore.Regions.WestDragonmaw.Northeim,

            RulebookLore.Regions.EastDragonmaw.VargasStand,
            RulebookLore.Regions.EastDragonmaw.PelRavan,

            RulebookLore.Regions.Hyperborea.FrozenWastes,
            RulebookLore.Regions.Hyperborea.Ryxos,

            RulebookLore.Regions.Eversun.AjJahra,
            RulebookLore.Regions.Eversun.ShiftingPlanes,
        };

        var fiendsFoundIn = new List<string>
        {
            RulebookLore.Regions.WestDragonmaw.Northeim,
            RulebookLore.Regions.WestDragonmaw.Midheim,

            RulebookLore.Regions.EastDragonmaw.VargasStand,
            RulebookLore.Regions.EastDragonmaw.Longshore,
            RulebookLore.Regions.EastDragonmaw.Farlindor,
            RulebookLore.Regions.EastDragonmaw.PelRavan,

            RulebookLore.Regions.Hyperborea.FrozenWastes,
            RulebookLore.Regions.Hyperborea.Ryxos,

            RulebookLore.Regions.ThreeSeas.Endar,
            RulebookLore.Regions.ThreeSeas.TwinVines,
            RulebookLore.Regions.ThreeSeas.Stormbork,
            RulebookLore.Regions.ThreeSeas.Calvinia,

            RulebookLore.Regions.Eversun.AjJahra,
            RulebookLore.Regions.Eversun.ShiftingPlanes,
            RulebookLore.Regions.Eversun.Peradin
        };

        var elementalFoundIn = new List<string>
        {
            RulebookLore.Regions.Hyperborea.Ryxos,

            RulebookLore.Regions.Eversun.ShiftingPlanes,
            RulebookLore.Regions.Eversun.Peradin
        };

        List<string> possibleRaces = new();

        if (animalsFoundIn.Contains(info.Region)) possibleRaces.Add(RulebookLore.Npcs.Races.Animal);
        if (monstersFoundIn.Contains(info.Region)) possibleRaces.Add(RulebookLore.Npcs.Races.Monster);
        if (humanoidsFoundIn.Contains(info.Region)) possibleRaces.Add(RulebookLore.Npcs.Races.Humanoid);
        if (undeadsFoundIn.Contains(info.Region)) possibleRaces.Add(RulebookLore.Npcs.Races.Undead);
        if (fiendsFoundIn.Contains(info.Region)) possibleRaces.Add(RulebookLore.Npcs.Races.Fiend);
        if (elementalFoundIn.Contains(info.Region)) possibleRaces.Add(RulebookLore.Npcs.Races.Elemental);

        var index = dice.Roll_dXY(0, possibleRaces.Count - 1);
        return possibleRaces[index];
    }

    private string SetNpcClass(string race)
    {

    }


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
