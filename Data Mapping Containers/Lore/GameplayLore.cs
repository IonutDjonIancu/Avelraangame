using Data_Mapping_Containers.Dtos;

namespace Data_Mapping_Containers.Lore;

public class GameplayLore
{
    public static class Camping
    {
        public const string Nightfall = "Night falls and your party members gather around the crackling campfire. They share a hearty meal, the tantalizing aroma of roasted game filling the air.";
        public const string Campfire = "Amidst the flickering flames of the campfire, a palpable silence hangs in the air. You observe your comrades, wrapped in solitude, meticulously inspecting and polishing their armor, sharpening weapons with unwavering focus, and performing personal, secretive tasks that speak to their unique backgrounds and skills.";
        public const string Laughter = "From the shelter of your campsite, you watch the slow orange sun descend into the sunset. Laughter and camaraderie fill the camp as stories and experiences are shared, bringing a sense of unity and warmth to your secluded wilderness campsite.";
        public const string Rainy = "As the rain continues to pour from the heavens, your party members huddle beneath the shelter of a makeshift canopy and tents, you are given a brief respite from the perils of your quest, offering a chance to recharge, bond, and reflect on the adventures that still lie ahead.";

        public static readonly List<string> All = new()
        {
            Nightfall, Campfire, Laughter, Rainy
        };
    }

    public static class Travel
    {
        public const string Disastrous = "You miraculously arrive at your destination after a near fatal trip. Some of your provisions were rotten so you had to ask for food, unable to live off the land due to lacking the proper travelling skillset. Your companions abandon you and you owe a band of merchants for saving you by sharing some of their supplies with you.";
        public const string Grievous = "You barely made it to your destination. It was a harsh trip and some companions have abandoned you due to your miscalculated ways.";
        public const string Adverse = "It was a costly trip for you. You have lost your way many times and that took its toll on your provisions.";
        public const string Unfortunate = "An easy trip for you and your men, but the bad weather slowed you down.";
        public const string Convenient = "Travelling was easy and at a normal pace.";
        public const string Favourable = "Your travels are swift and without incident.";
        public const string Excellent = "Your travelling skills have proven more than amazing. You manage to live off the land, making use of almost no provisions along the way.";

