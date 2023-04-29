namespace Data_Mapping_Containers.Dtos;

public static class QuestsLore
{
    public static class Difficulty
    {
        public const string Easy = "Easy";
        public const string Medium = "Medium";
        public const string Normal = "Normal";
        public const string Hard = "Hard";

        public static readonly List<string> All = new()
        {
            Easy, Medium, Normal, Hard
        };
    }

    public static class NpcType
    {
        public const string Monster = "Monster";
        public const string Animal = "Animal";
        public const string Humanoid = "Humanoid";
        public const string Fiend = "Fiend";
        public const string Elemental = "Elemental";

        public static readonly List<string> All = new()
        {
            Monster, Animal, Humanoid, Fiend, Elemental
        };

    }
}
