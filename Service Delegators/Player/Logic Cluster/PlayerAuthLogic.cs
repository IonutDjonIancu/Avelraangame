﻿using Data_Mapping_Containers.Dtos;
using Independent_Modules;

namespace Service_Delegators;

public interface IPlayerAuthLogic
{
    Authenticator AuthenticatePlayer(string playerName);
    Player AuthorizePlayer(PlayerLogin login);
    Player AuthorizeTestPlayer(string playerName);
}

public class PlayerAuthLogic : IPlayerAuthLogic
{
    private readonly object _lock = new();
    private readonly Snapshot snapshot;
    private readonly IAuthenticatorModule authModule;

    public PlayerAuthLogic(Snapshot snapshot)
    {
        this.snapshot = snapshot;
        authModule = new AuthenticatorModule();
    }

    public Authenticator AuthenticatePlayer(string playerName)
    {
        lock (_lock)
        {
            var id = Guid.NewGuid().ToString();
            var token = Guid.NewGuid().ToString();
            var player = new Player()
            {
                Identity = new PlayerIdentity
                {
                    Name = playerName.ToLower(),
                    Id = id,
                    Token = token,
                },
                LastAction = DateTime.Now.ToShortDateString(),
                IsAdmin = playerName == Environment.GetEnvironmentVariable("AvelraanAdmin"),
            };

            snapshot.Players.Add(player);

            var (imageUrl, code) = authModule.GenerateSetupCode(player.Identity.Id, player.Identity.Name);

            return new Authenticator()
            {
                SetupCode = code,
                SetupImage = imageUrl,
            };
        }
    }

    public Player AuthorizePlayer(PlayerLogin login)
    {
        lock (_lock)
        {
            // nevermind the misspell errors from players
            // simply lowercase their names since they are unique at creation
            var player = snapshot.Players.Find(p => p.Identity.Name.ToLower() == login.PlayerName.ToLower());

            var isValid = authModule.ValidateTfAPin(player!.Identity.Id, login.PlayerName, login.Code);

            if (!isValid) throw new Exception("Failed authorization.");

            player.Identity.Token = Guid.NewGuid().ToString();
            player.LastAction = DateTime.Now.ToShortDateString();

            return player;
        }
    }

    public Player AuthorizeTestPlayer(string playerName)
    {
        lock (_lock)
        {
            var player = snapshot.Players.Find(p => p.Identity.Name == playerName)!;

            player.Identity.Token = Guid.NewGuid().ToString();

            return player;
        }
    }
}
