using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;
using Persistance_Manager;

namespace Service_Delegators;

public interface ICharacterLogicDelegator
{
    CharacterStub CreateCharacterStub(string playerId, string stubId);
    Character SaveCharacterStub(CharacterRacialTraits traits, string playerId);
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

    Character SellItem(CharacterTrade tradeItem);
    Character BuyItem(CharacterTrade tradeItem);
    Character BuyProvisions(CharacterTrade tradeItem);
    Character GiveProvisions(CharacterTrade tradeItem);
    Character GiveWealth(CharacterTrade tradeItem);
    Character GiveItem(CharacterTrade tradeItem);
}

public class CharacterLogicDelegator : ICharacterLogicDelegator
{
    private readonly IValidations validations;
    private readonly IPersistenceService persistence;
    private readonly ICharacterCRUDLogic createLogic;
    private readonly ICharacterUpdateLogic infoLogic;
    private readonly ICharacterItemsLogic itemsLogic;
    private readonly ICharacterSpecialSkillsLogic specialSkillsLogic;
    private readonly ICharacterLevelupLogic levelupLogic;
    private readonly ICharacterTravelLogic travelLogic;
    private readonly ICharacterNpcInteraction npcInteractionLogic;

    public CharacterLogicDelegator(
        IValidations validations,
        IPersistenceService persistence,
        ICharacterCRUDLogic createLogic,
        ICharacterUpdateLogic infoLogic,
        ICharacterItemsLogic itemsLogic,
        ICharacterSpecialSkillsLogic specialSkillsLogic,
        ICharacterLevelupLogic levelupLogic,
        ICharacterTravelLogic travelLogic,
        ICharacterNpcInteraction npcInteractionLogic)
    {
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

    public CharacterStub CreateCharacterStub(string playerId, string stubId)
    {
        validations.ValidateCharacterMaxNrAllowed(playerId);
        validations.ValidateStubId(playerId, stubId);
        return createLogic.CreateStub(playerId);
    }

    public Character SaveCharacterStub(CharacterRacialTraits traits, string playerId)
    {
        validations.ValidateCharacterCreateTraits(traits, playerId);
        var character = createLogic.SaveStub(traits, playerId);
        return PersistAndReturn(character);
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
        return PersistAndReturn(character);
    }

    public Character UnequipCharacterItem(CharacterEquip unequip)
    {
        validations.ValidateCharacterEquipUnequipItem(unequip, false);
        var character = itemsLogic.UnequipItem(unequip);
        return PersistAndReturn(character);
    }

    public Character UpdateCharacterName(string name, CharacterIdentity identity)
    {
        validations.ValidateCharacterUpdateName(name, identity);
        var character = infoLogic.ChangeName(name, identity);
        return PersistAndReturn(character);
    }

    public Character LearnCharacterSpecialSkill(CharacterAddSpecialSkill spskAdd)
    {
        validations.ValidateCharacterLearnSpecialSkill(spskAdd);
        var character = specialSkillsLogic.ApplySpecialSkill(spskAdd);
        return PersistAndReturn(character);
    }

    public Character AddCharacterFame(string fame, CharacterIdentity identity)
    {
        validations.ValidateCharacterAddFame(fame, identity);
        var character = infoLogic.AddFame(fame, identity);
        return PersistAndReturn(character);
    }

    public Character AddCharacterWealth(int wealth, CharacterIdentity identity)
    {
        validations.ValidateCharacterAddWealth(wealth, identity);
        var character = infoLogic.AddWealth(wealth, identity);
        return PersistAndReturn(character);
    }
    
    public Character KillCharacter(CharacterIdentity identity)
    {
        validations.ValidateCharacterBeforeKill(identity);
        var character = createLogic.KillChar(identity);
        return PersistAndReturn(character);
    }

    public Character IncreaseCharacterStats(string stat, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(stat, CharactersLore.AttributeTypes.Stats, identity);    
        var character = levelupLogic.IncreaseStats(stat, identity);
        return PersistAndReturn(character);
    }
    
    public Character IncreaseCharacterAssets(string asset, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(asset, CharactersLore.AttributeTypes.Assets, identity);    
        var character = levelupLogic.IncreaseAsset(asset, identity);

        return PersistAndReturn(character);
    }

    public Character IncreaseCharacterSkills(string skill, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(skill, CharactersLore.AttributeTypes.Skills, identity);
        var character = levelupLogic.IncreaseSkill(skill, identity);

        return PersistAndReturn(character);
    }

    public Character TravelCharacterToLocation(CharacterTravel travel)
    {
        validations.ValidateCharacterBeforeTravel(travel);
        var character = travelLogic.MoveToLocation(travel);

        return PersistAndReturn(character);
    }

    public Character HireMercenaryForCharacter(CharacterHireMercenary hireMercenary)
    {
        validations.ValidateMercenaryBeforeHire(hireMercenary);
        var character = npcInteractionLogic.HireMercenary(hireMercenary);

        return PersistAndReturn(character);
    }

    public Character SellItem(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterItemBeforeSell(tradeItem);
        var character = itemsLogic.BuyOrSellItem(tradeItem);

        return PersistAndReturn(character);
    }

    public Character BuyItem(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterItemBeforeBuy(tradeItem);
        var character = itemsLogic.BuyOrSellItem(tradeItem);

        return PersistAndReturn(character);
    }

    public Character BuyProvisions(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterBeforeBuyProvisions(tradeItem);
        var character = itemsLogic.BuyProvisions(tradeItem);

        return PersistAndReturn(character);
    }

    public Character GiveProvisions(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterBeforeGiveProvisions(tradeItem);
        var character = itemsLogic.GiveProvisions(tradeItem);

        PersistOnly(tradeItem.TargetIdentity.PlayerId);

        return PersistAndReturn(character);
    }

    public Character GiveWealth(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterBeforeGiveWealth(tradeItem);
        var character = itemsLogic.GiveWealth(tradeItem);

        PersistOnly(tradeItem.TargetIdentity.PlayerId);

        return PersistAndReturn(character);
    }

    public Character GiveItem(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterBeforeGiveItem(tradeItem);
        var character = itemsLogic.GiveItem(tradeItem);

        PersistOnly(tradeItem.TargetIdentity.PlayerId);

        return PersistAndReturn(character);
    }

    #region private methods
    private Character PersistAndReturn(Character character)
    {
        persistence.PersistPlayer(character.Identity.PlayerId);

        return character;
    }

    private void PersistOnly(string playerId)
    {
        persistence.PersistPlayer(playerId);
    }
    #endregion
}
