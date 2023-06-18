namespace Tests;

public class NpcServiceTests : TestBase
{
    [Theory]
    [Description("Create an Npc.")]
    public void Generate_Npc_test()
    {
        var npcInfo = new NpcInfo
        {
            Difficulty = GameplayLore.Rulebook.Quests.Difficulty.Standard,
            Region = GameplayLore.Rulebook.Regions.EastDragonmaw.Farlindor,
            Tradition = GameplayLore.Rulebook.Tradition.Common,

            StatsMin = new CharacterStats
            {
                Strength = 5,
                Constitution = 5,
                Agility = 5,
                Willpower = 5,
                Perception = 5,
                Abstract = 5
            },
            StatsMax = new CharacterStats
            {
                Strength = 10,
                Constitution = 10,
                Agility = 10,
                Willpower = 10,
                Perception = 10,
                Abstract = 10
            },

            AssetsMin = new CharacterAssets
            {
                Resolve = 5,
                Harm = 5,
                Spot = 5,
                Defense = 5,
                Purge = 5,
                Mana = 5
            },
            AssetsMax = new CharacterAssets
            {
                Resolve = 15,
                Harm = 15,
                Spot = 15,
                Defense = 15,
                Purge = 15,
                Mana = 15
            },

            SkillsMin = new CharacterSkills
            {
                Combat = 10,
                Arcane = 10,
                Psionics = 10,
                Hide = 10,
                Traps = 10,
                Tactics = 10,
                Social = 10,
                Apothecary = 10,
                Travel = 10,
                Sail = 10
            },
            SkillsMax = new CharacterSkills
            {
                Combat = 20,
                Arcane = 20,
                Psionics = 20,
                Hide = 20,
                Traps = 20,
                Tactics = 20,
                Social = 20,
                Apothecary = 20,
                Travel = 20,
                Sail = 20
            }
        };

        var npc = npcService.GenerateNpc(npcInfo);

        npc.Should().NotBeNull();
        npc.Paperdoll.Should().NotBeNull();
        npc.Items.Should().NotBeNull();

        GameplayLore.Rulebook.Npcs.Races.All.Should().Contain(npc.Origins.Race);
        CharactersLore.Tradition.All.Should().Contain(npc.Origins.Tradition);
        CharactersLore.Classes.All.Should().Contain(npc.Origins.Class);

        npc.Paperdoll.Stats.Strength.Should().BeGreaterThanOrEqualTo(npcInfo.StatsMin.Strength);
        npc.Paperdoll.Assets.Resolve.Should().BeGreaterThanOrEqualTo(npcInfo.AssetsMin.Resolve);
        npc.Paperdoll.Skills.Combat.Should().BeGreaterThanOrEqualTo(npcInfo.SkillsMin.Combat);

        npc.Wealth.Should().BeGreaterThan(0);
    }
}
