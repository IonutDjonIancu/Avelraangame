using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Data_Mapping_Containers.Dtos;
using Service_Delegators;

namespace Avelraangame.Controllers;

[Route("api/palantir")]
[ApiController]
[EnableCors("allowSpecificOrigins")]
public class PalantirController : ControllerBase
{
    private readonly IValidations validations;

    private readonly IMetadataService metadata;

    private readonly IDatabaseLogicDelegator database;
    private readonly IPlayerLogicDelegator players;
    private readonly IItemsLogicDelegator items;
    private readonly ICharacterLogicDelegator characters;
    private readonly INpcLogicDelegator npcs;
    private readonly IGameplayLogicDelegator gameplay;

    public PalantirController(
        IValidations validations,
        IMetadataService metadata,
        IDatabaseLogicDelegator database,
        IPlayerLogicDelegator players,
        IItemsLogicDelegator items,
        ICharacterLogicDelegator characters,
        INpcLogicDelegator npcs,
        IGameplayLogicDelegator gameplay) 
    {
        this.validations = validations;
        this.metadata = metadata;
        this.database = database;
        this.players = players;
        this.items = items; 
        this.characters = characters;
        this.npcs = npcs;
        this.gameplay = gameplay;
    }

    #region ConnectionTest
    // GET: /api/palantir/Test/GetOk
    [HttpGet("Test/GetOk")]
    public IActionResult GetOk()
    {
        return Ok("All okay!");
    }
    #endregion

    #region Metadata
    // GET: /api/palantir/Metadata/GetPlayers
    [HttpGet("Metadata/GetPlayers")]
    public IActionResult GetPlayers()
    {
        return Ok(metadata.GetPlayers());
    }

    // GET: /api/palantir/Metadata/GetPlayer
    [HttpGet("Metadata/GetPlayer")]
    public IActionResult GetPlayer([FromQuery] string playerName, [FromQuery] string token)
    {
        var playerId = validations.ValidateApiRequest(new Request() { PlayerName = playerName, Token = token });

        return Ok(metadata.GetPlayer(playerId));
    }

    // GET: /api/palantir/Metadata/GetPlayerCharacter
    [HttpGet("Metadata/GetPlayerCharacter")]
    public IActionResult GetPlayer([FromQuery] string playerName, [FromQuery] string token, [FromQuery] string characterId)
    {
        var playerId = validations.ValidateApiRequest(new Request() { PlayerName = playerName, Token = token });

        return Ok(metadata.GetPlayer(playerId).Characters.Find(s => s.Identity.Id == characterId));
    }


    // GET: /api/palantir/Metadata/GetSpecialSkills
    [HttpGet("Metadata/GetSpecialSkills")]
    public IActionResult GetSpecialSkills()
    {
        return Ok(metadata.GetSpecialSkills());
    }

    // GET: /api/palantir/Metadata/GetRaces
    [HttpGet("Metadata/GetRaces")]
    public IActionResult GetRaces()
    {
        return Ok(metadata.GetRaces());
    }

    // GET: /api/palantir/Metadata/GetCultures
    [HttpGet("Metadata/GetCultures")]
    public IActionResult GetCultures()
    {
        return Ok(metadata.GetCultures());
    }

    // GET: /api/palantir/Metadata/GetClasses
    [HttpGet("Metadata/GetClasses")]
    public IActionResult GetClasses()
    {
        return Ok(metadata.GetClasses());
    }

    // GET: /api/palantir/Metadata/GetAllLocations
    [HttpGet("Metadata/GetAllLocations")]
    public IActionResult GetAllLocations()
    {
        return Ok(metadata.GetAllLocations());
    }
    #endregion

