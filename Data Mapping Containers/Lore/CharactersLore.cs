namespace Data_Mapping_Containers.Dtos;

public class CharactersLore
{
    public static class Races
    {
        public static readonly string Human = "Human";
        public static readonly string Elf = "Elf";
        public static readonly string Dwarf = "Dwarf";

        public static readonly List<string> All = new()
        {
            Human, Elf, Dwarf
        };
    }

    public static class Cultures
    {
        public static class Human
        {
            public static readonly string Danarian = "Danarian";
            public static readonly string Midheim = "Midheim";
            public static readonly string Ravanian = "Ravanian";
            public static readonly string Endarian = "Endarian";
            public static readonly string Calvinian = "Calvinian";

            public static readonly List<string> All = new()
            {
                Danarian, Midheim, Ravanian, Endarian, Calvinian
            };
        }

        public static class Elf
        {
            public static readonly string Highborn = "Highborn";

            public static readonly List<string> All = new()
            {
                Highborn
            };
        }

        public static class Dwarf
        {
            public static readonly string Undermountain = "Undermountain";

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

    public static class Traditions
    {
        public static readonly string None = "None";
        public static readonly string Ravanon = "Ravanon";
        public static readonly string Endarii = "Endarii";

        public static readonly List<string> All = new()
        {
            Ravanon, Endarii
        };
    }

    public static class Classes
    {
        public static readonly string Warrior = "Warrior";
        public static readonly string Spellcaster = "Spellcaster";
        public static readonly string Psionist = "Psionist";
        public static readonly string Merchant = "Merchant";
        public static readonly string Adventurer = "Adventurer";
        public static readonly string Apothecary = "Apothecary";
        public static readonly string Scout = "Scout";

        public static readonly List<string> All = new()
        {
            Warrior, Spellcaster, Psionist, Merchant, Adventurer, Apothecary, Scout
        };
    }

    public static class Stats
    {
        public static readonly string Strength = "Strength";
        public static readonly string Constitution = "Constitution";
        public static readonly string Agility = "Agility";
        public static readonly string Willpower = "Willpower";
        public static readonly string Perception = "Perception";
        public static readonly string Abstract = "Abstract";

        public static readonly List<string> All = new()
        {
            Strength, Constitution, Willpower, Agility, Perception, Abstract
        };
    }

    public static class Assets
    {
        public static readonly string Stamina = "Stamina";
        public static readonly string Harm = "Harm";
        public static readonly string Armour = "Armour";
        public static readonly string Purge = "Purge";
        public static readonly string Health = "Health";
        public static readonly string Mana = "Mana";

        public static readonly List<string> All = new()
        {
            Stamina, Harm, Armour, Purge, Health, Mana
        };
    }

    public static class Skills
    {
        public static readonly string Combat = "Combat";
        public static readonly string Arcane = "Arcane";
        public static readonly string Psionics = "Psionics";
        public static readonly string Hide = "Hide";
        public static readonly string Traps = "Traps";
        public static readonly string Tactics = "Tactics";
        public static readonly string Social = "Social";
        public static readonly string Apothecary = "Apothecary";
        public static readonly string Travel = "Travel";
        public static readonly string Sail = "Sail";

        public static readonly List<string> All = new()
        {
            Combat, Arcane, Psionics, Hide, Traps, Tactics, Social, Apothecary, Travel, Sail
        };
    }
}
