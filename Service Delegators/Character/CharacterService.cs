using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class CharacterService : ICharacterService
{
    private readonly IDatabaseService dbs;
    private readonly CharacterValidator validator;
    private readonly CharacterLogicDelegator logic;

    public CharacterService(
        IDatabaseService databaseService,
        IDiceRollService diceRollService,
        IItemService itemService)
    {
        dbs = databaseService;
        validator = new CharacterValidator(databaseService.Snapshot);
        logic = new CharacterLogicDelegator(databaseService, diceRollService, itemService);
    }

    public CharacterStub CreateCharacterStub(string playerId)
    {
        validator.ValidateMaxNumberOfCharacters(playerId);
        return logic.CreateStub(playerId);
    }

    public Character SaveCharacterStub(CharacterTraits traits, string playerId)
    {
        validator.ValidateTraitsOnSaveCharacter(traits, playerId);
        return logic.SaveStub(traits, playerId);
    }

    public Character UpdateCharacterName(string name, CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        validator.ValidateCharacterName(name);
        validator.ValidateIfCharacterIsLocked(identity);
        return logic.ChangeName(name, identity);
    }

    public Character UpdateCharacterFame(string fame, CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        validator.ValidateString(fame);
        return logic.AddFame(fame, identity);
    }

    public Character UpdateCharacterWealth(int wealth, CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        validator.ValidateIfCharacterIsLocked(identity);
        validator.ValidateNumber(wealth);
        return logic.AddWealth(wealth, identity);
    }

    public Character UpdateCharacterStats(string stat, CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        validator.ValidateIfCharacterIsLocked(identity);
        validator.ValidateStatExists(stat);
        validator.ValidateCharacterHasStatsPoints(identity);
        return logic.IncreaseStats(stat, identity);
    }

    public Character UpdateCharacterAssets(string asset, CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        validator.ValidateIfCharacterIsLocked(identity);
        validator.ValidateAssetExists(asset);
        validator.ValidateCharacterHasAssetsPoints(identity);
        return logic.IncreaseAsset(asset, identity);
    }
    
    public Character UpdateCharacterSkills(string skill, CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        validator.ValidateIfCharacterIsLocked(identity);
        validator.ValidateSkillExists(skill);
        validator.ValidateCharacterHasSkillsPoints(identity);
        return logic.IncreaseSkills(skill, identity);
    }

    public void KillCharacter(CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        logic.KillChar(identity);
    }

    public void DeleteCharacter(CharacterIdentity identity)
    {
        validator.ValidateIfCharacterIsLocked(identity);
        logic.DeleteChar(identity);
    }

    public Characters GetPlayerCharacters(string playerId)
    {
        var characters = dbs.Snapshot.Players.Find(p => p.Identity.Id == playerId)!.Characters;

        return new Characters
        {
            Count = characters!.Count,
            CharactersList = characters
        };
    }

    public Character CharacterEquipItem(CharacterEquip equip)
    {
        validator.ValidateCharacterEquipUnequipItem(equip, true);
        return logic.EquipItem(equip);
    }

    public Character CharacterUnequipItem(CharacterEquip unequip)
    {
        validator.ValidateCharacterEquipUnequipItem(unequip, false);
        return logic.UnequipItem(unequip);
    }

    public Character CharacterLearnHeroicTrait(CharacterSpecialSkillAdd trait)
    {
        validator.ValidateCharacterLearnHeroicTrait(trait);
        return logic.ApplyHeroicTrait(trait);
    }

    public void CharacterTravelToLocation(CharacterTravel positionTravel)
    {
        validator.ValidateBeforeTravel(positionTravel);
        logic.MoveToLocation(positionTravel);
    }

    public void CharacterHireMercenary(CharacterHireMercenary hireMercenary)
    {
        validator.ValidateMercenaryBeforeHire(hireMercenary);
        logic.HireMercenary(hireMercenary);
    }
}
