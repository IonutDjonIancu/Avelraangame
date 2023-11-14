namespace Data_Mapping_Containers.Lore;

public class CharactersLore
{
    public static class Races
    {
        public class Playable
        {
            public const string Human = "Human";
            public const string Elf = "Elf";
            public const string Dwarf = "Dwarf";
            public const string Orc = "Orc";
            //public const string Exoplanar = "Exoplanar";

            public static readonly List<string> All = new()
            {
                Human, Elf, Dwarf, Orc
            };
        }

        public class NonPlayable
        {
            public const string Undead = "Undead";
            public const string Animal = "Animal";

            public static readonly List<string> All = new()
            {
                Undead, Animal
            };
        }

        //TODO: add the rest of the races
        public static readonly List<string> All = new()
        {
            Playable.Human,
            Playable.Elf,
            Playable.Dwarf,
            Playable.Orc,

            NonPlayable.Animal,
            NonPlayable.Undead
        };
    }

    public static class Cultures
    {
        public static class Human
        {
            public const string Danarian = "Danarian";
            //public const string Midheim = "Midheim";
            //public const string Ravanian = "Ravanian";
            //public const string Endarian = "Endarian";
            //public const string Calvinian = "Calvinian";

            public static readonly List<string> All = new()
            {
                Danarian, /*Midheim, Ravanian, Endarian, Calvinian*/
            };
        }

        public static class Elf
        {
            public const string Highborn = "Highborn";

            public static readonly List<string> All = new()
            {
                Highborn
            };
        }

        public static class Dwarf
        {
            public const string Undermountain = "Undermountain";

            public static readonly List<string> All = new()
            {
                Undermountain
            };
        }

        public static class Orc
        {
            public const string Greenskin = "Greenskin";

            public static readonly List<string> All = new()
            {
                Greenskin
            };
        }

        public static class Animal
        {
            public const string Direwolf = "Werewolf";
            public const string Direbear = "Werebear";

            public static readonly List<string> All = new()
            {
                Direwolf, Direbear
            };
        }

        public static class Undead
        {
            public const string Zombie = "Zombie";
            public const string Skeleton = "Skeleton";

            public static readonly List<string> All = new()
            {
                Zombie, Skeleton
            };
        }

        public static readonly List<string> All = new()
        {
            Human.Danarian,

            Elf.Highborn,

            Dwarf.Undermountain,

            Orc.Greenskin,

            Animal.Direwolf,
            Animal.Direbear,

            Undead.Zombie,
            Undead.Skeleton
        };
    }

    public static class Tradition
    {
        public const string Martial = "Martial";
        public const string Common = "Common";

        public static readonly List<string> All = new()
        {
            Common, Martial
        };
    }

    public static class Classes
    {
        public const string Warrior = "Warrior";
        public const string Mage = "Mage";
        public const string Hunter = "Hunter";
        public const string Swashbuckler = "Swashbuckler";
        public const string Sorcerer = "Sorcerer";

        public static readonly List<string> All = new()
        {
            Warrior, Mage, Hunter, Swashbuckler, Sorcerer
        };
    }

    public static class Stats
    {
        public const string Strength = "Strength";
        public const string Constitution = "Constitution";
        public const string Agility = "Agility";
        public const string Willpower = "Willpower";
        public const string Perception = "Perception";
        public const string Abstract = "Abstract";

        public static readonly List<string> All = new()
        {
            Strength, Constitution, Willpower, Agility, Perception, Abstract
        };
    }

    public static class Assets
    {
        public const string Resolve = "Resolve";
        public const string Harm = "Harm";
        public const string Spot = "Spot";
        public const string Defense = "Defense";
        public const string Purge = "Purge";
        public const string Mana = "Mana";
        public const string Actions = "Actions";

        public static readonly List<string> All = new()
        {
            Resolve, Harm, Spot, Defense, Purge, Mana, Actions
        };
    }

    public static class Skills
    {
        public const string Melee = "Melee";
        public const string Arcane = "Arcane";
        public const string Psionics = "Psionics";
        public const string Hide = "Hide";
        public const string Traps = "Traps";
        public const string Tactics = "Tactics";
        public const string Social = "Social";
        public const string Apothecary = "Apothecary";
        public const string Travel = "Travel";
        public const string Sail = "Sail";

        public static readonly List<string> All = new()
        {
            Melee, Arcane, Psionics, Hide, Traps, Tactics, Social, Apothecary, Travel, Sail
        };
    }

    public static class AttributeTypes
    {
        public const string Stats = "Stats";
        public const string Assets = "Assets";
        public const string Skills = "Skills";

        public static readonly List<string> All = new() 
        { 
            Stats, Assets, Skills 
        };
    }
}
