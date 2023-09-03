using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

public interface ICharacterLogicDelegator
{
    CharacterStub CreateStub(string playerId);
    Character SaveStub(CharacterTraits traits, string playerId);
    Character KillCharacter(CharacterIdentity identity);
    void DeleteCharacter(CharacterIdentity identity);

    Character EquipItem(CharacterEquip equip);
    Character UnequipItem(CharacterEquip unequip);

    Character UpdateName(string name, CharacterIdentity identity);
    Character AddFame(string fame, CharacterIdentity identity);
    Character AddWealth(int wealth, CharacterIdentity identity);

    Character IncreaseStats(string stat, CharacterIdentity identity);
    Character IncreaseAssets(string asset, CharacterIdentity identity);
    Character IncreaseSkills(string skill, CharacterIdentity identity);
    
    Character HireMercenary(CharacterHireMercenary hireMercenary);
    Character LearnSpecialSkill(CharacterAddSpecialSkill trait);
    Character TravelToLocation(CharacterTravel travel);
}

public class CharacterLogicDelegator : ICharacterLogicDelegator
{
    public readonly Snapshot snapshot;
    public readonly Validations validations;
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
        Validations validations,
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

    public CharacterStub CreateStub(string playerId)
    {
        validations.ValidateCharacterMaxNrAllowed(playerId);
        var stub = createLogic.CreateStub(playerId);
        return stub;
    }

    public Character SaveStub(CharacterTraits traits, string playerId)
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

    public Character EquipItem(CharacterEquip equip)
    {
        validations.ValidateCharacterEquipUnequipItem(equip, true);
        var character = itemsLogic.EquipItem(equip);
        return PersistAndReturn(character, equip.CharacterIdentity.PlayerId);
    }

    public Character UnequipItem(CharacterEquip unequip)
    {
        validations.ValidateCharacterEquipUnequipItem(unequip, true);
        var character = itemsLogic.UnequipItem(unequip);
        return PersistAndReturn(character, unequip.CharacterIdentity.PlayerId);
    }

    public Character UpdateName(string name, CharacterIdentity identity)
    {
        validations.ValidateCharacterUpdateName(name, identity);
        var character = infoLogic.ChangeName(name, identity);
        return PersistAndReturn(character, identity.PlayerId);
    }

    public Character LearnSpecialSkill(CharacterAddSpecialSkill trait)
    {
        validations.ValidateCharacterLearnHeroicTrait(trait);
        var character = specialSkillsLogic.ApplySpecialSkill(trait);
        return PersistAndReturn(character, trait.CharacterIdentity.PlayerId);
    }

    public Character AddFame(string fame, CharacterIdentity identity)
    {
        validations.ValidateCharacterAddFame(fame, identity);
        var character = infoLogic.AddFame(fame, identity);
        return PersistAndReturn(character, identity.PlayerId);
    }

    public Character AddWealth(int wealth, CharacterIdentity identity)
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

    public Character IncreaseStats(string stat, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(stat, CharactersLore.AttributeTypes.Stats, identity);    
        var character = levelupLogic.IncreaseStats(stat, identity);
        return PersistAndReturn(character, identity.PlayerId);
    }
    
    public Character IncreaseAssets(string asset, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(asset, CharactersLore.AttributeTypes.Assets, identity);    
        var character = levelupLogic.IncreaseAsset(asset, identity);

        return PersistAndReturn(character, identity.PlayerId);
    }

    public Character IncreaseSkills(string skill, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(skill, CharactersLore.AttributeTypes.Skills, identity);
        var character = levelupLogic.IncreaseSkill(skill, identity);

        return PersistAndReturn(character, identity.PlayerId);
    }

    public Character TravelToLocation(CharacterTravel travel)
    {
        validations.ValidateCharacterBeforeTravel(travel);
        var character = travelLogic.MoveToLocation(travel);

        return PersistAndReturn(character, travel.CharacterIdentity.PlayerId);
    }

    public Character HireMercenary(CharacterHireMercenary hireMercenary)
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
