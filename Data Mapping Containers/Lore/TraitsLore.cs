namespace Data_Mapping_Containers.Dtos;

public class TraitsLore
{
    public static class Type
    {
        public static readonly string passive = "Passive"; // it will always apply and has its own logic
        public static readonly string active = "Active"; // you have to activate it during combat or otherwise
        public static readonly string bonus = "Bonus"; // increases a stat, skill or asset based on trait's logic

        public static readonly List<string> All = new() 
        { 
            passive, active, bonus
        };
    }

    public static class Subtype
    {
        public static readonly string common = "Common"; // can be chosen multiple times
        public static readonly string unique = "Unique"; // can only be chosen once

        public static readonly List<string> All = new()
        {
            common, unique
        };
    }

    public static class ActiveTraits
    {
        // 1
        public static readonly string metachaosDaemonology = "Metachaos Daemonology";
        public static readonly string metachaosDaemonology_description = "Increases your spellcraft evocation prowess by 50% for all spells during an entire round, will occur in the next 3 odd rounds.";
        public static readonly string metachaosDaemonology_lore = "It is said amongst mages that this technique was initially discovered by the forefathers of the first humans of Khardah. Others claim it came from the sorceres of V'ald during the first descent of the elves upon the Cloud Kingdom of mankind. Although uncertainty shrouds this spellweave expertise, there is evidence that links the great pyramid in Khardah to the Illithos that shows how deeply research has this been.";

        public static readonly List<HeroicTrait> All = new()
        {
            new HeroicTrait
            {
                Identity = new HeroicTraitIdentity
                {
                    Id = "7718aef3-4d96-4686-841f-45f5cae0266b",
                    Name = metachaosDaemonology,
                },
                Description = metachaosDaemonology_description,
                Lore = metachaosDaemonology_lore,
                Type = Type.active,
                Subtype = Subtype.unique,
                DeedsCost = 100
            }
        };
    }

    public static class PassiveTraits
    {
        // 1
        public static readonly string fatePoint = "Fate Point";
        public static readonly string fatePoint_description = "Your critical hits start from 19 out of a d20 die.";
        public static readonly string fatePoint_lore = "Any adventurer whose hands hold the marks of a sword hilt or the burns of spellcraft will eventually develop this ability. It marks its bearer as one who has travelled roads seen by a few.";

        public static readonly List<HeroicTrait> All = new()
        {
            new HeroicTrait
            {
                Identity = new HeroicTraitIdentity
                {
                    Id = "f85a2b0f-428d-4691-8cdc-caa6c399ec94",
                    Name = fatePoint,
                },
                Description = fatePoint_description,
                Lore = fatePoint_lore,
                Type = Type.passive,
                Subtype = Subtype.unique,
                DeedsCost = 50
            }
        };
    }

    public static class BonusTraits
    {
        // 1
        public static readonly string swordsman = "Swordsman";
        public static readonly string swordsman_description = "Increases the base Combat skill by 10 plus another 1% of its Paperdoll amount.";
        public static readonly string swordsman_lore = "Steady arm and a stout shield are the best teachers you have ever known.";
        // 2
        public static readonly string skillful = "Skillful";
        public static readonly string skillful_description = "Increases a specific skill by 20% of its base amount.";
        public static readonly string skillful_lore = "A testament of your accomplishments during your years as an adventurer.";

        public static readonly List<HeroicTrait> All = new()
        {
            new HeroicTrait
            {
                Identity = new HeroicTraitIdentity
                {
                    Id = "0d5e0310-013b-42bd-b479-5c961dd583e1",
                    Name = skillful,
                },
                Description = skillful_description,
                Lore = skillful_lore,
                Type = Type.bonus,
                Subtype = Subtype.unique,
                DeedsCost = 5
            },
            new HeroicTrait
            {
                Identity = new HeroicTraitIdentity
                {
                    Id = "ea91b5bb-c338-431d-bef5-915483aac4a0",
                    Name = swordsman,
                },
                Description = swordsman_description,
                Lore = swordsman_lore,
                Type = Type.bonus,
                Subtype = Subtype.common,
                DeedsCost = 1
            }
        };
    }

}
