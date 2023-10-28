namespace Data_Mapping_Containers.Dtos;

public class GameplayLore
{
    public static class Locations
    {
        public static class Dragonmaw
        {
            public const string RegionName = "Dragonmaw";

            public static class Farlindor
            {
                public const string SubregionName = "Farlindor";

                public static class Danar
                {
                    public const string LandName = "Danar";
                    #region locations
                    private const string AradaName = "Arada";
                    public static readonly Location Arada = new()
                    {
                        Name = AradaName,
                        FullName = $"{RegionName}_{SubregionName}_{LandName}_{AradaName}", // Arada
                        Position = new()
                        {
                            Region = RegionName,
                            Subregion = SubregionName,
                            Land = LandName,
                            Location = AradaName
                        },
                        Description = "The capital of The Kingdom of Danar. A well fortified city with two fortresses, a keep, several garisons with a small, but permanent standing army, and hundreds of thousands families living in or around it. This is where the King of Danar lives, a long lasting member of the Arada family.",
                        Effort = Effort.Normal,
                        TravelCostFromArada = 1
                    };
                    private const string LanwickName = "Lanwick Province";
                    public static readonly Location Lanwick = new()
                    {
                        Name = LanwickName,
                        FullName = $"{RegionName}_{SubregionName}_{LandName}_{LanwickName}", // Lanwick
                        Position = new()
                        {
                            Region = RegionName,
                            Subregion = SubregionName,
                            Land = LandName,
                            Location = LanwickName
                        },
                        Description = "A wealthy province in the kingdom of Danar, famous for its horsemen that make up the strong cavalry of the danarian elite. Although it's mostly flatlands, the north-western part has a four-hundred meter high hill, on top of which rests Lanwick Fortress, overlookin lake De'lac to the north.",
                        Effort = Effort.Normal,
                        TravelCostFromArada = 3
                    };
                    private const string BelfordshireName = "Belfordshire";
                    public static readonly Location Belfordshire = new()
                    {
                        Name = BelfordshireName,
                        FullName = $"{RegionName}_{SubregionName}_{LandName}_{BelfordshireName}", // Belfordshire
                        Position = new()
                        {
                            Region = RegionName,
                            Subregion = SubregionName,
                            Land = LandName,
                            Location = BelfordshireName
                        },
                        Description = "A modest settlement, mostly inhabited by soldiers safeguarding the southern border of Danar with the forests of Pel'Ravan mountains, merchants stopping by and their mercenaries. Expect mud, sweat, horses and the sharpening of steel to be omnious here.",
                        Effort = Effort.Gifted,
                        TravelCostFromArada = 5
                    };
                    #endregion
                }
            }
        }

        public static readonly List<Location> All = new()
        {
            Dragonmaw.Farlindor.Danar.Arada,
            Dragonmaw.Farlindor.Danar.Lanwick,
            Dragonmaw.Farlindor.Danar.Belfordshire
        };
    }

    public static class Effort
    {
        public const int Normal = 50; 
        public const int Gifted = 100; 
        public const int Chosen = 200;
        public const int Hero = 500;
        public const int Olympian = 1000; 
        public const int Planar = 9000;
    }

    public static class Tradition
    {
        public const string Martial = CharactersLore.Tradition.Martial;
        public const string Common = CharactersLore.Tradition.Common;

        public static readonly List<string> All = new()
        {
            Martial, Common
        };
    }

    public static class Camping
    {
        public const string AsNightFalls = "As night falls, your party members gather around the crackling campfire. They share a hearty meal, the tantalizing aroma of roasted game filling the air.";
        public const string AtTheCampFire = "Amidst the flickering flames of the campfire, a palpable silence hangs in the air. You observe your comrades, wrapped in solitude, meticulously inspecting and polishing their armor, sharpening weapons with unwavering focus, and performing personal, secretive tasks that speak to their unique backgrounds and skills.";
        public const string LaughterAtNight = "As the evening descends upon your campsite, laughter and camaraderie fill the camp as stories and experiences are shared, bringing a sense of unity and warmth to your secluded wilderness campsite.";
        public const string GettingReady = "As the rain continues to pour from the heavens, your party members huddle beneath the shelter of a makeshift canopy and tents, you are given a brief respite from the perils of your quest, offering a chance to recharge, bond, and reflect on the adventures that still lie ahead.";

