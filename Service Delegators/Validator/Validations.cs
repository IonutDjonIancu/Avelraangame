﻿using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;

namespace Service_Delegators;

public interface IValidations
{
    #region api
    string ApiRequest(Request request);
    #endregion

    #region database
    void AtDatabaseExportImportOperations(string requesterId);
    void AtPlayerImport(string requesterId, string playerJson);
    #endregion

    #region player
    void CreatePlayer(string playerName);
    void LoginPlayer(PlayerLogin login);
    void UpdatePlayerName(string newPlayerName, string playerId);
    void DeletePlayer(string playerId);
    #endregion
}

public class Validations : IValidations
{
    private readonly AppSettings appSettings;
    private readonly Snapshot snapshot;
    private readonly object playersLock = new();

    public Validations(
        Snapshot snapshot,
        AppSettings appSettings)
    {
        this.snapshot = snapshot;
        this.appSettings = appSettings;
    }

    #region api validations
    public string ApiRequest(Request request)
    {
        lock (playersLock)
        {
            var player = snapshot.Players.Find(p => p.Identity.Name == request.PlayerName) ?? throw new Exception("Player not found.");

            if (appSettings.AdminData.Banned.Contains(player.Identity.Name.ToLower())) throw new Exception("Player is banned.");
            if (request.Token != player.Identity.Token) throw new Exception("Token mismatch.");

            return player.Identity.Id;
        }
    }
    #endregion

    #region database validations
    public void AtDatabaseExportImportOperations(string requesterId)
    {
        ValidatePlayerIsAdmin(requesterId);
    }

    public void AtPlayerImport(string requesterId, string playerJson)
    {
        AtDatabaseExportImportOperations(requesterId);

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
    public void CreatePlayer(string playerName)
    {
        ValidateString(playerName);
        if (playerName.Length > 20) throw new Exception($"Player name: {playerName} is too long, 20 characters max.");

        lock (playersLock)
        {
            if (snapshot.Players.Count >= 20) throw new Exception("Server has reached the limit number of players, please contact admins.");
            if (snapshot.Players.Exists(p => p.Identity.Name.ToLower() == playerName.ToLower())) throw new Exception("Name unavailable.");
        }
    }

    public void LoginPlayer(PlayerLogin login)
    {
        ValidateObject(login);
        ValidateString(login.PlayerName);
        ValidateString(login.Code);

        lock (playersLock)
        {
            // we don't care for player misspelling their names
            // names will be unique anyway at creation
            if (!snapshot.Players.Exists(p => p.Identity.Name.ToLower() == login.PlayerName.ToLower())) throw new Exception("Player does not exist.");
        }
    }

    public void UpdatePlayerName(string newPlayerName, string playerId)
    {
        ValidateString(newPlayerName);
        ValidatePlayerExists(playerId);
    }

    public void DeletePlayer(string playerId)
    {
        ValidatePlayerExists(playerId);
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
    public void ValidateMaxNumberOfCharacters(string playerId)
    {
        lock (playersLock)
        {
            var playerCharsCount = snapshot.Players.Find(p => p.Identity.Id == playerId)!.Characters.Where(s => s.Status.IsAlive).ToList().Count;
            
            if (playerCharsCount >= 5) throw new Exception("Max number of alive characters reached (5 alive characters allowed per player).");
        }
    }

    internal void ValidateTraitsOnSaveCharacter(CharacterTraits traits, string playerId)
    {
        ValidateObject(traits);

        lock (playersLock)
        {
            if (!snapshot.Stubs!.Exists(s => s.PlayerId == playerId)) throw new Exception("No stub templates found for this player.");

        }

        ValidateRace(traits.Race);
        ValidateCulture(traits.Culture);
        ValidateTradition(traits.Tradition);
        ValidateClass(traits.Class);

        ValidateRaceCultureCombination(traits);
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

        lock (playersLock)
        {
            if (!snapshot.Players.Exists(p => p.Identity.Id == playerId)) throw new Exception("Player not found.");
        }
    }

    private void ValidatePlayerIsAdmin(string playerId)
    {
        ValidatePlayerExists(playerId);

        lock(playersLock)
        {
            var playerName = snapshot.Players.Find(s => s.Identity.Id == playerId)!.Identity.Name;

            if (!appSettings.AdminData.Admins.Contains(playerName)) throw new Exception("Player is not an admin.");
        }
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
    #endregion
}
