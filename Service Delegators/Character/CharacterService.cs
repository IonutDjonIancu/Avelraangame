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
        return logic.CreateStub(playerId);
    }

    public Character SaveCharacterStub(CharacterOrigins origins, string playerId)
    {
        validator.ValidateOriginsOnSaveCharacter(origins, playerId);

        return logic.SaveStub(origins, playerId);
    }

    public Character UpdateCharacterName(CharacterUpdate charUpdate, string playerId)
    {
        validator.ValidateCharacterOnNameUpdate(charUpdate, playerId);

        return logic.ChangeName(charUpdate, playerId);
    }

    public void DeleteCharacter(string characterId, string playerId)
    {
        validator.ValidateCharacterOnDelete(characterId, playerId);

        logic.DeleteCharacter(characterId, playerId);
    }

    public List<Character> GetCharacters(string playerName)
    {
        return dbm.Snapshot.Players.Find(p => p.Identity.Name == playerName).Characters;
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8603 // Possible null reference return.