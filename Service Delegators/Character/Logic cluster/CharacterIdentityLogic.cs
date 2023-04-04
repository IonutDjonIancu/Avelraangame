#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterIdentityLogic
{
    private readonly IDatabaseManager dbm;
    private readonly CharacterMetadata charMetadata;

    public CharacterIdentityLogic(
        IDatabaseManager databaseManager,
        CharacterMetadata characterMetadata)
    {
        charMetadata = characterMetadata;
        dbm = databaseManager;
    }

    internal Character ChangeName(CharacterUpdate charUpdate, string playerId)
    {
        var oldChar = charMetadata.GetCharacter(charUpdate.CharacterId, playerId);

        oldChar.Identity.Name = charUpdate.Name;

        var player = dbm.Metadata.GetPlayerById(playerId);

        dbm.PersistPlayer(player);

        return oldChar;
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.