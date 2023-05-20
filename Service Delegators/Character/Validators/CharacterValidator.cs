using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterValidator : ValidatorBase
{
    private readonly IDatabaseService dbs;

    private CharacterValidator() { }
    internal CharacterValidator(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

    internal void ValidateMaxNumberOfCharacters(string playerId)
    {
        var playerCharsCount = dbs.Snapshot.Players.Find(p => p.Identity.Id == playerId)!.Characters!.Count;

        if (playerCharsCount >= 5) Throw("Max number of characters reached (5 characters allowed per player)");
    }

    internal void ValidateOriginsOnSaveCharacter(CharacterOrigins origins, string playerId)
    {
        ValidateObject(origins);
        
        if (!dbs.Snapshot.CharacterStubs!.Exists(s => s.PlayerId == playerId)) Throw("No stub templates found for this player.");

        ValidateRace(origins.Race);
        ValidateCulture(origins.Culture);
        ValidateHeritage(origins.Heritage);
        ValidateClass(origins.Class);

        ValidateRaceCultureCombination(origins);
    }

    internal void ValidateCharacterLearnHeroicTrait(CharacterHeroicTrait trait)
    {
        ValidateCharacterPlayerCombination(new CharacterIdentity() { Id = trait.CharacterId, PlayerId = trait.PlayerId });
        ValidateGuid(trait.HeroicTraitId);
        if (trait.Skill != null)
        {
            ValidateString(trait.Skill);
            if (!CharactersLore.Skills.All.Contains(trait.Skill)) Throw("No such Skill was found with the indicated skill name.");
        }

        var character = dbs.Snapshot.Players.First(p => p.Identity.Id == trait.PlayerId).Characters.First(c => c.Identity.Id == trait.CharacterId);
        var heroicTrait = dbs.Snapshot.Traits.Find(t => t.Identity.Id == trait.HeroicTraitId) ?? throw new Exception("No such Heroic Trait found with the provided id.");

        if (heroicTrait.DeedsCost > character.LevelUp.DeedsPoints) Throw("Character does not have enough Deeds points to aquire said Heroic Trait.");

        if (heroicTrait.Subtype == TraitsLore.Subtype.onetime
            && character.HeroicTraits.Exists(t => t.Identity.Id == heroicTrait.Identity.Id)) Throw("Character already has that Heroic Trait and it can only be learned once.");
    }

    internal void ValidateCharacterEquipUnequipItem(CharacterEquip equip, bool toEquip)
    {
        ValidateObject(equip);
        ValidateGuid(equip.CharacterId);
        ValidateGuid(equip.ItemId);
        ValidateCharacterPlayerCombination(new CharacterIdentity() { Id = equip.CharacterId, PlayerId = equip.PlayerId });
        ValidateString(equip.InventoryLocation);
        if (!ItemsLore.InventoryLocation.All.Contains(equip.InventoryLocation)) Throw("Equipment location does not fit any possible slot in inventory.");

        if (!toEquip) return;

        var player = dbs.Snapshot.Players.Find(s => s.Identity.Id == equip.PlayerId);
        var character = player!.Characters.Find(s => s.Identity!.Id == equip.CharacterId);
        var itemSubtype = character!.Supplies!.Find(i => i.Identity.Id == equip.ItemId)?.Subtype;
        if (itemSubtype == null) Throw("No such item found on this character.");

        bool isItemAtCorrectLocation;

        // protection
        if (itemSubtype == ItemsLore.Subtypes.Protections.Helm)
        {
            isItemAtCorrectLocation =
                equip.InventoryLocation == ItemsLore.InventoryLocation.Head;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Protections.Armour)
        {
            isItemAtCorrectLocation =
                equip.InventoryLocation == ItemsLore.InventoryLocation.Body;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Protections.Shield)
        {
            isItemAtCorrectLocation =
                 equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand;
        }
        //weapons
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Sword)
        {
            isItemAtCorrectLocation =
                equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Pike)
        {
            isItemAtCorrectLocation =
                equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Crossbow)
        {
            isItemAtCorrectLocation =
                equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Polearm)
        {
            isItemAtCorrectLocation =
               equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Mace)
        {
            isItemAtCorrectLocation =
                equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Axe)
        {
            isItemAtCorrectLocation =
                equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand ||
                equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Dagger)
        {
            isItemAtCorrectLocation =
                equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
                equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand ||
                equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Bow)
        {
            isItemAtCorrectLocation =
                equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Sling)
        {
            isItemAtCorrectLocation =
               equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
               equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
        }
        else if (itemSubtype == ItemsLore.Subtypes.Weapons.Spear)
        {
            isItemAtCorrectLocation =
               equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand ||
               equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged;
        }
        // wealth
        else if (itemSubtype == ItemsLore.Subtypes.Wealth.Gems ||
            itemSubtype == ItemsLore.Subtypes.Wealth.Valuables ||
            itemSubtype == ItemsLore.Subtypes.Wealth.Trinket)
        {
            isItemAtCorrectLocation =
               equip.InventoryLocation == ItemsLore.InventoryLocation.Heraldry;

            if (character.Inventory!.Heraldry!.Count >= 5)
            {
                Throw("Heraldry is full, unequip some of the items first.");
            }
        }
        else
        {
            isItemAtCorrectLocation = false;
        }

        if (!isItemAtCorrectLocation) Throw("Item is being equipped at incorrect location.");
    }

    internal void ValidatePartyOnJoin(string partyId)
    {
        ValidateGuid(partyId);

        if (!dbs.Snapshot.Parties.Exists(p => p.Id == partyId)) Throw("This party does not exist.");
    }

    internal void ValidateCharacterPlayerCombination(CharacterIdentity identity)
    {
        ValidateGuid(identity.Id);
        ValidateGuid(identity.PlayerId);

        var player = dbs.Snapshot.Players.Find(p => p.Identity.Id == identity.PlayerId)!;

        if (!player.Characters.Exists(c => c.Identity!.Id == identity.Id)) Throw("Character not found.");
    }

    internal void ValidateStatExists(string stat)
    {
        ValidateString(stat);
        if (!CharactersLore.Stats.All.Contains(stat)) Throw($"Stat {stat} does not math any possible character stats.");
    }

    internal void ValidateCharacterHasStatsPoints(CharacterIdentity chr)
    {
        var hasPoints = dbs.Snapshot.Players.Find(p => p.Identity.Id == chr.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Id)!.LevelUp.StatPoints > 0;

        if (!hasPoints) Throw($"Character does not have any stat points to distribute.");
    }

    internal void ValidateCharacterHasSkillsPoints(CharacterIdentity chr)
    {
        var hasPoints = dbs.Snapshot.Players.Find(p => p.Identity.Id == chr.PlayerId)!.Characters.Find(c => c.Identity.Id == chr.Id)!.LevelUp.SkillPoints > 0;

        if (!hasPoints) Throw($"Character does not have any skill points to distribute.");
    }

    internal void ValidateSkillExists(string skill)
    {
        ValidateString(skill);
        if (!CharactersLore.Skills.All.Contains(skill)) Throw($"Skill {skill} does not math any possible character skills.");
    }

    internal void ValidateCharacterName(string name)
    {
        ValidateString(name, "Invalid string for name.");
        if (name.Length < 3) Throw("Character name is too short, minimum of 3 letters allowed.");
        if (name.Length > 30) Throw("Character name is too long, maximum of 30 letters allowed.");
    }

    #region private methods
    private static void ValidateRaceCultureCombination(CharacterOrigins origins)
    {
        string message = "Invalid race culture combination";

        if (origins.Race == CharactersLore.Races.Human)
        {
            if (!CharactersLore.Cultures.Human.All.Contains(origins.Culture)) Throw(message);
        }
        else if (origins.Race == CharactersLore.Races.Elf)
        {
            if (!CharactersLore.Cultures.Elf.All.Contains(origins.Culture)) Throw(message);
        }
        else if (origins.Race == CharactersLore.Races.Dwarf)
        {
            if (!CharactersLore.Cultures.Dwarf.All.Contains(origins.Culture)) Throw(message);
        }
    }

    private void ValidateClass(string classes)
    {
        ValidateString(classes, "Invalid class string.");
        if (!CharactersLore.Classes.All.Contains(classes)) Throw($"Class {classes} not found.");
    }

    private void ValidateHeritage(string heritage)
    {
        ValidateString(heritage, "Invalid heritage string.");
        if (!CharactersLore.Heritage.All.Contains(heritage)) Throw($"Heritage {heritage} not found..");
    }

    private void ValidateCulture(string culture)
    {
        ValidateString(culture, "Invalid culture string.");
        if (!CharactersLore.Cultures.All.Contains(culture)) Throw($"Culture {culture} not found.");
    }

    private void ValidateRace(string race)
    {
        ValidateString(race, "Invalid race string.");
        if (!CharactersLore.Races.All.Contains(race)) Throw($"Race {race} not found.");
    }
    #endregion
}
