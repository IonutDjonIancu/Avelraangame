using Data_Mapping_Containers.Dtos;

namespace Data_Mapping_Containers.Lore;

public class ItemsLore
{
    public static class LevelNames
    {
        public static readonly string Relic = "Relic";              // 6 (80)
        public static readonly string Artifact = "Artifact";        // 5 (60)
        public static readonly string Heirloom = "Heirloom";        // 4 (50)
        public static readonly string Masterwork = "Masterwork";    // 3 (40)
        public static readonly string Refined = "Refined";          // 2 (20)
        public static readonly string Common = "Common";            // 1

        public static readonly List<string> LevelNamesAll = new()
        {
            Relic, Artifact, Heirloom, Masterwork, Refined, Common
        };
    }

    public static class Types
    {
        public static readonly string Protection = "Protection";
        public static readonly string Weapon = "Weapon";
        public static readonly string Wealth = "Wealth";

        public static readonly List<string> All = new()
        {
            Protection, Weapon, Wealth
        };
    }

    public static class InventoryLocation
    {
        public const string Head = "Head";
        public const string Body = "Body";
        public const string Mainhand = "Mainhand";
        public const string Offhand = "Offhand";
        public const string Ranged = "Ranged";
        public const string Heraldry = "Heraldry";

        public static readonly List<string> All = new()
        {
            Head, Body, Mainhand, Offhand, Ranged, Heraldry
        };
    }

    public static class Subtypes
    {
        public static class Weapons
        {
            public const string Sword = "Sword";
            public const string Pike = "Pike";
            public const string Crossbow = "Crossbow";
            public const string Polearm = "Polearm";
            public const string Mace = "Mace";
            public const string Axe = "Axe";
            public const string Dagger = "Dagger";
            public const string Bow = "Bow";
            public const string Sling = "Sling";
            public const string Spear = "Spear";

            public static readonly List<string> All = new()
            {
                Sword,
                Pike,
                Crossbow,
                Polearm,
                Mace,
                Axe,
                Dagger,
                Bow,
                Sling,
                Spear
            };
        }

        public static class Protections
        {
            public const string Armour = "Armour";
            public const string Helm = "Helm";
            public const string Shield = "Shield";

            public static readonly List<string> All = new()
            {
                Armour,
                Helm,
                Shield
            };
        }
        
        public static class Wealth
        {
            public const string Gems = "Gems";
            public const string Valuables = "Valuables";
            public const string Trinket = "Trinket";
            public const string Goods = "Goods";

            public static readonly List<string> All = new()
            {
                Gems,
                Valuables,
                Trinket,
                Goods
            };
        }

        public static readonly List<string> All = new()
        {
            Weapons.Sword,
            Weapons.Pike,
            Weapons.Crossbow,
            Weapons.Polearm,
            Weapons.Mace,
            Weapons.Axe,
            Weapons.Dagger,
            Weapons.Bow,
            Weapons.Sling,
            Weapons.Spear,

            Protections.Armour,
            Protections.Helm,
            Protections.Shield,

            Wealth.Gems,
            Wealth.Valuables,
            Wealth.Trinket,
            Wealth.Goods
        };
    }

    public static class Categories
    {
        public static readonly Dictionary<string, Dictionary<string, string>> Weapons = new()
        {
            { Subtypes.Weapons.Sword, ItemsLore.Weapons.Swords },
            { Subtypes.Weapons.Pike, ItemsLore.Weapons.Pikes },
            { Subtypes.Weapons.Crossbow, ItemsLore.Weapons.Crossbows },
            { Subtypes.Weapons.Polearm, ItemsLore.Weapons.Polearms },
            { Subtypes.Weapons.Mace, ItemsLore.Weapons.Maces },
            { Subtypes.Weapons.Axe, ItemsLore.Weapons.Axes },
            { Subtypes.Weapons.Dagger, ItemsLore.Weapons.Daggers },
            { Subtypes.Weapons.Bow, ItemsLore.Weapons.Bows },
            { Subtypes.Weapons.Sling, ItemsLore.Weapons.Slings },
            { Subtypes.Weapons.Spear, ItemsLore.Weapons.Spears }
        };

        public static readonly Dictionary<string, Dictionary<string, string>> Protections = new()
        {
            { Subtypes.Protections.Armour, ItemsLore.Protections.Armours },
            { Subtypes.Protections.Helm, ItemsLore.Protections.Helms },
            { Subtypes.Protections.Shield, ItemsLore.Protections.Shields }
        };

        public static readonly Dictionary<string, Dictionary<string, string>> Wealth = new()
        {
            { Subtypes.Wealth.Gems, ItemsLore.Wealth.Gems },
            { Subtypes.Wealth.Valuables, ItemsLore.Wealth.Valuables },
            { Subtypes.Wealth.Trinket, ItemsLore.Wealth.Trinkets },
            { Subtypes.Wealth.Goods, ItemsLore.Wealth.Goods }
        };
    }

