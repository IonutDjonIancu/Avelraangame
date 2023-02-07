#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;
using Service_Delegators.Validators;

namespace Service_Delegators;

public class CharacterService : ICharacterService
{
    private readonly IDatabaseManager dbm;
    private readonly CharacterLogic logic;
    private readonly CharacterValidator validator;

    public CharacterService(
        IDatabaseManager manager,
        IDiceRollService diceRollService,
        IItemService itemService)
    {
        dbm = manager;

        var metadata = new CharacterMetadata(dbm);

        logic = new CharacterLogic(dbm, diceRollService, itemService, metadata);
        validator = new CharacterValidator(dbm, metadata);
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

    public Character UpdateCharacter(CharacterUpdate charUpdate, string playerId)
    {
        validator.ValidateCharacterOnUpdate(charUpdate, playerId);

        return logic.UpdateCharacter(charUpdate, playerId);
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