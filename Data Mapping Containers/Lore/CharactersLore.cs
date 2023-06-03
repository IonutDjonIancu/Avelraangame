﻿namespace Data_Mapping_Containers.Dtos;

public class CharactersLore
{
    public static class Races
    {
        public const string Human = "Human";
        public const string Elf = "Elf";
        public const string Dwarf = "Dwarf";

        public static readonly List<string> All = new()
        {
            Human, Elf, Dwarf
        };
    }

    public static class Cultures
    {
        public static class Human
        {
            public const string Danarian = "Danarian";
            public const string Midheim = "Midheim";
            public const string Ravanian = "Ravanian";
            public const string Endarian = "Endarian";
            public const string Calvinian = "Calvinian";

            public static readonly List<string> All = new()
            {
                Danarian, Midheim, Ravanian, Endarian, Calvinian
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

        public static readonly List<string> All = new()
        {
            Human.Danarian,
            Human.Midheim,
            Human.Ravanian,
            Human.Endarian,
            Human.Calvinian,

            Elf.Highborn,

            Dwarf.Undermountain
        };
    }

    public static class Heritage
    {
        public const string Traditional = "Traditional";
        public const string Martial = "Martial";

        public static readonly List<string> All = new()
        {
            Traditional, Martial
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

        public static readonly List<string> All = new()
        {
            Resolve, Harm, Spot, Defense, Purge, Mana
        };
    }

    public static class Skills
    {
        public const string Combat = "Combat";
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
            Combat, Arcane, Psionics, Hide, Traps, Tactics, Social, Apothecary, Travel, Sail
        };
    }
}