    public static class Subcategories
    {
        public const string Onehanded = "Onehanded";
        public const string Twohanded = "Twohanded";
        public const string Ranged = "Ranged";
        public const string Garment = "Garment";
        public const string Tradeable = "Tradeable";

        public static readonly List<string> All = new()
        {
            Onehanded, Twohanded, Ranged, Garment, Tradeable
        };
    }

    public static class Weapons
    {
        public static readonly Dictionary<string, string> Swords = new()
        {
            // danarian
            { "Arming sword"    , "A straight, double-edged sword with a single-handed, cross-shaped hilt and a blade length of about 70 to 80 cm. A common weapon within the Southern human kingdoms of the Dragonmaw continent." },
            { "Short sword"     , "A two-edged blade with a handle and a tapered point primarily used for thrusting. Nothing fancy, but it gets the job done." },
            { "Ring-sword"      , "A blade with normally smooth or a very shallow fuller, often having multiple bands of pattern-welding within the central portion. Having its origins with the Dalkyr and the northern kindoms, although some can be found among the peoples of the Stormwatch Mountains." },
            { "Spatha"          , "A straight and long sword, measuring between 50 to 100 cm, with a handle length of between 18 and 20 cm. Usually seen being used by light cavalry, there is no clear reason to why this sword is prefered over the others. The sword is fine when using on foot as well." },
            { "Falcata"         , "A single-edged blade that pitches forward towards the point, the edge being concave near the hilt, but convex near the point. This shape distributes the weight in such a way that the falcata is capable of delivering a blow with the momentum of an axe, while maintaining the longer cutting edge of a sword and some facility to execute a thrust. The grip is typically hook-shaped, the end often stylized in the shape of a horse or a bird." },
            { "Longsword"       , "A longsword (also spelled as long sword or long-sword) is a type of Danarian sword characterized as having a cruciform hilt with a grip for primarily two-handed use (around 16 to 28 cm), a straight double-edged blade of around 85 to 110 cm, and weighing approximately 1 to 1.5 kg. This well balanced sword has a multitude of roles in combat, as the martial prowess of knights from human kingdoms has proven many times." },
            { "Falchion"        , "A one-handed, single-edged sword of Midheim origin which slowly made its way into the eastern parts of the Dragonmaw continent, most probably brought in by merchants and their guards." },
            { "Claymore"        , "A great sword characterised as having a cross hilt of forward-sloping quillons with quatrefoil terminations. A symbol of physical strength and prowess, and a link to the historic Highland way of life." },
            { "Zweihander"      , "A large two-handed sword, sometimes having a wavy blade called Flammenschwert. These swords represent the final stage in the trend of increasing size that started at the same time with the early castle development in western human kingdoms. In its developed form, the Zweihander acquired the characteristics of a polearm rather than a sword due to their large size and weight and therefore increased range and striking power. Consequently, it is not carried in a sheath but across the shoulder like a halberd." },
            { "Sabre"           , "A type of backsword with a curved blade originally associated with the light cavalry of the Northern Three Seas people." },
            { "Messer"          , "A single-edged sword of about 1.5 m long, with a defining characteristic in hilt construction. The blade is attached to the hilt via a slab tang sandwiched between two wooden grip plates that were pegged into place. It includes a straight cross-guard and a Nagel: a nail-like protrusion that juts out from the right side of the cross-guard away from the flat of the blade, to protect the wielder's sword hand. The length of the hilt can accommodate one- or two-handed grips." },

            // calvinian
            { "Shamshir"        , "A curved sword, featuring a slim blade that has almost no taper until the very tip. Instead of being worn upright (hilt-high), it is worn horizontally, with the hilt and tip pointing up. It is normally used for slashing unarmored opponents either on foot or mounted; while the tip could be used for thrusting, the drastic curvature of blade made accuracy more difficult. It has an offset pommel, and its two lengthy quillons form a simple crossguard. The tang of the blade is covered by slabs of bone, ivory, wood, or other material fastened by pins or rivets to form the grip. This type of sword is famously used by Calvinian soldiers." },
            { "Nikoto"          , "A sword characterized by a curved, single-edged blade with a circular or squared guard and long grip to accommodate two hands. Particularly worn with the edge facing upward. Typical features are a three-dimensional cross-sectional shape of an elongated pentagonal to hexagonal blade, a style in which the blade is integrated and fixed to the hilt with a pin, and a gentle curve. This style of blade has a high cutting ability." },

            // elven
            { "Maegil"          , "An elven hand-and-a-half longsword, almost always having a straight double-edged blade with an elaborately decorated hilt, handle and pommel." },
            { "Luivalron"       , "A large bladed, single-edged messer-like weapon of elvish origin, more commonly worn by the elves of the Three Seas continent. This sword type varies in size, from a one-handed to that of dagger." },
            { "Reistamel"       , "A slender sabre-like elven small-sword. In fact the Reistamel is a slender one-handed version of the heavier Autharil." },
            { "Autharil"        , "A leaf-shaped elven greatsword or heavy two-handed, single-edged sword." },
        
            // dwarven
            { "Rune blade"      , "A crucible steel blade characterized by a pattern of bands and high resilient content, a technique perfected by dwarven craftsmen. The distinct patterns of dwarven steel that can be made through forging are wave, ladder, and rose patterns with finely spaced bonds. However, with hammering, dyeing, and etching further customized patterns are made. The result is an abundance of ultrahard metallic steel in the weapon's structure, famous for their sharpness and toughness." },
        };

