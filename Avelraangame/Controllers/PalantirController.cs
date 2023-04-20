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
    // PUT: /api/palantir/Database/ExportDatabase
    [HttpPut("Database/ExportDatabase")]
    public IActionResult ExportDatabase([FromQuery] Request request)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            var isDbExported = factory.Dbm.Metadata.ExportDatabase(playerId);

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
    public IActionResult ExportLogs([FromQuery] Request request, int days)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            var areLogsExported = factory.Dbm.Metadata.ExportLogs(days, playerId);

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

    // PUT: /api/palantir/Character/UpdateCharacter
    [HttpPut("Character/UpdateCharacter")]
    public IActionResult UpdateCharacter([FromQuery] Request request, [FromBody] CharacterUpdate charUpdate)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.UpdateCharacter(charUpdate, playerId);

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

    // PUT: /api/palantir/Character/EquipItem
    [HttpPut("Character/EquipItem")]
    public IActionResult EquipItem([FromQuery] Request request, [FromBody] CharacterEquip equip)
    {
        try
        {
            var playerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.EquipCharacterItem(equip, playerId);

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
            var playerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.UnequipCharacterItem(unequip, playerId);

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
            var playerId = MatchTokensForPlayer(request);

            var character = factory.ServiceFactory.CharacterService.LearnHeroicTrait(trait, playerId);

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

            var character = factory.ServiceFactory.CharacterService.GetCharacterPaperdoll(characterId, playerId);

            return Ok(character);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region HeroicTraits
    // GET: /api/palantir/HeroicTraits/GetHeroicTraits
    [HttpGet("HeroicTraits/GetHeroicTraits")]
    public IActionResult GetHeroicTraits()
    {
        try
        {
            var traits = factory.ServiceFactory.CharacterService.GetHeroicTraits();

            return Ok(traits);
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

