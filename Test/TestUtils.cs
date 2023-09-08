using System.Xml.Linq;

namespace Tests;

public static class TestUtils
{
    internal static string CreatePlayer(string name, IPlayerLogicDelegator _players, Snapshot _snapshot)
    {
        _players.CreatePlayer(name);
        
        return _snapshot.Players.Find(s => s.Identity.Name == name)!.Identity.Id;
    }

    internal static Player CreateAndGetPlayer(string name, IPlayerLogicDelegator _players, Snapshot _snapshot)
    {
        CreatePlayer(name, _players, _snapshot);

        return _snapshot.Players.Find(s => s.Identity.Name == name)!;
    }

    internal static Player GetPlayer(string id, Snapshot _snapshot)
    {
        return _snapshot.Players.Find(s => s.Identity.Id == id) ?? throw new Exception("Player not found.");
    }

    internal static Character CreateAndGetCharacter(string playerName, IPlayerLogicDelegator _players, ICharacterLogicDelegator _characters, Snapshot _snapshots)
    {
        var traits = new CharacterTraits
        {
            Race = CharactersLore.Races.Playable.Human,
            Culture = CharactersLore.Cultures.Human.Danarian,
            Class = CharactersLore.Classes.Warrior,
            Tradition = CharactersLore.Tradition.Martial
        };

        var playerId = CreatePlayer(playerName, _players, _snapshots);
        _characters.CreateCharacterStub(playerId);
        return _characters.SaveCharacterStub(traits, playerId);
    }

    internal static CharacterIdentity GetCharacterIdentity(Character character)
    {
        return new CharacterIdentity
        {
            Id = character.Identity.Id,
            PlayerId = character.Identity.PlayerId,
        };
    }
}