        public static readonly Dictionary<string, string> Pikes = new()
        {
            // danarian
            { "Pike"            , "A long weapon, varying considerably in size, from 3 to 7.5 m long. Generally, a spear becomes a pike when it is too long to be wielded with one hand in combat." },
            { "Lance"           , "A lance is a spear designed to be used by a mounted warrior or cavalry soldier (lancer)." },
            { "Sarissa"         , "A long spear or pike about 4 to 6 m in length, made of tough and resilient cornel wood, weighing approximately 5.5 to 6.5 kg. It has a sharp iron head shaped like a leaf and a bronze butt-spike, which could be anchored in the ground to stop charges by the enemy. The spike also served to balance out the spear, making it easier for soldiers to wield, and can be used as a back-up point should the main one break." },

            // calvinian
            { "Kovtos"          , "A long spear of about 4 m with a reinforced core. It is reputedly a weapon of great power compared to other cavalry weapons of its region. It is said to be able to impale two men at once, thus, it has to be wielded with two hands while directing the horse using the knees; this makes it a specialist weapon that requires a lot of training and good horsemanship to use. Only highly trained cavalrymen such as those fielded by the Khardah-Endari dynasties can use such weapons." },
            { "Euvston"         , "A type of spear composed of a very long wood staff and two pointed iron spear heads. The spear shaft measures 3 m and it is almost always made of cornel wood. The middle of the spear is thinner than the outer edges, giving the spear the impression of a concave shape. This type of spear is widely used by the Endari people, and made its way South into Calvinia where it has become the standard for their foot soldiers." }
        };

        public static readonly Dictionary<string, string> Crossbows = new()
        {
            // danarian
            { "Crossbow"        , "A hand-held bow-like assembly on a long stock with a string lock. A weapon made famous by the Empire of Seracleea due to its en-mass use." },
            { "Arbalest"        , "A large crossbow with a steel prod (the bow portion of the weapon). This late variation is more common among the people of Danar." },
            { "Windlass"        , "A heavy crossbow which evolved from the Arbalest, with the addition of a windlass reloading system, consisting of a horizontal cylinder (barrel), which is rotated by the turn of a crank or belt. A winch is affixed to one or both ends, and a rope is wound around the winch." },

            // dwarven
            { "Zirahkeel"       , "A dwarven arbalest with an iron lever which makes the loading of the crossbow much easier, if you have the strength to push it." },
            { "Kazdbaruk"       , "A crossbow of dwarven origin which uses a form of an axe (one- or two-handed) for the stock, making the weapon light or heavy in terms of draw weight." },
        };

        public static readonly Dictionary<string, string> Polearms = new()
        {
            // danarian
            { "Halberd"         , "An axe blade topped with a spike mounted on a long shaft, usually of about 1.5 to 1.8 m long. It always has a hook or thorn on the back side of the axe blade for grappling mounted combatants. The halberd is inexpensive to produce and very versatile in battle." },
            { "Bardiche"        , "A type of polearm having neither a hook at the back nor a spear point at the top, but a long prominent blade attached on one side. The blade varies greatly in shape, but is most often a long, cleaver-type blade, with a sharp point at the top." },
            { "Poleaxe"         , "A heavy wooden haft some 1.2 – 2.0 m long, mounted with a steel axe head, and a spike, hammer, or fluke on the reverse. In addition, there is a spike projecting from the end of the haft, often square in cross section. The poleaxe design arose from the need to breach the plate armour of men at arms" },

            // calvinian
            { "Glaive"          , "A single-edged blade on the end of a wooden pole, adopted and used by Southern Three Seas armies. Some people claim the weapon has elvish origins, but there is no evidence to sustain such claims." },
            { "Long flail"      , "A weapon consisting of a striking head attached to a long handle by a flexible rope, strap, or chain. The chief tactical virtue of the flail was its capacity to strike around a defender's shield or parry. Its chief liability was a lack of precision and the difficulty of using it in close combat, or closely-ranked formations. It is primarily considered a peasant's weapon, and while not common, they have been deployed in combat." },
        };

