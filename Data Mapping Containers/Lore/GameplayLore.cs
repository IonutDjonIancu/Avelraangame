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
        public static class Dragonmaw // continent
        {
            public const string Name = "Dragonmaw";
            public static class Farlindor // land
            {
                public const string Name = "Farlindor";
                public static class Danar // kingdom
                {
                    public const string Name = "Danar";
                    public static class Locations // settlement
                    {
                        private const string AradaName = "Arada";
                        public static readonly Location Arada = new()
                        {
                            Name = AradaName,
                            FullName = $"{Dragonmaw.Name}_{Farlindor.Name}_{Danar.Name}_{AradaName}",
                            Description = "The capital of The Kingdom of Danar. A well fortified city with two fortresses, a keep, several garisons with a small, but permanent serving army, and hundreds of thousands families living in or around it. This is where the King of Danar lives, a long lasting member of the Arada family.",
                            Effort = Effort.Normal,
                            EffortLower = Effort.NormalRange.Lower,
                            EffortUpper = Effort.NormalRange.Upper,
                            TravelToCost = 1
                        };

                        private const string LanwickName = "Lanwick Province";
                        public static readonly Location Lanwick = new()
                        {
                            Name = LanwickName,
                            FullName = $"{Dragonmaw.Name}_{Farlindor.Name}_{Danar.Name}_{LanwickName}",
                            Description = "A rich province in the kingdom of Danar, famous for its horsemen that make up the strong cavalry of the danarian elite. Although it's mostly flatlands, the north-western part has a four-hundred meter high hill, on top of which rests Lanwick Fortress, overlookin lake De'lac to the north.",
                            Effort = Effort.Normal,
                            EffortLower = Effort.NormalRange.Lower,
                            EffortUpper = Effort.NormalRange.Upper,
                            TravelToCost = 3
                        };

                        private const string BelfordshireName = "Belfordshire";
                        public static readonly Location Belfordshire = new()
                        {
                            Name = BelfordshireName,
                            FullName = $"{Dragonmaw.Name}_{Farlindor.Name}_{Danar.Name}_{BelfordshireName}",
                            Description = "A modest settlement, mostly inhabited by soldiers safeguarding the southern border of Danar with the forests of Pel'Ravan mountains, merchants stopping by and their mercenaries. Expect mud, sweat, horses and the sharpening of steel to be omnious here.",
                            Effort = Effort.Gifted,
                            EffortLower = Effort.GiftedRange.Lower,
                            EffortUpper = Effort.GiftedRange.Upper,
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
        public static readonly QuestDetails Patrol = new()
        {
            Name = "Patrol",
            ShortDescription = $"Mudpaddler and spears",
            Description = $"An ordinary job for any sellsword in these lands. It doesn't shine, but it pays well. You are being handed a hand-drawn map on a rigid cyperus paper, probably of eastern origin. " +
            $"The huscarl in front of you explains your mission and points at where the location of several outposts lay on the map. You are to go to each outpost, gather their reports and come back. A couple of gold crowns are placed on a piece of " +
            $"linnen cloth in front of you to illustrate your payment. After a long silence, you nod in agreement, the man rolls the paper into a salted hollow leather cylinder and hands it to you.",
            IsRepeatable = true,
            EffortRequired = Effort.NormalRange.Upper,
            AvailableAt = new List<string>
            {
                MapLocations.Dragonmaw.Farlindor.Danar.Name,
            },
            Reward = new QuestReward
            {
                HasWealth = true,
                HasItem = false,
                HasLoot = false,
                HasStats = false,
                HasSkills = false,
                HasTraits = false,
            }
        };

        public static readonly QuestDetails KillGoblins = new()
        {
            Name = "Kill goblins",
            ShortDescription = $"Goblins around",
            Description = $"The lowest scum in the world, goblins are feared everywhere for their ways to terrorize settlements. You will find disemboweled, half eaten animals or people, burned crops, and destroyed viliges when these " +
            $"hated creatures from Pel'Ravan mountains raid the lowlands. Therefore, the marshals have been giving rewards for all sellswords that can bring goblin heads to the gutters. " +
            $"You are to find the camp, and remove it by any means necessary. This is easier said than done since goblins are known to be deceitful and shifty creatures.",
            IsRepeatable = true,
            EffortRequired = Effort.NormalRange.Upper,
            AvailableAt = new List<string>
            {
                MapLocations.Dragonmaw.Farlindor.Danar.Name,
            },
            Reward = new QuestReward
            {
                HasWealth = true,
                HasItem = false,
                HasLoot = false,
                HasStats = false,
                HasSkills = false,
                HasTraits = false,
            }
        };

        public static readonly QuestDetails RescueLordFromRansom = new()
        {
            Name = "Rescue lord from ransom",
            ShortDescription = $"Rescue a lord from being ransomed.",
            Description = $"Following an skirmish between the armies of two local noblemen, one of their retinue is now held for ransom. As you understand the situation, in order to avoid a border clash between the banners, " +
            $"people like you are called in to try to save the imprisoned knight from being ransomed. You have no involvement in the matter and it's a chance to build up a reputation... as well as get your hands on a hefty reward. " +
            $"The man that brought this up to your attention, a member of the local clergy, asks for your discression, telling you that if you get caught there won't be anybody sent after you, and that the lord that hired you " +
            $"will, as is customary, deny any involvement in the matter. If you fail you'll probably be publicly quartered and your head will end up on a pike at the entrance to a castle.",
            IsRepeatable = false,
            EffortRequired = Effort.NormalRange.Upper,
            AvailableAt = new List<string>
            {
                MapLocations.Dragonmaw.Farlindor.Danar.Name,
            },
            Reward = new QuestReward
            {
                HasWealth = true,
                HasItem = true,
                HasLoot = false,
                HasStats = false,
                HasSkills = false,
                HasTraits = false,
            }
        };

        public static readonly List<QuestDetails> All = new()
        {
            RescueLordFromRansom,
            KillGoblins,
            Patrol
        };
    }
}
