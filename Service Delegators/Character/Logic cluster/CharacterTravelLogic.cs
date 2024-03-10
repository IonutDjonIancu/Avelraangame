using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Service_Delegators;

public interface ICharacterTravelLogic
{
    CharacterTravelResponse MoveToLocation(CharacterTravel travel);
}

public class CharacterTravelLogic : ICharacterTravelLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;
    private readonly IDiceLogicDelegator dice;
    private readonly IGameplayLogicDelegator gameplay;

    public CharacterTravelLogic(
        Snapshot snapshot,
        IDiceLogicDelegator dice,
        IGameplayLogicDelegator gameplay)
    {
        this.snapshot = snapshot;
        this.dice = dice;
        this.gameplay = gameplay;
    }

    public CharacterTravelResponse MoveToLocation(CharacterTravel travel)
    {
        lock (_lock)
        {
            CharacterTravelResponse travelResponse = new();
            var destination = gameplay.GetOrGenerateLocation(travel.Destination);

            var character = ServicesUtils.GetPlayerCharacter(travel.CharacterIdentity, snapshot);
            travelResponse.Character = character;

            var board = snapshot.Battleboards.Find(s => s.Id == character.Status.Gameplay.BattleboardId);

            var currentLocationFullName = ServicesUtils.GetLocationFullNameFromPosition(character.Status.Position);
            var location = GameplayLore.Locations.All.Find(s => s.FullName == currentLocationFullName)!;

            int travelCostPerPerson = CalculateDistanceCost(location.TravelCostFromArada, destination.TravelCostFromArada);
            var totalPeopleInParty = GetTotalPeopleInParty(character, board);
            
            var totalProvisionsCost = travelCostPerPerson * totalPeopleInParty;

            var totalProvisions = GetTotalProvisionsOfParty(character, board);
            if (totalProvisionsCost > totalProvisions) throw new Exception($"Not enough provisions for all the party to travel to {destination.Position.Location}.");

            var effort = dice.Roll_1_to_n(destination.Effort);
            var highestRoll = GetHighestRollOfParty(character, board);

            if (highestRoll <= effort / 10)
            {
                character.Inventory.Provisions -= travelCostPerPerson * 10;
                character.Mercenaries.Clear();

                travelResponse.Result = GameplayLore.Travel.Disastrous;
            }
            else if (highestRoll <= effort / 5)
            {
                character.Inventory.Provisions -= travelCostPerPerson * 5;

                if (character.Mercenaries.Count > 0)
                {
                    var totalMenLost = dice.Roll_1_to_n(character.Mercenaries.Count);
                    for (var i = 0; i < totalMenLost; i++)
                    {
                        character.Mercenaries.RemoveAt(i);
                    }
                }

                travelResponse.Result = GameplayLore.Travel.Grievous;
            }
            else if (highestRoll <= effort / 2)
            {
                character.Inventory.Provisions -= travelCostPerPerson * 2;
                character.Mercenaries.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson * 2);

                travelResponse.Result = GameplayLore.Travel.Adverse;
            }
            else if (highestRoll <= effort)
            {
                character.Inventory.Provisions -= travelCostPerPerson + 1;
                character.Mercenaries.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson);

                travelResponse.Result = GameplayLore.Travel.Unfortunate;
            }
            else if (highestRoll >= 10 * effort)
            {
                // minimum cost accounted for, party lives off the land
                travelResponse.Result = GameplayLore.Travel.Excellent;
            }
            else if (highestRoll >= 5 * effort)
            {
                character.Inventory.Provisions -= travelCostPerPerson / 5 + 1;
                character.Mercenaries.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson / 5 + 1);

                travelResponse.Result = GameplayLore.Travel.Favourable;
            }
            else if (highestRoll >= 2 * effort)
            {
                character.Inventory.Provisions -= travelCostPerPerson / 2 + 1;
                character.Mercenaries.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson / 2 + 1);

                travelResponse.Result = GameplayLore.Travel.Convenient;
            } 
            else
            {
                character.Inventory.Provisions -= travelCostPerPerson / 2 + 1;
                character.Mercenaries.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson / 2 + 1);

                travelResponse.Result = GameplayLore.Travel.Convenient;
            }

            MovePartyToDestination(character, board, destination.Position);

            return travelResponse;
        }
    }

    #region private methods
    private static void MovePartyToDestination(Character mainChr, Battleboard? board, Position destination)
    {
        if (board != null)
        {
            board.GoodGuys.ForEach(s =>
            {
                s.Status.Position = destination;
                s.Mercenaries.ForEach(p => p.Status.Position = destination);
            });
        }
        else
        {
            mainChr.Status.Position = destination;
            mainChr.Mercenaries.ForEach(s => s.Status.Position = destination);
        }
    }
    private int GetHighestRollOfParty(Character mainChr, Battleboard? board)
    {
        var listOfRolls = new List<int>();

        if (board != null)
        {
            listOfRolls.Add(RollForCharacters(board.GoodGuys));
            board.GoodGuys.ForEach(s => listOfRolls.Add(RollForCharacters(s.Mercenaries)));
        }
        else
        {
            listOfRolls.Add(dice.Roll_game_dice(false, CharactersLore.Skills.Travel, mainChr));
            listOfRolls.Add(RollForCharacters(mainChr.Mercenaries));
        }

        return listOfRolls.Max();
    }

    private static int GetTotalProvisionsOfParty(Character mainChr, Battleboard? board)
    {
        var totalProvisions = 0;
        if (board != null)
        {
            board.GoodGuys.ForEach(s =>
            {
                totalProvisions += s.Inventory.Provisions;
                s.Mercenaries.ForEach(p => totalProvisions += p.Inventory.Provisions);
            });
        }
        else
        {
            totalProvisions += mainChr.Inventory.Provisions;
            mainChr.Mercenaries.ForEach(s => totalProvisions += s.Inventory.Provisions);
        }

        return totalProvisions;
    }

    private static int GetTotalPeopleInParty(Character mainChr, Battleboard? board)
    {
        var totalPeopleInParty = 0;

        if (board != null)
        {
            totalPeopleInParty += board.GoodGuys.Count;
            board.GoodGuys.ForEach(s => totalPeopleInParty += s.Mercenaries.Count);
        }
        else
        {
            totalPeopleInParty += mainChr.Mercenaries.Count + 1; // including the party lead
        }

        return totalPeopleInParty;
    }

    private static int CalculateDistanceCost(int travelFromCost, int destinationToCost)
    {
        var value = travelFromCost - destinationToCost;

        return 1 + value <= 0 ? value * (-1) : value;
    }

    private int RollForCharacters(List<Character> chars)
    {
        var highestRoll = 0;

        chars.ForEach(chr =>
        {
            var roll = dice.Roll_game_dice(false, CharactersLore.Skills.Travel, chr);
            if (roll > highestRoll) highestRoll = roll;
        });

        return highestRoll;
    }
    #endregion
}
