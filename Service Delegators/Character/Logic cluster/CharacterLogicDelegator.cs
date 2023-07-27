using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterLogicDelegator
{
    private readonly CharacterSheetLogic charSheetLogic;
    private readonly CharacterTraitsLogic charTraitsLogic;
    private readonly CharacterLevelupLogic charLevelupLogic;
    private readonly CharacterItemsLogic charItemsLogic;
    private readonly CharacterCreateLogic charCreateLogic;
    private readonly CharacterPaperdollLogic charPaperdollLogic;
    private readonly CharacterInfoLogic charInfoLogic;
    private readonly CharacterTravelLogic charTravelLogic;
    private readonly CharacterNpcInteraction charNpcInteractionLogic;

    private CharacterLogicDelegator() { }
    internal CharacterLogicDelegator(
        IDatabaseService databaseService,
        IDiceRollService diceRollService,
        IItemService itemService)
    {
        charSheetLogic = new CharacterSheetLogic(diceRollService);
        charInfoLogic = new CharacterInfoLogic(databaseService);
        charItemsLogic = new CharacterItemsLogic(databaseService);
        charLevelupLogic = new CharacterLevelupLogic(databaseService);
        charPaperdollLogic = new CharacterPaperdollLogic(databaseService, diceRollService);
        charTraitsLogic = new CharacterTraitsLogic(databaseService, charPaperdollLogic);
        charCreateLogic = new CharacterCreateLogic(databaseService, diceRollService, itemService, charSheetLogic);
        charTravelLogic = new CharacterTravelLogic(databaseService, diceRollService, charPaperdollLogic);
        charNpcInteractionLogic = new CharacterNpcInteraction(databaseService);
    }

    internal CharacterStub CreateStub(string playerId)
    {
        return charCreateLogic.CreateStub(playerId);
    }

    internal Character SaveStub(CharacterOrigins origins, string playerId)
    {
        return charCreateLogic.SaveStub(origins, playerId);
    }

    internal Character ChangeName(string name, CharacterIdentity identity)
    {
       return charInfoLogic.ChangeName(name, identity);
    }

    internal Character AddFame(string fame, CharacterIdentity identity)
    {
        return charInfoLogic.AddFame(fame, identity);
    }

    internal Character AddWealth(int wealth, CharacterIdentity identity)
    {
        return charInfoLogic.AddWealth(wealth, identity);
    }

    internal Character IncreaseStats(string stat, CharacterIdentity identity)
    {
        return charLevelupLogic.IncreaseStats(stat, identity);
    }

    internal Character IncreaseSkills(string skill, CharacterIdentity identity)
    {
       return charLevelupLogic.IncreaseSkills(skill, identity);
    }
    
    internal void KillChar(CharacterIdentity identity)
    {
        charInfoLogic.KillChar(identity);
    }

    internal void DeleteChar(CharacterIdentity identity)
    {
        charInfoLogic.DeleteChar(identity);
    }

    internal Character EquipItem(CharacterEquip equip)
    { 
        return charItemsLogic.EquipItem(equip);
    }

    internal Character UnequipItem(CharacterEquip unequip)
    {
        return charItemsLogic.UnequipItem(unequip);
    }

    internal Character ApplyHeroicTrait(CharacterHeroicTrait trait)
    {
        return charTraitsLogic.ApplyHeroicTrait(trait);
    }

    internal CharacterPaperdoll CalculatePaperdollForCharacterIdentity(CharacterIdentity identity)
    {
        return charPaperdollLogic.CalculatePaperdoll(identity);
    }

    internal CharacterPaperdoll CalculatePaperdollForCharacterNpc(CharacterIdentity identity, string npcId)
    {
        return charPaperdollLogic.CalculatePaperdoll(identity, npcId);
    }

    internal CharacterPaperdoll CalculatePaperdollForCharacter(ICharacter character)
    {
        return charPaperdollLogic.CalculatePaperdoll(character);
    }

    internal int PaperdollRoll(string attributeRolled, Character character)
    {
        return charPaperdollLogic.PaperdollDiceRoll(attributeRolled, character);
    }

    internal void MoveToLocation(CharacterTravel positionTravel)
    {
        charTravelLogic.MoveToLocation(positionTravel);
    }

    internal void MercenaryHire(CharacterHireMercenary hireMercenary)
    {
        charNpcInteractionLogic.MercenaryHire(hireMercenary);
    }
}
