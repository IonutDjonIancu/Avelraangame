namespace Data_Mapping_Containers.Dtos;

public class RulebookLore
{
    public static class Races
    {
        public static class Playable
        {
            public static class Human
            {
                public const int Strength     = 5;
                public const int Constitution = 5;
                public const int Agility      = 5;
                public const int Willpower    = 5;
                public const int Perception   = 5;
                public const int Abstract     = 5;
            }
            public static class Elf
            {
                public const int Strength     = 2;
                public const int Constitution = 7;
                public const int Agility      = 15;
                public const int Willpower    = 7;
                public const int Perception   = 10;
                public const int Abstract     = 10;
            }
            public static class Dwarf
            {
                public const int Strength     = 12;
                public const int Constitution = 10;
                public const int Agility      = 2;
                public const int Willpower    = 10;
                public const int Perception   = 3;
                public const int Abstract     = 10;
            }

            public static class Orc
            {
                public const int Strength     = 11;
                public const int Constitution = 9;
                public const int Agility      = 7;
                public const int Willpower    = 2;
                public const int Perception   = 9;
                public const int Abstract     = 1;
            }
        }

        public static class NonPlayable
        {
            public static class Undead
            {
                public const int Strength     = 15;
                public const int Constitution = 20;
                public const int Agility      = 1;
                public const int Willpower    = 1;
                public const int Perception   = 1;
                public const int Abstract     = 1;
            }

            public static class Animal
            {
                public const int Strength     = 15;
                public const int Constitution = 5;
                public const int Agility      = 15;
                public const int Willpower    = 1;
                public const int Perception   = 15;
                public const int Abstract     = 1;
            }
        }
    }

    public static class Cultures
    {
        public static class Humans
        {
            public static class Danarian
            {
                // stats
                public const int Strength     = 0;
                public const int Constitution = 0;
                public const int Agility      = 0;
                public const int Willpower    = 0;
                public const int Perception   = 0;
                public const int Abstract     = 0;
                // assets
                public const int Resolve      = 5;
                public const int Harm         = 0;
                public const int Spot         = 0;
                public const int Defence      = 0;
                public const int Purge        = 0;
                public const int Mana         = 0;
                // skills
                public const int Combat       = 40;
                public const int Arcane       = 10;
                public const int Psionics     = 10;
                public const int Hide         = -10;
                public const int Traps        = 10;
                public const int Tactics      = 10;
                public const int Social       = 20;
                public const int Apothecary   = 10;
                public const int Travel       = 20;
                public const int Sail         = 10;
            }
        }

        public static class Elves
        {
            public static class Highborn
            {
                // stats
                public const int Strength     = 0;
                public const int Constitution = 0;
                public const int Agility      = 0;
                public const int Willpower    = 10;
                public const int Perception   = 0;
                public const int Abstract     = 0;
                // assets
                public const int Resolve      = 10;
                public const int Harm         = 0;
                public const int Spot         = 50;
                public const int Defence      = 0;
                public const int Purge        = 0;
                public const int Mana         = 40;
                // skills
                public const int Combat       = 20;
                public const int Arcane       = 40;
                public const int Psionics     = 5;
                public const int Hide         = 10;
                public const int Traps        = 10;
                public const int Tactics      = 10;
                public const int Social       = 20;
                public const int Apothecary   = 10;
                public const int Travel       = -40;
                public const int Sail         = -10;
            }
        }

        public static class Dwarves
        {
            public static class Undermountain
            {
                // stats
                public const int Strength     = 10;
                public const int Constitution = 10;
                public const int Agility      = 0;
                public const int Willpower    = 0;
                public const int Perception   = 0;
                public const int Abstract     = 0;
                // assets
                public const int Resolve      = 20;
                public const int Harm         = 10;
                public const int Spot         = 0;
                public const int Defense      = 10;
                public const int Purge        = 10;
                public const int Mana         = 0;
                // skills
                public const int Combat       = 30;
                public const int Arcane       = 0;
                public const int Psionics     = 10;
                public const int Hide         = -20;
                public const int Traps        = 10;
                public const int Tactics      = 10;
                public const int Social       = -5;
                public const int Apothecary   = 0;
                public const int Travel       = -20;
                public const int Sail         = -100;
            }
        }

