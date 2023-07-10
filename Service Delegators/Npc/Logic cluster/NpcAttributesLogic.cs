using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;

namespace Service_Delegators;

internal class NpcAttributesLogic
{
    private readonly IDiceRollService dice;

    private NpcAttributesLogic() { }
    internal NpcAttributesLogic(IDiceRollService diceService)
    {
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
    }

    #region private methods
    private void SetNpcCharacterInfo(NpcInfo npcInfo, Character npcChar)
    {
        npcChar.Info.EntityLevel = RandomizeEntityLevel(dice);
        npcChar.Info.DateOfBirth = DateTime.Now.ToShortDateString();
        npcChar.Info.IsAlive = true;
        npcChar.Info.Fame = "Not much is know about this one.";
        npcChar.Info.Origins = GetOriginsByRegion(npcInfo);
        npcChar.Info.Name = $"Npc-{npcChar.Info.Origins.Race}-{DateTime.Now.Millisecond}"; // TODO: generate NPC name dynamically

        if (npcChar.Info.Origins.Race != GameplayLore.Npcs.Races.Animal 
            || npcChar.Info.Origins.Race != GameplayLore.Npcs.Races.Elemental)
        {
            npcChar.Info.Wealth = dice.Roll_d100(true);
        }
    }

    private CharacterOrigins GetOriginsByRegion(NpcInfo info)
    {
        var origins = new CharacterOrigins
        {
            Culture = "NPC",
            //Tradition = GameplayLore.Rulebook.Regions.AllEastern.Contains(info.Region) ? CharactersLore.Tradition.Common : CharactersLore.Tradition.Martial,
            Tradition = GameplayLore.Tradition.Common, // TODO: need to be generated dynamically
            Race = SetNpcRace(info)
        };

        origins.Class = SetNpcClass(origins.Race);

        return origins;
    }

    private string SetNpcRace(NpcInfo info)
    {
        var animalsFoundIn = new List<string>
        {
            GameplayLore.Map.Dragonmaw.Farlindor.FarlindorName,
        };

        var monstersFoundIn = new List<string>
        {
             GameplayLore.Map.Dragonmaw.Farlindor.FarlindorName,
        };

        var humanoidsFoundIn = new List<string>
        {
             GameplayLore.Map.Dragonmaw.Farlindor.FarlindorName,
        };

        var undeadsFoundIn = new List<string>
        {
             GameplayLore.Map.Dragonmaw.Farlindor.FarlindorName,
        };

        var fiendsFoundIn = new List<string>
        {
             GameplayLore.Map.Dragonmaw.Farlindor.FarlindorName,
        };

        var elementalFoundIn = new List<string>
        {
             GameplayLore.Map.Dragonmaw.Farlindor.FarlindorName,
        };

        List<string> possibleRaces = new();

        if (animalsFoundIn.Contains(info.Subregion)) possibleRaces.Add(GameplayLore.Npcs.Races.Animal);
        if (monstersFoundIn.Contains(info.Subregion)) possibleRaces.Add(GameplayLore.Npcs.Races.Monster);
        if (humanoidsFoundIn.Contains(info.Subregion)) possibleRaces.Add(GameplayLore.Npcs.Races.Humanoid);
        if (undeadsFoundIn.Contains(info.Subregion)) possibleRaces.Add(GameplayLore.Npcs.Races.Undead);
        if (fiendsFoundIn.Contains(info.Subregion)) possibleRaces.Add(GameplayLore.Npcs.Races.Fiend);
        if (elementalFoundIn.Contains(info.Subregion)) possibleRaces.Add(GameplayLore.Npcs.Races.Elemental);

        var index = dice.Roll_XdY(0, possibleRaces.Count - 1);
        return possibleRaces[index];
    }

