using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Serilog;
using Avelraangame.Factories;
using Avelraangame.Controllers.Validators;
using Data_Mapping_Containers.ApiDtos;
using Data_Mapping_Containers.Dtos;

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
        validator = new ControllerValidator(factory.Dbm);
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

    #region Database
    // PUT: /api/palantir/Database/OverwriteDatabase
    [HttpPut("Database/OverwriteDatabase")]
    public IActionResult OverwriteDatabase([FromQuery] Request request, [FromBody] DatabaseOverwrite overwrite)
    {
        try
        {
            MatchTokensForPlayer(request);

            var isDbOverwritten = factory.Dbm.Metadata.OverwriteDatabase(overwrite);

            if (isDbOverwritten) return Ok("Database overwritten successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }

        return Conflict("Unable to overwrite database.");
    }

    // PUT: /api/palantir/Database/ExportDatabase
    [HttpPut("Database/ExportDatabase")]
    public IActionResult ExportDatabase([FromQuery] Request request, [FromBody] string recipient)
    {
        try
        {
            MatchTokensForPlayer(request);

            var isDbExported = factory.Dbm.Metadata.ExportDatabase(recipient);

            if (isDbExported)
            {
                return Ok("Database exported successfully.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }

        return Conflict("Could not export db.");
    }

    // PUT: /api/palantir/Database/ExportLogs
    [HttpPut("Database/ExportLogs")]
    public IActionResult ExportLogs([FromQuery] Request request, [FromBody] LogsExport export)
    {
        try
        {
            MatchTokensForPlayer(request);

            var areLogsExported = factory.Dbm.Metadata.ExportLogs(export);

            if (areLogsExported) return Ok("Logs exported successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }

        return Conflict("Unable to export logs.");
    }
    #endregion

    #region Player
    // POST: /api/palantir/Player/CreatePlayer
    [HttpPost("Player/CreatePlayer")]
    public IActionResult CreatePlayer([FromQuery] string playerName)
    {
        try
        {
            var player = factory.ServiceFactory.PlayerService.CreatePlayer(playerName);

            if (player != null)
            {
                return Ok(player);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }

        return Conflict("Unable to create player.");
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
            MatchTokensForPlayer(request);

            var isPlayerRemoved = factory.ServiceFactory.PlayerService.DeletePlayer(request.PlayerName);

            if (isPlayerRemoved) return Ok("Player deleted successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }

        return Conflict("Unable to remove player.");
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
    // GET: /api/palantir/Character/GetCharacters
    [HttpGet("Character/GetCharacters")]
    public IActionResult GetCharacters([FromQuery] Request request)
    {
        try
        {
            MatchTokensForPlayer(request);

            var characters = factory.ServiceFactory.CharacterService.GetCharacters(request.PlayerName);

            return Ok(characters);
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
    public IActionResult UpdateCharacterName([FromQuery] Request request, [FromBody] CharacterUpdate charUpdate)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.UpdateCharacterName(charUpdate, playerId);

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
    public IActionResult DeleteCharacter([FromQuery] Request request, [FromBody] string characterId)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            factory.ServiceFactory.CharacterService.DeleteCharacter(characterId, playerId);

            return Ok("Character deleted");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region privates
    private string MatchTokensForPlayer(Request request)
    {
        validator.ValidateObject(request);
        validator.ValidateString(request.PlayerName);
        validator.ValidateGuid(request.Token);

        var player = factory.Dbm.Snapshot.Players?.Find(p => p.Identity.Name == request.PlayerName);

        validator.ValidateObject(player, "Player not found.");
        validator.ValidateIfPlayerIsBanned(player!.Identity.Name);
        validator.MatchingTokens(request.Token, player.Identity.Token);

        return player.Identity.Id;
    }
    #endregion
}