        public static readonly Dictionary<string, string> Maces = new()
        {
            // danarian
            { "Flanged mace"    , "A solid metal mace able to inflict damage on well armoured knights, as the force of a blow from a mace is great enough to cause damage without penetrating the armour. Being simple to make, cheap, and straightforward in application, they are quite common weapons. The flanged mace is a favorite weapon among the Calvinian elite, as it became a symbol of power of their military leaders." },
            { "Morningstar"     , "A club-like weapons consisting of a shaft with an attached ball adorned with one or more spikes. Each used, to varying degrees, a combination of blunt-force and puncture attack to kill or wound the enemy. The mace is a traditional knightly weapon that developed somewhat independently; as the mace transitioned to being constructed entirely of metal, the morning star retained its characteristic wooden shaft." },
            { "Warhammer"       , "A war hammer consists of a handle and a head. These weapons, especially when mounted on a pole, could in some cases transmit their impact through helms and cause concussions. Most warhammers often have a spike on one side of the head, making them more versatile weapons. The spike end could be used for grappling the target's armour, reins, or shield, If against mounted opponents, the weapon could also be directed at the legs of a horse, toppling the armoured foe to the ground where they could be more easily attacked." },

            // calvinian
            { "Flail"           , "A short weapon consisting of a wooden haft connected by a chain, rope, or leather to one or more striking ends. Despite being very common in the East, around the Three Seas continent, most military commanders of the western armies doubt the utility of such a weapon in actual practice, due to the hazard the weapon poses to its wielder." },
        };

        public static readonly Dictionary<string, string> Axes = new()
        {
            // danarian
            { "Battle axe"      , "A arm-length weapon borne in one or both hands. Compared to a sword swing, it delivers more cleaving power against a smaller target area, making it more effective against armour, due to concentrating more of its weight in the axehead." },
            { "Norse axe"       , "A long-handled weapon with a large flat blade, often attributed to the Norsemen of Darkyrdom and Hyperborea. The blade itself is reasonably light and forged very thin, making it superb for cutting, nonetheless it also has pronounced horns at both the toe and heel of the bit, having the toe of the bit swept forward for superior shearing capability. Some also feature a brass haft cap, often richly decorated, which presumably serves to keep the head of the weapon secure on the haft, as well as protecting the end of the haft from the rigours of battle. Ash and oak are the most likely materials for the haft, as they have always been the primary materials used for polearms by the peoples of Varga's Stand." },

            // calvinian
            { "Torrak"          , "A type of single-handed axe used by the many peoples and nations of the forests and mountains of Endar, as well as those that live in the rainforests of the Twin Vines due to its inexpensive cost to produce and maintain. It traditionally resembles a hatchet with a straight shaft, and are general-purpose tools, often employed as a hand-to-hand weapon." },
            { "Zagariz"         , "A long-shafted weapon with a metal head, with an either sharp (axe-like) or blunt (hammer-like) edge on one side and a sharp (straight or curving) 'ice-pick'-like point on the other. A weapon used mostly by the steppe people of the Far Endar." },

            // elven
            { "Na-Ga"           , "A legendary elven one-handed twin bladed axe used by the Daifathrim elves of Pel'Ravan. It features two crescent blades facing off from each other, able to deliver a striking blow to an lightly armoured opponent or used in grapling." },

            // dwarven
            { "Rune axe"        , "A heavy twin battle axe, used with either one or both hands, favored by the Undermountain dwarven clans. Its runes come from differential hardening used in dwarven bladesmithing to increase the toughness of a blade while keeping very high hardness and strength at the edge. This helps to make the blade very resistant to breaking, by making the spine very soft and bendable, allowing greater hardness at the edge than would be possible if the blade was uniformly quenched and tempered. This helps to create a tough blade that will maintain a very sharp, wear-resistant edge, even during rough use such as found in combat." },
        };

        public static readonly Dictionary<string, string> Daggers = new()
        {
            // danarian
            { "Seax dagger"     , "A large single-edged blade commongly used by the Dalkyr and the people of the Varga's Stand. The blade is worn horizontally inside a scabbard attached to the belt, with the edge of the blade upwards" },
            { "Quillon dagger"  , "A brown leather over a wood grip, steel guard and pommel and double sided steel blade. The dagger is often employed in the role of a secondary defense weapon in close combat." },
            { "Rondel dagger"   , "A type of stiff-bladed dagger made of steel, typically long and slim with a tapering needle point, measuring 30 cm or more. The dagger gets its name from its round (or similarly shaped, e.g. octagonal) hand guard and round or spherical pommel (knob on the end of the grip). They are principally designed for use with a stabbing action, either underarm, or over arm with a reverse grip." },

            // calvinian
            { "Keris"           , "An asymmetrical dagger with distinctive blade-patterning achieved through alternating laminations of iron and nickelous iron, famous for its distinctive wavy blade." },
        
            // elven
            { "Esthenn"         , "A short, curved dagger with a hollow blade, typically of elven origin. These daggers are sometimes made of crystal, although this is a tradition of Three Seas elven origin." },
        };

