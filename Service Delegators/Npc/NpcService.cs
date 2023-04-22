using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;
using Persistance_Manager;

namespace Service_Delegators;

public class NpcService : INpcService
{
    private readonly IDatabaseManager dbm;
    private readonly IItemService itemService;
    private readonly ICharacterService characterService;

    public NpcService(
        IDatabaseManager databaseManager,
        IDiceRollService diceRollService,
        IItemService itemService,
        ICharacterService characterService)
    {
        dbm = databaseManager;
        this.itemService = itemService;
        this.characterService = characterService;

    }

    public CharacterPaperdoll GenerateNpc()
    {
        var id = Guid.NewGuid().ToString();
        var level = 1;
        var money = 100;
        var statsAverage = 5;
        var assetsAverage = 5;
        var skillsAverage = 5;
        string[] npcClass = { CharactersLore.Classes.Warrior, CharactersLore.Classes.Mage, CharactersLore.Classes.Hunter };


        var character = new Character
        {
            Identity = new CharacterIdentity
            {
                Id = id,
                Name = $"Npc-{id}",
                PlayerId = Guid.Empty.ToString()
            },
            Info = new CharacterInfo
            {
                EntityLevel = level,
                Wealth = money,

                Race = CharactersLore.Races.Human,
                Culture = CharactersLore.Cultures.Human.Danarian,
                Class = npcClass[0],
                Heritage = CharactersLore.Heritage.Traditional
            },
            IsAlive = true,

            Sheet = new CharacterSheet
            {
                Stats = new CharacterStats
                {
                    Strength = statsAverage,
                    Constitution = statsAverage,
                    Agility = statsAverage,
                    Willpower = statsAverage,
                    Perception = statsAverage,
                    Abstract = statsAverage,
                },
                Assets = new CharacterAssets
                {
                    Resolve = assetsAverage,
                    Harm = assetsAverage,
                    Spot = assetsAverage,
                    Defense = assetsAverage,
                    Purge = assetsAverage,
                    Mana = assetsAverage
                },
                Skills = new CharacterSkills
                {
                    Combat = skillsAverage,
                    Arcane = skillsAverage,
                    Psionics = skillsAverage,
                    Hide = skillsAverage,
                    Traps = skillsAverage,
                    Tactics = skillsAverage
                }
            },

            Inventory = new CharacterInventory
            {
                Head = itemService.GenerateSpecificItem("protection", "helm"),
                Body = itemService.GenerateSpecificItem("protection", "armour"),
                Shield = itemService.GenerateSpecificItem("protection", "shield"),
                Mainhand = itemService.GenerateSpecificItem("weapon", "sword"),
                Ranged = itemService.GenerateSpecificItem("weapon", "bow")
            }
        };

        var paperdoll = characterService.CalculateCharacterPaperdollForNpc(character);

        return paperdoll;
    }


}
