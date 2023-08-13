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
                public const int Actions      = 0;    
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
                public const int Actions      = 1;    
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
                public const int Actions      = 0;    
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
                public const int Actions      = 0;    
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
                CharactersLore.Stats.Willpower,
            };
            public static readonly List<string> UnlikelyStats = new()
            {
                CharactersLore.Stats.Agility,
                CharactersLore.Stats.Perception,
                CharactersLore.Stats.Abstract,
            };
            public static readonly List<string> LikelyAssets = new()
            {
                CharactersLore.Assets.Resolve,
                CharactersLore.Assets.Harm,
                CharactersLore.Assets.Defense,
            };
            public static readonly List<string> UnlikelyAssets = new()
            {
                CharactersLore.Assets.Spot,
                CharactersLore.Assets.Purge,
                CharactersLore.Assets.Mana,
                CharactersLore.Assets.Actions,

            };
            public static readonly List<string> LikelySkills = new()
            {
                CharactersLore.Skills.Combat,
                CharactersLore.Skills.Travel,
            };
            public static readonly List<string> UnlikelySkills = new()
            {
                CharactersLore.Skills.Arcane,
                CharactersLore.Skills.Psionics,
                CharactersLore.Skills.Hide,
                CharactersLore.Skills.Traps,
                CharactersLore.Skills.Tactics,
                CharactersLore.Skills.Social,
                CharactersLore.Skills.Apothecary,
                CharactersLore.Skills.Sail,
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
                CharactersLore.Stats.Strength,
                CharactersLore.Stats.Agility,
                CharactersLore.Stats.Perception,
            };
            public static readonly List<string> LikelyAssets = new()
            {
                CharactersLore.Assets.Mana,
                CharactersLore.Assets.Actions
            };
            public static readonly List<string> UnlikelyAssets = new()
            {
                CharactersLore.Assets.Defense,
                CharactersLore.Assets.Harm,
                CharactersLore.Assets.Resolve,
                CharactersLore.Assets.Spot,
                CharactersLore.Assets.Purge,
            };
            public static readonly List<string> LikelySkills = new()
            {
                CharactersLore.Skills.Arcane,
                CharactersLore.Skills.Travel,
                CharactersLore.Skills.Apothecary,
                CharactersLore.Skills.Social,
            };
            public static readonly List<string> UnlikelySkills = new()
            {
                CharactersLore.Skills.Combat,
                CharactersLore.Skills.Psionics,
                CharactersLore.Skills.Hide,
                CharactersLore.Skills.Traps,
                CharactersLore.Skills.Tactics,
                CharactersLore.Skills.Sail,
            };
        }

        public static class Hunter
        {
            public static readonly List<string> LikelyStats = new()
            {
                CharactersLore.Stats.Constitution,
                CharactersLore.Stats.Agility,
                CharactersLore.Stats.Perception,
            };
            public static readonly List<string> UnlikelyStats = new()
            {
                CharactersLore.Stats.Willpower,
                CharactersLore.Stats.Strength,
                CharactersLore.Stats.Abstract,
            };
            public static readonly List<string> LikelyAssets = new()
            {
                CharactersLore.Assets.Resolve,
                CharactersLore.Assets.Harm,
                CharactersLore.Assets.Spot,
            };
            public static readonly List<string> UnlikelyAssets = new()
            {
                CharactersLore.Assets.Defense,
                CharactersLore.Assets.Purge,
                CharactersLore.Assets.Mana,
                CharactersLore.Assets.Actions,

            };
            public static readonly List<string> LikelySkills = new()
            {
                CharactersLore.Skills.Combat,
                CharactersLore.Skills.Travel,
                CharactersLore.Skills.Traps,
                CharactersLore.Skills.Hide,
            };
            public static readonly List<string> UnlikelySkills = new()
            {
                CharactersLore.Skills.Arcane,
                CharactersLore.Skills.Psionics,
                CharactersLore.Skills.Tactics,
                CharactersLore.Skills.Social,
                CharactersLore.Skills.Apothecary,
                CharactersLore.Skills.Sail,
            };
        }

        public static class Swashbuckler
        {
            public static readonly List<string> LikelyStats = new()
            {
                CharactersLore.Stats.Agility,
                CharactersLore.Stats.Abstract,
                CharactersLore.Stats.Willpower,
            };
            public static readonly List<string> UnlikelyStats = new()
            {
                CharactersLore.Stats.Constitution,
                CharactersLore.Stats.Strength,
                CharactersLore.Stats.Perception,
            };
            public static readonly List<string> LikelyAssets = new()
            {
                CharactersLore.Assets.Harm,
                CharactersLore.Assets.Defense,
                CharactersLore.Assets.Mana,
            };
            public static readonly List<string> UnlikelyAssets = new()
            {
                CharactersLore.Assets.Resolve,
                CharactersLore.Assets.Purge,
                CharactersLore.Assets.Spot,
                CharactersLore.Assets.Actions,

            };
            public static readonly List<string> LikelySkills = new()
            {
                CharactersLore.Skills.Combat,
                CharactersLore.Skills.Social,
                CharactersLore.Skills.Travel,
            };
            public static readonly List<string> UnlikelySkills = new()
            {
                CharactersLore.Skills.Arcane,
                CharactersLore.Skills.Psionics,
                CharactersLore.Skills.Hide,
                CharactersLore.Skills.Traps,
                CharactersLore.Skills.Tactics,
                CharactersLore.Skills.Apothecary,
                CharactersLore.Skills.Sail,
            };
        }

        public static class Sorcerer
        {
            public static readonly List<string> LikelyStats = new()
            {
                CharactersLore.Stats.Constitution,
                CharactersLore.Stats.Abstract,
            };
            public static readonly List<string> UnlikelyStats = new()
            {
                CharactersLore.Stats.Willpower,
                CharactersLore.Stats.Strength,
                CharactersLore.Stats.Agility,
                CharactersLore.Stats.Perception,
            };
            public static readonly List<string> LikelyAssets = new()
            {
                CharactersLore.Assets.Mana,
                CharactersLore.Assets.Actions,
            };
            public static readonly List<string> UnlikelyAssets = new()
            {
                CharactersLore.Assets.Resolve,
                CharactersLore.Assets.Harm,
                CharactersLore.Assets.Defense,
                CharactersLore.Assets.Spot,
                CharactersLore.Assets.Purge,

            };
            public static readonly List<string> LikelySkills = new()
            {
                CharactersLore.Skills.Arcane,
            };
            public static readonly List<string> UnlikelySkills = new()
            {
                CharactersLore.Skills.Travel,
                CharactersLore.Skills.Combat,
                CharactersLore.Skills.Psionics,
                CharactersLore.Skills.Hide,
                CharactersLore.Skills.Traps,
                CharactersLore.Skills.Tactics,
                CharactersLore.Skills.Social,
                CharactersLore.Skills.Apothecary,
                CharactersLore.Skills.Sail,
            };
        }
    }

    public static class Formulae
    {
        public static class Assets
        {
            public static int CalculateResolve(CharacterStats stats)
            {
                return 2 * stats.Strength + 3 * stats.Constitution + stats.Agility + 2 * stats.Willpower + stats.Perception + stats.Abstract;
            }

            public static int CalculateHarm(CharacterStats stats)
            {
                return 2 * stats.Strength + stats.Agility + stats.Perception;
            }

            public static int CalculateSpot(CharacterStats stats)
            {
                return 3 * stats.Perception + stats.Agility;
            }

            public static int CalculateDefense(CharacterStats stats)
            {
                return (2 * stats.Constitution + stats.Agility + stats.Perception) / 10;
            }

            public static int CalculatePurge(CharacterStats stats)
            {
                return (2 * stats.Willpower + stats.Constitution + stats.Perception) / 10;
            }

            public static int CalculateMana(CharacterStats stats)
            {
                return (2 * stats.Constitution + 3 * stats.Abstract) / 2;
            }

            public static int CalculateActions(CharacterStats stats)
            {
                // based on the Resolve formula
                return (2 * stats.Strength + 3 * stats.Constitution + stats.Agility + 2 * stats.Willpower + stats.Perception + stats.Abstract) / 100;
            }
        }

        public static class Skills
        {
            public static int CalculateCombat(CharacterStats stats)
            {
                return stats.Strength + stats.Constitution + stats.Agility;
            }

            public static int CalculateArcane(CharacterStats stats)
            {
                return stats.Abstract + stats.Willpower + stats.Constitution;
            }

            public static int CalculatePsionics(CharacterStats stats)
            {
                return 2 * stats.Willpower + stats.Constitution;
            }

            public static int CalculateHide(CharacterStats stats)
            {
                return 2 * stats.Agility + stats.Perception + stats.Abstract - stats.Strength;
            }
            
            public static int CalculateTraps(CharacterStats stats)
            {
                return 2 * stats.Abstract + stats.Perception;
            }

            public static int CalculateTactics(CharacterStats stats)
            {
                return 3 * stats.Abstract;
            }

            public static int CalculateSocial(CharacterStats stats)
            {
                return stats.Perception + stats.Abstract + stats.Willpower;
            }

            public static int CalculateApothecary(CharacterStats stats)
            {
                return stats.Abstract + stats.Perception + stats.Constitution;
            }

            public static int CalculateTravel(CharacterStats stats)
            {
                return 2 * stats.Constitution + stats.Agility + stats.Willpower - stats.Strength;
            }

            public static int CalculateSail(CharacterStats stats)
            {
                return 2 * stats.Constitution + stats.Abstract;
            }
        }
    }
}
