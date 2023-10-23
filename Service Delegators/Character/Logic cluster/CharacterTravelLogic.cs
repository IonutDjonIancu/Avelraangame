using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

public interface ICharacterTravelLogic
{
    Character MoveToLocation(CharacterTravel travel);
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

    public Character MoveToLocation(CharacterTravel travel)
    {
        lock (_lock)
        {
            var character = Utils.GetPlayerCharacter(travel.CharacterIdentity, snapshot);

            var currentLocationFullName = Utils.GetLocationFullNameFromPosition(character.Status.Position);
            var location = GameplayLore.Locations.All.Find(s => s.FullName == currentLocationFullName)!;
            var destinationLocationFullName = Utils.GetLocationFullNameFromPosition(travel.Destination);
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
            }
            else if (highestRoll <= effort / 2)
            {
                character.Inventory.Provisions -= travelCostPerPerson * 2;
                character.Mercenaries.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson * 2);
            }
            else if (highestRoll <= effort)
            {
                character.Inventory.Provisions -= travelCostPerPerson + 1;
                character.Mercenaries.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson);
            }
            else if (highestRoll >= 10 * effort)
            {
                // minimum cost accounted for, party lives off the land
            }
            else if (highestRoll >= 5 * effort)
            {
                character.Inventory.Provisions -= travelCostPerPerson / 5 + 1;
                character.Mercenaries.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson / 5);
            }
            else if (highestRoll >= 2 * effort)
            {
                character.Inventory.Provisions -= travelCostPerPerson / 2;
                character.Mercenaries.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson / 2);
            }

            character.Inventory.Provisions -= 1;
            character.Mercenaries.ForEach(s => s.Inventory.Provisions -= 1);

            return character;
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
