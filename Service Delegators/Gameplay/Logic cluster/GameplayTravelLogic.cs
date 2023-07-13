using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayTravelLogic
{
    private readonly IDatabaseService dbs;
    private readonly IDiceRollService diceService;

    private GameplayTravelLogic() { }
    internal GameplayTravelLogic(
        IDatabaseService databaseService,
        IDiceRollService diceRollService)
    {
        dbs = databaseService;
        diceService = diceRollService;
    }

    internal void MoveToLocation(PositionTravel positionTravel)
    {
        var character = Utils.GetPlayerCharacter(dbs, positionTravel.CharacterIdentity);

        var currentLocationFullName = Utils.GetLocationFullName(character.Position);
        var location = GameplayLore.Map.All.Find(s => s.FullName == currentLocationFullName)!;
        var destinationLocationFullName = Utils.GetLocationFullName(positionTravel.Destination);
        var destination = GameplayLore.Map.All.Find(s => s.FullName == destinationLocationFullName)!;

        int travelCostPerPerson = GetDistance(location.TravelCostFromArada, destination.TravelCostFromArada);
        var totalPeopleInParty = character.Henchmen.Count + 1; // including the party lead
        var totalProvisionsCost = travelCostPerPerson * totalPeopleInParty;
        var totalProvisions = character.Inventory.Provisions + character.Henchmen.Select(s => s.Inventory.Provisions).Sum();
        if (totalProvisionsCost > totalProvisions) throw new Exception($"Not enough provisions for all the party to travel to {destination.Position.Location}.");

        var effort = diceService.Roll_1_to_n(destination.Effort);
        var (grade, crits) = diceService.Roll_gameplay_dice(character.Info.Origins.Tradition, character.Sheet.Skills.Travel);












    }

    #region private methods
    public static int GetDistance(int travelFromCost, int destinationToCost)
    {
        var value = travelFromCost - destinationToCost;

        return 1 + value <= 0 ? value * (-1) : value;
    }
    #endregion
}
