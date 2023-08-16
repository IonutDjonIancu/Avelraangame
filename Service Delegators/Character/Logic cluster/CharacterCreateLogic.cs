using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;

namespace Service_Delegators;

internal class CharacterCreateLogic
{
    private readonly IDatabaseService dbs;
    private readonly IDiceRollService dice;
    private readonly IItemService items;

    private readonly CharacterSheetLogic sheetLogic;

    private CharacterCreateLogic() { }
    internal CharacterCreateLogic(
        IDatabaseService databaseService,
        IDiceRollService diceService,
        IItemService itemService,
        CharacterSheetLogic characterSheetLogic)
    {
        dbs = databaseService;
        dice = diceService;
        items = itemService;

        sheetLogic = characterSheetLogic;
    }

    internal CharacterStub CreateStub(string playerId)
    {
        dbs.Snapshot.CharacterStubs.RemoveAll(s => s.PlayerId == playerId);

        var entityLevel = RandomizeEntityLevel();

        var stub = new CharacterStub
        {
            PlayerId = playerId,
            EntityLevel = entityLevel,
            StatPoints = RandomizeStatPoints(entityLevel),
            SkillPoints = RandomizeSkillPoints(entityLevel),
        };

        dbs.Snapshot.CharacterStubs.Add(stub);

        return stub;
    }

    internal Character SaveStub(CharacterTraits traits, string playerId)
    {
        var stub = dbs.Snapshot.CharacterStubs!.Find(s => s.PlayerId == playerId)!;

        Character character = new()
        {
            Identity = new CharacterIdentity
            {
                Id = Guid.NewGuid().ToString(),
                PlayerId = playerId,
            }
        };

        SetStatus(traits, stub, character);
        SetSheet(stub, character);
        SetSuppliesAndProvisions(character);
        SetWealthAndWorth(character);

        //TODO: set cultural bonuses like Human Danarian gets extra armour pieces, etc, wood elves get a bow, etc

        dbs.Snapshot.CharacterStubs.RemoveAll(s => s.PlayerId == playerId);

        var player = dbs.Snapshot.Players.Find(p => p.Identity.Id == playerId)!;
        player.Characters!.Add(character);

        dbs.PersistPlayer(playerId);

        return character;
    }

    #region private methods
    private int RandomizeEntityLevel()
    {
        var roll = dice.Roll_20_withReroll();

        if      (roll >= 100)   return 6;
        else if (roll >= 80)    return 5;
        else if (roll >= 60)    return 4;
        else if (roll >= 40)    return 3;
        else if (roll >= 20)    return 2;
        else  /*(roll >= 1)*/   return 1;
    }

    private int RandomizeStatPoints(int entityLevel)
    {
        var roll = dice.Roll_20_withReroll();
        return roll * entityLevel;
    }

    private int RandomizeSkillPoints(int entityLevel)
    {
        var roll = dice.Roll_20_withReroll();
        return roll * entityLevel;
    }

    private void SetSuppliesAndProvisions(Character character)
    {
        var roll = dice.Roll_1_to_n(6);

        for (int i = 0; i < roll; i++)
        {
            var item = items.GenerateRandomItem();
            character.Inventory.Supplies.Add(item);
        }

        character.Inventory.Provisions = dice.Roll_100_noReroll();
    }

    private static string SetFame(string culture, string classes)
    {
        return $"Known as the {culture} {classes.ToLower()}";
    }

    private void SetWealthAndWorth(Character character)
    {
        var sumOfSkills = character.Sheet.Skills.Combat
            + character.Sheet.Skills.Arcane
            + character.Sheet.Skills.Psionics
            + character.Sheet.Skills.Hide
            + character.Sheet.Skills.Traps
            + character.Sheet.Skills.Tactics
            + character.Sheet.Skills.Social
            + character.Sheet.Skills.Apothecary
            + character.Sheet.Skills.Travel
            + character.Sheet.Skills.Sail;

        var wealth = 10;
        var rollTimes = dice.Roll_1_to_n(6);
        for (int i = 0; i < rollTimes; i++)
        {
            wealth += dice.Roll_1_to_n(100);
        }

        character.Status.Worth = (int)((sumOfSkills + wealth) * 0.25);
        character.Status.Wealth = wealth;
    }

    private void SetSheet(CharacterStub stub, Character character)
    {
        sheetLogic.SetCharacterSheet(stub.StatPoints, stub.SkillPoints, character);
    }

    private static void SetStatus(CharacterTraits traits, CharacterStub stub, Character character)
    {
        character.Status = new()
        {
            Name = $"The {traits.Culture.ToLower()}",
            EntityLevel = stub!.EntityLevel,
            DateOfBirth = DateTime.Now.ToShortDateString(),
            Traits = new CharacterTraits
            {
                Race = traits.Race,
                Culture = traits.Culture,
                Tradition = traits.Tradition,
                Class = traits.Class,
            },
            Gameplay = new CharacterGameplay
            {
                ArenaId = string.Empty,
                QuestId = string.Empty,
                StoryId = string.Empty,
            },
            // all characters start from Arada due to it's travel dinstance logic
            // moreover the story focuses on Danar as starting position
            Position = new Position
            {
                Region = GameplayLore.Locations.Dragonmaw.RegionName,
                Subregion = GameplayLore.Locations.Dragonmaw.Farlindor.SubregionName,
                Land = GameplayLore.Locations.Dragonmaw.Farlindor.Danar.LandName,
                Location = GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.LocationName
            },
            IsAlive = true,
            IsNpc = false,
            IsLockedToModify = false,
            Worth = 0,
            Wealth = 0,
            Fame = SetFame(traits.Culture, traits.Class),
            NrOfQuestsFinished = 0,
            QuestsFinished = new List<string>()
        };
    }
    #endregion
}
