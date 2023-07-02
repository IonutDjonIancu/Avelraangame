namespace Data_Mapping_Containers.Dtos;

public static class QuestsLore
{
    #region repeatable quests
    // 1. Mudpaddler of Belford
    private const string mudpaddlerOfBelford_name = "Mudpaddler of Belford";
    private const string mudpaddlerOfBelford_description = "You are summoned by the huscarl captain within the walls of Belford Keep. " +
        "Clad in a chainmail hauberk, adorned with a surcoat displaying his lord's coat of arms, and wearing a helmet that conceals his face save for the visor, he exudes an aura of authority and experience. " +
        "The housecarl captain's demeanor reflects his years of military service and leadership. " +
        "He possesses a commanding presence, exhibiting a stern yet composed expression that instills confidence in his men. " +
        "His voice carries authority, commanding attention. You've known this man since you were a child, you will listen to what he has to say. " +
        "\"A patrol needs to be sent south\" towards Pel'Ravan's forests, he points at the map. \"It's a three day journey to reach all outposts and report back. Gather supplies and depart at dawn.\"";
    #endregion

    public static readonly Quest mudpaddlerOfBelford = new()
    {
        Name = mudpaddlerOfBelford_name,
        Description = mudpaddlerOfBelford_description,
        Difficulty = GameplayLore.Quests.Difficulty.Easy,
        Effort = GameplayLore.Effort.Normal,
        Ratio = new QuestRatio()
        {
            Diplomacy = 5,
            Utilitarian = 15,
            Overcome = 20,

            DiplomacyMultiplier = 4,
            UtilitarianMultiplier = 3,
            OvercomeMultiplier = 2,

            RewardType = GameplayLore.Quests.RewardTypes.Low
        },
        IsRepeatable = true,
        Position = new Position()
        {
            Region = GameplayLore.MapLocations.Dragonmaw.Name,
            Subregion = GameplayLore.MapLocations.Dragonmaw.Farlindor.Name,
            Land = GameplayLore.MapLocations.Dragonmaw.Farlindor.Danar.Name,
            Location = GameplayLore.MapLocations.Dragonmaw.Farlindor.Danar.Locations.Belfordshire.Name
        }
    };

    public static readonly List<Quest> All = new()
    {
        mudpaddlerOfBelford
    };
}
