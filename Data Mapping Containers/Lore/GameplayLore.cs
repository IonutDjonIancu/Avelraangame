namespace Data_Mapping_Containers.Dtos;

public class GameplayLore
{
    public static class Calculations
    {
        public static class Acronyms
        {
            public static class Stats
            {
                public const string Str = "Str";
                public const string Con = "Con";
                public const string Agi = "Agi";
                public const string Wil = "Wil";
                public const string Per = "Per";
                public const string Abs = "Abs";
            }

            public static class Assets
            {
                public const string Res = "Res";
                public const string Har = "Har";
                public const string Spo = "Spo";
                public const string Def = "Def";
                public const string Pur = "Pur";
                public const string Man = "Man";
            }

            public static class Skills
            {
                public const string Com = "Com";
                public const string Arc = "Arc";
                public const string Psi = "Psi";
                public const string Hid = "Hid";
                public const string Tra = "Tra";
                public const string Tac = "Tac";
                public const string Soc = "Soc";
                public const string Apo = "Apo";
                public const string Trv = "Trv";
                public const string Sai = "Sai";
            }
        }

        public static class Races
        {
            public static class Human
            {
                public const int Str = 5;
                public const int Con = 5;
                public const int Agi = 5;
                public const int Wil = 5;
                public const int Per = 5;
                public const int Abs = 5;
            }

            public static class Elf
            {
                public const int Str = 2;
                public const int Con = 7;
                public const int Agi = 15;
                public const int Wil = 7;
                public const int Per = 10;
                public const int Abs = 10;
            }

            public static class Dwarf
            {
                public const int Str = 12;
                public const int Con = 10;
                public const int Agi = 2;
                public const int Wil = 10;
                public const int Per = 3;
                public const int Abs = 10;
            }
        }

        public static class Cultures
        {
            public static class Humans
            {
                public static class Danarian
                {
                    // stats
                    public const int Str = 0;
                    public const int Con = 0;
                    public const int Agi = 0;
                    public const int Wil = 0;
                    public const int Per = 0;
                    public const int Abs = 0;
                    // assets
                    public const int Res = 5;
                    public const int Har = 0;
                    public const int Spo = 0;
                    public const int Def = 0;
                    public const int Pur = 0;
                    public const int Man = 0;
                    // skills
                    public const int Com = 40;
                    public const int Arc = 10;
                    public const int Psi = 10;
                    public const int Hid = -10;
                    public const int Tra = 10;
                    public const int Tac = 10;
                    public const int Soc = 20;
                    public const int Apo = 10;
                    public const int Trv = 20;
                    public const int Sai = 10;
                }
            }

            public static class Elves
            {
                public static class Highborn
                {
                    // stats
                    public const int Str = 0;
                    public const int Con = 0;
                    public const int Agi = 0;
                    public const int Wil = 10;
                    public const int Per = 0;
                    public const int Abs = 0;
                    // assets
                    public const int Res = 10;
                    public const int Har = 0;
                    public const int Spo = 50;
                    public const int Def = 0;
                    public const int Pur = 0;
                    public const int Man = 0;
                    // skills
                    public const int Com = 20;
                    public const int Arc = 40;
                    public const int Psi = 5;
                    public const int Hid = 10;
                    public const int Tra = 10;
                    public const int Tac = 10;
                    public const int Soc = 20;
                    public const int Apo = 10;
                    public const int Trv = -40;
                    public const int Sai = -10;
                }
            }

            public static class Dwarves
            {
                public static class Undermountain
                {
                    // stats
                    public const int Str = 10;
                    public const int Con = 10;
                    public const int Agi = 0;
                    public const int Wil = 0;
                    public const int Per = 0;
                    public const int Abs = 0;
                    // assets
                    public const int Res = 20;
                    public const int Har = 10;
                    public const int Spo = 0;
                    public const int Def = 10;
                    public const int Pur = 10;
                    public const int Man = 0;
                    // skills
                    public const int Com = 30;
                    public const int Arc = 0;
                    public const int Psi = 10;
                    public const int Hid = -20;
                    public const int Tra = 10;
                    public const int Tac = 10;
                    public const int Soc = -5;
                    public const int Apo = 0;
                    public const int Trv = -20;
                    public const int Sai = -100;
                }
            }
        }

        public static class Classes
        {
            public static class Warrior
            {
                public static readonly List<string> LikelyStats = new()
                {
                    Acronyms.Stats.Str,
                    Acronyms.Stats.Con,
                };
                public static readonly List<string> UnlikelyStats = new()
                {
                    Acronyms.Stats.Str,
                    Acronyms.Stats.Wil,
                    Acronyms.Stats.Agi,
                    Acronyms.Stats.Per,
                };
                public static readonly List<string> LikelySkills = new()
                {
                    Acronyms.Skills.Com,
                };
                public static readonly List<string> UnlikelySkills = new()
                {
                    Acronyms.Skills.Com,
                    Acronyms.Skills.Tra,
                    Acronyms.Skills.Trv,
                    Acronyms.Skills.Hid,
                };
            }

