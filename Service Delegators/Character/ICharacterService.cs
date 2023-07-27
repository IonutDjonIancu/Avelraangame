using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterService
{
    #region character gets
    Characters GetPlayerCharacters(string playerId);
    #endregion

    #region char creation
    CharacterStub CreateCharacterStub(string playerId);
    Character SaveCharacterStub(CharacterOrigins origins, string playerId);
    #endregion

    #region updates of character sheet
    // character info
    Character UpdateCharacterName(string name, CharacterIdentity identity);
    Character UpdateCharacterFame(string fame, CharacterIdentity identity);
    Character UpdateCharacterWealth(int wealth, CharacterIdentity identity);
    
    // character lvlup
    Character UpdateCharacterStats(string stat, CharacterIdentity identity);
    Character UpdateCharacterSkills(string skill, CharacterIdentity identity);
    #endregion

    #region character termination
    void KillCharacter(CharacterIdentity identity);
    void DeleteCharacter(CharacterIdentity identity);
    #endregion

    #region paperdoll
    CharacterPaperdoll CalculatePaperdollForPlayerCharacter(CharacterIdentity identity);
    CharacterPaperdoll CalculatePaperdollForCharacter(ICharacter character);
    int CharacterPaperdollRoll(string attributeRolled, Character character);
    #endregion

    #region character actions
    Character CharacterEquipItem(CharacterEquip equip);
    Character CharacterUnequipItem(CharacterEquip unequip);
    Character CharacterLearnHeroicTrait(CharacterHeroicTrait trait);
    void CharacterTravelToLocation(CharacterTravel positionTravel);
    void CharacterHireMercenary(CharacterHireMercenary hireMercenary);
    #endregion
}