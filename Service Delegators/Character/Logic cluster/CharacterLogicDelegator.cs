#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterLogicDelegator
{
    private readonly IDatabaseManager dbm;
    private readonly IDiceRollService dice;

    private readonly CharacterSheetLogic charSheet;
    private readonly CharacterTraitsLogic charTraits;
    private readonly CharacterLevelupLogic charLevelup;
    private readonly CharacterItemsLogic charItems;
    private readonly CharacterCreateLogic charCreate;
    private readonly CharacterIdentityLogic charIdentity;

    private CharacterLogicDelegator() { }

    internal CharacterLogicDelegator(
        IDatabaseManager databaseManager,
        IDiceRollService diceRollService,
        IItemService itemService,
        CharacterMetadata charMetadata)
    {
        dbm = databaseManager;
        dice = diceRollService;

        charSheet = new CharacterSheetLogic(dbm, dice);
        charTraits = new CharacterTraitsLogic(dbm, charMetadata);
        charLevelup = new CharacterLevelupLogic(dbm, charMetadata);
        charItems = new CharacterItemsLogic(dbm, charMetadata);
        charCreate = new CharacterCreateLogic(dbm, dice, itemService, charSheet);
        charIdentity = new CharacterIdentityLogic(dbm, charMetadata);
    }

    internal CharacterStub CreateStub(string playerId)
    {
        return charCreate.CreateStub(playerId);
    }

    internal Character SaveStub(CharacterOrigins origins, string playerId)
    {
        return charCreate.SaveStub(origins, playerId);
    }

    internal Character ChangeName(CharacterUpdate charUpdate, string playerId)
    {
       return charIdentity.ChangeName(charUpdate, playerId);
    }

    internal Character IncreaseStats(CharacterUpdate charUpdate, string playerId)
    {
        return charLevelup.IncreaseStats(charUpdate, playerId);
    }

    internal Character IncreaseSkills(CharacterUpdate charUpdate, string playerId)
    {
       return charLevelup.IncreaseSkills(charUpdate, playerId);
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
        return charItems.EquipItem(equip, playerId);
    }

    internal Character UnequipItem(CharacterEquip unequip, string playerId)
    {
        return charItems.UnequipItem(unequip, playerId);
    }

    internal Character ApplyHeroicTrait(CharacterHeroicTrait trait, string playerId)
    {
        return charTraits.ApplyHeroicTrait(trait, playerId);
    }

}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.