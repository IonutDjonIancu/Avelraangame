#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Pocos;
using Persistance_Manager;

namespace Service_Delegators;

internal class CharacterLogic
{
    private readonly IDatabaseManager dbm;
    private readonly IDiceRollService dice;
    private readonly IItemService itemService;
    private readonly CharacterMetadata charMetadata;
    private readonly CharacterDollOperations dollOperations;

    private CharacterLogic() { }

    internal CharacterLogic(
        IDatabaseManager databaseManager,
        IDiceRollService diceRollService,
        IItemService itemService,
        CharacterMetadata charMetadata)
    {
        dbm = databaseManager;
        dice = diceRollService;
        this.itemService = itemService;
        this.charMetadata = charMetadata;
        dollOperations = new CharacterDollOperations(dice);
    }

    internal CharacterStub CreateStub(string playerId)
    {
        dbm.Snapshot.CharacterStubs?.RemoveAll(s => s.PlayerId == playerId);

        var entityLevel = RandomizeEntityLevel();

        var stub = new CharacterStub
        {
            PlayerId = playerId,
            EntityLevel = entityLevel,
            StatPoints = RandomizeStatPoints(entityLevel),
            SkillPoints = RandomizeSkillPoints(entityLevel),
        };

        dbm.Snapshot.CharacterStubs?.Add(stub);

        return stub;
    }

    internal Character SaveStub(CharacterOrigins origins, string playerId)
    {
        var stub = dbm.Snapshot.CharacterStubs!.Find(s => s.PlayerId == playerId)!;
        var info = CreateCharacterInfo(origins, stub);

        Character character = new()
        {
            Identity = new CharacterIdentity
            {
                Id = Guid.NewGuid().ToString(),
                PlayerId = playerId,
            },

            Info = info,

            LevelUp = new CharacterLevelUp(),

            Doll = dollOperations.SetPaperDoll(info, stub.StatPoints, stub.SkillPoints),
            Traits = new List<CharacterTrait>(),
            Inventory = new CharacterInventory(),
            Supplies = SetSupplies(),

            IsAlive = true,
        };

        character.Info.Wealth = SetWealth();

        // set cultural bonuses like Human Danarian gets extra armour pieces, etc, wood elves get a bow, etc

        dbm.Snapshot.CharacterStubs.RemoveAll(s => s.PlayerId == playerId);

        var player = dbm.Metadata.GetPlayerById(playerId); 
        player.Characters!.Add(character);

        dbm.PersistPlayer(player);

        return character;
    }

    internal Character ChangeName(CharacterUpdate charUpdate, string playerId)
    {
        var oldChar = charMetadata.GetCharacter(charUpdate.CharacterId, playerId);

        oldChar.Info.Name = charUpdate.Name;

        var player = dbm.Metadata.GetPlayerById(playerId);
        
        dbm.PersistPlayer(player);

        return oldChar;
    }

    internal void DeleteCharacter(string characterId, string playerId)
    {
        var player = dbm.Metadata.GetPlayerById(playerId);
        var character = player.Characters.Find(c => c.Identity.Id == characterId);

        player.Characters.Remove(character);
        
        dbm.PersistPlayer(player);
    }

    internal Character IncreaseSkills(CharacterUpdate charUpdate, string playerId)
    {
        var storedChar = charMetadata.GetCharacter(charUpdate.CharacterId, playerId);

        if      (charUpdate.Skill == CharactersLore.Skills.Combat) storedChar.Doll.Combat++;
        else if (charUpdate.Skill == CharactersLore.Skills.Arcane) storedChar.Doll.Arcane++;
        else if (charUpdate.Skill == CharactersLore.Skills.Psionics) storedChar.Doll.Psionics++;
        else if (charUpdate.Skill == CharactersLore.Skills.Hide) storedChar.Doll.Hide++;
        else if (charUpdate.Skill == CharactersLore.Skills.Traps) storedChar.Doll.Traps++;
        else if (charUpdate.Skill == CharactersLore.Skills.Tactics) storedChar.Doll.Tactics++;
        else if (charUpdate.Skill == CharactersLore.Skills.Social) storedChar.Doll.Social++;
        else if (charUpdate.Skill == CharactersLore.Skills.Apothecary) storedChar.Doll.Apothecary++;
        else if (charUpdate.Skill == CharactersLore.Skills.Travel) storedChar.Doll.Travel++;
        else if (charUpdate.Skill == CharactersLore.Skills.Sail) storedChar.Doll.Sail++;
        else throw new Exception("Unrecognized skill.");
        
        storedChar.LevelUp.SkillPoints--;

        var player = dbm.Metadata.GetPlayerById(playerId);

        Thread.Sleep(100);
        dbm.PersistPlayer(player);

        return storedChar;
    }

