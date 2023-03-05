using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterService
{
    CharacterStub CreateCharacterStub(string playerId);
    Character SaveCharacterStub(CharacterOrigins origins, string playerId);

    Character UpdateCharacter(CharacterUpdate charUpdate, string playerId);
    void DeleteCharacter(string characterId, string playerId);

    Characters GetCharacters(string playerName);   
}