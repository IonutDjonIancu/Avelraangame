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
        var fullName = Utils.GetLocationFullNameFromPosition(position);

        var locationData = GameplayLore.Locations.All.Find(s => s.FullName == fullName)!;

        var location = dbs.Snapshot.Locations.Find(s => s.FullName == fullName);

        if (location != null)
        {
            var isDateDifference = (location!.LastTimeVisited - DateTime.Now) > new TimeSpan(0);

            if (!isDateDifference)
            {
                return location;
            }
            else
            {
                dbs.Snapshot.Locations.Remove(location!);
                
                location.LastTimeVisited = DateTime.Now;

                location.PossibleQuests = GetPossibleQuests(position, locationData.Effort);
                location.Market = GenerateMarketItems(location.Effort);
                location.Mercenaries = GenerateMercenaries(position, location.Effort);
            }
        }
        else
        {
            location = new Location()
            {
                FullName = locationData.FullName,
                Description = locationData.Description,
                Effort = locationData.Effort,
                TravelCostFromArada = locationData.TravelCostFromArada,
                LastTimeVisited = DateTime.Now,
                PossibleQuests = GetPossibleQuests(position, locationData.Effort),
                Market = GenerateMarketItems(locationData.Effort),
                Mercenaries = GenerateMercenaries(position, locationData.Effort)
            };
        }

        dbs.Snapshot.Locations.Add(location);

        return location;
    }

    #region private methods
    private static List<string> GetPossibleQuests(Position position, int effortUpper)
    {
        return GameplayLore.Quests.All.Where(s => s.AvailableAt.Contains(position.Land) && s.EffortRequired <= effortUpper).Select(s => s.Name).ToList();
    }

    private List<Item> GenerateMarketItems(int effortUpper)
    {
        var items = new List<Item>();
        var nrOfItems = diceService.Roll_1_to_n(effortUpper / 2);

        for (int i = 0; i < nrOfItems; i++)
        {
            items.Add(itemsService.GenerateRandomItem());
        }

        return items;
    }

    private List<Character> GenerateMercenaries(Position position, int effortUpper)
    {
        var mercs = new List<Character>();
        var rollForMercs = diceService.Roll_1_to_n(effortUpper / 10);
        var nrOfMercs = rollForMercs < 1 ? 1 : rollForMercs;

        for (int i = 0; i < nrOfMercs; i++)
        {
            mercs.Add(npcsService.GenerateGoodGuyNpc(Utils.GetLocationByPosition(position).LocationName));
        }

        return mercs;
    }
    #endregion
}
