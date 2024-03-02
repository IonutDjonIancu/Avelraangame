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

    public CharacterTravelLogic(
        Snapshot snapshot,
        IDiceLogicDelegator dice)
    {
        this.snapshot = snapshot;
        this.dice = dice;
    }

    public CharacterTravelResponse MoveToLocation(CharacterTravel travel)
    {
        lock (_lock)
        {
            CharacterTravelResponse travelResponse = new();

            var character = ServicesUtils.GetPlayerCharacter(travel.CharacterIdentity, snapshot);
            travelResponse.Character = character;

            var currentLocationFullName = ServicesUtils.GetLocationFullNameFromPosition(character.Status.Position);
            var location = GameplayLore.Locations.All.Find(s => s.FullName == currentLocationFullName)!;
            var destinationLocationFullName = ServicesUtils.GetLocationFullNameFromPosition(travel.Destination);
            var destination = GameplayLore.Locations.All.Find(s => s.FullName == destinationLocationFullName)!;

            int travelCostPerPerson = CalculateDistanceCost(location.TravelCostFromArada, destination.TravelCostFromArada);
            var totalPeopleInParty = character.Mercenaries.Count + 1; // including the party lead
            var totalProvisionsCost = travelCostPerPerson * totalPeopleInParty;
            var totalProvisions = character.Inventory.Provisions + character.Mercenaries.Select(s => s.Inventory.Provisions).Sum();
            if (totalProvisionsCost > totalProvisions) throw new Exception($"Not enough provisions for all the party to travel to {destination.Position.Location}.");

            var effort = dice.Roll_1_to_n(destination.Effort);
            var listOfRolls = new List<int>();

            var characterRoll = dice.Roll_game_dice(false, CharactersLore.Skills.Travel, character);
            character.Status.Position = destination.Position;
            listOfRolls.Add(characterRoll);

            foreach (var npc in character.Mercenaries)
            {
                var npcRoll = dice.Roll_game_dice(false, CharactersLore.Skills.Travel, npc);
                npc.Status.Position = destination.Position;
                listOfRolls.Add(npcRoll);
            }

            var highestRoll = listOfRolls.Max();

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

            return travelResponse;
        }
    }

    #region private methods
    public static int CalculateDistanceCost(int travelFromCost, int destinationToCost)
    {
        var value = travelFromCost - destinationToCost;

        return 1 + value <= 0 ? value * (-1) : value;
    }
    #endregion
}
