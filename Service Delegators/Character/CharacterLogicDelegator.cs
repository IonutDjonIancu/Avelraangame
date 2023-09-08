using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

public interface ICharacterLogicDelegator
{
    CharacterStub CreateCharacterStub(string playerId);
    Character SaveCharacterStub(CharacterTraits traits, string playerId);
    Character KillCharacter(CharacterIdentity identity);
    void DeleteCharacter(CharacterIdentity identity);

    Character EquipCharacterItem(CharacterEquip equip);
    Character UnequipCharacterItem(CharacterEquip unequip);

    Character UpdateCharacterName(string name, CharacterIdentity identity);
    Character AddCharacterFame(string fame, CharacterIdentity identity);
    Character AddCharacterWealth(int wealth, CharacterIdentity identity);

    Character IncreaseCharacterStats(string stat, CharacterIdentity identity);
    Character IncreaseCharacterAssets(string asset, CharacterIdentity identity);
    Character IncreaseCharacterSkills(string skill, CharacterIdentity identity);
    
    Character HireMercenaryForCharacter(CharacterHireMercenary hireMercenary);
    Character LearnCharacterSpecialSkill(CharacterAddSpecialSkill spskAdd);
    Character TravelCharacterToLocation(CharacterTravel travel);
}

public class CharacterLogicDelegator : ICharacterLogicDelegator
{
    public readonly Snapshot snapshot;
    public readonly IValidations validations;
    public readonly IPersistenceService persistence;
    public readonly ICharacterCreateLogic createLogic;
    public readonly ICharacterInfoLogic infoLogic;
    public readonly ICharacterItemsLogic itemsLogic;
    public readonly ICharacterSpecialSkillsLogic specialSkillsLogic;
    public readonly ICharacterLevelupLogic levelupLogic;
    public readonly ICharacterTravelLogic travelLogic;
    public readonly ICharacterNpcInteraction npcInteractionLogic;

    public CharacterLogicDelegator(
        Snapshot snapshot,
        IValidations validations,
        IPersistenceService persistence,
        ICharacterCreateLogic createLogic,
        ICharacterInfoLogic infoLogic,
        ICharacterItemsLogic itemsLogic,
        ICharacterSpecialSkillsLogic specialSkillsLogic,
        ICharacterLevelupLogic levelupLogic,
        ICharacterTravelLogic travelLogic,
        ICharacterNpcInteraction npcInteractionLogic)
    {
        this.snapshot = snapshot;
        this.validations = validations;
        this.persistence = persistence;
        this.createLogic = createLogic;
        this.infoLogic = infoLogic;
        this.itemsLogic = itemsLogic;
        this.specialSkillsLogic = specialSkillsLogic;
        this.levelupLogic = levelupLogic;
        this.travelLogic = travelLogic;
        this.npcInteractionLogic = npcInteractionLogic;
    }

    public CharacterStub CreateCharacterStub(string playerId)
    {
        validations.ValidateCharacterMaxNrAllowed(playerId);
        var stub = createLogic.CreateStub(playerId);
        return stub;
    }

    public Character SaveCharacterStub(CharacterTraits traits, string playerId)
    {
        validations.ValidateCharacterCreateTraits(traits, playerId);
        var character = createLogic.SaveStub(traits, playerId);
        return PersistAndReturn(character, playerId);
    }

    public void DeleteCharacter(CharacterIdentity identity)
    {
        validations.ValidateCharacterBeforeDelete(identity);
        createLogic.DeleteCharacter(identity);
        PersistOnly(identity.PlayerId);
    }

    public Character EquipCharacterItem(CharacterEquip equip)
    {
        validations.ValidateCharacterEquipUnequipItem(equip, true);
        var character = itemsLogic.EquipItem(equip);
        return PersistAndReturn(character, equip.CharacterIdentity.PlayerId);
    }

    public Character UnequipCharacterItem(CharacterEquip unequip)
    {
        validations.ValidateCharacterEquipUnequipItem(unequip, false);
        var character = itemsLogic.UnequipItem(unequip);
        return PersistAndReturn(character, unequip.CharacterIdentity.PlayerId);
    }

    public Character UpdateCharacterName(string name, CharacterIdentity identity)
    {
        validations.ValidateCharacterUpdateName(name, identity);
        var character = infoLogic.ChangeName(name, identity);
        return PersistAndReturn(character, identity.PlayerId);
    }

    public Character LearnCharacterSpecialSkill(CharacterAddSpecialSkill spskAdd)
    {
        validations.ValidateCharacterLearnSpecialSkill(spskAdd);
        var character = specialSkillsLogic.ApplySpecialSkill(spskAdd);
        return PersistAndReturn(character, spskAdd.CharacterIdentity.PlayerId);
    }

    public Character AddCharacterFame(string fame, CharacterIdentity identity)
    {
        validations.ValidateCharacterAddFame(fame, identity);
        var character = infoLogic.AddFame(fame, identity);
        return PersistAndReturn(character, identity.PlayerId);
    }

    public Character AddCharacterWealth(int wealth, CharacterIdentity identity)
    {
        validations.ValidateCharacterAddWealth(wealth, identity);
        var character = infoLogic.AddWealth(wealth, identity);
        return PersistAndReturn(character, identity.PlayerId);
    }
    
    public Character KillCharacter(CharacterIdentity identity)
    {
        validations.ValidateCharacterBeforeKill(identity);
        var character = createLogic.KillChar(identity);
        return PersistAndReturn(character, identity.PlayerId);
    }

    public Character IncreaseCharacterStats(string stat, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(stat, CharactersLore.AttributeTypes.Stats, identity);    
        var character = levelupLogic.IncreaseStats(stat, identity);
        return PersistAndReturn(character, identity.PlayerId);
    }
    
    public Character IncreaseCharacterAssets(string asset, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(asset, CharactersLore.AttributeTypes.Assets, identity);    
        var character = levelupLogic.IncreaseAsset(asset, identity);

        return PersistAndReturn(character, identity.PlayerId);
    }

    public Character IncreaseCharacterSkills(string skill, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(skill, CharactersLore.AttributeTypes.Skills, identity);
        var character = levelupLogic.IncreaseSkill(skill, identity);

        return PersistAndReturn(character, identity.PlayerId);
    }

    public Character TravelCharacterToLocation(CharacterTravel travel)
    {
        validations.ValidateCharacterBeforeTravel(travel);
        var character = travelLogic.MoveToLocation(travel);

        return PersistAndReturn(character, travel.CharacterIdentity.PlayerId);
    }

    public Character HireMercenaryForCharacter(CharacterHireMercenary hireMercenary)
    {
        validations.ValidateMercenaryBeforeHire(hireMercenary);
        var character = npcInteractionLogic.HireMercenary(hireMercenary);

        return PersistAndReturn(character, hireMercenary.CharacterIdentity.PlayerId);
    }

    #region private methods
    private Character PersistAndReturn(Character character, string playerId)
    {
        persistence.PersistPlayer(playerId);

        return character;
    }

    private void PersistOnly(string playerId)
    {
        persistence.PersistPlayer(playerId);
    }

    #endregion
}
