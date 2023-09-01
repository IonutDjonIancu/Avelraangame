using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;

namespace Service_Delegators;

public interface IValidations
{
    #region api
    string ValidateApiRequest(Request request);
    #endregion

    #region database
    void ValidateDatabaseExportImportOperations(string requesterId);
    void ValidateDatabasePlayerImport(string requesterId, string playerJson);
    #endregion

    #region player
    void ValidatePlayerCreate(string playerName);
    void ValidatePlayerLogin(PlayerLogin login);
    void ValidatePlayerUpdateName(string newPlayerName, string playerId);
    void ValidatePlayerDelete(string playerId);
    #endregion
}

public class Validations : IValidations
{
    private readonly AppSettings appSettings;
    private readonly Snapshot snapshot;
    private readonly object _lock = new();

    public Validations(
        Snapshot snapshot,
        AppSettings appSettings)
    {
        this.snapshot = snapshot;
        this.appSettings = appSettings;
    }

    #region api validations
    public string ValidateApiRequest(Request request)
    {
        lock (_lock)
        {
            var player = GetPlayerByName(request.PlayerName);

            if (appSettings.AdminData.Banned.Contains(player.Identity.Name.ToLower())) throw new Exception("Player is banned.");
            if (request.Token != player.Identity.Token) throw new Exception("Token mismatch.");

            return player.Identity.Id;
        }
    }
    #endregion

    #region database validations
    public void ValidateDatabaseExportImportOperations(string requesterId)
    {
        lock (_lock)
        {
            ValidatePlayerIsAdmin(requesterId);
        }
    }

    public void ValidateDatabasePlayerImport(string requesterId, string playerJson)
    {
        ValidateDatabaseExportImportOperations(requesterId);

        try
        {
            JsonConvert.DeserializeObject<Player>(playerJson);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to parse player json string. JsonConvert threw error: {ex}");
        }
    }
    #endregion

    #region player validations
    public void ValidatePlayerCreate(string playerName)
    {
        lock (_lock)
        {
            ValidateString(playerName);
            if (playerName.Length > 20) throw new Exception($"Player name: {playerName} is too long, 20 characters max.");
            if (snapshot.Players.Count >= 20) throw new Exception("Server has reached the limit number of players, please contact admins.");

            // we don't care for player misspelling their names
            // names will be unique during creation
            if (snapshot.Players.Exists(p => p.Identity.Name.ToLower() == playerName.ToLower())) throw new Exception("Name unavailable.");
        }
    }

    public void ValidatePlayerLogin(PlayerLogin login)
    {
        lock (_lock)
        {
            ValidateObject(login);
            ValidateString(login.PlayerName);
            ValidateString(login.Code);

            // we don't care for player misspelling their names
            // names will be unique during creation
            if (!snapshot.Players.Exists(p => p.Identity.Name.ToLower() == login.PlayerName.ToLower())) throw new Exception("Player does not exist.");
        }
    }

    public void ValidatePlayerUpdateName(string newPlayerName, string playerId)
    {
        lock (_lock)
        {
            ValidateString(newPlayerName);
            ValidatePlayerExists(playerId);
        }
    }

    public void ValidatePlayerDelete(string playerId)
    {
        lock (_lock)
        {
            ValidatePlayerExists(playerId);
        }
    }
    #endregion

    #region item validations
    public static void CreateItemWithTypeAndSubtype(string type, string subtype)
    {
        ValidateString(type); 
        ValidateString(subtype);

        if (!ItemsLore.Types.All.Contains(type)) throw new Exception("Wrong item type.");
        if (!ItemsLore.Subtypes.All.Contains(subtype)) throw new Exception("Wrong item subtype.");
    }


    #endregion

    #region character validations
    public void ValidateCharacterUpdateName(string name, CharacterIdentity identity)
    {
        lock (_lock)
        {
            ValidateString(name);
            if (name.Length >= 20) throw new Exception("Character name too long.");

            ValidateCharacterPlayerCombination(identity);
            ValidateCharacterIsLocked(identity);
        }
    }

    public void ValidateCharacterMaxNrAllowed(string playerId)
    {
        lock (_lock)
        {
            var playerCharsCount = snapshot.Players.Find(p => p.Identity.Id == playerId)!.Characters.Where(s => s.Status.IsAlive).ToList().Count;
            
            if (playerCharsCount >= 5) throw new Exception("Max number of alive characters reached (5 alive characters allowed per player).");
        }
    }

    internal void ValidateCharacterCreateTraits(CharacterTraits traits, string playerId)
    {
        lock (_lock)
        {
            ValidateObject(traits);
            
            if (!snapshot.Stubs!.Exists(s => s.PlayerId == playerId)) throw new Exception("No stub templates found for this player.");

            ValidateRace(traits.Race);
            ValidateCulture(traits.Culture);
            ValidateTradition(traits.Tradition);
            ValidateClass(traits.Class);

            ValidateRaceCultureCombination(traits);
        }
    }

    internal void ValidateCharacterBeforeDelete(CharacterIdentity charIdentity)
    {
        lock (_lock) 
        { 
            ValidateObject(charIdentity);
            if (GetPlayerCharacter(charIdentity).Status.IsLockedToModify) throw new Exception("Unable to delete character at this time.");
        }
    }

