#pragma warning disable CA1822 // Mark members as static

using Data_Mapping_Containers.Dtos;

namespace Service_Delegators.Validators;

internal class PlayerValidator : ValidatorBase
{
    private readonly IDatabaseService dbs;

    internal PlayerValidator(IDatabaseService databaseService)
	{
		dbs = databaseService;
	}

    internal void ValidatePlayerCanLogin(string? token)
    {
        if (token == null) Throw("Not allowed to login.");
    }

    internal void ValidatePlayerOnLogin(PlayerLogin login)
    {
        ValidateObject(login);
        ValidatePlayerExistsByName(login.PlayerName);
        ValidateString(login.Code);
    }

    internal void ValidatePlayerExistsByName(string name)
    {
        ValidateString(name);

        if (!dbs.Snapshot.Players.Exists(p => p.Identity.Name == name)) Throw("Player does not exist.");
    }

    internal void ValidatePlayerExists(string playerId)
    {
        ValidateIfPlayerExists(dbs.Snapshot, playerId);
    }

    internal void ValidatePlayersCount()
    {
        if (dbs.Snapshot.Players!.Count >= 20) Throw("Player limit reached, please contact admins.");
    }

    internal void ValidatePlayerOnCreate(string playerName)
    {
        ValidatePlayerName(playerName);
        ValidatePlayersCount();

        if (dbs.Snapshot.Players!.Exists(p => p.Identity.Name.ToLower() == playerName.ToLower())) Throw("Player already exists.");
    }

    internal void ValidatePlayerName(string name)
    {
        ValidateString(name);
        if (name.Length > 20) Throw($"Name {name} too long, 20 characters max.");
    }
}

#pragma warning restore CA1822 // Mark members as static