using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterService
{
    #region character gets
    Characters GetPlayerCharacters(string playerId);
    #endregion

    #region char creation
    CharacterStub CreateCharacterStub(string playerId);
    Character SaveCharacterStub(CharacterTraits traits, string playerId);
    #endregion

    #region character sheet
    // character info
    Character UpdateCharacterName(string name, CharacterIdentity identity);
    Character UpdateCharacterFame(string fame, CharacterIdentity identity);
    Character UpdateCharacterWealth(int wealth, CharacterIdentity identity);
    
    // character lvlup
    Character UpdateCharacterStats(string stat, CharacterIdentity identity);
    Character UpdateCharacterAssets(string asset, CharacterIdentity identity);
    Character UpdateCharacterSkills(string skill, CharacterIdentity identity);
    #endregion

    #region character termination
    void KillCharacter(CharacterIdentity identity);
    void DeleteCharacter(CharacterIdentity identity);
    #endregion

    #region character actions
    Character CharacterEquipItem(CharacterEquip equip);
    Character CharacterUnequipItem(CharacterEquip unequip);
    Character CharacterLearnHeroicTrait(CharacterSpecialSkillAdd trait);
    void CharacterTravelToLocation(CharacterTravel positionTravel);
    void CharacterHireMercenary(CharacterHireMercenary hireMercenary);
    #endregion
}