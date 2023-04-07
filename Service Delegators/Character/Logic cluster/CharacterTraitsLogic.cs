#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

using Data_Mapping_Containers.Dtos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterTraitsLogic
{
    private readonly IDatabaseManager dbm;
    private readonly CharacterMetadata charMetadata;

    public CharacterTraitsLogic(
        IDatabaseManager databaseManager,
        CharacterMetadata charMetadata) 
    {
        this.charMetadata = charMetadata;
        dbm = databaseManager;
    }

    internal Character ApplyHeroicTrait(CharacterHeroicTrait trait, string playerId)
    {
        var character = charMetadata.GetCharacter(trait.CharacterId, playerId);
        var heroicTrait = dbm.Snapshot.Traits.Find(t => t.Identity.Id == trait.HeroicTraitId);

        character.HeroicTraits.Add(heroicTrait);
        character.LevelUp.DeedsPoints -= heroicTrait.DeedsCost;

        if (heroicTrait.Type == TraitsLore.Type.bonus) ApplyBonusHeroicTrait(character, heroicTrait, trait);

        dbm.PersistPlayer(dbm.Metadata.GetPlayerById(playerId));

        return character;
    }

    #region private methods
    private static void ApplyBonusHeroicTrait(Character character, HeroicTrait heroicTrait, CharacterHeroicTrait trait)
    {
        if (heroicTrait.Identity.Name == TraitsLore.BonusTraits.swordsman)
        {
            var value = 10 + (int)Math.Ceiling(character.Sheet.Combat /*character PaperDoll*/ * 0.01); // TODO: should calculate the PaperDoll amount as stated in the HT's description
            character.Sheet.Combat += value;
        }
        else if (heroicTrait.Identity.Name == TraitsLore.BonusTraits.skillful)
        {
            if (trait.Skill == CharactersLore.Skills.Combat)
            {
                var value = (int)Math.Ceiling(character.Sheet.Combat * 0.2);
                character.Sheet.Combat += value;
            }

            // cater for all the other skills as well
        }
    }
    #endregion
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.