        public static readonly List<string> All = new()
        {
            AsNightFalls, AtTheCampFire, LaughterAtNight, GettingReady
        };
    }

    //public static class Quests
    //{
    //    public static readonly QuestDetails Patrol = new()
    //    {
    //        Name = "Patrol",
    //        ShortDescription = $"Mudpaddlers and spears",
    //        Description = $"An ordinary job for any sellsword in these lands. It doesn't shine, but it pays well. You are being handed a hand-drawn map on a rigid cyperus paper, probably of eastern origin. " +
    //        $"The huscarl in front of you explains your mission and points at where the location of several outposts lay on the map. You are to go to each outpost, gather their reports and come back. A couple of gold crowns are placed on a piece of " +
    //        $"linnen cloth in front of you to illustrate your payment. After a long silence, you nod in agreement, the man rolls the paper into a salted hollow leather cylinder and hands it to you.",
    //        IsRepeatable = true,
    //        EffortRequired = Effort.Normal,
    //        AvailableAt = new List<string>
    //        {
    //            Locations.Dragonmaw.Farlindor.Danar.LandName,
    //        },
    //        Reward = new QuestReward
    //        {
    //            HasWealth = true,
    //            HasItem = false,
    //            HasLoot = false,
    //            HasStats = false,
    //            HasSkills = false,
    //            HasTraits = false,
    //        }
    //    };

    //    public static readonly QuestDetails KillGoblins = new()
    //    {
    //        Name = "Kill goblins",
    //        ShortDescription = $"Goblins around",
    //        Description = $"The lowest scum in the world, goblins are feared everywhere for their ways to terrorize settlements. You will find disemboweled, half eaten animals or people, burned crops, and destroyed viliges when these " +
    //        $"hated creatures from Pel'Ravan mountains raid the lowlands. Therefore, the marshals have been giving rewards for all sellswords that can bring goblin heads to the gutters. " +
    //        $"You are to find the camp, and remove it by any means necessary. This is easier said than done since goblins are known to be deceitful and shifty creatures.",
    //        IsRepeatable = true,
    //        EffortRequired = Effort.Normal,
    //        AvailableAt = new List<string>
    //        {
    //            Locations.Dragonmaw.Farlindor.Danar.LandName,
    //        },
    //        Reward = new QuestReward
    //        {
    //            HasWealth = true,
    //            HasItem = false,
    //            HasLoot = false,
    //            HasStats = false,
    //            HasSkills = false,
    //            HasTraits = false,
    //        }
    //    };

    //    public static readonly QuestDetails RescueLordFromRansom = new()
    //    {
    //        Name = "Rescue lord from ransom",
    //        ShortDescription = $"Rescue a lord from being ransomed.",
    //        Description = $"Following an skirmish between the armies of two local noblemen, one of their retinue is now held for ransom. As you understand the situation, in order to avoid a border clash between the banners, " +
    //        $"people like you are called in to try to save the imprisoned knight from being ransomed. You have no involvement in the matter and it's a chance to build up a reputation... as well as get your hands on a hefty reward. " +
    //        $"The man that brought this up to your attention, a member of the local clergy, asks for your discression, telling you that if you get caught there won't be anybody sent after you, and that the lord that hired you " +
    //        $"will, as is customary, deny any involvement in the matter. If you fail you'll probably be publicly quartered and your head will end up on a pike at the entrance to a castle.",
    //        IsRepeatable = false,
    //        EffortRequired = Effort.Gifted,
    //        AvailableAt = new List<string>
    //        {
    //            Locations.Dragonmaw.Farlindor.Danar.LandName,
    //        },
    //        Reward = new QuestReward
    //        {
    //            HasWealth = true,
    //            HasItem = true,
    //            HasLoot = false,
    //            HasStats = false,
    //            HasSkills = false,
    //            HasTraits = false,
    //        }
    //    };

    //    public static readonly List<QuestDetails> All = new()
    //    {
    //        RescueLordFromRansom,
    //        KillGoblins,
    //        Patrol
    //    };
    //}
}
