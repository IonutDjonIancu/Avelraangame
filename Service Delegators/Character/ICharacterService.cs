using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterService
{
    CharacterStub CreateCharacterStub(string playerId);
    Character SaveCharacterStub(CharacterOrigins origins, string playerId);

    Character UpdateCharacterName(string name, CharacterIdentity identity);
    Character UpdateCharacterFame(string fame, CharacterIdentity identity);
    Character UpdateCharacterWealth(int wealth, CharacterIdentity identity);
    Character UpdateCharacterStats(string stat, CharacterIdentity identity);
    Character UpdateCharacterSkills(string skill, CharacterIdentity identity);
    
    void KillCharacter(CharacterIdentity identity);
    void DeleteCharacter(CharacterIdentity identity);

    Characters GetCharacters(string playerId);   

    Character EquipCharacterItem(CharacterEquip equip);
    Character UnequipCharacterItem(CharacterEquip unequip);

    CharacterPaperdoll CalculatePaperdollForPlayerCharacter(CharacterIdentity identity);
    CharacterPaperdoll CalculatePaperdoll(Character character);

    Character LearnHeroicTrait(CharacterHeroicTrait trait);
}