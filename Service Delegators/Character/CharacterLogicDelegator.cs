using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using System.ComponentModel.DataAnnotations;

namespace Service_Delegators;

public interface ICharacterLogicDelegator
{
    Character SaveCharacterStub(CharacterTraits traits, string playerId);
    CharacterStub CreateCharacterStub(string playerId);
    void DeleteCharacter(CharacterIdentity identity);

    Character CharacterEquipItem(CharacterEquip equip);
    Character CharacterUnequipItem(CharacterEquip unequip);

    Character UpdateCharacterName(string name, CharacterIdentity identity);
    Character UpdateCharacterAssets(string asset, CharacterIdentity identity);
    Character UpdateCharacterFame(string fame, CharacterIdentity identity);
    Character UpdateCharacterSkills(string skill, CharacterIdentity identity);
    Character UpdateCharacterStats(string stat, CharacterIdentity identity);
    Character UpdateCharacterWealth(int wealth, CharacterIdentity identity);
    
    void CharacterHireMercenary(CharacterHireMercenary hireMercenary);
    Character CharacterLearnHeroicTrait(CharacterSpecialSkillAdd trait);
    void CharacterTravelToLocation(CharacterTravel positionTravel);

    void KillCharacter(CharacterIdentity identity);
}

public class CharacterLogicDelegator : ICharacterLogicDelegator
{
    public readonly Snapshot snapshot;
    public readonly Validations validations;
    public readonly IPersistenceService persistence;
    public readonly ICharacterCreateLogic characterCreate;
    public readonly ICharacterInfoLogic characterInfo;
    public readonly ICharacterItemsLogic characterItems;

    public CharacterLogicDelegator(
        Snapshot snapshot,
        Validations validations,
        IPersistenceService persistence,
        ICharacterCreateLogic characterCreate,
        ICharacterInfoLogic characterInfo,
        ICharacterItemsLogic characterItems)
    {
        this.snapshot = snapshot;
        this.validations = validations;
        this.persistence = persistence;
        this.characterCreate = characterCreate;
        this.characterInfo = characterInfo;
        this.characterItems = characterItems;
    }

    public CharacterStub CreateCharacterStub(string playerId)
    {
        validations.ValidateCharacterMaxNrAllowed(playerId);
        var stub = characterCreate.CreateStub(playerId);

        return stub;
    }

    public Character SaveCharacterStub(CharacterTraits traits, string playerId)
    {
        validations.ValidateCharacterCreateTraits(traits, playerId);
        var character = characterCreate.SaveStub(traits, playerId);
        persistence.PersistPlayer(playerId);

        return character;
    }

    public void DeleteCharacter(CharacterIdentity identity)
    {
        validations.ValidateCharacterBeforeDelete(identity);
        characterCreate.DeleteCharacter(identity);
    }

    public Character CharacterEquipItem(CharacterEquip equip)
    {
        validations.ValidateCharacterEquipUnequipItem(equip, true);
        return characterItems.EquipItem(equip);
    }

    public Character CharacterUnequipItem(CharacterEquip unequip)
    {
        validations.ValidateCharacterEquipUnequipItem(unequip, true);
        return characterItems.UnequipItem(unequip);
    }

    public Character UpdateCharacterName(string name, CharacterIdentity identity)
    {
        validations.ValidateCharacterUpdateName(name, identity);
        var character = characterInfo.ChangeName(name, identity);
        persistence.PersistPlayer(identity.PlayerId);

        return character;
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
