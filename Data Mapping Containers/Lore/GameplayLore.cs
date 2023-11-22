using Data_Mapping_Containers.Dtos;

namespace Data_Mapping_Containers.Lore;

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
        public const string Nightfall = "Nigh falls and your party members gather around the crackling campfire. They share a hearty meal, the tantalizing aroma of roasted game filling the air.";
        public const string Campfire = "Amidst the flickering flames of the campfire, a palpable silence hangs in the air. You observe your comrades, wrapped in solitude, meticulously inspecting and polishing their armor, sharpening weapons with unwavering focus, and performing personal, secretive tasks that speak to their unique backgrounds and skills.";
        public const string Laughter = "From the shelter of your campsite, you watch the slow orange sun descend into the sunset. Laughter and camaraderie fill the camp as stories and experiences are shared, bringing a sense of unity and warmth to your secluded wilderness campsite.";
        public const string Rainy = "As the rain continues to pour from the heavens, your party members huddle beneath the shelter of a makeshift canopy and tents, you are given a brief respite from the perils of your quest, offering a chance to recharge, bond, and reflect on the adventures that still lie ahead.";

        public static readonly List<string> All = new()
        {
            Nightfall, Campfire, Laughter, Rainy
        };
    }

    public static class EncounterType
    {
        // bad
        public const string SaveVs = "SaveVs";
        public const string Combat = "Combat";
        public const string Curse = "Curse";
        public const string Disease = "Disease";
        // good
        public const string Boon = "Boon";
        public const string ItemFind = "ItemFind";
        public const string Storyline = "Storyline";
        public const string Wealth = "Wealth";
        // total 8

        public static readonly List<string> All = new()
        {
            SaveVs,     // 1  
            Combat,     // 2
            Curse,      // 3
            Disease,    // 4
            Boon,       // 5
            ItemFind,   // 6
            Storyline,  // 7
            Wealth,     // 8
        };
    }

    public static class EncounterTypeText
    {
        public static class SaveVs
        {
            public const string UnseenEnemy = "One of your party members is attacked, but before you reach, the unseen enemy has fled the scene.";
            public const string Relics = "Your party stumbles upon ancient relics. You are unable to descipher the writings on the wall, and one of your party members triggers a trap.";
            public const string TestOfStat = "One party member is challenged by a local for a trial, before you can react the combatants face each other briefly.";
            public const string ElfWitch = "Passing through a forest, you hear an enchanted song. A tale about a world called Ryxos. The enchantment is foul and wishes you ill.";
            public const string TrappedSpirit = "You find a man-made cave. Old trails lead in and out, but you decide it would be wise to stay on your path, but as you turn away a manifestation reaches out to grab one of you.";
            public const string GoblinTrap = "You step into a trap, by the looks of it it's goblin made.";
            // total 6

            public static readonly List<string> All = new()
            {
                UnseenEnemy,    // 1
                Relics,         // 2
                TestOfStat,     // 3
                ElfWitch,       // 4
                TrappedSpirit,  // 5
                GoblinTrap,     // 6
            };
        }


    }

    public static class QuestReward
    {
        public const string Wealth = "Wealth";
        public const string Item = "Item";
        public const string Loot = "Loot";
        public const string Stats = "Stats";
        public const string Skills = "Skills";
        public const string Deeds = "Deeds";
        // 6 total

        public static readonly List<string> All = new()
        {
            Wealth, // 1 
            Item,   // 2
            Loot,   // 3 
            Stats,  // 4
            Skills, // 5
            Deeds,  // 6
        };
    }

    public static class QuestFame
    {
        public const string Mudpaddler = "Mudpaddler";
        public const string Patrol = "Patrol";              
        public const string Fight = "Fighter";                
        public const string Cult = "Cult";                  
        public const string Challenge = "Challenge";        
        public const string Assassin = "Assassin";    
        public const string Militia = "Militia";                
        public const string Rescue = "Rescue";              
        public const string Sellsword = "Sellsword";
        public const string Deliverance = "Deliverance";
        public const string Furtive = "Furtive";
        public const string BaneOfTheOccult = "Bane of the occult";
        public const string Gambler = "Gambler";
        public const string Sweetalker = "Sweetalker";
        public const string Gang = "Gang";
        //  12 total

        public static readonly List<string> All = new()
        {
            Mudpaddler,     // 1
            Sellsword,      // 2
            Deliverance,    // 3
            Furtive,        // 4
            BaneOfTheOccult // 5
        };
    }

    public static class Quests
    {
        public static class Repeatable
        {
            public static readonly QuestTemplate Mudpaddler = new()
            {
                Fame = QuestFame.Mudpaddler,
                Description = "An ordinary job for any sellsword in these lands. It doesn't shine, but it pays well. You are being handed a charcoal stained hand-drawn map on a rigid cyperus paper, probably of eastern origin. " +
                "The huscarl in front of you explains your mission and points at where the location of several outposts lay on the map. You are to go to each outpost, gather their reports and come back. He points his finger towards a piece of " +
                "linnen cloth in front of you to illustrate your payment alongside other negotiated agreements. After a long silence the man rolls the paper into a salted hollow leather cylinder and hands it to you.",
                Result = "You arrive back behind the safety of the walls. The local maester is informed of your deeds, the reward is yours.",
                IsRepeatable = true,
                MaxEffortLvl = Effort.Normal,
            };
            public static readonly QuestTemplate Sellsword = new()
            {
                Fame = QuestFame.Sellsword,
                Description = "The lowest scum in the world, blackblood, greenskinned marauders are feared everywhere for their ways to terrorize settlements. You will find disemboweled, half eaten animals or people, burned crops, and destroyed viliges when these " +
                "hated creatures from Pel'Ravan mountains raid the lowlands, running away to hide again before any militia can be mustered. Therefore, the local marshals have been giving rewards for all sellswords and headhunters that can hunt these creatures down. " +
                "You are to find the camp, and remove it by any means necessary. This is easier said than done since blackblood races are known to be deceitful and shifty creatures.",
                Result = "Battered and tired, yet glorious you leave the marshal's office with your reward.",
                IsRepeatable = true,
                MaxEffortLvl = Effort.Normal,
            };
            // 2 total

            public static readonly List<QuestTemplate> All = new()
            {
                Mudpaddler, // 1
                Sellsword   // 2
            };
        }

        public static class OneTime
        {
            public static readonly QuestTemplate RescueNobleFromRansom = new()
            {
                Fame = QuestFame.Deliverance,
                Description = "Following a skirmish between the armies of two local lords, a noble belonging to one of their retinue is now held for ransom. As you understand the situation, in order to avoid a border clash between the banners, " +
                "people like you are called in to try to save the imprisoned knight from its captors. You have no involvement in the matter and it's a chance to build up reputation... as well as get your hands on a hefty reward. " +
                "The man that brought this up to your attention, a member of the local clergy, asks for your discression, telling you that if you get caught there won't be anybody sent after you, and that the lord that hired you " +
                "will, as is customary, deny any involvement in the matter. If you fail you'll likely be publicly quartered and your head will end up on a pike at the entrance to a castle.",
                Result = "You are praised for your deeds at the noble's house, an attention you wouldn't necessarily need, but do end up enjoying it.",
                IsRepeatable = false,
                MaxEffortLvl = Effort.Normal,
            };
            public static readonly QuestTemplate AmbushOutlanders = new()
            {
                Fame = QuestFame.Furtive,
                Description = "There is strong resentment towards outlanders and their business in these lands. You are approached by a representative of a clandestine organization. Rumors hint at a nefarious plot that could tip the delicate balance of power in the realm. " +
                "An enigmatic duo, hailing from a distant land, has drawn the attention of said organization, and they seek a pair of skilled adventurers to delve into the heart of this inconvenience. The air is thick with tension and the gravity of the task at hand hangs palpably, but you couldn't care less.",
                Result = "You meet up with your employer. 'Is it done?' it asks. You nod in agreement. 'Good...' and walks away after handing you the reward.",
                IsRepeatable = false,
                MaxEffortLvl = Effort.Normal,
            };
            public static readonly QuestTemplate KillCultists = new()
            {
                Fame = QuestFame.BaneOfTheOccult,
                Description = "Whispers of unholy deeds have reached the ears of the devout abbot. You listen to his story about a cult steeped in darkness, performing unspeakable rituals and sacrificing innocents to unseen entities. " +
                "The abbot provides a cryptic map leading to the cult's hidden sanctum. The path is treacherous, and you know the woods themselves seem to murmur with an otherworldly presence.",
                Result = "You were never able to track down the abbot upon your arrival, but a monastery boy gives you a small linnen bag with your reward, inside you find a papyrus note thanking you.",
                IsRepeatable = false,
                MaxEffortLvl = Effort.Normal,
            };
            // total 3

            public static readonly List<QuestTemplate> All = new()
            {
                RescueNobleFromRansom,  // 1
                AmbushOutlanders,       // 2
                KillCultists            // 3
            };
        }
    }
}
