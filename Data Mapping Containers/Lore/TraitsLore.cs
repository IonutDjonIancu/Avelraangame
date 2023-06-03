namespace Data_Mapping_Containers.Dtos;

public class TraitsLore
{
    public static class Type
    {
        public const string passive = "Passive"; // it will always apply and has its own logic
        public const string active = "Active"; // you have to activate it during combat or otherwise, also known as SpecialSkills for Paperdoll
        public const string bonus = "Bonus"; // increases a stat, skill or asset based on trait's logic

        public static readonly List<string> All = new() 
        { 
            passive, active, bonus
        };
    }

    public static class Subtype
    {
        public const string multiple = "Multiple"; // can be chosen multiple times (only Bonus type HT can be Common)
        public const string onetime = "Onetime"; // can only be chosen once

        public static readonly List<string> All = new()
        {
            multiple, onetime
        };
    }

    public static class Category
    {
        public const string stats = "Stats";
        public const string assets = "Assets";
        public const string skills = "Skills";
        public const string rolls = "Rolls";
    }

    public static class ActivateTraits
    {
        // 1
        public static readonly HeroicTrait metachaosDaemonology = new()
        {
            Identity = new HeroicTraitIdentity
            {
                Id = "7718aef3-4d96-4686-841f-45f5cae0266b",
                Name = "Metachaos Daemonology",
            },
            Description = "Increases your spellcraft evocation prowess by 50% for all spells during an entire round, will occur in the next 3 odd rounds.",
            Lore = "It is said amongst mages that this technique was initially discovered by the forefathers of the first humans of Khardah. Others claim it came from the sorceres of V'ald during the first descent of the elves upon the Cloud Kingdom of mankind. Although uncertainty shrouds this spellweave expertise, there is evidence that links the great pyramid in Khardah to the Illithos that shows how deeply research has this been.",
            Type = Type.active,
            Subtype = Subtype.onetime,
            Category = Category.rolls,
            DeedsCost = 100
        };
    }

    public static class PassiveTraits
    {
        // 1
        public static readonly HeroicTrait fatePoint = new()
        {
            Identity = new HeroicTraitIdentity
            {
                Id = "f85a2b0f-428d-4691-8cdc-caa6c399ec94",
                Name = "Fate Point",
            },
            Description = "Your critical hits start from 19 out of a d20 die.",
            Lore = "Any adventurer whose hands hold the marks of a sword hilt or the burns of spellcraft will eventually develop this ability. It marks its bearer as one who has travelled roads seen by a few.",
            Type = Type.passive,
            Subtype = Subtype.onetime,
            Category = Category.rolls,
            DeedsCost = 50
        };
        // 2
        public static readonly HeroicTrait theStrengthOfMany = new()
        {
            Identity = new HeroicTraitIdentity
            {
                Id = "782d8a39-b6cc-46bb-8f6a-622525bfcba1",
                Name = "The Strength of Many",
            },
            Description = "Increases Paperdoll stat Strength by 10%.",
            Lore = "At one point in your life you have developed an almost unnatural ability to bend steel.",
            Type = Type.passive,
            Subtype = Subtype.onetime,
            Category = Category.stats,
            DeedsCost = 10
        };
        // 3
        public static readonly HeroicTrait lifeInThePits = new()
        {
            Identity = new HeroicTraitIdentity
            {
                Id = "782d8a39-b6cc-46bb-8f6a-622525bfcba1",
                Name = "Life in the Pits",
            },
            Description = "Increases Paperdoll asset Resolve by 50.",
            Lore = "You've spent some three months in fighting pits as wall decorator.",
            Type = Type.passive,
            Subtype = Subtype.onetime,
            Category = Category.assets,
            DeedsCost = 3
        };
        // 4
        public static readonly HeroicTrait candlelight = new()
        {
            Identity = new HeroicTraitIdentity
            {
                Id = "e0fe3f49-16c2-4ed2-8273-8e3036402508",
                Name = "Candlelight",
            },
            Description = "Increases Paperdoll skill Arcane by 20.",
            Lore = "Your candlelight studies have finally proven effective... somewhat.",
            Type = Type.passive,
            Subtype = Subtype.onetime,
            Category = Category.skills,
            DeedsCost = 2
        };
    }

    public static class BonusTraits
    {
        // 1
        public static readonly HeroicTrait swordsman = new()
        {
            Identity = new HeroicTraitIdentity
            {
                Id = "ea91b5bb-c338-431d-bef5-915483aac4a0",
                Name = "Swordsman",
            },
            Description = "Increases the base Combat skill by 5 plus another 1% of the character's Paperdoll amount.",
            Lore = "Steady arm and stout shield are the best teachers you have ever known.",
            Type = Type.bonus,
            Subtype = Subtype.multiple,
            Category = Category.skills,
            DeedsCost = 1
        };
        // 2
        public static readonly HeroicTrait skillful = new()
        {
            Identity = new HeroicTraitIdentity
            {
                Id = "0d5e0310-013b-42bd-b479-5c961dd583e1",
                Name = "Skillful",
            },
            Description = "Increases a specific skill by 20% of its base amount.",
            Lore = "A testament of your accomplishments during your years as an adventurer.",
            Type = Type.bonus,
            Subtype = Subtype.onetime,
            Category = Category.skills,
            DeedsCost = 5
        };
    }

    public static readonly List<HeroicTrait> All = new()
    {
        ActivateTraits.metachaosDaemonology,

        PassiveTraits.fatePoint,
        PassiveTraits.theStrengthOfMany,
        PassiveTraits.lifeInThePits,
        PassiveTraits.candlelight,

        BonusTraits.swordsman,
        BonusTraits.skillful,
    };
}
