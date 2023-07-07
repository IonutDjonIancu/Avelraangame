using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class GameplayQuestsLogic
{
    private readonly IDatabaseService dbs;
    private readonly IDiceRollService diceService;
    private readonly IItemService itemsService;
    private readonly INpcService npcsService;

    private GameplayQuestsLogic() { }
    internal GameplayQuestsLogic(
        IDatabaseService databaseService,
        IDiceRollService diceRollService,
        IItemService itemService,
        INpcService npcService)
    {
        dbs = databaseService;
        diceService = diceRollService;
        itemsService = itemService;
        npcsService = npcService;
    }

    internal Location GenerateLocation(Position position)
    {
        var fullName = Utils.GetLocationFullName(position);

        var locationData = GameplayLore.MapLocations.All.Find(s => s.FullName == fullName)!;

        var location = dbs.Snapshot.Map.Locations.Find(s => s.FullName == fullName);

        if (location != null)
        {
            var isDateDifference = (location!.LastTimeVisited - DateTime.Now) > new TimeSpan(0);

            if (!isDateDifference)
            {
                return location;
            }
            else
            {
                dbs.Snapshot.Map.Locations.Remove(location!);
                
                location.LastTimeVisited = DateTime.Now;

                //// generate new quests
                //currentLocation.Quests = ...;

                location.Market = GenerateMarketItems(location.EffortUpper);
                location.Npcs = GenerateMercenaries(position, location.EffortUpper);
            }
        }
        else
        {
            location = new Location()
            {
                Name = locationData.Name,
                FullName = locationData.FullName,
                Description = locationData.Description,
                Effort = locationData.Effort,
                EffortLower = locationData.EffortLower,
                EffortUpper = locationData.EffortUpper,
                TravelToCost = locationData.TravelToCost,
                LastTimeVisited = DateTime.Now,
                //// generate new quests
                //Quests = ...,
                Market = GenerateMarketItems(locationData.EffortUpper),
                Npcs = GenerateMercenaries(position, locationData.EffortUpper)
            };
        }

        dbs.Snapshot.Map.Locations.Add(location);
        dbs.PersistLocation(location.FullName);

        return location;
    }

    #region private methods
    private List<Item> GenerateMarketItems(int effortUpper)
    {
        var items = new List<Item>();
        var nrOfItems = diceService.Roll_1dX(effortUpper / 2);

        for (int i = 0; i < nrOfItems; i++)
        {
            items.Add(itemsService.GenerateRandomItem());
        }

        return items;
    }

    private List<NpcCharacter> GenerateMercenaries(Position position, int effortUpper)
    {
        var mercs = new List<NpcCharacter>();
        var rollForMercs = diceService.Roll_1dX(effortUpper / 10);
        var nrOfMercs = rollForMercs < 1 ? 1 : rollForMercs;

        for (int i = 0; i < nrOfMercs; i++)
        {
            mercs.Add(npcsService.GenerateGoodGuyNpc(position, effortUpper));
        }

        return mercs;
    }
    #endregion
}
