using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal static class CharacterOperations
{
    internal static int SumStats(CharacterSheet sheet)
    {
        return sheet.Strength
            + sheet.Constitution
            + sheet.Agility
            + sheet.Willpower
            + sheet.Perception
            + sheet.Abstract;
    }

    internal static int SumSkills(CharacterSheet sheet)
    {
        return sheet.Combat
            + sheet.Arcane
            + sheet.Psionics
            + sheet.Hide
            + sheet.Traps
            + sheet.Tactics
            + sheet.Social
            + sheet.Apothecary
            + sheet.Travel
            + sheet.Sail;
    }
}
