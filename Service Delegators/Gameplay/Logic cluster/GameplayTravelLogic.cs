using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayTravelLogic
{
    private readonly IDatabaseService dbs;

    private GameplayTravelLogic() { }
    internal GameplayTravelLogic(IDatabaseService databaseService)
    {
        dbs = databaseService;
    }

    internal void GoToLocation(PositionTravel positionTravel)
    {
        //    var partyLead = Utils.GetPlayerCharacter(dbs, positionTravel.CharacterIdentity);

        //    var currentLocationFullName = Utils.GetLocationFullName(partyLead.Position);
        //    var location = GameplayLore.Map.All.Find(s => s.FullName == currentLocationFullName)!;

        //    var destinationLocationFullName = Utils.GetLocationFullName(positionTravel.Destination);
        //    var destination = GameplayLore.Map.All.Find(s => s.FullName == destinationLocationFullName)!;

        //    int travelCostPerPerson = 1;

        //    if (location.)
        //    {

        //    }

        throw new NotImplementedException();



    }

    #region private methods
    public static int GetDistance(int travelFromCost, int travelToCost)
    {
        var value = travelFromCost - travelToCost;

        return 1 + value <= 0 ? value * (-1) : value;
    }
    #endregion
}