            public static class Mage
            {
                public static readonly List<string> LikelyStats = new()
                {
                    Acronyms.Stats.Abs,
                    Acronyms.Stats.Con,
                };
                public static readonly List<string> UnlikelyStats = new()
                {
                    Acronyms.Stats.Wil,
                    Acronyms.Stats.Agi,
                    Acronyms.Stats.Per,
                };
                public static readonly List<string> LikelySkills = new()
                {
                    Acronyms.Skills.Arc,
                    Acronyms.Skills.Apo,
                    Acronyms.Skills.Com,
                };
                public static readonly List<string> UnlikelySkills = new()
                {
                    Acronyms.Skills.Psi,
                    Acronyms.Skills.Hid,
                    Acronyms.Skills.Sai,
                };
            }

            public static class Hunter
            {
                public static readonly List<string> LikelyStats = new()
                {
                    Acronyms.Stats.Agi,
                    Acronyms.Stats.Per,
                };
                public static readonly List<string> UnlikelyStats = new()
                {
                    Acronyms.Stats.Agi,
                    Acronyms.Stats.Con,
                    Acronyms.Stats.Per,
                };
                public static readonly List<string> LikelySkills = new()
                {
                    Acronyms.Skills.Com,
                    Acronyms.Skills.Hid,
                    Acronyms.Skills.Tra,
                    Acronyms.Skills.Trv,
                };
                public static readonly List<string> UnlikelySkills = new()
                {
                    Acronyms.Skills.Com,
                    Acronyms.Skills.Tra,
                };
            }

            public static class Swashbuckler
            {
                public static readonly List<string> LikelyStats = new()
                {
                    Acronyms.Stats.Str,
                    Acronyms.Stats.Agi,
                };
                public static readonly List<string> UnlikelyStats = new()
                {
                    Acronyms.Stats.Agi,
                    Acronyms.Stats.Con,
                    Acronyms.Stats.Per,
                    Acronyms.Stats.Wil,
                };
                public static readonly List<string> LikelySkills = new()
                {
                    Acronyms.Skills.Com,
                    Acronyms.Skills.Arc,
                    Acronyms.Skills.Soc,
                };
                public static readonly List<string> UnlikelySkills = new()
                {
                    Acronyms.Skills.Com,
                    Acronyms.Skills.Tra,
                    Acronyms.Skills.Trv,
                    Acronyms.Skills.Sai,
                };
            }

            public static class Sorcerer
            {
                public static readonly List<string> LikelyStats = new()
                {
                    Acronyms.Stats.Abs,
                };
                public static readonly List<string> UnlikelyStats = new()
                {
                    Acronyms.Stats.Abs,
                    Acronyms.Stats.Con,
                };
                public static readonly List<string> LikelySkills = new()
                {
                    Acronyms.Skills.Arc,
                };
                public static readonly List<string> UnlikelySkills = new()
                {
                    Acronyms.Skills.Arc,
                    Acronyms.Skills.Apo,
                    Acronyms.Skills.Trv,
                };
            }
        }

        public static class Formulae
        {
            public static class Assets
            {
                public const string Res = "2*Str + 3*Con + Agi + 2*Wil + Per + Abs";
                public const string Har = "2*Str + Agi + Per";
                public const string Spo = "3*Per + Agi";
                public const string Def = "(2*Con + Agi + Per) / 10";
                public const string Pur = "(2*Wil + Con + Per) / 10";
                public const string Man = "(2*Con + 3*Abs) / 5";
            }

            public static class Skills
            {
                public const string Com = "Str + Con + Agi";
                public const string Arc = "Abs + Wil + Con";
                public const string Psi = "2*Wil + Con";
                public const string Hid = "Agi + Per + Abs";
                public const string Tra = "2*Abs + Per";
                public const string Tac = "3*Abs";
                public const string Soc = "Per + Abs + Wil";
                public const string Apo = "Abs + Per + Con";
                public const string Trv = "2*Con + Agi + Wil - Str";
                public const string Sai = "2*Con + Abs";
            }

            public static class Misc
            {
                public static int CalculateActionTokens(int resolve)
                {
                    return resolve / 100;
                } 
            }
        }
    }

    public static class Rulebook
    {
        public static class Npcs 
        {
            public const int StatsDifferenceFactor  = 20;
            public const int AssetsDifferenceFactor = 20;
            public const int SkillsDifferenceFactor = 20;

