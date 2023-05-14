using Serilog;
using Avelraangame.Factories;
using Data_Mapping_Containers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Data_Mapping_Containers.Dtos;
using Avelraangame.Controllers.Validators;

namespace Avelraangame.Controllers;

[Route("api/palantir")]
[ApiController]
[EnableCors("allowSpecificOrigins")]
public class PalantirController : ControllerBase
{
    private readonly IFactoryManager factory;
    private readonly ControllerValidator validator;

    public PalantirController(IFactoryManager factory)
    {
        validator = new ControllerValidator(factory.ServiceFactory.DatabaseService);
        this.factory = factory;
    }

    #region ConnectionTest
    // GET: /api/palantir/Test/GetOk
    [HttpGet("Test/GetOk")]
    public IActionResult GetOk()
    {
        return Ok("Okay");
    }
    #endregion

    #region Database gets
    // technically these could be moved to a different api
    // technically these could be moved to a different api
    // technically these could be moved to a different api

    // GET: /api/palantir/Database/GetParties
    [HttpGet("Database/GetParties")]
    public IActionResult GetParties()
    {
        var response = factory.ServiceFactory.DatabaseService.GetParties();

        return Ok(response);
    }

    // GET: /api/palantir/Database/GetPlayers
    [HttpGet("Database/GetPlayers")]
    public IActionResult GetPlayers()
    {
        var response = factory.ServiceFactory.DatabaseService.GetPlayers();

        return Ok(response);
    }

    // GET: /api/palantir/Database/GetHeroicTraits
    [HttpGet("Database/GetHeroicTraits")]
    public IActionResult GetHeroicTraits()
    {
        var response = factory.ServiceFactory.DatabaseService.GetHeroicTraits();

        return Ok(response);
    }

    // GET: /api/palantir/Database/GetRaces
    [HttpGet("Database/GetRaces")]
    public IActionResult GetRaces()
    {
        var response = factory.ServiceFactory.DatabaseService.GetRaces();

        return Ok(response);
    }

    // GET: /api/palantir/Database/GetCultures
    [HttpGet("Database/GetCultures")]
    public IActionResult GetCultures()
    {
        var response = factory.ServiceFactory.DatabaseService.GetCultures();

        return Ok(response);
    }

    // GET: /api/palantir/Database/GetClasses
    [HttpGet("Database/GetClasses")]
    public IActionResult GetClasses()
    {
        var response = factory.ServiceFactory.DatabaseService.GetClasses();

        return Ok(response);
    }
    #endregion

    #region Database ops
    // PUT: /api/palantir/Database/ExportDatabase
    [HttpPut("Database/ExportDatabase")]
    public IActionResult ExportDatabase([FromQuery] Request request)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            factory.ServiceFactory.DatabaseService.ExportDatabase(playerId);

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
            var playerId = MatchTokensForPlayer(request);

            factory.ServiceFactory.DatabaseService.ExportLogs(days, playerId);

            return Ok("Logs exported successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Database/ImportDatabase
    [HttpPut("Database/ImportDatabase")]
    public IActionResult ImportDatabase([FromQuery] Request request, [FromBody] string databaseJsonString)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            throw new NotImplementedException();

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
            var playerId = MatchTokensForPlayer(request);

            throw new NotImplementedException();

            return Ok("Logs exported successfully.");
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
            var autheticatorSetupInfo = factory.ServiceFactory.PlayerService.CreatePlayer(playerName);

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
            var token = factory.ServiceFactory.PlayerService.LoginPlayer(login);

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
            var playerId = MatchTokensForPlayer(request);

            factory.ServiceFactory.PlayerService.DeletePlayer(playerId);

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
        var item = factory.ServiceFactory.ItemService.GenerateRandomItem();

        return Ok(item);
    }

    // GET: /api/palantir/Item/GenerateSpecificItem
    [HttpGet("Item/GenerateSpecificItem")]
    public IActionResult GenerateSpecificItem(string type, string subtype)
    {
        try
        {
            var item = factory.ServiceFactory.ItemService.GenerateSpecificItem(type, subtype);

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
    // GET: /api/palantir/Character/GetPlayerCharacters
    [HttpGet("Character/GetPlayerCharacters")]
    public IActionResult GetPlayerCharacters([FromQuery] Request request)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            var characters = factory.ServiceFactory.CharacterService.GetCharacters(playerId);

            return Ok(characters);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // GET: /api/palantir/Character/GetPlayerCharacter
    [HttpGet("Character/GetPlayerCharacter")]
    public IActionResult GetPlayerCharacter([FromQuery] Request request, string characterId)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.GetCharacters(playerId).CharactersList.Find(c => c.Identity!.Id == characterId);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // GET: /api/palantir/Character/CreateCharacter
    [HttpGet("Character/CreateCharacter")]
    public IActionResult CreateCharacter([FromQuery] Request request)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            var stub = factory.ServiceFactory.CharacterService.CreateCharacterStub(playerId);

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
    public IActionResult SaveCharacter([FromQuery] Request request, [FromBody] CharacterOrigins origins)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.SaveCharacterStub(origins, playerId);

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
            var playerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.UpdateCharacterName(name, new CharacterIdentity() { Id = characterId, PlayerId = playerId});

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
            var playerId = MatchTokensForPlayer(request);

            factory.ServiceFactory.CharacterService.DeleteCharacter(new CharacterIdentity() { Id = characterId, PlayerId = playerId });

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
            equip.PlayerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.EquipCharacterItem(equip);

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
            unequip.PlayerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.UnequipCharacterItem(unequip);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: /api/palantir/Character/LearnHeroicTrait
    [HttpPut("Character/LearnHeroicTrait")]
    public IActionResult LearnHeroicTrait([FromQuery] Request request, [FromBody] CharacterHeroicTrait trait)
    {
        try
        {
            trait.PlayerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.LearnHeroicTrait(trait);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // GET: /api/palantir/Character/GetCharacterPaperdoll
    [HttpGet("Character/GetCharacterPaperdoll")]
    public IActionResult GetCharacterPaperdoll([FromQuery] Request request, string characterId)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.CalculatePaperdollForPlayerCharacter(new CharacterIdentity() { Id = characterId, PlayerId = playerId });

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Quests



    #endregion

    #region Rulebook
    // GET: /api/palantir/Rulebook/GetRulebook
    [HttpGet("Rulebook/GetRulebook")]
    public IActionResult GetRulebook()
    {
        try
        {
            var stub = factory.ServiceFactory.CharacterService.GetRulebook();

            return Ok(stub);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Npcs
    // POST: /api/palantir/NPC/GenerateNPC
    [HttpPost("NPC/GenerateNPC")]
    public IActionResult GenerateNPC([FromBody] NpcInfo info)
    {
        try
        {
            var character = factory.ServiceFactory.NpcService.GenerateNpc(info);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region private methods
    private string MatchTokensForPlayer(Request request)
    {
        validator.ValidateRequestObject(request);

        return validator.ValidateRequesterAndReturnId(request);
    }
    #endregion
}