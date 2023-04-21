#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterLogicDelegator
{
    private readonly IDatabaseManager dbm;

    private readonly CharacterSheetLogic charSheetLogic;
    private readonly CharacterTraitsLogic charTraitsLogic;
    private readonly CharacterLevelupLogic charLevelupLogic;
    private readonly CharacterItemsLogic charItemsLogic;
    private readonly CharacterCreateLogic charCreateLogic;
    private readonly CharacterIdentityLogic charIdentityLogic;
    private readonly CharacterPaperdollLogic charPaperdollLogic;

    private CharacterLogicDelegator() { }

    internal CharacterLogicDelegator(
        IDatabaseManager databaseManager,
        IDiceRollService diceRollService,
        IItemService itemService)
    {
        dbm = databaseManager;

        charPaperdollLogic = new CharacterPaperdollLogic(databaseManager);
        charLevelupLogic = new CharacterLevelupLogic(databaseManager);
        charItemsLogic = new CharacterItemsLogic(databaseManager);
        charIdentityLogic = new CharacterIdentityLogic(databaseManager);
        charTraitsLogic = new CharacterTraitsLogic(databaseManager, charPaperdollLogic);
        charSheetLogic = new CharacterSheetLogic(databaseManager, diceRollService);
        charCreateLogic = new CharacterCreateLogic(databaseManager, diceRollService, itemService, charSheetLogic);
    }

    internal CharacterStub CreateStub(string playerId)
    {
        return charCreateLogic.CreateStub(playerId);
    }

    internal Character SaveStub(CharacterOrigins origins, string playerId)
    {
        return charCreateLogic.SaveStub(origins, playerId);
    }

    internal Character ChangeName(CharacterUpdate charUpdate, string playerId)
    {
       return charIdentityLogic.ChangeName(charUpdate, playerId);
    }

    internal Character IncreaseStats(CharacterUpdate charUpdate, string playerId)
    {
        return charLevelupLogic.IncreaseStats(charUpdate, playerId);
    }

    internal Character IncreaseSkills(CharacterUpdate charUpdate, string playerId)
    {
       return charLevelupLogic.IncreaseSkills(charUpdate, playerId);
    }

    internal void DeleteCharacter(string characterId, string playerId)
    {
        var player = dbm.Metadata.GetPlayerById(playerId);
        var character = player.Characters.Find(c => c.Identity.Id == characterId);

        player.Characters.Remove(character);
        
        dbm.PersistPlayer(player);
    }

    internal Character EquipItem(CharacterEquip equip, string playerId)
    { 
        return charItemsLogic.EquipItem(equip, playerId);
    }

    internal Character UnequipItem(CharacterEquip unequip, string playerId)
    {
        return charItemsLogic.UnequipItem(unequip, playerId);
    }

    internal Character ApplyHeroicTrait(CharacterHeroicTrait trait, string playerId)
    {
        return charTraitsLogic.ApplyHeroicTrait(trait, playerId);
    }

    internal CharacterPaperdoll CalculatePaperdoll(string characterId, string playerId)
    {
        return charPaperdollLogic.CalculatePaperdoll(characterId, playerId);
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.