        public static class Orcs
        {
            public static class Greenskin
            {
                // stats
                public const int Strength     = 0;
                public const int Constitution = 0;
                public const int Agility      = 0;
                public const int Willpower    = 0;
                public const int Perception   = 10;
                public const int Abstract     = 0;
                // assets
                public const int Resolve      = 0;
                public const int Harm         = 20;
                public const int Spot         = 10;
                public const int Defense      = 0;
                public const int Purge        = 0;
                public const int Mana         = 0;
                // skills
                public const int Combat       = 10;
                public const int Arcane       = 0;
                public const int Psionics     = 0;
                public const int Hide         = 10;
                public const int Traps        = 10;
                public const int Tactics      = 0;
                public const int Social       = 0;
                public const int Apothecary   = 20;
                public const int Travel       = 40;
                public const int Sail         = -100;
            }
        }
    }

    public static class Classes
    {
        public static class Warrior
        {
            public static readonly List<string> LikelyStats = new()
            {
                CharactersLore.Stats.Strength,
                CharactersLore.Stats.Constitution,
            };
            public static readonly List<string> UnlikelyStats = new()
            {
                CharactersLore.Stats.Strength,
                CharactersLore.Stats.Willpower,
                CharactersLore.Stats.Agility,
                CharactersLore.Stats.Perception,
            };
            public static readonly List<string> LikelySkills = new()
            {
                CharactersLore.Skills.Combat,
            };
            public static readonly List<string> UnlikelySkills = new()
            {
                CharactersLore.Skills.Combat,
                CharactersLore.Skills.Traps,
                CharactersLore.Skills.Travel,
                CharactersLore.Skills.Hide,
            };
        }

        public static class Mage
        {
            public static readonly List<string> LikelyStats = new()
            {
                CharactersLore.Stats.Abstract,
                CharactersLore.Stats.Constitution,
            };
            public static readonly List<string> UnlikelyStats = new()
            {
                CharactersLore.Stats.Willpower,
                CharactersLore.Stats.Agility,
                CharactersLore.Stats.Perception,
            };
            public static readonly List<string> LikelySkills = new()
            {
                CharactersLore.Skills.Arcane,
                CharactersLore.Skills.Apothecary,
                CharactersLore.Skills.Combat,
            };
            public static readonly List<string> UnlikelySkills = new()
            {
                CharactersLore.Skills.Psionics,
                CharactersLore.Skills.Hide,
                CharactersLore.Skills.Sail,
            };
        }

        public static class Hunter
        {
            public static readonly List<string> LikelyStats = new()
            {
                CharactersLore.Stats.Agility,
                CharactersLore.Stats.Perception,
            };
            public static readonly List<string> UnlikelyStats = new()
            {
                CharactersLore.Stats.Agility,
                CharactersLore.Stats.Constitution,
                CharactersLore.Stats.Perception,
            };
            public static readonly List<string> LikelySkills = new()
            {
                CharactersLore.Skills.Combat,
                CharactersLore.Skills.Hide,
                CharactersLore.Skills.Traps,
                CharactersLore.Skills.Travel,
            };
            public static readonly List<string> UnlikelySkills = new()
            {
                CharactersLore.Skills.Combat,
                CharactersLore.Skills.Traps,
            };
        }