        public static readonly List<string> All = new()
        {
            Disastrous, Grievous, Adverse, Unfortunate, Convenient, Favourable, Excellent
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
    
    public static class EncounterType
    {
        // bad 5
        public const string SaveVsStats = "SaveVs";
        public const string Combat = "Combat";
        public const string Curse = "Curse";
        public const string Disease = "Disease";
        public const string LoseWealth = "LoseWealth";
        // good 5
        public const string Boon = "Boon";
        public const string ItemFind = "ItemFind";
        public const string Storyline = "Storyline";
        public const string GainWealth = "GainWealth";
        public const string Experience = "Experience";
        // total 10

        public static readonly List<string> All = new()
        {
            SaveVsStats, // 1  
            Combat,      // 2
            Curse,       // 3
            Disease,     // 4
            Boon,        // 5
            ItemFind,    // 6
            Storyline,   // 7
            GainWealth,  // 8
            LoseWealth,  // 9
            Experience   // 10
        };
    }
    
    public static class EncounterTypeResults
    {
        public static class SaveVs
        {
            public const string Text1 = "One of your party members is under attack. But before you reach him, the unseen enemy has fled the scene.";
            public const string Text2 = "Your party stumbles upon ancient relics. You are unable to descipher the writings on the wall, and one of your party members triggers a trap.";
            public const string Text3 = "One party member is challenged by a local for a trial, before you can react the combatants face each other briefly.";
            // total 3

            public static readonly List<string> All = new()
            {
                Text1,    
                Text2,    
                Text3,    
            };
        }

        public static class Combat
        {
            public const string Text1 = "You have been waylaid by enemies...";
            public const string Text2 = "... you should defend yourself.";
            public const string Text3 = "There is a dangerous world out there.";
            public const string Text4 = "Quickly, get off the road.";
            public const string Text5 = "A fight.";
            // total 5

            public static readonly List<string> All = new()
            {
                Text1,
                Text2,
                Text3,
                Text4,
                Text5,
            };
        }

        public static class Curse
        {
            public const string Text1 = "You've been cursed.";
            public const string Text2 = "One of your party members suffers from a terrible curse.";
            public const string Text3 = "You catch with the corner of your eye a strange woman, dressed in a black robe, waving her hands frantically towards you.";
            // total 3

            public static readonly List<string> All = new()
            {
                Text1,
                Text2,
                Text3,
            };
        }

        public static class Disease
        {
            public const string Text1 = "One of us is suffering from a strage disease";
            public const string Text2 = "Disease spreads among us";
            public const string Text3 = "We find sickness in the lands we travel";
            // total 3

            public static readonly List<string> All = new()
            {
                Text1,
                Text2,
                Text3,
            };
        }

        public static class SaveVsAssets
        {
            public const string Text1 = "Passing through a forest, you hear an enchanted song. A tale about a world called Ryxos. The enchantment is foul and wishes you ill.";
            public const string Text2 = "You find a man-made cave. Old trails lead in and out, but you decide it would be wise to stay on your path. As you turn away a manifestation reaches out to grab one of you.";
            public const string Text3 = "You step into a trap, by the looks of it it's goblin made.";
            // total 3

            public static readonly List<string> All = new()
            {
                Text1,
                Text2,
                Text3,
            };
        }

        public static class LoseWealth
        {
            public const string Text1 = "The gambling addiction does not build a frugal mind.";
            public const string Text2 = "A game of Avelraan arcomage takes its toll on you and a nearby cat.";
            public const string Text3 = "You discover gambling within your ranks.";
            // total 3

            public static readonly List<string> All = new()
            {
                Text1,
                Text2,
                Text3,
            };
        }

        public static class Boon
        {
            public const string Text1 = "You find a small well in a forest. Its walls are decorated and elven ornaments can be seen all over them. The well is in bad repair, time has taken its toll on it, and vines and animals have degraded its beauty. You spend time to clean it, and leave. A while later you start feeling better about yourself.";
            public const string Text2 = "You pass by a village that tell you a story about a young fair maiden living by the sea. She had the power to cure all disease, all illnesses, yet somehow, she never had the strength to cure her own affliction. You ask about her name, but the peasants can't agree to remember which was it.";
            public const string Text3 = "A faint gust of wind wipes the sweat off your brow.";
            // total 3

            public static readonly List<string> All = new()
            {
                Text1,
                Text2,
                Text3,
            };
        }

        public static class ItemFind
        {
            public const string Text1 = "You run into a cave full of rusty, broken, items. One is still in good shape.";
            public const string Text2 = "You travel along a merchant caravan for several miles, when you part ways they offer you an item.";
            public const string Text3 = "You help a passing stranger cross a river, and are reward with an item.";
            // total 3

            public static readonly List<string> All = new()
            {
                Text1,
                Text2,
                Text3,
            };
        }

        public static class Storyline
        {
            public const string Text1 = "You pass along by what it seems to be man-made hill. From afar, the hillshide looks as if it resembles a face. While spending long moments watching it, a passerby tells you that this is the work of the Stone-men, an ancient civilization that ruled this world before the age of men.";
            public const string Text2 = "During the night, you stare at the distant stars. Your eye catches a small glimpse of a green flash up on the moonless night sky. The flash turns into an aurora, and sooner rather than later it covers the entire sky in a beautiful jade green veil. The effect lasts for several hours, a mesmerizing display of light scattering most people witness once or twice in their entire lifetime.";
            public const string Text3 = "You ride along the road with the pleasant ochre light of the sun on the side of your cheek. You stop to enjoy the sunset and its warmth, the brown-orange sun covering half the size of the horizon.";
            // total 3

            public static readonly List<string> All = new()
            {
                Text1,
                Text2,
                Text3,
            };
        }

        public static class GainWealth
        {
            public const string Text1 = "A rider finds you. He hands you a parchment. The goods you had for sale in Arada found a good price.";
            public const string Text2 = "You stumble upon an abandoned cart. Several long dead by its sides. You uncover the goods and are amazed of its riches.";
            public const string Text3 = "The goods you found a while back have made some wealth for you.";
            // total 3

            public static readonly List<string> All = new()
            {
                Text1,
                Text2,
                Text3,
            };
        }

        public static class Experience
        {
            public const string Text1 = "Your experience serves you well.";
            public const string Text2 = "A passerby stranger trains you in his arts.";
            public const string Text3 = "The sunrise brings new experiences.";
            // total 3

            public static readonly List<string> All = new()
            {
                Text1,
                Text2,
                Text3,
            };
        }
    }
    
    public static class EncounterTexts
    {
        public static class CombatEncounters
        {
            public const string Normal = "Some enemies approach.";
            public const string Gifted = "A large group of enemies are approaching.";
            public const string Chosen = "You face an angry mob, you are in for a fight.";
            public const string Hero = "A disorganized band of ruffians blocks your path, you are attacked.";
            public const string Olympian = "You are discovered by a warband's forward scouts. To hide their intentions, they won't let you live.";
            public const string Planar = "You run into an army's avangarde. They mistake you for enemy scouts and attack.";

            public static readonly List<string> All = new()
            {
                Normal, Gifted, Chosen, Hero, Olympian, Planar
            };
        }

        public static class CurseEncounters
        {
            public const string Skills = "Curse of the Unskilled.";
            public const string Stats = "Curse of the Deformed.";
            public const string Assets = "Curse of the Feeble.";
            public const string Item = "Curse of the Broken.";
            public const string Items = "Curse of the Wrecked.";
            public const string WealthWorthAndFame = "Curse of the Unremembered.";

            public static readonly List<string> All = new()
            {
                Skills, Stats, Assets, Item, Items, WealthWorthAndFame
            };
        }

    }

    
    public static class Locations
    {
        public static class Dragonmaw
        {
            public const string RegionName = "Dragonmaw";

            public static class Soudheim
            {
                public const string SubregionName = "Soudheim";

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
                        Description = "The capital of The Kingdom of Danar. A well fortified city with two fortresses, a keep, several garisons with a small, but permanent standing army, and a few thousand families living in or around it. This is where the King of Danar lives, a long lasting member of the Arada family.",
                        Effort = Effort.Normal,
                        TravelCostFromArada = 1
                    };
                    private const string LanwickName = "Lanwick";
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
                        Effort = Effort.Normal,
                        TravelCostFromArada = 5
                    };
                    #endregion
                }
            }
        }

