#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using Service_Delegators.Validators;

namespace Service_Delegators;

public class CharacterService : ICharacterService
{
    private readonly IDatabaseManager dbm;
    private readonly CharacterValidator validator;
    private readonly CharacterLogicDelegator logic;

    public CharacterService(
        IDatabaseManager databaseManager,
        IDiceRollService diceRollService,
        IItemService itemService)
    {
        dbm = databaseManager;

        validator = new CharacterValidator(dbm);
        logic = new CharacterLogicDelegator(dbm, diceRollService, itemService);
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

    public Characters GetCharacters(string playerId)
    {
        var player = dbm.Metadata.GetPlayerById(playerId);

        validator.ValidateObject(player);

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

    public Character LearnHeroicTrait(CharacterHeroicTrait trait, string playerId)
    {
        validator.ValidateCharacterLearnHeroicTrait(trait, playerId);

        return logic.ApplyHeroicTrait(trait, playerId);
    }

    public CharacterPaperdoll CalculateCharacterPaperdoll(string characterId, string playerId)
    {
        validator.ValidateCharacterPlayerCombination(characterId, playerId);

        return logic.CalculatePaperdoll(characterId, playerId);
    }

    public CharacterPaperdoll CalculateCharacterPaperdoll(Character character)
    {
        return logic.CalculatePaperdollForNpc(character);
    }

    public List<HeroicTrait> GetHeroicTraits()
    {
        return dbm.Snapshot.Traits;
    }

    public Rulebook GetRulebook()
    {
        return dbm.Snapshot.Rulebook;
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
#pragma warning restore CS8603 // Possible null reference return.