        public static readonly Dictionary<string, string> Bows = new()
        {
            // danarian
            { "Longbow"         , "A type of tall bow that makes a fairly long draw possible. A longbow is not significantly recurved. Its limbs are relatively narrow and are circular or D-shaped in cross section. Generally made from yew and with a length of 1.8 m, these weapons are famous because they can be made from a single piece of wood, it can be crafted relatively easily and quickly. Estimates for the draw weight of these bows are considerable." },

            // calvinian
            { "Short bow"       , "A traditional composite bow made from horn, wood, and sinew laminated together, a form of laminated bow. The horn is on the belly, facing the archer, and sinew on the outer side of a wooden core. When the bow is drawn, the sinew (stretched on the outside) and horn (compressed on the inside) store more energy than wood for the same length of bow. More powerful than the longbow, they can also retain a much higher degree of accuracy and a faster reload rate. Mostly employed by mounted archers and steppe people, they have been used extensively to stop Kardah or Calvinian expansions in the West Endari steppes." },
        
            // elven
            { "Quithil"         , "A lightweight medium-sized bow of elven origin which fits perfectly and inexplicably well in the palm of even a lesser experienced archer, able to deliver powerful and precise shots, as long as arrows are in no short supply." },
        };

        public static readonly Dictionary<string, string> Slings = new()
        {
            // danarian
            { "Sling"           , "The sling is much more than merely an extension of the arm. By its double-pendulum kinetics, the sling enables stones (or spears) to be thrown much further than they could be by hand alone. The sling is inexpensive and easy to build. A sling bullet lobbed in a high trajectory can achieve ranges in excess of 400 m." },

            // calvinian
            { "Atl"             , "A tool that uses leverage to achieve greater velocity in dart or javelin-throwing, and includes a bearing surface which allows the user to store energy during the throw. It consists of a shaft with a cup or a spur at the end that supports and propels the butt of the dart. It's usually about as long as the user's arm or forearm, as those used by the people of the Twin Vines forests." },
        };

        public static readonly Dictionary<string, string> Spears = new()
        {
            // danarian
            { "Spear"           , "A pole weapon consisting of a shaft, usually of wood, with a pointed head. The most common design for hunting or combat spears since ancient times has incorporated a metal spearhead shaped like a triangle, lozenge, or leaf. Some heads of spears feature barbs or serrated edges." },
            { "Boar spear"      , "A relatively short and heavy spear which has two lugs or wings on the spearsocket behind the blade, these act as a barrier to prevent the spear from penetrating too deeply into the target where it might get stuck or break, and to stop an injured and furious enemy from working its way up the shaft of the spear to attack." },
            { "Hoggeir"         , "A larger-headed spear with the tip blade measuring between 20 to 60 cm in length and a hollow shaft, mounted on wooden shafts typically made from ash wood. Often used by the Dalkyr and the Midheim peoples respectively with slight differences, since swords were more expensive to make and only wealthy warriors could afford them." },

            // calvinian
            { "Azaga"           , "A pole weapon used for throwing, usually a light spear or javelin made up of a wooden handle and an iron tip. This weapon is typically used with one hand while the off hand holds a shield for protection." },
        };
    }

    public static class Protections
    {
        public static readonly Dictionary<string, string> Armours = new()
        {
            // danarian
            { "Gambeson"        , "A padded defensive jacket, worn as armour separately, or combined with mail or plate armour. Gambesons are produced with a sewing technique called quilting. They are usually constructed of linen or wool; the stuffing varied, and could be for example scrap cloth or horse hair. For common soldiers who could not afford mail or plate armour, the gambeson, combined with a helm as the only additional protection, remains a common sight on western battlefields, throughout the entire Dragonmaw continent." },
            { "Chain mail"      , "Properly called mail or maille, but usually called chain mail or chainmail, is a type of armour consisting of small metal rings linked together in a pattern to form a mesh. It is one of the most common armour type of the professional Danarian and Midheim armies of the western Av'el'Raan." },
            { "Brigandine"      , "A garment typically made of heavy cloth, canvas, or leather, lined internally with small oblong plates riveted to the fabric, sometimes with a second layer of fabric on the inside. Unlike armour for the torso made from large plates, the brigandine is flexible, with a degree of movement between each of the overlapping plates. Many brigandines appear to have larger, somewhat L-shaped plates over the central chest area. The rivets attaching the plates to the fabric are often decorated, being gilt, or of latten, and sometimes embossed with a design. The rivets are also often grouped to produce a repeating decorative pattern." },
            { "Plate armour"    , "A full suit of plate armour consisting of a gorget (or bevor), spaulders, pauldrons with gardbraces to cover the armpits or besagews (also known as rondels), rerebraces, couters, vambraces, gauntlets, a cuirass (breastplate and backplate) with a fauld, tassets and a culet, a mail skirt, cuisses, poleyns, greaves, and sabatons. This is the most complete type of armour any soldier can field at any time." },

            // calvinian
            { "Linothorax"      , "A type of upper body armor usually quilted and stuffed with loose fibre or stitched together many layers thick, sometimes made with a special weave called twining which creates a thick, tough fabric. It has a top piece which covers the shoulders and is tied down on the chest, a main body piece wrapping around the wearer and covering the chest from the waist up, and a row of pteruges or flaps around the bottom which cover the belly and hips. This type of armour originated in the Kingdom of Parus, Three Seas continent, and went on to see much use by eastern armies." },
            { "Scale mail"      , "A form of armour consisting of many individual small armour scales (plates) of various shapes attached to each other and to a backing of cloth or leather in overlapping rows resembling the scales of a fish/reptile or roofing tiles. The scales are usually assembled and strapped by lacing or rivets. It is one of the most common armour type of the professional Calvinian and Endari armies of the eastern Av'el'Raan." },
            { "Coat of plates"  , "A form of segmented torso armour consisting of overlapping metal plates riveted inside a cloth or leather garment. Unlike scale armour which has plates on the outside, a coat of plates has the plates on the inside of the foundation garment. It is generally distinguished from its western, Danarian brigandine, counterpart by having larger plates, though there may be no distinction in some examples." },
        };

