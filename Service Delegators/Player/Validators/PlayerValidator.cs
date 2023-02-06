using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Validators;
using Persistance_Manager;

namespace Service_Delegators.Validators;

public class PlayerValidator : ValidatorBase
{
    private readonly IDatabaseManager dbm;

	public PlayerValidator(IDatabaseManager manager)
	{
		dbm = manager;
	}

    public void ValidatePlayerCanLogin(string? token)
    {
        if (token == null) Throw("Not allowed to login.");
    }

    public void ValidatePlayerOnLogin(PlayerLogin login)
    {
        ValidateObject(login);
        ValidatePlayerExistsByName(login.PlayerName);
        ValidateString(login.Code);
    }

    public void ValidatePlayerOnCreate(string playerName)
    {
        ValidatePlayerName(playerName);

        if (dbm.Snapshot.Players!.Exists(p => p.Identity.Name.ToLower() == playerName.ToLower())) Throw("Player already exists");
    }

    public void ValidatePlayerExistsById(string id)
    {
        ValidateGuid(id);
        if (!dbm.Snapshot.Players!.Exists(p => p.Identity.Id == id)) Throw("Player not found");
    }

    public void ValidatePlayerExistsByName(string name)
    {
        ValidateString(name);
        if (!dbm.Snapshot.Players!.Exists(p => p.Identity.Name.ToLower() == name.ToLower())) Throw("Player not found");
    }

    public void PlayerBanned(string email)
    {
        ValidateString(email);
        if (dbm.Metadata.IsPlayerBanned(email)) Throw($"Player {email} is banned.");
    }

    public void PlayerToken(string token)
    {
        ValidateString(token);
    }

    public void ValidatePlayerEmail(string email)
    {
        ValidateString(email);
        if (!email.Contains('@')) Throw($"Wrong email {email} format.");
    }

    public void ValidatePlayerName(string name)
    {
        ValidateString(name);
        if (name.Length > 50) Throw($"Name {name} too long, 50 characters max.");
    }
}
