using Data_Mapping_Containers;
using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface IGameplayCharactersLogic
{
    Ladder GetCharactersLadder();
}

public class GameplayCharactersLogic : IGameplayCharactersLogic
{
    public readonly object _lock = new();

    public readonly Snapshot snapshot;

    public GameplayCharactersLogic(
        Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public Ladder GetCharactersLadder()
    {
        var ladder = new Ladder();
        var allCharacters = snapshot.Players.SelectMany(s => s.Characters).ToList();

        foreach ( var character in allCharacters )
        {
            var charLadder = new CharacterLadder
            {
                CharacterName = character.Status.Name,
                PlayerName = snapshot.Players.Find(p => p.Identity.Id == character.Identity.PlayerId)!.Identity.Name,
                Race = character.Status.Traits.Race,
                Icon = character.Status.Traits.Icon!.Value,
                Wealth = character.Status.Wealth,
                Worth = character.Status.Worth,
            };
            
            ladder.CharactersByWealth.Add(charLadder);
            ladder.CharactersByWorth.Add(charLadder);
        }

        ladder.CharactersByWealth = ladder.CharactersByWealth.OrderByDescending(s => s.Wealth).ToList();
        ladder.CharactersByWorth = ladder.CharactersByWorth.OrderByDescending(s => s.Worth).ToList();

        return ladder;
    }
}