    internal Character IncreaseStats(CharacterUpdate charUpdate, string playerId)
    {
        var storedChar = charMetadata.GetCharacter(charUpdate.CharacterId, playerId);

        if      (charUpdate.Stat == CharactersLore.Stats.Strength) storedChar.Doll.Strength++;
        else if (charUpdate.Stat == CharactersLore.Stats.Constitution) storedChar.Doll.Constitution++;
        else if (charUpdate.Stat == CharactersLore.Stats.Agility) storedChar.Doll.Agility++;
        else if (charUpdate.Stat == CharactersLore.Stats.Willpower) storedChar.Doll.Willpower++;
        else if (charUpdate.Stat == CharactersLore.Stats.Perception) storedChar.Doll.Perception++;
        else if (charUpdate.Stat == CharactersLore.Stats.Abstract) storedChar.Doll.Abstract++;
        else throw new Exception("Unrecognized stat.");

        storedChar.LevelUp.StatPoints--;
        
        var player = dbm.Metadata.GetPlayerById(playerId);
        
        Thread.Sleep(100);
        dbm.PersistPlayer(player);

        return storedChar;
    }

    internal Character UnequipItem(CharacterEquip unequip, string playerId)
    {
        var chr = charMetadata.GetCharacter(unequip.CharacterId, playerId);
        Item item;

        if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Head)
        {
            item = chr.Inventory.Head;
            chr.Inventory.Head = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Body)
        {
            item = chr.Inventory.Body;
            chr.Inventory.Body = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Shield)
        {
            item = chr.Inventory.Shield;
            chr.Inventory.Shield = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand)
        {
            item = chr.Inventory.Mainhand;
            chr.Inventory.Mainhand = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Offhand)
        {
            item = chr.Inventory.Offhand;
            chr.Inventory.Offhand = null;
        }
        else if (unequip.InventoryLocation == ItemsLore.InventoryLocation.Ranged)
        {
            item = chr.Inventory.Ranged;
            chr.Inventory.Ranged = null;
        }
        else
        {
            item = chr.Inventory.Heraldry.Find(i => i.Identity.Id == unequip.CharacterId);
            chr.Inventory.Heraldry.Remove(item);
        }

        chr.Supplies.Add(item);

        Thread.Sleep(100);
        dbm.PersistPlayer(dbm.Metadata.GetPlayerById(playerId));

        return chr;
    }

    internal Character EquipItem(CharacterEquip equip, string playerId)
    {
        var chr = charMetadata.GetCharacter(equip.CharacterId, playerId);
        var item = chr.Supplies.Find(i => i.Identity.Id == equip.ItemId);

        if (equip.InventoryLocation == ItemsLore.InventoryLocation.Head)
        {
            if (chr.Inventory.Head != null) UnequipItem(equip, playerId);
            chr.Inventory.Head = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Body)
        {
            if (chr.Inventory.Body != null) UnequipItem(equip, playerId);
            chr.Inventory.Body = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Shield)
        {
            if (chr.Inventory.Shield != null) UnequipItem(equip, playerId);
            chr.Inventory.Shield = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Mainhand)
        {
            if (chr.Inventory.Mainhand != null) UnequipItem(equip, playerId);
            chr.Inventory.Mainhand = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Offhand)
        {
            if (chr.Inventory.Offhand != null) UnequipItem(equip, playerId);
            chr.Inventory.Offhand = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Ranged)
        {
            if (chr.Inventory.Ranged != null) UnequipItem(equip, playerId);
            chr.Inventory.Ranged = item;
        }
        else if (equip.InventoryLocation == ItemsLore.InventoryLocation.Heraldry)
        {
            chr.Inventory.Heraldry.Add(item);
        }

        chr.Supplies.Remove(item);

        Thread.Sleep(100);
        dbm.PersistPlayer(dbm.Metadata.GetPlayerById(playerId));

        return chr;
    }

    #region privates
    private int RandomizeEntityLevel()
    {
        var roll = dice.Roll_d20(true);

        if      (roll >= 100)   return 6;
        else if (roll >= 80)    return 5;
        else if (roll >= 60)    return 4;
        else if (roll >= 40)    return 3;
        else if (roll >= 20)    return 2;
        else  /*(roll >= 1)*/   return 1;
    }

    private int RandomizeStatPoints(int entityLevel)
    {
        var roll = dice.Roll_d20(true);
        return roll * entityLevel;
    }

    private int RandomizeSkillPoints(int entityLevel)
    {
        var roll = dice.Roll_d20(true);
        return roll * entityLevel;
    }

    private List<Item> SetSupplies()
    {
        var roll = dice.Roll_dX(6);
        var supplies = new List<Item>();

        for (int i = 0; i < roll; i++)
        {
            var item = itemService.GenerateRandomItem();
            supplies.Add(item);
        }

        return supplies;
    }

    private static string SetFame(string culture, string classes)
    {
        return $"Known as the {culture} {classes.ToLower()}";
    }

    private int SetWealth()
    {
        var rollTimes = dice.Roll_dX(6);
        var total = 10;
        for (int i = 0; i < rollTimes; i++)
        {
            total += dice.Roll_dX(100);
        }

        return total;
    }

    private static CharacterInfo CreateCharacterInfo(CharacterOrigins origins, CharacterStub stub)
    {
        return new CharacterInfo
        {
            Name = $"The {origins.Culture.ToLower()}",
            EntityLevel = stub!.EntityLevel,

            Race = origins.Race,
            Culture = origins.Culture,
            Tradition = origins.Tradition,
            Class = origins.Class,

            DateOfBirth = DateTime.Now.ToShortDateString(),

            Fame = SetFame(origins.Culture, origins.Class),
        };
    }

    #endregion
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.


