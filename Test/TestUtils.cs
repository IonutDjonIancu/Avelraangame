﻿namespace Tests;

public static class TestUtils
{
    internal static string CreatePlayer(string name, IPlayerLogicDelegator _players, Snapshot _snapshot)
    {
        var playerData = new PlayerData
        {
            PlayerName = name.ToLower()
        };

        _players.CreatePlayer(playerData);
        
        return _snapshot.Players.Find(s => s.Identity.Name == name.ToLower())!.Identity.Id;
    }

    internal static Player CreateAndGetPlayer(string name, IPlayerLogicDelegator _players, Snapshot _snapshot)
    {
        CreatePlayer(name.ToLower(), _players, _snapshot);

        return _snapshot.Players.Find(s => s.Identity.Name == name.ToLower())!;
    }

    internal static Player GetPlayer(string id, Snapshot _snapshot)
    {
        return _snapshot.Players.Find(s => s.Identity.Id == id) ?? throw new Exception("Player not found.");
    }

    internal static Character CreateAndGetCharacter(string playerName, IPlayerLogicDelegator _players, ICharacterLogicDelegator _characters, Snapshot _snapshot)
    {
        var traits = new CharacterRacialTraits
        {
            Race = CharactersLore.Races.Playable.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Class = CharactersLore.Classes.Warrior,
            Tradition = CharactersLore.Tradition.Martial
        };

        var playerId = CreatePlayer(playerName.ToLower(), _players, _snapshot);
        _characters.CreateCharacterStub(playerId, "");
        return _characters.SaveCharacterStub(traits, playerId);
    }

    internal static Character GetCharacter(string characterId, string playerId, Snapshot snapshot)
    {
        var player = GetPlayer(playerId, snapshot);

        return player.Characters.Find(s => s.Identity.Id == characterId) ?? throw new Exception("Character not found.");
    }

    internal static CharacterIdentity GetCharacterIdentity(Character character)
    {
        return new CharacterIdentity
        {
            Id = character.Identity.Id,
            PlayerId = character.Identity.PlayerId,
        };
    }

    internal static CharacterData GetCharacterData(Character character)
    {
        return new CharacterData
        {
            CharacterId = character.Identity.Id,
            PlayerId = character.Identity.PlayerId,
            CharacterName = character.Status.Name
        };
    }
}
