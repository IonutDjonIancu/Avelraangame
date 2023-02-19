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
        ValidatePlayersCount();

        if (dbm.Snapshot.Players!.Exists(p => p.Identity.Name.ToLower() == playerName.ToLower())) Throw("Player already exists");
    }

    public void ValidatePlayersCount()
    {
        if (dbm.Snapshot.Players!.Count >= 30) Throw("Player limit reached, please contact support.");
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
        if (name.Length > 20) Throw($"Name {name} too long, 20 characters max.");
    }
}
