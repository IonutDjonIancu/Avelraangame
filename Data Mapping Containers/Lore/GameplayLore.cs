namespace Data_Mapping_Containers.Dtos;

public class GameplayLore
{
    public static class Npcs 
    {
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


    public static class Map
    {
        public static class Dragonmaw
        {
            public const string DragonmawName = "Dragonmaw";

            public static class Farlindor
            {
                public const string FarlindorName = "Farlindor";

                public static class Danar
                {
                    public const string DanarName = "Danar";
                    #region locations
                    public const string AradaName = "Arada";
                    public static readonly Location Arada = new()
                    {
                        FullName = $"{DragonmawName}_{FarlindorName}_{DanarName}_{AradaName}", // Arada
                        Position = new()
                        {
                            Region = DragonmawName,
                            Subregion = FarlindorName,
                            Land = DanarName,
                            Location = AradaName
                        },
                        Description = "The capital of The Kingdom of Danar. A well fortified city with two fortresses, a keep, several garisons with a small, but permanent standing army, and hundreds of thousands families living in or around it. This is where the King of Danar lives, a long lasting member of the Arada family.",
                        Effort = Effort.Normal,
                        TravelCost = 1
                    };
                    public const string LanwickName = "Lanwick Province";
                    public static readonly Location Lanwick = new()
                    {
                        FullName = $"{DragonmawName}_{FarlindorName}_{DanarName}_{LanwickName}", // Lanwick
                        Position = new()
                        {
                            Region = DragonmawName,
                            Subregion = FarlindorName,
                            Land = DanarName,
                            Location = LanwickName
                        },
                        Description = "A rich province in the kingdom of Danar, famous for its horsemen that make up the strong cavalry of the danarian elite. Although it's mostly flatlands, the north-western part has a four-hundred meter high hill, on top of which rests Lanwick Fortress, overlookin lake De'lac to the north.",
                        Effort = Effort.Normal,
                        TravelCost = 3
                    };
                    public const string BelfordshireName = "Belfordshire";
                    public static readonly Location Belfordshire = new()
                    {
                        FullName = $"{DragonmawName}_{FarlindorName}_{DanarName}_{BelfordshireName}", // Belfordshire
                        Position = new()
                        {
                            Region = DragonmawName,
                            Subregion = FarlindorName,
                            Land = DanarName,
                            Location = BelfordshireName
                        },
                        Description = "A modest settlement, mostly inhabited by soldiers safeguarding the southern border of Danar with the forests of Pel'Ravan mountains, merchants stopping by and their mercenaries. Expect mud, sweat, horses and the sharpening of steel to be omnious here.",
                        Effort = Effort.Gifted,
                        TravelCost = 4
                    };
                    #endregion
                    public static readonly int[,] Distances = {
                        { 1, 2, 4 },
                        { 2, 1, 2 },
                        { 5, 2, 1 }
                    };
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
            EffortRequired = Effort.Normal,
            AvailableAt = new List<string>
            {
                Map.Dragonmaw.Farlindor.Danar.DanarName,
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
            EffortRequired = Effort.Normal,
            AvailableAt = new List<string>
            {
                Map.Dragonmaw.Farlindor.Danar.DanarName,
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
            EffortRequired = Effort.Gifted,
            AvailableAt = new List<string>
            {
                Map.Dragonmaw.Farlindor.Danar.DanarName,
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