    private string SetNpcClass(string race)
    {
        var roll = dice.Roll_d100();

        if (race == GameplayLore.Npcs.Races.Animal)
        {
            return CharactersLore.Classes.Warrior;   
        }
        else if (race == GameplayLore.Npcs.Races.Monster)
        {
            return roll <= 70 ? CharactersLore.Classes.Warrior : (roll >= 95 ? CharactersLore.Classes.Mage : CharactersLore.Classes.Hunter);
        }
        else if (race == GameplayLore.Npcs.Races.Humanoid)
        {
            return roll <= 50 ? CharactersLore.Classes.Warrior : (roll >= 80 ? CharactersLore.Classes.Mage : CharactersLore.Classes.Hunter);
        }
        else if (race == GameplayLore.Npcs.Races.Undead)
        {
            return roll <= 80 ? CharactersLore.Classes.Warrior : CharactersLore.Classes.Mage;
        }
        else if (race == GameplayLore.Npcs.Races.Fiend)
        {
            return roll <= 30 ? CharactersLore.Classes.Warrior : (roll >= 50 ? CharactersLore.Classes.Mage : CharactersLore.Classes.Hunter);
        }
        else if (race == GameplayLore.Npcs.Races.Elemental)
        {
            return roll <= 50 ? CharactersLore.Classes.Warrior : CharactersLore.Classes.Mage;
        }
        else
        {
            return CharactersLore.Classes.Warrior;
        }
    }


    private CharacterSkills CalculateNpcSkills(NpcInfo npcInfo)
    {
        var skills = new CharacterSkills();
        var skillsFactor = 0; //TODO: to remove

        var comMin = npcInfo.SkillsMin.Combat - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Combat - skillsFactor;
        var comMax = npcInfo.SkillsMax.Combat + skillsFactor;
        skills.Combat = dice.Roll_XdY(comMin, comMax);

        var arcMin = npcInfo.SkillsMin.Arcane - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Arcane - skillsFactor;
        var arcMax = npcInfo.SkillsMax.Arcane + skillsFactor;
        skills.Arcane = dice.Roll_XdY(arcMin, arcMax);

        var psiMin = npcInfo.SkillsMin.Psionics - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Psionics - skillsFactor;
        var psiMax = npcInfo.SkillsMax.Psionics + skillsFactor;
        skills.Psionics = dice.Roll_XdY(psiMin, psiMax);

        var hidMin = npcInfo.SkillsMin.Hide - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Hide - skillsFactor;
        var hidMax = npcInfo.SkillsMax.Hide + skillsFactor;
        skills.Hide = dice.Roll_XdY(hidMin, hidMax);

        var traMin = npcInfo.SkillsMin.Traps - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Traps - skillsFactor;
        var traMax = npcInfo.SkillsMax.Traps + skillsFactor;
        skills.Traps = dice.Roll_XdY(traMin, traMax);

        var tacMin = npcInfo.SkillsMin.Tactics - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Tactics - skillsFactor;
        var tacMax = npcInfo.SkillsMax.Tactics + skillsFactor;
        skills.Tactics = dice.Roll_XdY(tacMin, tacMax);

        var socMin = npcInfo.SkillsMin.Social - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Social - skillsFactor;
        var socMax = npcInfo.SkillsMax.Social + skillsFactor;
        skills.Social = dice.Roll_XdY(socMin, socMax);

        var apoMin = npcInfo.SkillsMin.Apothecary - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Apothecary - skillsFactor;
        var apoMax = npcInfo.SkillsMax.Apothecary + skillsFactor;
        skills.Apothecary = dice.Roll_XdY(apoMin, apoMax);

        var trvMin = npcInfo.SkillsMin.Travel - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Travel - skillsFactor;
        var trvMax = npcInfo.SkillsMax.Travel + skillsFactor;
        skills.Travel = dice.Roll_XdY(trvMin, trvMax);

        var saiMin = npcInfo.SkillsMin.Sail - skillsFactor < 0 ? 10 : npcInfo.SkillsMin.Sail - skillsFactor;
        var saiMax = npcInfo.SkillsMax.Sail + skillsFactor;
        skills.Sail = dice.Roll_XdY(saiMin, saiMax);

        return skills;
    }

