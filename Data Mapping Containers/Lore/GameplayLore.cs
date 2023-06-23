namespace Data_Mapping_Containers.Dtos;

public class GameplayLore
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
        public const string Dragonmaw = "Dragonmaw";

        public static readonly List<string> All = new()
        {
            Dragonmaw
        };
    }

    public static class Subregions
    {
        public static class Dragonmaw
        {
            public const string Farlindor = "Farlindor";
        }

        public static readonly List<string> All = new()
        {
            Dragonmaw.Farlindor,
        };
    }

    public static class Lands
    {
        public static class Farlindor
        {
            public const string Danar = "Danar";
        }
    }

    public static class Locations
    {
        public static class Danar 
        {
            // in any map start counting its locations from top-left corner
            public enum Locations
            {
                Arada = 0,
                Lanwick,
                Belfordshire
            };

            public static readonly List<string> All = new()
            {
                Locations.Arada.ToString(),
                Locations.Lanwick.ToString(),
                Locations.Belfordshire.ToString()
            };
                
            private static readonly int[,] Distances = {{ 1, 2, 3 },
                                                        { 2, 1, 2 },
                                                        { 3, 2, 1 }};

            public static int GetDistance(Danar.Locations from, Danar.Locations to)
            {
                return (int)Distances.GetValue((int)from, (int)to)!;
            }
        }
    }

    //public static class Regions
    //{
    //    public static class WestDragonmaw
    //    {
    //        public const string Nordheim = "Nordheim";
    //        public const string Midheim = "Midheim";
    //        public const string Soudheim = "Soudheim";
    //    }

    //    public static class EastDragonmaw
    //    {
    //        public const string VargasStand = "Varga's Stand";
    //        public const string Longshore = "Longshore";
    //        public const string Farlindor = "Farlindor";
    //        public const string PelRavan = "Pel'ravan";
    //    }

    //    public static class Hyperborea
    //    {
    //        public const string FrozenWastes = "FrozenWastes";
    //        public const string Brimland = "Brimland";
    //        public const string Ryxos = "Ryxos";
    //    }

    //    public static class ThreeSeas
    //    {
    //        public const string Endar = "Endar";
    //        public const string TwinVines = "Twin Vines";
    //        public const string Stormbork = "Stormbork";
    //        public const string Calvinia = "Calvinia";
    //    }

    //    public static class Eversun
    //    {
    //        public const string AjJahra = "Aj Jahra";
    //        public const string ShiftingPlanes = "Shifting Planes";
    //        public const string Peradin = "Peradin";
    //    }

    //    public static readonly List<string> AllNorthern = new()
    //    {
    //        Hyperborea.FrozenWastes,
    //        Hyperborea.Brimland,
    //        Hyperborea.Ryxos
    //    };

    //    public static readonly List<string> AllWestern = new()
    //    {
    //        WestDragonmaw.Nordheim,
    //        WestDragonmaw.Midheim,
    //        WestDragonmaw.Soudheim,

    //        EastDragonmaw.VargasStand,
    //        EastDragonmaw.Longshore,
    //        EastDragonmaw.Farlindor,
    //        EastDragonmaw.PelRavan,

    //        Eversun.Peradin
    //    };

    //    public static readonly List<string> AllEastern = new()
    //    {
    //        ThreeSeas.Endar,
    //        ThreeSeas.TwinVines,
    //        ThreeSeas.Stormbork,
    //        ThreeSeas.Calvinia
    //    };

    //    public static readonly List<string> AllSouthern = new()
    //    {
    //        Eversun.AjJahra,
    //        Eversun.ShiftingPlanes
    //    };

    //    public static readonly List<string> All = new()
    //    {
    //        WestDragonmaw.Nordheim,
    //        WestDragonmaw.Midheim,
    //        WestDragonmaw.Soudheim,

    //        EastDragonmaw.VargasStand,
    //        EastDragonmaw.Longshore,
    //        EastDragonmaw.Farlindor,
    //        EastDragonmaw.PelRavan,

    //        Hyperborea.FrozenWastes,
    //        Hyperborea.Brimland,
    //        Hyperborea.Ryxos,

    //        ThreeSeas.Endar,
    //        ThreeSeas.TwinVines,
    //        ThreeSeas.Stormbork,
    //        ThreeSeas.Calvinia,

    //        Eversun.AjJahra,
    //        Eversun.ShiftingPlanes,
    //        Eversun.Peradin
    //    };
    //}

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