    internal void ValidateCharacterEquipUnequipItem(CharacterEquip equip, bool toEquip)
    {
        lock (_lock)
        {
            ValidateObject(equip);
            ValidateGuid(equip.CharacterIdentity.Id);
            ValidateGuid(equip.ItemId);
            ValidateString(equip.InventoryLocation);

            ValidateCharacterPlayerCombination(equip.CharacterIdentity);
            ValidateCharacterIsLocked(equip.CharacterIdentity);

            var character = GetPlayerCharacter(equip.CharacterIdentity);
            if (!ItemsLore.InventoryLocation.All.Contains(equip.InventoryLocation)) throw new Exception("Equipment location does not fit any possible slot in inventory.");

            if (!toEquip) return;

            var itemSubtype = (character!.Inventory.Supplies!.Find(i => i.Identity.Id == equip.ItemId)?.Subtype) ?? throw new Exception("No such item found on this character.");
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
                    throw new Exception("Heraldry is full, unequip some of the items first.");
                }
            }
            else
            {
                isItemAtCorrectLocation = false;
            }

            if (!isItemAtCorrectLocation) throw new Exception("Item is being equipped at incorrect location.");
        }
    }
    #endregion

    #region private methods
    private static void ValidateString(string str, string message = "")
    {
        if (string.IsNullOrWhiteSpace(str)) throw new Exception(message.Length > 0 ? message : "The provided string is invalid.");
    }

    private static void ValidateObject(object? obj, string message = "")
    {
        if (obj == null) throw new Exception(message.Length > 0 ? message : $"Object found null.");
    }

    private static void ValidateNumberGreaterThanZero(int num, string message = "")
    {
        if (num <= 0) throw new Exception(message.Length > 0 ? message : "Number cannot be smaller or equal to zero.");
    }

    private static void ValidateGuid(string str, string message = "")
    {
        ValidateString(str);

        var isGuidValid = Guid.TryParse(str, out var id);

        if (!isGuidValid) throw new Exception(message.Length > 0 ? message : "Invalid guid.");

        if (id == Guid.Empty) throw new Exception("Guid cannot be an empty guid.");
    }

    private void ValidatePlayerExists(string playerId)
    {
        ValidateString(playerId);
        GetPlayer(playerId);
    }

    private void ValidatePlayerIsAdmin(string playerId)
    {
        ValidatePlayerExists(playerId);

        var playerName = GetPlayer(playerId)!.Identity.Name;

        if (!appSettings.AdminData.Admins.Contains(playerName)) throw new Exception("Player is not an admin.");
    }

    private void ValidateClass(string classes)
    {
        ValidateString(classes, "Invalid class string.");
        if (!CharactersLore.Classes.All.Contains(classes)) throw new Exception($"Class {classes} not found.");
    }

    private void ValidateCulture(string culture)
    {
        ValidateString(culture, "Invalid culture string.");
        if (!CharactersLore.Cultures.All.Contains(culture)) throw new Exception($"Culture {culture} not found.");
    }

    private void ValidateTradition(string tradition)
    {
        ValidateString(tradition, "Invalid tradition string.");
        if (!CharactersLore.Tradition.All.Contains(tradition)) throw new Exception($"Tradition {tradition} not found.");
    }

    private void ValidateRace(string race)
    {
        ValidateString(race, "Invalid race string.");
        if (!CharactersLore.Races.Playable.All.Contains(race)) throw new Exception($"Race {race} not found.");
    }

    private static void ValidateRaceCultureCombination(CharacterTraits origins)
    {
        string message = "Invalid race culture combination";

        if (origins.Race == CharactersLore.Races.Playable.Human)
        {
            if (!CharactersLore.Cultures.Human.All.Contains(origins.Culture)) throw new Exception(message);
        }
        else if (origins.Race == CharactersLore.Races.Playable.Elf)
        {
            if (!CharactersLore.Cultures.Elf.All.Contains(origins.Culture)) throw new Exception(message);
        }
        else if (origins.Race == CharactersLore.Races.Playable.Dwarf)
        {
            if (!CharactersLore.Cultures.Dwarf.All.Contains(origins.Culture)) throw new Exception(message);
        }
    }

    private void ValidateCharacterIsLocked(CharacterIdentity identity)
    {
        if (GetPlayerCharacter(identity).Status.IsLockedToModify) throw new Exception("Cannot modify character at this time");
    }

    private void ValidateCharacterPlayerCombination(CharacterIdentity identity)
    {
        var player = GetPlayer(identity.PlayerId);
        if (!player.Characters.Exists(s => s.Identity.Id == identity.Id)) throw new Exception("Character does not match player.");
    }

    private Player GetPlayer(string playerId)
    {
        return snapshot.Players.Find(s => s.Identity.Id == playerId) ?? throw new Exception("PLayer not found.");
    }

    private Player GetPlayerByName(string name)
    {
        return snapshot.Players.Find(s => s.Identity.Name == name) ?? throw new Exception("PLayer not found.");
    }

    private Character GetPlayerCharacter(CharacterIdentity charIdentity)
    {
        return GetPlayer(charIdentity.PlayerId).Characters.Find(s => s.Identity.Id == charIdentity.Id) ?? throw new Exception("Character not found.");
    }
    #endregion
}
