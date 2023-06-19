using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public class CharacterService : ICharacterService
{
    private readonly IDatabaseService dbs;
    private readonly CharacterValidator validator;
    private readonly CharacterLogicDelegator logic;

    public CharacterService(
        IDatabaseService databaseService,
        IDiceRollService diceRollService,
        IItemService itemService)
    {
        dbs = databaseService;
        validator = new CharacterValidator(databaseService.Snapshot);
        logic = new CharacterLogicDelegator(databaseService, diceRollService, itemService);
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

    public Character UpdateCharacterName(string name, CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        validator.ValidateCharacterName(name);
        validator.ValidateIfPartyIsAdventuring(identity.Id);
        return logic.ChangeName(name, identity);
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
        validator.ValidateIfPartyIsAdventuring(identity.Id);
        validator.ValidateNumber(wealth);
        return logic.AddWealth(wealth, identity);
    }

    public Character UpdateCharacterStats(string stat, CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        validator.ValidateIfPartyIsAdventuring(identity.Id);
        validator.ValidateStatExists(stat);
        validator.ValidateCharacterHasStatsPoints(identity);
        return logic.IncreaseStats(stat, identity);
    }

    public Character UpdateCharacterSkills(string skill, CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        validator.ValidateIfPartyIsAdventuring(identity.Id);
        validator.ValidateSkillExists(skill);
        validator.ValidateCharacterHasSkillsPoints(identity);
        return logic.IncreaseSkills(skill, identity);
    }

    public void KillCharacter(CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        logic.KillChar(identity);
    }

    public void DeleteCharacter(CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        validator.ValidateIfCharacterInParty(identity.Id);
        logic.DeleteChar(identity);
    }

    public Characters GetPlayerCharacters(string playerId)
    {
        var characters = dbs.Snapshot.Players.Find(p => p.Identity.Id == playerId)!.Characters;

        return new Characters
        {
            Count = characters!.Count,
            CharactersList = characters
        };
    }

    public Character EquipCharacterItem(CharacterEquip equip)
    {
        validator.ValidateCharacterEquipUnequipItem(equip, true);
        validator.ValidateIfPartyIsAdventuring(equip.CharacterId);
        return logic.EquipItem(equip);
    }

    public Character UnequipCharacterItem(CharacterEquip unequip)
    {
        validator.ValidateCharacterEquipUnequipItem(unequip, false);
        validator.ValidateIfPartyIsAdventuring(unequip.CharacterId);
        return logic.UnequipItem(unequip);
    }

    public CharacterPaperdoll CalculatePaperdollForPlayerCharacter(CharacterIdentity identity)
    {
        validator.ValidateCharacterPlayerCombination(identity);
        return logic.CalculatePaperdollForCharacter(identity);
    }

    public CharacterPaperdoll CalculatePaperdoll(Character character)
    {
        return logic.CalculatePaperdollOnly(character);
    }

    public Character LearnHeroicTrait(CharacterHeroicTrait trait)
    {
        validator.ValidateCharacterLearnHeroicTrait(trait);
        validator.ValidateIfPartyIsAdventuring(trait.CharacterId);
        return logic.ApplyHeroicTrait(trait);
    }
}