    private CharacterAssets CalculateNpcAssets(NpcInfo npcInfo)
    {
        var assets = new CharacterAssets();
        var assetsFactor = 0; //TODO: to remove

        var resMin = npcInfo.AssetsMin.Resolve - assetsFactor < 0 ? 5 : npcInfo.AssetsMin.Resolve - assetsFactor;
        var resMax = npcInfo.AssetsMax.Resolve + assetsFactor;
        assets.Resolve = dice.Roll_XdY(resMin, resMax);

        var harMin = npcInfo.AssetsMin.Harm - assetsFactor < 0 ? 0 : npcInfo.AssetsMin.Harm - assetsFactor;
        var harMax = npcInfo.AssetsMax.Harm + assetsFactor;
        assets.Harm = dice.Roll_XdY(harMin, harMax);

        var spoMin = npcInfo.AssetsMin.Spot - assetsFactor < 0 ? 0 : npcInfo.AssetsMin.Spot - assetsFactor;
        var spoMax = npcInfo.AssetsMax.Spot + assetsFactor;
        assets.Spot = dice.Roll_XdY(spoMin, spoMax);

        var defMin = 0;
        var defMax = 10;
        assets.Defense = dice.Roll_XdY(defMin, defMax);

        var purMin = 0;
        var purMax = 25;
        assets.Purge = dice.Roll_XdY(purMin, purMax);

        var manMin = npcInfo.AssetsMin.Mana - assetsFactor < 0 ? 0 : npcInfo.AssetsMin.Mana - assetsFactor;
        var manMax = npcInfo.AssetsMax.Mana + assetsFactor;
        assets.Mana = dice.Roll_XdY(manMin, manMax);

        return assets;
    }

    private CharacterStats CalculateNpcStats(NpcInfo npcInfo)
    {
        var stats = new CharacterStats();
        var statsFactor = 0; //TODO: to remove

        var strMin = npcInfo.StatsMin.Strength - statsFactor < 0 ? 10 : npcInfo.StatsMin.Strength - statsFactor;
        var strMax = npcInfo.StatsMax.Strength + statsFactor;
        stats.Strength = dice.Roll_XdY(strMin, strMax);

        var conMin = npcInfo.StatsMin.Constitution - statsFactor < 0 ? 10 : npcInfo.StatsMin.Constitution - statsFactor;
        var conMax = npcInfo.StatsMax.Constitution + statsFactor;
        stats.Constitution = dice.Roll_XdY(conMin, conMax);

        var agiMin = npcInfo.StatsMin.Agility - statsFactor < 0 ? 10 : npcInfo.StatsMin.Agility - statsFactor;
        var agiMax = npcInfo.StatsMax.Agility + statsFactor;
        stats.Agility = dice.Roll_XdY(agiMin, agiMax);

        var wilMin = npcInfo.StatsMin.Willpower - statsFactor < 0 ? 10 : npcInfo.StatsMin.Willpower - statsFactor;
        var wilMax = npcInfo.StatsMax.Willpower + statsFactor;
        stats.Willpower = dice.Roll_XdY(wilMin, wilMax);

        var perMin = npcInfo.StatsMin.Perception - statsFactor < 0 ? 10 : npcInfo.StatsMin.Perception - statsFactor;
        var perMax = npcInfo.StatsMax.Perception + statsFactor;
        stats.Perception = dice.Roll_XdY(perMin, perMax);

        var absMin = npcInfo.StatsMin.Abstract - statsFactor < 0 ? 10 : npcInfo.StatsMin.Abstract - statsFactor;
        var absMax = npcInfo.StatsMax.Abstract + statsFactor;
        stats.Abstract = dice.Roll_XdY(absMin, absMax);

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