        public static readonly Dictionary<string, string> Helms = new()
        {
            // danarian
            { "Norse helm"      , "The construction of this helm is complex. It consists of four parts: an iron skull cap with brass edging and decorations, two iron cheek guards with brass edging, and camail protecting the neck. The cap of the helm has eight iron components. A brow band encircles the head; a nose-to-nape band extends from back to front, where it narrows and continues downwards as the nasal; two lateral bands each connect the side of the brow band to the top of the nose-to-nape band; and four subtriangular infill plates sit underneath the resulting holes. The eight pieces are riveted together. A usual sight on the battlefield of the northern Dragonmaw continent." },
            { "Bascinet"        , "An open-faced combat helm. It evolved from a type of iron or steel skullcap, but had a more pointed apex to the skull, and it extended downwards at the rear and sides to afford protection for the neck. A mail curtain (aventail or camail) is usually attached to the lower edge of the helm to protect the neck and shoulders." },
            { "Armet"           , "A fully enclosed helm, and narrowed to follow the contours of the neck and throat, it has to have a mechanical means of opening and closing to enable it to be worn. The typical armet consists of four pieces: the skull, the two large hinged cheek-pieces which locked at the front over the chin, and a visor which had a double pivot, one either side of the skull. The cheek-pieces open laterally by means of horizontal hinges; when closed they overlapped at the chin, fastening by a spring-pin which engaged in a corresponding hole, or by a swivel-hook and pierced staple. A reinforcement for the bottom half of the face, known as a wrapper, is sometimes added; its straps were protected by a metal disc at the base of the skull piece called a rondel. The visor is attached to each pivot via hinges with removable pins. Finally, a small aventail, a piece of mail, is attached to the bottom edge of each cheek-piece." },
            { "Barbute"         , "A visorless war helm often with a distinctive T shaped or Y shaped opening for the eyes and mouth. Barbutes are most commonly raised from a single sheet of metal, and are essentially of iron faced with steel, which is annealed and quenched to give it the desired characteristics of a hard outer surface, with a ductile inner layer which prevented shattering. Many barbutes feature a low front-to-back ridge, raised from the top of the helm's skull; this serves to strengthen the helm without adding a significant amount of weight." },
            { "Skull cap"       , "Cheap and easy to produce and thus much used by commoners and non-professional soldiers who could not afford more advanced protection, the skull cap is a round helm worn above the ears and offers minimal protection for the head." },
            { "Sallet"          , "A round-skulled helm with a distinctive rear part curved into a flange to protect the neck. It has a short tail and a jawbone type visor with a brow reinforcing. Stylistically, it is termed a high crowned helm. A plume holder was added to the helm at some time after its manufacture. These types of helms are considered both rare and important, therefore only high-born members of the society wear them in combat." },
            { "Great helm"      , "In its simplest form, the great helm is a flat-topped cylinder, usually made of metal, that completely covers the head and has only very small openings for ventilation and vision. Some Danarian makers employ more of a curved design, particularly on the top, to deflect or lessen the impact of blows. The helm is also extended downward until it reached shoulders." },
            { "Kettle hat"      , "A type of helm usually made of iron or steel in the shape of a brimmed hat. Of a simple design requiring less time and skill to produce than some other helm types, it is relatively inexpensive. It is worn most commonly by infantry, and is common all over the Southern parts of the Dragonmaw continent." },
            { "Nasal helm"      , "A helm characterised by the possession of a nose-guard, or nasal, composed of a single strip of metal that extended down from the skull or browband over the nose to provide facial protection." },
            { "Spangenhelm"     , "Spangen refers to the metal strips that form the framework for the helm and could be translated as braces. The strips connect three to six steel or bronze plates. The frame takes a conical design that curves with the shape of the head and culminates in a point. The front of the helm may include a nose protector (a nasal). Older spangenhelms often include cheek flaps made from metal or leather. Spangenhelms may incorporate mail as neck protection, thus forming a partial aventail. Some spangenhelms include eye protection in a shape that resembles modern eyeglass frames. Other spangenhelms include a full face mask, as is ubiquitous with the Paladins of Seracleea." },

            // calvinian
            { "Korintos helm"   , "A helm that covers the entire head and neck, with slits for the eyes and mouth. A large curved projection protected the nape of the neck. This helm is massively used by the soldiers of Three Seas dynasties." },
            { "Katabuto"        , "Is a traditional helm worn by the people of the Stormwatch Mts. It is made by combining dozens of thin iron plates, has a conical shape and is largely covered in decorations." },
            { "Lamellar helm"   , "A helment formed by caps made from overlapping lamellar scales, in addition to a brow plate and nose guard, cheek guards, and camail. It has a conical shape and plumes at the top, as is the eastern custom." },
        };

