using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;

namespace Service_Delegators;

public interface ICharacterCRUDLogic
{
    CharacterStub CreateStub(string playerId);
    Character SaveStub(CharacterRacialTraits traits, string playerId);
    Character KillChar(CharacterIdentity charIdentity);
    void DeleteCharacter(CharacterIdentity charIdentity);
}

public class CharacterCRUDLogic : ICharacterCRUDLogic
{
    private readonly object _lock = new();

    private readonly Snapshot snapshot;
    private readonly IDiceLogicDelegator dice;
    private readonly IItemsLogicDelegator items;
    private readonly ICharacterSheetLogic characterSheet;
    private readonly IGameplayLogicDelegator gameplayLogic;

    public CharacterCRUDLogic(
        Snapshot snapshot,
        IDiceLogicDelegator dice,
        IItemsLogicDelegator items,
        ICharacterSheetLogic characterSheet,
        IGameplayLogicDelegator gameplayLogic)
    {
        this.snapshot = snapshot;
        this.dice = dice;
        this.items = items;
        this.characterSheet = characterSheet;
        this.gameplayLogic = gameplayLogic;
    }

    public CharacterStub CreateStub(string playerId)
    {
        lock (_lock)
        {
            snapshot.Stubs.RemoveAll(s => s.PlayerId == playerId);

            var entityLevel = RandomizeEntityLevel();

            var stub = new CharacterStub
            {
                Id = Guid.NewGuid().ToString(),
                PlayerId = playerId,
                EntityLevel = entityLevel,
                StatPoints = RandomizeStatPoints(entityLevel),
                SkillPoints = RandomizeSkillPoints(entityLevel),
            };

            snapshot.Stubs.Add(stub);
            
            return stub;
        }
    }

    public Character SaveStub(CharacterRacialTraits traits, string playerId)
    {
        lock (_lock)
        {
            var stub = snapshot.Stubs.Find(s => s.PlayerId == playerId)!;
            
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

            snapshot.Stubs.RemoveAll(s => s.PlayerId == playerId);

            var player = snapshot.Players.Find(p => p.Identity.Id == playerId)!;
            player.Characters!.Add(character);

            gameplayLogic.GetOrGenerateLocation(character.Status.Position);

            return character;
        }
    }

    public Character KillChar(CharacterIdentity charIdentity)
    {
        lock (_lock)
        {
            var character = ServicesUtils.GetPlayerCharacter(charIdentity, snapshot);
            character.Status!.Gameplay.IsAlive = false;

            return character;
        }
    }

    public void DeleteCharacter(CharacterIdentity charIdentity)
    {
        lock ( _lock)
        {
            var player = snapshot.Players.Find(s => s.Identity.Id == charIdentity.PlayerId)!;
            var character = player.Characters.Find(s => s.Identity.Id == charIdentity.Id)!;
            player.Characters.Remove(character);
        }
    }

    #region private methods
    private int RandomizeEntityLevel()
    {
        var roll = dice.Roll_d20_withReroll();

        if (roll >= 100) return 6;
        else if (roll >= 80) return 5;
        else if (roll >= 60) return 4;
        else if (roll >= 40) return 3;
        else if (roll >= 20) return 2;
        else  /*(roll >= 1)*/   return 1;
    }

    private int RandomizeStatPoints(int entityLevel)
    {
        var roll = dice.Roll_d20_withReroll();
        return roll * entityLevel;
    }

    private int RandomizeSkillPoints(int entityLevel)
    {
        var roll = dice.Roll_d20_withReroll();
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

        character.Inventory.Provisions = dice.Roll_d100_noReroll();
    }

    private static string SetFame(string culture, string classes)
    {
        return $"Known as the {culture} {classes.ToLower()}";
    }

    private void SetWealthAndWorth(Character character)
    {
        var sumOfSkills = character.Sheet.Skills.Melee
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
        characterSheet.SetCharacterSheet(stub.StatPoints, stub.SkillPoints, character);
    }

    private static void SetStatus(CharacterRacialTraits traits, CharacterStub stub, Character character)
    {
        character.Status = new()
        {
            Name = $"The {traits.Culture.ToLower()}",
            EntityLevel = stub!.EntityLevel,
            DateOfBirth = DateTime.Now.ToShortDateString(),
            Traits = new CharacterRacialTraits
            {
                Race = traits.Race,
                Culture = traits.Culture,
                Tradition = traits.Tradition,
                Class = traits.Class,
            },
            Gameplay = new CharacterGameplay
            {
                BattleboardId = string.Empty,
                IsAlive = true,
                IsNpc = false,
                IsLocked = false,
            },
            // all characters start from Arada due to it's travel dinstance logic
            // moreover the story focuses on Danar as starting position
            Position = new Position
            {
                Region = GameplayLore.Locations.Dragonmaw.RegionName,
                Subregion = GameplayLore.Locations.Dragonmaw.Farlindor.SubregionName,
                Land = GameplayLore.Locations.Dragonmaw.Farlindor.Danar.LandName,
                Location = GameplayLore.Locations.Dragonmaw.Farlindor.Danar.Arada.Name
            },
            Worth = 0,
            Wealth = 0,
            Fame = SetFame(traits.Culture, traits.Class),
            NrOfQuestsFinished = 0,
            QuestsFinished = new List<string>()
        };
    }
    #endregion
}