            public static class Races
            {
                public const string Animal      = "Animal";
                public const string Monster     = "Monster";
                public const string Humanoid    = "Humanoid";
                public const string Undead      = "Undead";
                public const string Fiend       = "Fiend";
                public const string Elemental   = "Elemental";

                public static readonly List<string> All = new()
                {
                    Monster, Undead, Animal, Humanoid, Fiend, Elemental
                };
            }
        }

        public static class Regions
        {
            public static class WestDragonmaw
            {
                public const string Nordheim = "Nordheim";
                public const string Midheim = "Midheim";
                public const string Southeim = "Southeim";
            }

            public static class EastDragonmaw
            {
                public const string VargasStand = "Varga's Stand";
                public const string Longshore = "Longshore";
                public const string Farlindor = "Farlindor";
                public const string PelRavan = "Pel'ravan";
            }

            public static class Hyperborea
            {
                public const string FrozenWastes = "FrozenWastes";
                public const string Brimland = "Brimland";
                public const string Ryxos = "Ryxos";
            }

            public static class ThreeSeas
            {
                public const string Endar = "Endar";
                public const string TwinVines = "Twin Vines";
                public const string Stormbork = "Stormbork";
                public const string Calvinia = "Calvinia";
            }

            public static class Eversun
            {
                public const string AjJahra = "Aj Jahra";
                public const string ShiftingPlanes = "Shifting Planes";
                public const string Peradin = "Peradin";
            }

            public static readonly List<string> AllNorthern = new()
            {
                Hyperborea.FrozenWastes,
                Hyperborea.Brimland,
                Hyperborea.Ryxos
            };

            public static readonly List<string> AllEastern = new()
            {
                WestDragonmaw.Nordheim,
                WestDragonmaw.Midheim,
                WestDragonmaw.Southeim,

                EastDragonmaw.VargasStand,
                EastDragonmaw.Longshore,
                EastDragonmaw.Farlindor,
                EastDragonmaw.PelRavan,

                Eversun.Peradin
            };

            public static readonly List<string> AllWestern = new()
            {
                ThreeSeas.Endar,
                ThreeSeas.TwinVines,
                ThreeSeas.Stormbork,
                ThreeSeas.Calvinia,
            };

            public static readonly List<string> AllSouthern = new()
            {
                Eversun.AjJahra,
                Eversun.ShiftingPlanes
            };

            public static readonly List<string> All = new()
            {
                WestDragonmaw.Nordheim,
                WestDragonmaw.Midheim,
                WestDragonmaw.Southeim,

                EastDragonmaw.VargasStand,
                EastDragonmaw.Longshore,
                EastDragonmaw.Farlindor,
                EastDragonmaw.PelRavan,

                Hyperborea.FrozenWastes,
                Hyperborea.Brimland,
                Hyperborea.Ryxos,

                ThreeSeas.Endar,
                ThreeSeas.TwinVines,
                ThreeSeas.Stormbork,
                ThreeSeas.Calvinia,

                Eversun.AjJahra,
                Eversun.ShiftingPlanes,
                Eversun.Peradin
            };
        }

        public static class Tradition
        {
            public const string Martial = CharactersLore.Tradition.Martial;
            public const string Common = CharactersLore.Tradition.Common;
        }

        public static class Quests
        {
            public static class Difficulty
            {
                public const string Easy = "Easy";
                public const string Medium = "Medium";
                public const string Standard = "Standard";
                public const string Hard = "Hard";

                public static readonly List<string> All = new()
                {
                    Easy, Medium, Standard, Hard
                };
            }
            public static class Effort
            {
                public const string Normal = "Normal";
                public const string Gifted = "Gifted";
                public const string Chosen = "Chosen";
                public const string Hero = "Hero";
                public const string Olympian = "Olympian";
                public const string Planar = "Planar";

                public static readonly List<string> All = new()
                {
                    Normal, Gifted, Chosen, Hero, Olympian, Planar
                };

                public struct NormalRange
                {
                    public const int Lower = 5;
                    public const int Upper = 50;
                }
                public struct GiftedRange
                {
                    public const int Lower = 51;
                    public const int Upper = 100;
                }
                public struct ChosenRange
                {
                    public const int Lower = 101;
                    public const int Upper = 200;
                }
                public struct HeroRange
                {
                    public const int Lower = 201;
                    public const int Upper = 500;
                }
                public struct OlympianRange
                {
                    public const int Lower = 501;
                    public const int Upper = 1000;
                }
                public struct PlanarRange
                {
                    public const int Lower = 1001;
                    public const int Upper = 9000;
                }
            }
        }

    }
}
