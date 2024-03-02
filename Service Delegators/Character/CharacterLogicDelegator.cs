using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Service_Delegators;

public interface ICharacterLogicDelegator
{
    void Character(string playerId);
    CharacterStub CreateCharacterStub(string playerId, string stubId);
    Character SaveCharacterStub(CharacterRacialTraits traits, string playerId);
    Character KillCharacter(CharacterIdentity identity);
    void DeleteCharacter(CharacterIdentity identity);

    Character EquipCharacterItem(CharacterEquip equip);
    Character UnequipCharacterItem(CharacterEquip unequip);

    Character UpdateCharacterName(CharacterData characterData);
    Character AddCharacterFame(string fame, CharacterIdentity identity);
    Character AddCharacterWealth(int wealth, CharacterIdentity identity);

    Character IncreaseCharacterStats(string stat, CharacterIdentity identity);
    Character IncreaseCharacterAssets(string asset, CharacterIdentity identity);
    Character IncreaseCharacterSkills(string skill, CharacterIdentity identity);
    
    Character HireMercenaryForCharacter(CharacterHireMercenary hireMercenary);
    Character LearnCharacterSpecialSkill(CharacterAddSpecialSkill spskAdd);
    CharacterTravelResponse TravelCharacterToLocation(CharacterTravel travel);

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
    private readonly ICharacterCRUDLogic createLogic;
    private readonly ICharacterUpdateLogic infoLogic;
    private readonly ICharacterItemsLogic itemsLogic;
    private readonly ICharacterSpecialSkillsLogic specialSkillsLogic;
    private readonly ICharacterLevelupLogic levelupLogic;
    private readonly ICharacterTravelLogic travelLogic;
    private readonly ICharacterNpcInteraction npcInteractionLogic;

    public CharacterLogicDelegator(
        IValidations validations,
        ICharacterCRUDLogic createLogic,
        ICharacterUpdateLogic infoLogic,
        ICharacterItemsLogic itemsLogic,
        ICharacterSpecialSkillsLogic specialSkillsLogic,
        ICharacterLevelupLogic levelupLogic,
        ICharacterTravelLogic travelLogic,
        ICharacterNpcInteraction npcInteractionLogic)
    {
        this.validations = validations;
        this.createLogic = createLogic;
        this.infoLogic = infoLogic;
        this.itemsLogic = itemsLogic;
        this.specialSkillsLogic = specialSkillsLogic;
        this.levelupLogic = levelupLogic;
        this.travelLogic = travelLogic;
        this.npcInteractionLogic = npcInteractionLogic;
    }

    public void Character(string playerId)
    {
        validations.ValidateCharacterMaxNrAllowed(playerId);
        createLogic.ClearStubs(playerId);
    }

    public CharacterStub CreateCharacterStub(string playerId, string stubId)
    {
        validations.ValidateStubId(playerId, stubId);
        return createLogic.CreateStub(playerId);
    }

    public Character SaveCharacterStub(CharacterRacialTraits traits, string playerId)
    {
        validations.ValidateCharacterCreateTraits(traits, playerId);
        return createLogic.SaveStub(traits, playerId);
    }

    public void DeleteCharacter(CharacterIdentity identity)
    {
        validations.ValidateCharacterBeforeDelete(identity);
        createLogic.DeleteCharacter(identity);
    }

    public Character EquipCharacterItem(CharacterEquip equip)
    {
        validations.ValidateCharacterEquipUnequipItem(equip, true);
        return itemsLogic.EquipItem(equip);
    }

    public Character UnequipCharacterItem(CharacterEquip unequip)
    {
        validations.ValidateCharacterEquipUnequipItem(unequip, false);
        return itemsLogic.UnequipItem(unequip);
    }

    public Character UpdateCharacterName(CharacterData characterData)
    {
        validations.ValidateCharacterUpdateName(characterData);
        return infoLogic.ChangeName(characterData);
    }

    public Character LearnCharacterSpecialSkill(CharacterAddSpecialSkill spskAdd)
    {
        validations.ValidateCharacterLearnSpecialSkill(spskAdd);
        return specialSkillsLogic.ApplySpecialSkill(spskAdd);
    }

    public Character AddCharacterFame(string fame, CharacterIdentity identity)
    {
        validations.ValidateCharacterAddFame(fame, identity);
        return infoLogic.AddFame(fame, identity);
    }

    public Character AddCharacterWealth(int wealth, CharacterIdentity identity)
    {
        validations.ValidateCharacterAddWealth(wealth, identity);
        return infoLogic.AddWealth(wealth, identity);
    }
    
    public Character KillCharacter(CharacterIdentity identity)
    {
        validations.ValidateCharacterBeforeKill(identity);
        return createLogic.KillChar(identity);
    }

    public Character IncreaseCharacterStats(string stat, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(stat, CharactersLore.AttributeTypes.Stats, identity);    
        return levelupLogic.IncreaseStats(stat, identity);
    }
    
    public Character IncreaseCharacterAssets(string asset, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(asset, CharactersLore.AttributeTypes.Assets, identity);    
        return levelupLogic.IncreaseAsset(asset, identity);
    }

    public Character IncreaseCharacterSkills(string skill, CharacterIdentity identity)
    {
        validations.ValidateAttributesBeforeIncrease(skill, CharactersLore.AttributeTypes.Skills, identity);
        return levelupLogic.IncreaseSkill(skill, identity);
    }

    public CharacterTravelResponse TravelCharacterToLocation(CharacterTravel travel)
    {
        validations.ValidateCharacterBeforeTravel(travel);
        return travelLogic.MoveToLocation(travel);
    }

    public Character HireMercenaryForCharacter(CharacterHireMercenary hireMercenary)
    {
        validations.ValidateMercenaryBeforeHire(hireMercenary);
        return npcInteractionLogic.HireMercenary(hireMercenary);
    }

    public Character SellItem(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterItemBeforeSell(tradeItem);
        return itemsLogic.BuyOrSellItem(tradeItem);
    }

    public Character BuyItem(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterItemBeforeBuy(tradeItem);
        return itemsLogic.BuyOrSellItem(tradeItem);
    }

    public Character BuyProvisions(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterBeforeBuyProvisions(tradeItem);
        return itemsLogic.BuyProvisions(tradeItem);
    }

    public Character GiveProvisions(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterBeforeGiveProvisions(tradeItem);
        return itemsLogic.GiveProvisions(tradeItem);
    }

    public Character GiveWealth(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterBeforeGiveWealth(tradeItem);
        return itemsLogic.GiveWealth(tradeItem);
    }

    public Character GiveItem(CharacterTrade tradeItem)
    {
        validations.ValidateCharacterBeforeGiveItem(tradeItem);
        return itemsLogic.GiveItem(tradeItem);
    }

}
