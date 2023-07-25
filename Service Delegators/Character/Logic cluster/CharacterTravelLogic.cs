using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class CharacterTravelLogic
{
    private readonly IDatabaseService dbs;
    private readonly IDiceRollService diceService;
    private readonly CharacterPaperdollLogic paperdollLogic;

    private CharacterTravelLogic() { }
    internal CharacterTravelLogic(
        IDatabaseService databaseService,
        IDiceRollService diceRollService,
        CharacterPaperdollLogic characterPaperdollLogic)
    {
        dbs = databaseService;
        paperdollLogic = characterPaperdollLogic;
        diceService = diceRollService;
    }

    internal void MoveToLocation(CharacterTravel positionTravel)
    {
        var character = Utils.GetPlayerCharacter(dbs, positionTravel.CharacterIdentity);

        var currentLocationFullName = Utils.GetLocationFullName(character.Position);
        var location = GameplayLore.Map.All.Find(s => s.FullName == currentLocationFullName)!;
        var destinationLocationFullName = Utils.GetLocationFullName(positionTravel.Destination);
        var destination = GameplayLore.Map.All.Find(s => s.FullName == destinationLocationFullName)!;
        
        int travelCostPerPerson = CalculateDistanceCost(location.TravelCostFromArada, destination.TravelCostFromArada);
        var totalPeopleInParty = character.Henchmen.Count + 1; // including the party lead
        var totalProvisionsCost = travelCostPerPerson * totalPeopleInParty;
        var totalProvisions = character.Inventory.Provisions + character.Henchmen.Select(s => s.Inventory.Provisions).Sum();
        if (totalProvisionsCost > totalProvisions) throw new Exception($"Not enough provisions for all the party to travel to {destination.Position.Location}.");
        
        var effort = diceService.Roll_1_to_n(destination.Effort);
        var listOfRolls = new List<int>();

        var charPaperdoll = paperdollLogic.CalculateCharPaperdoll(character);
        var characterRoll = diceService.Roll_gameplay_dice(character.Info.Origins.Tradition, charPaperdoll.Skills.Travel);
        paperdollLogic.LevelUpChar(characterRoll.crits, character);
        character.Position = destination.Position;

        listOfRolls.Add(characterRoll.grade);

        foreach (var npc in character.Henchmen)
        {
            var npcPaperdoll = paperdollLogic.CalculateNpcPaperdoll(npc);
            var npcRoll = diceService.Roll_gameplay_dice(npc.Info.Origins.Tradition, npcPaperdoll.Skills.Travel);
            paperdollLogic.LevelUpNpc(npcRoll.crits, npc);
            npc.Position = destination.Position;

            listOfRolls.Add(npcRoll.grade);
        }

        var highestRoll = listOfRolls.Max();

        if (highestRoll <= effort / 10)
        {
            character.Inventory.Provisions -= travelCostPerPerson * 10 + 1;
            character.Henchmen.Clear();
        }
        else if (highestRoll <= effort / 5)
        {
            character.Inventory.Provisions -= travelCostPerPerson * 5 + 1;

            if (character.Henchmen.Count > 0)
            {
                var totalMenLost = diceService.Roll_1_to_n(character.Henchmen.Count);
                for (var i = 0; i < totalMenLost; i++)
                {
                    character.Henchmen.RemoveAt(i);
                }                                                     
            }
        }
        else if (highestRoll <= effort / 2)
        {
            character.Inventory.Provisions -= travelCostPerPerson * 2 + 1;
            character.Henchmen.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson * 2 + 1);
        }
        else if (highestRoll <= effort)
        {
            character.Inventory.Provisions -= travelCostPerPerson + 1;
            character.Henchmen.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson + 1);
        }
        else if (highestRoll >= 10 * effort)
        {
            character.Inventory.Provisions -= 1;
            character.Henchmen.ForEach(s => s.Inventory.Provisions -= 1);
        }
        else if (highestRoll >= 5 * effort)
        {
            character.Inventory.Provisions -= travelCostPerPerson / 5 + 1;
            character.Henchmen.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson / 5 + 1);
        }
        else if (highestRoll >= 2 * effort)
        {
            character.Inventory.Provisions -= travelCostPerPerson / 2;
            character.Henchmen.ForEach(s => s.Inventory.Provisions -= travelCostPerPerson / 2);
        }
        dbs.PersistPlayer(character.Identity.PlayerId);
    }

    #region private methods
    public static int CalculateDistanceCost(int travelFromCost, int destinationToCost)
    {
        var value = travelFromCost - destinationToCost;

        return 1 + value <= 0 ? value * (-1) : value;
    }
    #endregion
}