        public static class Swashbuckler
        {
            public static readonly List<string> LikelyStats = new()
            {
                CharactersLore.Stats.Strength,
                CharactersLore.Stats.Agility,
            };
            public static readonly List<string> UnlikelyStats = new()
            {
                CharactersLore.Stats.Agility,
                CharactersLore.Stats.Constitution,
                CharactersLore.Stats.Perception,
                CharactersLore.Stats.Willpower,
            };
            public static readonly List<string> LikelySkills = new()
            {
                CharactersLore.Skills.Combat,
                CharactersLore.Skills.Arcane,
                CharactersLore.Skills.Social,
            };
            public static readonly List<string> UnlikelySkills = new()
            {
                CharactersLore.Skills.Combat,
                CharactersLore.Skills.Traps,
                CharactersLore.Skills.Travel,
                CharactersLore.Skills.Sail,
            };
        }

        public static class Sorcerer
        {
            public static readonly List<string> LikelyStats = new()
            {
                CharactersLore.Stats.Abstract,
            };
            public static readonly List<string> UnlikelyStats = new()
            {
                CharactersLore.Stats.Abstract,
                CharactersLore.Stats.Constitution,
            };
            public static readonly List<string> LikelySkills = new()
            {
                CharactersLore.Skills.Arcane,
                CharactersLore.Skills.Apothecary,
            };
            public static readonly List<string> UnlikelySkills = new()
            {
                CharactersLore.Skills.Arcane,
                CharactersLore.Skills.Apothecary,
                CharactersLore.Skills.Travel,
            };
        }
    }

    public static class Formulae
    {
        public static class Assets
        {
            public static int CalculateResolve(CharacterStats cs)
            {
                return 2 * cs.Strength + 3 * cs.Constitution + cs.Agility + 2 * cs.Willpower + cs.Perception + cs.Abstract;
            }

            public static int CalculateHarm(CharacterStats cs)
            {
                return 2 * cs.Strength + cs.Agility + cs.Perception;
            }

            public static int CalculateSpot(CharacterStats cs)
            {
                return 3 * cs.Perception + cs.Agility;
            }

            public static int CalculateDefense(CharacterStats cs)
            {
                return (2 * cs.Constitution + cs.Agility + cs.Perception) / 10;
            }

            public static int CalculatePurge(CharacterStats cs)
            {
                return (2 * cs.Willpower + cs.Constitution + cs.Perception) / 10;
            }

            public static int CalculateMana(CharacterStats cs)
            {
                return (2 * cs.Constitution + 3 * cs.Abstract) / 2;
            }

            public static int CalculateActionTokens(CharacterStats cs)
            {
                // based on the Resolve formula
                return (2 * cs.Strength + 3 * cs.Constitution + cs.Agility + 2 * cs.Willpower + cs.Perception + cs.Abstract) / 100;
            }
        }

        public static class Skills
        {
            public static int CalculateCombat(CharacterStats cs)
            {
                return cs.Strength + cs.Constitution + cs.Agility;
            }

            public static int CalculateArcane(CharacterStats cs)
            {
                return cs.Abstract + cs.Willpower + cs.Constitution;
            }

            public static int CalculatePsionics(CharacterStats cs)
            {
                return 2 * cs.Willpower + cs.Constitution;
            }

            public static int CalculateHide(CharacterStats cs)
            {
                return 2 * cs.Agility + cs.Perception + cs.Abstract - cs.Strength;
            }
            
            public static int CalculateTraps(CharacterStats cs)
            {
                return 2 * cs.Abstract + cs.Perception;
            }

            public static int CalculateTactics(CharacterStats cs)
            {
                return 3 * cs.Abstract;
            }

            public static int CalculateSocial(CharacterStats cs)
            {
                return cs.Perception + cs.Abstract + cs.Willpower;
            }

            public static int CalculateApothecary(CharacterStats cs)
            {
                return cs.Abstract + cs.Perception + cs.Constitution;
            }

            public static int CalculateTravel(CharacterStats cs)
            {
                return 2 * cs.Constitution + cs.Agility + cs.Willpower - cs.Strength;
            }

            public static int CalculateSail(CharacterStats cs)
            {
                return 2 * cs.Constitution + cs.Abstract;
            }
        }

        public static class Misc
        {
           
        }
    }
}
