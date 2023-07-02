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


    public static class MapLocations
    {
        // regions
        public static class Dragonmaw
        {
            public const string Name = "Dragonmaw";
            public static class Farlindor
            {
                public const string Name = "Farlindor";
                public static class Danar
                {
                    public const string Name = "Danar";
                    public static class Locations
                    {
                        private const string AradaName = "Arada";
                        public static readonly Location Arada = new()
                        {
                            Name = AradaName,
                            FullName = $"{AradaName}_{Danar.Name}_{Farlindor.Name}_{Dragonmaw.Name}",
                            Description = "The capital of Danar Land. A well fortified city with two fortresses a keep, garisons, and hundreds of thousands families living in or around it. This is where the King of Danar lives, a long lasting member of the Arada family.",
                            Effort = Effort.Normal,
                            TravelToCost = 1
                        };
                        private const string LanwickName = "Lanwick Province";
                        public static readonly Location Lanwick = new()
                        {
                            Name = LanwickName,
                            FullName = $"{LanwickName}_{Danar.Name}_{Farlindor.Name}_{Dragonmaw.Name}",
                            Description = "A rich province in the kingdom of Danar, famous for its horsemen that make up the strong cavalry of the danarian elite. Although it's mostly flatlands, the north-western part has a four-hundred meter high hill, on top of which rests Lanwick Fortress, overlookin lake De'lac to the north.",
                            Effort = Effort.Normal,
                            TravelToCost = 3
                        };
                        private const string BelfordshireName = "Belfordshire";
                        public static readonly Location Belfordshire = new()
                        {
                            Name = BelfordshireName,
                            FullName = $"{BelfordshireName}_{Danar.Name}_{Farlindor.Name}_{Dragonmaw.Name}",
                            Description = "A modest settlement, mostly inhabited by soldiers safeguarding the southern border of Danar with the forests of Pel'Ravan mountains, merchants stopping by and their mercenaries. Expect mud, sweat, horses and the sharpening of steel to be omnious here.",
                            Effort = Effort.Gifted,
                            TravelToCost = 4
                        };
                    }
                    public static readonly int[,] Distances = {
                        { 1, 2, 4 },
                        { 2, 1, 2 },
                        { 5, 2, 1 }
                    };

                    // ... add more
                }
            }
        }

        public static readonly List<Location> All = new()
        {
            Dragonmaw.Farlindor.Danar.Locations.Arada,
            Dragonmaw.Farlindor.Danar.Locations.Lanwick,
            Dragonmaw.Farlindor.Danar.Locations.Belfordshire
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


    public static int GetDistance(int travelFromCost, int travelToCost)
    {
        var value = travelFromCost - travelToCost;

        return 1 + value <= 0 ? value * (-1) : value;
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
        
        public static class RewardTypes
        {
            public const string Low = "Low";
            public const string Average = "Average";
            public const string High = "High";
            public const string Heroic = "Heroic";
            public const string Legendary = "Legendary";
        }

        public static class Encounters
        {
            public static class Types
            {
                public const string Diplomacy = "Diplomacy";
                public const string Utilitarian = "Utilitarian";
                public const string Overcome = "Overcome";
                public const string Fight = "Fight";
            }

            public static class Diplomacy
            {
                public static class Choices
                {
                    private const string choice1 = "Let's think about this.";
                    private const string choice2 = "I feel I can find a way through this entanglement.";
                    private const string choice3 = "Maybe a smarter way is to avoid a confrontation.";

                    public static readonly List<string> All = new()
                    {
                        choice1,
                        choice2,
                        choice3,
                    };
                }

                public static class Passes
                {
                    private const string pass1 = "Your diplomatic tongue gets you around.";
                    private const string pass2 = "Your methods seem to work.";
                    private const string pass3 = "A non-violent approach was chosen.";

                    public static readonly List<string> All = new()
                    {
                        pass1,
                        pass2,
                        pass3,
                    };
                }

                public class  Fails
                {
                    private const string fail1 = "You can't seem to work your mind around it.";
                    private const string fail2 = "This was way too difficult for you.";
                    private const string fail3 = "You failed.";

                    public static readonly List<string> All = new()
                    {
                        fail1,
                        fail2,
                        fail3,
                    };
                }
            }

            public static class Utilitarian
            {
                public static class Choices
                {
                    private const string choice1 = "Let's use some of our stuff to get around this.";
                    private const string choice2 = "I'll pay.";
                    private const string choice3 = "Perhaps trade is the better approach.";

                    public static readonly List<string> All = new()
                    {
                        choice1,
                        choice2,
                        choice3,
                    };
                }

                public static class Passes
                {
                    private const string pass1 = "Richness has its advantages.";
                    private const string pass2 = "That worked.";
                    private const string pass3 = "Prosperity provides means to get you by.";

                    public static readonly List<string> All = new()
                    {
                        pass1,
                        pass2,
                        pass3,
                    };
                }

                public class Fails
                {
                    private const string fail1 = "No richness can get you over this.";
                    private const string fail2 = "Perhaps it was way more expensive than you thought.";
                    private const string fail3 = "That failed.";

                    public static readonly List<string> All = new()
                    {
                        fail1,
                        fail2,
                        fail3,
                    };
                }
            }

            public static class Overcome
            {
                public static class Choices
                {
                    private const string choice1 = "We can overcome this.";
                    private const string choice2 = "Let's make a run for it.";
                    private const string choice3 = "We can force it through.";

                    public static readonly List<string> All = new()
                    {
                        choice1,
                        choice2,
                        choice3,
                    };
                }

                public static class Passes
                {
                    private const string pass1 = "Courage and resolve pay off.";
                    private const string pass2 = "You seem to overcome this easily.";
                    private const string pass3 = "You display an outstanding performance.";

                    public static readonly List<string> All = new()
                    {
                        pass1,
                        pass2,
                        pass3,
                    };
                }

                public class Fails
                {
                    private const string fail1 = "Too weak to overcome this.";
                    private const string fail2 = "Pathetic.";
                    private const string fail3 = "Failure.";

                    public static readonly List<string> All = new()
                    {
                        fail1,
                        fail2,
                        fail3,
                    };
                }
            }

            public static class Fight
            {
                public static class Choices
                {
                    private const string choice1 = "I am death incarnate.";
                    private const string choice2 = "You dig like a dog...";
                    private const string choice3 = "*say nothing and slowly draw your weapon*";

                    public static readonly List<string> All = new()
                    {
                        choice1,
                        choice2,
                        choice3,
                    };
                }

                public static class Passes
                {
                    private const string pass1 = "Valor has proven you right once again.";
                    private const string pass2 = "Tales will be told of your deeds one today.";
                    private const string pass3 = "As they fell to the ground, you clean your blade and move on.";

                    public static readonly List<string> All = new()
                    {
                        pass1,
                        pass2,
                        pass3,
                    };
                }

                public class Fails
                {
                    private const string fail1 = "You have been bested.";
                    private const string fail2 = "Defeat.";
                    private const string fail3 = "Death comes faster to those who seek it.";

                    public static readonly List<string> All = new()
                    {
                        fail1,
                        fail2,
                        fail3,
                    };
                }
            }
        }
    }
}
