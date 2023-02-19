using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal static class CharacterOperations
{
    internal static int SumStats(CharacterPaperDoll doll)
    {
        return doll.Strength
            + doll.Constitution
            + doll.Agility
            + doll.Willpower
            + doll.Perception
            + doll.Abstract;
    }

    internal static int SumSkills(CharacterPaperDoll doll)
    {
        return doll.Combat
            + doll.Arcane
            + doll.Psionics
            + doll.Hide
            + doll.Traps
            + doll.Tactics
            + doll.Social
            + doll.Apothecary
            + doll.Travel
            + doll.Sail;
    }
}
