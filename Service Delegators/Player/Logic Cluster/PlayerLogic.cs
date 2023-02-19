using Data_Mapping_Containers.Dtos;
using Data_Mapping_Containers.Dtos.ApiDtos;
using Independent_Modules;
using Persistance_Manager;

namespace Service_Delegators;

internal class PlayerLogic
{
    private readonly IDatabaseManager dbm;
    private readonly IAuthenticatorModule authModule;

    private PlayerLogic() { }

    internal PlayerLogic(IDatabaseManager databaseManager)
    {
        dbm = databaseManager;
        authModule = new AuthenticatorModule();
    }

    internal Authenticator AddPlayer(string playerName)
    {
        var player = new Player()
        {
            Identity = new PlayerIdentity
            {
                Name = playerName,
                Id = Guid.NewGuid().ToString(),
                Token = Guid.NewGuid().ToString(),
            },
            LastAction = DateTime.Now.ToShortDateString(),
            IsAdmin = dbm.Metadata.IsPlayerAdmin(playerName),
            Characters = new List<Character>()
        };

        dbm.Snapshot.Players!.Add(player);
        dbm.Persist();

        var (imageUrl, code) = authModule.GenerateSetupCode(player.Identity.Id, player.Identity.Name);

        var response = new Authenticator()
        {
            SetupCode = code,
            SetupImage = imageUrl,
        };

        return response;
    }

    internal string? LoginPlayer(PlayerLogin login)
    {
        var player = dbm.Snapshot.Players!.Find(p => p.Identity.Name.ToLower() == login.PlayerName.ToLower());

        var isValid = authModule.ValidateTfApin(player!.Identity.Id, login.PlayerName, login.Code);

        if (isValid)
        {
            player.Identity.Token = Guid.NewGuid().ToString();
            dbm.Persist();
        }
        else
        {
            return null;
        }

        return player.Identity.Token;
    }

    internal bool RemovePlayer(string playerName)
    {
        var player = dbm.Snapshot.Players!.FirstOrDefault(p => p.Identity.Name == playerName);

        if (player != null)
        {
            dbm.Snapshot.Players!.Remove(player);
            dbm.Persist();
            return true;
        } else
        {
            return false;
        }
    }
}