    #region Database
    // PUT: /api/palantir/Database/ExportDatabase
    [HttpPut("Database/ExportDatabase")]
    public IActionResult ExportDatabase([FromQuery] Request request)
    {
        try
        {
            var playerId = validations.ValidateApiRequest(request);

            database.ExportDatabase(playerId);

            return Ok("Database exported successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Database/ExportLogs
    [HttpPut("Database/ExportLogs")]
    public IActionResult ExportLogs([FromQuery] Request request, int days)
    {
        try
        {
            var playerId = validations.ValidateApiRequest(request);

            database.ExportLogs(playerId, days);

            return Ok("Logs exported successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Database/ImportPlayer
    [HttpPut("Database/ImportPlayer")]
    public IActionResult ImportPlayer([FromQuery] Request request, [FromBody] string playerJsonString)
    {
        try
        {
            var playerId = validations.ValidateApiRequest(request);

            database.ImportPlayer(playerId, playerJsonString);

            return Ok("Player imported successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Player
    // POST: /api/palantir/Player/CreatePlayer
    [HttpPost("Player/CreatePlayer")]
    public IActionResult CreatePlayer([FromQuery] string playerName)
    {
        try
        {
            var autheticatorSetupInfo = players.CreatePlayer(playerName);

            if (autheticatorSetupInfo == null) return Conflict("Unable to create player."); 
            
            return Ok(autheticatorSetupInfo);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Player/LoginPlayer
    [HttpPut("Player/LoginPlayer")]
    public IActionResult LoginPlayer([FromQuery] PlayerLogin login)
    {
        try
        {
            var token = players.LoginPlayer(login);

            return Ok(token);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // DELETE: /api/palantir/Player/DeletePlayer
    [HttpDelete("Player/DeletePlayer")]
    public IActionResult DeletePlayer([FromQuery] Request request)
    {
        try
        {
            var playerId = validations.ValidateApiRequest(request);

            players.DeletePlayer(playerId);

            return Ok("Player deleted successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Items
    // GET: /api/palantir/Item/GenerateRandomItem
    [HttpGet("Item/GenerateRandomItem")]
    public IActionResult GenerateRandomItem()
    {
        var item = items.GenerateRandomItem();

        return Ok(item);
    }

    // GET: /api/palantir/Item/GenerateSpecificItem
    [HttpGet("Item/GenerateSpecificItem")]
    public IActionResult GenerateSpecificItem(string type, string subtype)
    {
        try
        {
            var item = items.GenerateSpecificItem(type, subtype);

            return Ok(item);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Characters
    // GET: /api/palantir/Character/CreateCharacter
    [HttpGet("Character/CreateCharacter")]
    public IActionResult CreateCharacter([FromQuery] Request request)
    {
        try
        {
            var playerId = validations.ValidateApiRequest(request);
            var stub = characters.CreateCharacterStub(playerId);

            return Ok(stub);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // POST: /api/palantir/Character/SaveCharacter
    [HttpPost("Character/SaveCharacter")]
    public IActionResult SaveCharacter([FromQuery] Request request, [FromBody] CharacterTraits traits)
    {
        try
        {
            var playerId = validations.ValidateApiRequest(request);
            var character = characters.SaveCharacterStub(traits, playerId);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/UpdateCharacterName
    [HttpPut("Character/UpdateCharacterName")]
    public IActionResult UpdateCharacterName([FromQuery] Request request, string name, string characterId)
    {
        try
        {
            var playerId = validations.ValidateApiRequest(request);
            var character = characters.UpdateCharacterName(name, new CharacterIdentity { Id = characterId, PlayerId = playerId });

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // DELETE: /api/palantir/Character/DeleteCharacter
    [HttpDelete("Character/DeleteCharacter")]
    public IActionResult DeleteCharacter([FromQuery] Request request, string characterId)
    {
        try
        {
            var playerId = validations.ValidateApiRequest(request);
            characters.DeleteCharacter(new CharacterIdentity() { Id = characterId, PlayerId = playerId });

            return Ok("Character deleted");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/EquipItem
    [HttpPut("Character/EquipItem")]
    public IActionResult EquipItem([FromQuery] Request request, [FromBody] CharacterEquip equip)
    {
        try
        {
            equip.CharacterIdentity.PlayerId = validations.ValidateApiRequest(request);
            var character = characters.EquipCharacterItem(equip);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/UnEquipItem
    [HttpPut("Character/UnEquipItem")]
    public IActionResult UnEquipItem([FromQuery] Request request, [FromBody] CharacterEquip unequip)
    {
        try
        {
            unequip.CharacterIdentity.PlayerId = validations.ValidateApiRequest(request);
            var character = characters.UnequipCharacterItem(unequip);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/LearnSpecialSkill
    [HttpPut("Character/LearnSpecialSkill")]
    public IActionResult LearnSpecialSkill([FromQuery] Request request, [FromBody] CharacterAddSpecialSkill trait)
    {
        try
        {
            trait.CharacterIdentity.PlayerId = validations.ValidateApiRequest(request);
            var character = characters.LearnCharacterSpecialSkill(trait);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/IncreaseStats
    [HttpPut("Character/IncreaseStats")]
    public IActionResult IncreaseStats([FromQuery] Request request, [FromBody] CharacterIncreaseAttributes attributes)
    {
        try
        {
            attributes.CharacterIdentity.PlayerId = validations.ValidateApiRequest(request);
            var character = characters.IncreaseCharacterStats(attributes.Stat, attributes.CharacterIdentity);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/IncreaseAssets
    [HttpPut("Character/IncreaseAssets")]
    public IActionResult IncreaseAssets([FromQuery] Request request, [FromBody] CharacterIncreaseAttributes attributes)
    {
        try
        {
            attributes.CharacterIdentity.PlayerId = validations.ValidateApiRequest(request);
            var character = characters.IncreaseCharacterAssets(attributes.Asset, attributes.CharacterIdentity);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/IncreaseSkills
    [HttpPut("Character/IncreaseSkills")]
    public IActionResult IncreaseSkills([FromQuery] Request request, [FromBody] CharacterIncreaseAttributes attributes)
    {
        try
        {
            attributes.CharacterIdentity.PlayerId = validations.ValidateApiRequest(request);
            var character = characters.IncreaseCharacterSkills(attributes.Skill, attributes.CharacterIdentity);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/TravelToLocation
    [HttpPut("Character/TravelToLocation")]
    public IActionResult TravelToLocation([FromQuery] Request request, [FromBody] CharacterTravel travel)
    {
        try
        {
            travel.CharacterIdentity.PlayerId = validations.ValidateApiRequest(request);
            var character = characters.TravelCharacterToLocation(travel);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/HireMercenary
    [HttpPut("Character/HireMercenary")]
    public IActionResult HireMercenary([FromQuery] Request request, [FromBody] CharacterHireMercenary hireMercenary)
    {
        try
        {
            hireMercenary.CharacterIdentity.PlayerId = validations.ValidateApiRequest(request);
            var character = characters.HireMercenaryForCharacter(hireMercenary);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/SellItem
    [HttpPut("Character/SellItem")]
    public IActionResult SellItem([FromQuery] Request request, [FromBody] CharacterItemTrade tradeItem)
    {
        try
        {
            tradeItem.CharacterIdentity.PlayerId = validations.ValidateApiRequest(request);
            var character = characters.SellItem(tradeItem);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Npcs
    // GET: /api/palantir/NPC/GenerateGoodGuyNPC
    [HttpGet("NPC/GenerateGoodGuyNPC")]
    public IActionResult GenerateGoodGuyNPC(string locationName)
    {
        try
        {
            var npc = npcs.GenerateGoodGuy(locationName);

            return Ok(npc);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // GET: /api/palantir/NPC/GenerateBadGuyNPC
    [HttpGet("NPC/GenerateBadGuyNPC")]
    public IActionResult GenerateBadGuyNPC(string locationName)
    {
        try
        {
            var npc = npcs.GenerateBadGuy(locationName);

            return Ok(npc);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Gameplay
    // POST: /api/palantir/Gameplay/GetLocation
    [HttpPost("Gameplay/GetLocation")]
    public IActionResult GetLocation([FromBody] Position position)
    {
        try
        {
            var location = gameplay.GetOrGenerateLocation(position);

            return Ok(location);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion
}