        public static readonly Dictionary<string, string> Shields = new()
        {
            { "Buckler"         , "A small in size, round and usually made of metal kind of shield. Its round shape and lightweight is ideal for use in hand-to-hand combat, although too small to block much of the body. Due to its size, the Bucker shield could be hung from the soldier's belt." },
            { "Round shield"    , "A shield used by infantry troops. More specifically, this is a concave shield fitted with enarmes on the inside, one adjustable by a buckle, to be attached to the forearm, and the other fixed as a grip for the left hand. Somewhere between 45 to 55 cm, this shield is made from two very thin layers of flat wooden boards, with the grain of each layer at right angles to the other. They are fixed together with small wooden pegs, forming plywood. The front is covered with a tough cowhide, which is often decorated with embossed stylish patterns. This is fixed to the wood with many brass, or in some cases, silver, nails, as occasionally brass plates are also fixed to the face for strength and decoration. Round shields generally have center bosses of brass." },
            { "Heater shield"   , "Smaller than the large shields, it is more manageable and can be used either mounted or on foot. Heater shields are typically made from thin wood overlaid with leather. They are often made of wood braced with metals such as steel or iron. Heater shields do not strap to the arm, but are held and maneuvered by a combination of a hand-strap and a belt called a guige, which is slung around the neck and used to support the shield, as well as sling it around the back when not in use." },
            { "Normaund shield" , "A large, almond-shaped shield rounded at the top and curving down to a point or rounded point at the bottom. This shield is developed for mounted cavalry, and its dimensions correlate to the approximate space between a horse's neck and its rider's thigh. To compensate for their awkward nature, these shields are equipped with enarmes, which grip the shield tightly to the arm and facilitate keeping it in place even when a knight relaxes their arm; this was a significant departure from most earlier circular shields, which possessed only a single handle. Some examples are apparently also fitted with an additional guige strap that allowed the shield to be slung over one shoulder when not in use." },
            { "Scutum"          , "The scutum is a 10 kg large rectangle curved shield made from three sheets of wood glued together and covered with canvas and leather, usually with a spindle shaped boss along the vertical length of the shield. The scutum is light enough to be held in one hand and its large height and width cover the entire soldier, making him very unlikely to be hit by missile fire and in hand-to-hand combat." },
            { "Pavise"          , "A very large oblong shield, often large enough to cover the entire body. The pavise is characterized by its prominent central ridge with a spike attached to the bottom." },
        };
    }

    public static class Wealth
    {
        public static readonly Dictionary<string, string> Gems = new()
        {
            { "Gemstone"        , "A piece of mineral crystal which, in cut and polished form, is used to make jewelry or other adornments." },
        };

        public static readonly Dictionary<string, string> Valuables = new()
        {
            { "Ornaments"       , "Items often worn to embellish, enhance, or distinguish the wearer, and to define cultural, social, or religious status within a specific community." },
        };

        public static readonly Dictionary<string, string> Trinkets = new()
        {
            { "Jewelry"       , "A small ornament or item of jewellery that is of symbolic value." },
        };

        public static readonly Dictionary<string, string> Goods = new()
        {
            { "Commodities"     , "Commodities are found in the majority of goods that end up in the hands of merchants, including spice, tea, meats, liquor, and clothing. Other most common commodities include copper, oils, wheat, coffee beans, and semi-precious stones." },
        };
    }

    public static class Qualities
    {
        public static readonly List<string> Common = new()
        {
            "Common",
            "Dead weight",
            "Stolen-improvised",
            "Improvised",
            "Old and broken",
            "Somewhat useful for desired purpose",
            "Low quality",
            "Splintered",
            "Incomplete",
            "Raw",
            "Poorly crafted",
            "Regular",
            "Unremarkable",
            "Normal",
            "Ordinary",
            "Simple",
            "Battered",
            "Unfinished",
            "Poor",
        };

