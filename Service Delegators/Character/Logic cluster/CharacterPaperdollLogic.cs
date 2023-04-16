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
        var paperdoll = new CharacterPaperdoll(dbm.Snapshot.Rulebook.Acronyms);

        #region apply bonuses from items to stats
        var itemStrBonus = 0;
        var itemConBonus = 0;
        var itemAgiBonus = 0;
        var itemWilBonus = 0;
        var itemPerBonus = 0;
        var itemAbsBonus = 0;
        
        if (items.Count > 0)
        {
            itemStrBonus = items.Sum(s => s.Sheet.Stats.Strength);
            itemConBonus = items.Sum(s => s.Sheet.Stats.Constitution);
            itemAgiBonus = items.Sum(s => s.Sheet.Stats.Agility);
            itemWilBonus = items.Sum(s => s.Sheet.Stats.Willpower);
            itemPerBonus = items.Sum(s => s.Sheet.Stats.Perception);
            itemAbsBonus = items.Sum(s => s.Sheet.Stats.Abstract);
        }
        
        paperdoll.Stats = new CharacterStats
        {
            Strength        = character.Sheet.Stats.Strength + itemStrBonus,
            Constitution    = character.Sheet.Stats.Constitution + itemConBonus,
            Agility         = character.Sheet.Stats.Agility + itemAgiBonus,
            Willpower       = character.Sheet.Stats.Willpower + itemWilBonus,
            Perception      = character.Sheet.Stats.Perception + itemPerBonus,
            Abstract        = character.Sheet.Stats.Abstract + itemAbsBonus
        };
        #endregion

        #region calculate assets
        paperdoll.Assets = new CharacterAssets
        {
            Resolve   = paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.Assets.ResolveFormula),
            Harm        = paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.Assets.HarmFormula),
            Defense     = paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.Assets.DefenseFormula),
            Purge       = paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.Assets.PurgeFormula),
            Spot        = paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.Assets.SpotFormula),
            Mana        = paperdoll.InterpretFormula(dbm.Snapshot.Rulebook.Assets.ManaFormula)
        };
        #endregion

        #region apply bonuses from items to assets
        var itemEndBonus = 0;
        var itemHarBonus = 0;
        var itemDefBonus = 0;
        var itemPurBonus = 0;
        var itemSpoBonus = 0;
        var itemHeaBonus = 0;
        var itemManBonus = 0;
        #endregion
        // calculate skills
        // apply bonuses from items to skills

        // cater in for HT which modify stats, skills or assets

        return paperdoll;
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.