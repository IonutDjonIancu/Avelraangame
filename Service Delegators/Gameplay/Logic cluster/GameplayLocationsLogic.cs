﻿using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Service_Delegators;

public interface IGameplayLocationsLogic
{
    Location GetOrGenerateLocation(Position position);
}

public class GameplayLocationsLogic : IGameplayLocationsLogic
{
    public readonly object _lock = new();

    public readonly Snapshot snapshot;
    public readonly IValidations validations;
    public readonly IDiceLogicDelegator dice;
    public readonly IItemsLogicDelegator items;
    public readonly INpcLogicDelegator npcs;
    public readonly IGameplayQuestLogic quests;

    public GameplayLocationsLogic(
        Snapshot snapshot,
        IValidations validations,
        IDiceLogicDelegator dice,
        IItemsLogicDelegator items,
        INpcLogicDelegator npcs,
        IGameplayQuestLogic quests)
    {
        this.snapshot = snapshot;
        this.validations = validations;
        this.dice = dice;
        this.items = items;
        this.npcs = npcs;
        this.quests = quests;
    }

    public Location GetOrGenerateLocation(Position position)
    {
        lock (_lock)
        {
            var fullName = ServicesUtils.GetLocationFullNameFromPosition(position);

            var locationData = GameplayLore.Locations.All.Find(s => s.FullName == fullName)!;

            var location = snapshot.Locations.Find(s => s.FullName == fullName);

            if (location != null)
            {
                var isDateDifference = (DateTime.Parse(location!.LastTimeVisited) - DateTime.Now) > new TimeSpan(0);

                if (!isDateDifference)
                {
                    return location;
                }
                else
                {
                    snapshot.Locations.Remove(location!);

                    location.LastTimeVisited = DateTime.Now.ToShortDateString();

                    location.Quests = quests.GenerateLocationQuests(location.Effort);
                    location.Market = GenerateMarketItems(location.Effort);
                    location.Mercenaries = GenerateMercenaries(position, location.Effort);
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
                    TravelCostFromArada = locationData.TravelCostFromArada,
                    LastTimeVisited = DateTime.Now.ToShortDateString(),
                    Quests = quests.GenerateLocationQuests(locationData.Effort),
                    Market = GenerateMarketItems(locationData.Effort),
                    Mercenaries = GenerateMercenaries(position, locationData.Effort),
                    Position = position
                };
            }

            snapshot.Locations.Add(location);

            return location;
        }
    }

    #region private methods
    private List<Item> GenerateMarketItems(int effortUpper)
    {
        var itemsList = new List<Item>();
        var nrOfItems = dice.Roll_1_to_n(effortUpper / 2);

        for (int i = 0; i < nrOfItems; i++)
        {
            itemsList.Add(items.GenerateRandomItem());
        }

        return itemsList;
    }

    private List<Character> GenerateMercenaries(Position position, int effortUpper)
    {
        var mercs = new List<Character>();
        var rollForMercs = dice.Roll_1_to_n(effortUpper / 10);
        var nrOfMercs = rollForMercs < 1 ? 1 : rollForMercs;

        for (int i = 0; i < nrOfMercs; i++)
        {
            mercs.Add(npcs.GenerateGoodGuy(ServicesUtils.GetLoreLocationByPosition(position).Name));
        }

        return mercs;
    }
    #endregion
}
