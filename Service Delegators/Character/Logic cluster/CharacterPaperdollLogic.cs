#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterPaperdollLogic
{
    private readonly IDatabaseManager dbm;

    internal CharacterPaperdollLogic(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
    }

    internal CharacterPaperdoll CalculatePaperdoll(string characterId, string playerId)
    {
        var character = dbm.Metadata.GetCharacterById(characterId, playerId);

        var items = character.Inventory.GetAllEquipedItems();

        int strBonus = 0;
        
        if (items.Count > 0)
        {
            strBonus = items.Sum(s => s.Sheet.Stats.Strength);
        }



        var paperdoll = new CharacterPaperdoll
        {
            Stats = new CharacterStats
            {
                Strength = character.Sheet.Stats.Strength + strBonus,
            }
        };

        return paperdoll;
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.