#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using Service_Delegators.Validators;

namespace Service_Delegators;

public class CharacterService : ICharacterService
{
    private readonly IDatabaseManager dbm;
    private readonly CharacterValidator validator;
    private readonly CharacterLogic logic;

    public CharacterService(
        IDatabaseManager manager,
        IDiceRollService diceRollService,
        IItemService itemService)
    {
        dbm = manager;

        var metadata = new CharacterMetadata(dbm);

        validator = new CharacterValidator(dbm, metadata);
        logic = new CharacterLogic(dbm, diceRollService, itemService, metadata);
    }

    public CharacterStub CreateCharacterStub(string playerId)
    {
        validator.ValidateMaxNumberOfCharacters(playerId);

        return logic.CreateStub(playerId);
    }

    public Character SaveCharacterStub(CharacterOrigins origins, string playerId)
    {
        validator.ValidateOriginsOnSaveCharacter(origins, playerId);

        return logic.SaveStub(origins, playerId);
    }

    public Character UpdateCharacter(CharacterUpdate charUpdate, string playerId)
    {
        if      (!string.IsNullOrWhiteSpace(charUpdate.Name)) return ModifyName(charUpdate, playerId);
        else if (!string.IsNullOrWhiteSpace(charUpdate.Stat)) return ModifyStats(charUpdate, playerId);
        else if (!string.IsNullOrWhiteSpace(charUpdate.Skill)) return ModifySkills(charUpdate, playerId);
        else throw new Exception("No changes detected on character update.");
    }

    public void DeleteCharacter(string characterId, string playerId)
    {
        validator.ValidateCharacterOnDelete(characterId, playerId);

        logic.DeleteCharacter(characterId, playerId);
    }

    public Characters GetCharacters(string playerName)
    {
        var player = dbm.Metadata.GetPlayerByName(playerName);

        return new Characters
        {
            Count = player.Characters.Count,
            CharactersList = player.Characters
        };
    }

    public Character EquipCharacterItem(CharacterEquip equip, string playerId)
    {
        validator.ValidateCharacterEquipUnequipItem(equip, playerId, true);

        return logic.EquipItem(equip, playerId);
    }

    public Character UnequipCharacterItem(CharacterEquip unequip, string playerId)
    {
        validator.ValidateCharacterEquipUnequipItem(unequip, playerId, false);

        return logic.UnequipItem(unequip, playerId);
    }

    #region private methods
    private Character ModifyName(CharacterUpdate charUpdate, string playerId)
    {
        validator.ValidateCharacterOnNameUpdate(charUpdate, playerId);

        return logic.ChangeName(charUpdate, playerId);
    }

    private Character ModifyStats(CharacterUpdate charUpdate, string playerId)
    {
        validator.ValidateObject(charUpdate);
        validator.ValidateGuid(charUpdate.CharacterId);
        validator.ValidateStatsToDistribute(charUpdate.CharacterId, charUpdate.Stat, playerId);

        return logic.IncreaseStats(charUpdate, playerId);
    }

    private Character ModifySkills(CharacterUpdate charUpdate, string playerId)
    {
        validator.ValidateObject(charUpdate);
        validator.ValidateGuid(charUpdate.CharacterId);
        validator.ValidateSkillsToDistribute(charUpdate.CharacterId, charUpdate.Skill, playerId);

        return logic.IncreaseSkills(charUpdate, playerId);
    }
    #endregion
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.