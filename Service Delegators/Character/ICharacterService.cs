﻿using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterService
{
    CharacterStub CreateCharacterStub(string playerId);
    Character SaveCharacterStub(CharacterOrigins origins, string playerId);

    Character UpdateCharacter(CharacterUpdate charUpdate, string playerId);
    void DeleteCharacter(string characterId, string playerId);

    Characters GetCharacters(string playerId);   

    Character EquipCharacterItem(CharacterEquip equip, string playerId);
    Character UnequipCharacterItem(CharacterEquip unequip, string playerId);

    CharacterPaperdoll GetCharacterPaperdoll(string characterId, string playerId);

    Character LearnHeroicTrait(CharacterHeroicTrait trait, string playerId);
    List<HeroicTrait> GetHeroicTraits();   

    Rulebook GetRulebook();
}