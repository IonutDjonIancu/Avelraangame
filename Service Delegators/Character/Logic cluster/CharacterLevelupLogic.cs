using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterLevelupLogic
{
    private readonly IDatabaseService dbs;

    private CharacterLevelupLogic() { }
    internal CharacterLevelupLogic(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

    internal Character IncreaseStat(string stat, CharacterIdentity identity)
    {
        var character = Utils.GetPlayerCharacter(dbs, identity);

        var levelupStatPts = 1;
        var likelyStatPts = 0;

        if      (character.Status.Traits.Class == CharactersLore.Classes.Warrior)      likelyStatPts += RulebookLore.Classes.Warrior.LikelyStats.Contains(stat) ? 1 : 0;        
        else if (character.Status.Traits.Class == CharactersLore.Classes.Mage)         likelyStatPts += RulebookLore.Classes.Mage.LikelyStats.Contains(stat) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Hunter)       likelyStatPts += RulebookLore.Classes.Hunter.LikelyStats.Contains(stat) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Swashbuckler) likelyStatPts += RulebookLore.Classes.Swashbuckler.LikelyStats.Contains(stat) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Sorcerer)     likelyStatPts += RulebookLore.Classes.Sorcerer.LikelyStats.Contains(stat) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Mage)         likelyStatPts += RulebookLore.Classes.Mage.LikelyStats.Contains(stat) ? 1 : 0;

        var total = levelupStatPts + likelyStatPts;

        if      (stat == CharactersLore.Stats.Strength)     character.Sheet!.Stats.Strength += total;
        else if (stat == CharactersLore.Stats.Constitution) character.Sheet!.Stats.Constitution += total;
        else if (stat == CharactersLore.Stats.Agility)      character.Sheet!.Stats.Agility += total;
        else if (stat == CharactersLore.Stats.Willpower)    character.Sheet!.Stats.Willpower += total;
        else if (stat == CharactersLore.Stats.Perception)   character.Sheet!.Stats.Perception += total;
        else if (stat == CharactersLore.Stats.Abstract)     character.Sheet!.Stats.Abstract += total;

        character!.LevelUp!.StatPoints--;

        Thread.Sleep(500); // a workaround to allow the text file to be saved before next call
        dbs.PersistPlayer(character.Identity.PlayerId);

        return character;
    }

    internal Character IncreaseAsset(string asset, CharacterIdentity identity)
    {
        var character = Utils.GetPlayerCharacter(dbs, identity);

        var levelupAssetPts = 1;
        var likelyAssetPts = 0;

        if      (character.Status.Traits.Class == CharactersLore.Classes.Warrior)      likelyAssetPts += RulebookLore.Classes.Warrior.LikelyAssets.Contains(asset) ? 1 : 0;        
        else if (character.Status.Traits.Class == CharactersLore.Classes.Mage)         likelyAssetPts += RulebookLore.Classes.Mage.LikelyAssets.Contains(asset) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Hunter)       likelyAssetPts += RulebookLore.Classes.Hunter.LikelyAssets.Contains(asset) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Swashbuckler) likelyAssetPts += RulebookLore.Classes.Swashbuckler.LikelyAssets.Contains(asset) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Sorcerer)     likelyAssetPts += RulebookLore.Classes.Sorcerer.LikelyAssets.Contains(asset) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Mage)         likelyAssetPts += RulebookLore.Classes.Mage.LikelyAssets.Contains(asset) ? 1 : 0;

        var total = levelupAssetPts + likelyAssetPts;

        if      (asset == CharactersLore.Assets.Resolve) character.Sheet!.Assets.Resolve += total * 2 + (int)(RulebookLore.Formulae.Assets.CalculateResolve(character.Sheet.Stats) * 0.02);
        else if (asset == CharactersLore.Assets.Harm)    character.Sheet!.Assets.Harm += total + (int)(RulebookLore.Formulae.Assets.CalculateHarm(character.Sheet.Stats) * 0.02);
        else if (asset == CharactersLore.Assets.Spot)    character.Sheet!.Assets.Spot += total + (int)(RulebookLore.Formulae.Assets.CalculateSpot(character.Sheet.Stats) * 0.02);
        else if (asset == CharactersLore.Assets.Defense) character.Sheet!.Assets.Defense += (total + RulebookLore.Formulae.Assets.CalculateDefense(character.Sheet.Stats)) * 0.025;
        else if (asset == CharactersLore.Assets.Purge)   character.Sheet!.Assets.Purge += (total + RulebookLore.Formulae.Assets.CalculatePurge(character.Sheet.Stats)) * 0.3;
        else if (asset == CharactersLore.Assets.Mana)    character.Sheet!.Assets.Mana += total + (int)(RulebookLore.Formulae.Assets.CalculateMana(character.Sheet.Stats) * 0.1);
        else if (asset == CharactersLore.Assets.Actions) character.Sheet!.Assets.Actions += (total + RulebookLore.Formulae.Assets.CalculateActions(character.Sheet.Stats)) * 0.04;

        character!.LevelUp!.AssetPoints--;

        Thread.Sleep(500); // a workaround to allow the text file to be saved before next call
        dbs.PersistPlayer(character.Identity.PlayerId);

        return character;
    }

    internal Character IncreaseSkill(string skill, CharacterIdentity identity)
    {
        var character = Utils.GetPlayerCharacter(dbs, identity);

        var levelupSkillPts = 1;
        var likelySkillPts = 0;

        if      (character.Status.Traits.Class == CharactersLore.Classes.Warrior)      likelySkillPts += RulebookLore.Classes.Warrior.LikelySkills.Contains(skill) ? 1 : 0;        
        else if (character.Status.Traits.Class == CharactersLore.Classes.Mage)         likelySkillPts += RulebookLore.Classes.Mage.LikelySkills.Contains(skill) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Hunter)       likelySkillPts += RulebookLore.Classes.Hunter.LikelySkills.Contains(skill) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Swashbuckler) likelySkillPts += RulebookLore.Classes.Swashbuckler.LikelySkills.Contains(skill) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Sorcerer)     likelySkillPts += RulebookLore.Classes.Sorcerer.LikelySkills.Contains(skill) ? 1 : 0;
        else if (character.Status.Traits.Class == CharactersLore.Classes.Mage)         likelySkillPts += RulebookLore.Classes.Mage.LikelySkills.Contains(skill) ? 1 : 0;

        var total = levelupSkillPts + likelySkillPts;

        if      (skill == CharactersLore.Skills.Combat)     character.Sheet!.Skills.Combat += total + (int)(RulebookLore.Formulae.Skills.CalculateCombat(character.Sheet.Stats) * 0.02);
        else if (skill == CharactersLore.Skills.Arcane)     character.Sheet!.Skills.Arcane += total + (int)(RulebookLore.Formulae.Skills.CalculateArcane(character.Sheet.Stats) * 0.02);
        else if (skill == CharactersLore.Skills.Psionics)   character.Sheet!.Skills.Psionics += total + (int)(RulebookLore.Formulae.Skills.CalculatePsionics(character.Sheet.Stats) * 0.02);
        else if (skill == CharactersLore.Skills.Hide)       character.Sheet!.Skills.Hide += total + (int)(RulebookLore.Formulae.Skills.CalculateHide(character.Sheet.Stats) * 0.02);
        else if (skill == CharactersLore.Skills.Traps)      character.Sheet!.Skills.Traps += total + (int)(RulebookLore.Formulae.Skills.CalculateTraps(character.Sheet.Stats) * 0.02);
        else if (skill == CharactersLore.Skills.Tactics)    character.Sheet!.Skills.Tactics += total + (int)(RulebookLore.Formulae.Skills.CalculateTactics(character.Sheet.Stats) * 0.02);
        else if (skill == CharactersLore.Skills.Social)     character.Sheet!.Skills.Social += total + (int)(RulebookLore.Formulae.Skills.CalculateSocial(character.Sheet.Stats) * 0.02);
        else if (skill == CharactersLore.Skills.Apothecary) character.Sheet!.Skills.Apothecary += total + (int)(RulebookLore.Formulae.Skills.CalculateApothecary(character.Sheet.Stats) * 0.02);
        else if (skill == CharactersLore.Skills.Travel)     character.Sheet!.Skills.Travel += total + (int)(RulebookLore.Formulae.Skills.CalculateTravel(character.Sheet.Stats) * 0.02);
        else if (skill == CharactersLore.Skills.Sail)       character.Sheet!.Skills.Sail += total + (int)(RulebookLore.Formulae.Skills.CalculateSail(character.Sheet.Stats) * 0.02);

        character.LevelUp!.SkillPoints--;

        Thread.Sleep(500); // a workaround to allow the text file to be saved before next call
        dbs.PersistPlayer(character.Identity.PlayerId);

        return character;
    }
}