        public static readonly List<string> Refined = new()
        {
            "Refined",
            "Blacksmith's",
            "Newly forged",
            "Increased quality",
            "Purified",
            "Polished",
            "Elegant",
            "Fine",
            "Gracious",
            "Elaborate",
            "Remarkable"
        };

        public static readonly List<string> Masterwork = new()
        {
            "Mastercraft",
            "Masterwork",
            "Seracleean",
            "Reinmar",
            "Infused",
        };

        public static readonly List<string> Heirloom = new()
        {
            "Heirloom"
        };

        public static readonly List<string> Artifact = new()
        {
            "Artifact"
        };

        public static readonly List<string> Relic = new()
        {
            "Relic"
        };
    }

    public static class Heirlooms
    {
        public static readonly string Meteoric = "Meteoric";
        public static readonly Item MeteoricHeirloom = new()
        {
            Quality = Meteoric,
            Lore = "An item of outstanding properties, forged from meteoric materials. It has a constant blue hue when exposed to sun- or candlelight. As Av'el'Raan goes through a belt of asteroids once every couple of hundred years, some crash down to the planet surface. Grand-master blacksmiths pay a hefty price for such materials, and sell their products to the richest noble houses.",
        };

        public static readonly string Dragon = "Dragon";
        public static readonly Item DragonHeirloom = new()
        {
            Quality = Dragon,
            Lore = "An item forged from the scales and bones of a fallen dragon.",
        };

        public static readonly string Galvron = "Galvron";
        public static readonly Item GalvronHeirloom = new()
        {
            Quality = Galvron,
            Lore = "An item forged from a famous crimson-red metal called galvron."
        };

        public static readonly string Ectoquartz = "Ectoquartz";
        public static readonly Item EctoquartzHeirloom = new()
        {
            Quality = Ectoquartz,
            Lore = "An item made from the calvinian ectoquartz. This is extracted quartz infused with ectoplast material."
        };

        public static readonly string Kemheil = "Kemheil";
        public static readonly Item KemheilHeirloom = new()
        {
            Quality = Kemheil,
            Lore = "A durable and viscous white material, used by the Jinns of Midheim."
        };

        public static readonly string Mithril = "Mithril";
        public static readonly Item MithrilHeirloom = new()
        {
            Quality = Mithril,
            Lore = "Dwarven mithril at its finest."
        };

        public static readonly List<Item> All = new()
        {
            MeteoricHeirloom,
            DragonHeirloom,
            GalvronHeirloom,
            EctoquartzHeirloom,
            KemheilHeirloom,
            MithrilHeirloom
        };
    }

    public static class Artifacts
    {
        public static readonly string MarshardDaggers = "Marshard Daggers";
        public static readonly Item MarshardDaggersArtifact = new()
        {
            Name = MarshardDaggers,
            Quality = MarshardDaggers,
            Description = "A pair of jade-green crystal straight daggers. The edge seems blunt, but it cuts amazingly well.",
            Lore = "This artifact was the pride of the Jademoor Isles. They were lost soon after the elves of Jademoor made peace with the High King of Parus. Weirdly enough, they come in pairs."
        };

        public static readonly string HammerfistOfThunderLords = "Hammerfist Of Thunder Lords";
        public static readonly Item HammerfistOfThunderLordsArtifact = new()
        {
            Name = HammerfistOfThunderLords,
            Quality = HammerfistOfThunderLords,
            Description = "A maul hammer having two metal heads shaped in the form of a clenched fist.",
            Lore = "This artifact was the border mark of the Imperial province of Epion in the southern Kingdom of Calvinia, before the rise of the Saal’grun Empire."
        };

        public static readonly List<Item> All = new()
        {
            MarshardDaggersArtifact,
            HammerfistOfThunderLordsArtifact
        };

    }

    public static class Relics
    {
        // metafluid relics
        public static readonly string IlithosRod = "Ilithos Rod";
        public static readonly Item IlithosRodRelic = new()
        {
            Name = IlithosRod,
            Quality = IlithosRod,
            Description = "A wooden short staff covered in metafluid goo, running up and down the rod as if melting, but never touching the wielder's hand.",
            Lore = "A legendary item that represents the pinnacle of evolution of the Ilithos race."
        };

        // purecrystal relics
        public static readonly string SwordOfLuvoran = "Sword of Luvoran";
        public static readonly Item SwordOfLuvoranRelic = new()
        {
            Name = SwordOfLuvoran,
            Quality = SwordOfLuvoran,
            Description = "The object is made from dead Purecrystal (due to its exposure to Arcane), with a golden sun on the hilt.",
            Lore = "This legendary sword belonged to the Grand Master Luvoran Reinauld during the first crusade against the Saal'grun kingdom of Calvinia."
        };

        public static readonly List<Item> All = new()
        {
            IlithosRodRelic,
            SwordOfLuvoranRelic
        };
    }
}
