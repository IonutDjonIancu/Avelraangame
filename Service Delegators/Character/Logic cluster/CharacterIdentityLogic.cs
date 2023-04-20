#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterIdentityLogic
{
    private readonly IDatabaseManager dbm;

    public CharacterIdentityLogic(
        IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
    }

    internal Character ChangeName(CharacterUpdate charUpdate, string playerId)
    {
        var oldChar = dbm.Metadata.GetCharacterById(charUpdate.CharacterId, playerId);

        oldChar.Identity.Name = charUpdate.Name;

        var player = dbm.Metadata.GetPlayerById(playerId);

        dbm.PersistPlayer(player);

        return oldChar;
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.