﻿using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Lore;
using static System.Collections.Specialized.BitVector32;

namespace Service_Delegators;

public interface ICharacterCRUDLogic
{
    void ClearStubs(string playerId);
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

    public void ClearStubs(string playerId)
    {
        lock (_lock)
        {
            snapshot.Stubs.RemoveAll(s => s.PlayerId == playerId);
        }
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
            SetIcon(traits, character);
            SetSuppliesAndProvisions(character);

            //TODO: set cultural bonuses like Human Danarian gets extra armour pieces, etc, wood elves get a bow, etc

            snapshot.Stubs.RemoveAll(s => s.PlayerId == playerId);

            var player = snapshot.Players.Find(p => p.Identity.Id == playerId)!;
            player.Characters!.Add(character);

            gameplayLogic.GetOrGenerateLocation(character.Status.Position);
            SetWealthAndWorth(character);

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

            character.Mercenaries.ForEach(merc =>
            {
                if (merc.Status.Gameplay.IsAlive)
                {
                    merc.Identity.PlayerId = Guid.Empty.ToString();
                    merc.Status.Gameplay.BattleboardId = string.Empty;
                    merc.Status.Gameplay.IsHidden = false;
                    merc.Status.Gameplay.IsLocked = false;
                    merc.Status.Position.Region = GameplayLore.Locations.Dragonmaw.RegionName;
                    merc.Status.Position.Subregion = GameplayLore.Locations.Dragonmaw.Soudheim.SubregionName;
                    merc.Status.Position.Land = GameplayLore.Locations.Dragonmaw.Soudheim.Danar.LandName;
                    merc.Status.Position.Region = GameplayLore.Locations.Dragonmaw.Soudheim.Danar.Arada.Name;
                }
            });

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
            item.Identity.CharacterId = character.Identity.Id;
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
        var wealth = 10;
        var rollTimes = dice.Roll_1_to_n(6);
        for (int i = 0; i < rollTimes; i++)
        {
            wealth += dice.Roll_1_to_n(100);
        }

        character.Status.Worth = ServicesUtils.CalculateWorth(character, dice);
        character.Status.Wealth = wealth;
    }

    private void SetSheet(CharacterStub stub, Character character)
    {
        characterSheet.SetCharacterSheet(stub.StatPoints, stub.SkillPoints, character);
    }

    private void SetIcon(CharacterRacialTraits traits, Character character)
    {
        if (traits.Icon != 0)
        {
            character.Status.Traits.Icon = traits.Icon;

        } else
        {
            var iconNr = dice.Roll_1_to_n(3);
            character.Status.Traits.Icon = iconNr;
        }
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
                Subregion = GameplayLore.Locations.Dragonmaw.Soudheim.SubregionName,
                Land = GameplayLore.Locations.Dragonmaw.Soudheim.Danar.LandName,
                Location = GameplayLore.Locations.Dragonmaw.Soudheim.Danar.Arada.Name
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