        public static readonly List<Location> All = new()
        {
            Dragonmaw.Soudheim.Danar.Arada,
            Dragonmaw.Soudheim.Danar.Lanwick,
            Dragonmaw.Soudheim.Danar.Belfordshire
        };
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

    public static class QuestReward
    {
        public const string Wealth = "Wealth";
        public const string Item = "Item";
        public const string Loot = "Loot";
        public const string Stats = "Stats";
        public const string Skills = "Skills";
        public const string Deeds = "Deeds";
        // total 6

        public static readonly List<string> All = new()
        {
            Wealth,// 1 
            Item,  // 2
            Loot,  // 3 
            Stats, // 4
            Skills,// 5
            Deeds, // 6
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
        public const string Occult = "Bane of the occult";
        public const string Gambler = "Gambler";
        public const string Sweetalker = "Sweetalker";
        public const string Gang = "Gang";
        //  total 12

        public static readonly List<string> All = new()
        {
            Mudpaddler,    // 1
            Sellsword,     // 2
            Deliverance,   // 3
            Furtive,       // 4
            Occult// 5
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
            // total 2

            public static readonly List<QuestTemplate> All = new()
            {
                Mudpaddler,// 1
                Sellsword  // 2
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
                Fame = QuestFame.Occult,
                Description = "Whispers of unholy deeds have reached the ears of the devout abbot. You listen to his story about a cult steeped in darkness, performing unspeakable rituals and sacrificing innocents to unseen entities. " +
                "The abbot provides a cryptic map leading to the cult's hidden sanctum. The path is treacherous, and you know the woods themselves seem to murmur with an otherworldly presence.",
                Result = "You were never able to track down the abbot upon your arrival, but a monastery boy gives you a small linnen bag with your reward, inside you find a papyrus note thanking you.",
                IsRepeatable = false,
                MaxEffortLvl = Effort.Normal,
            };
            // total 3

            public static readonly List<QuestTemplate> All = new()
            {
                RescueNobleFromRansom,// 1
                AmbushOutlanders,     // 2
                KillCultists          // 3
            };
        }
    }
}
