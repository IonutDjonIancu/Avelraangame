using Persistance_Manager;
using Independent_Modules;
using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal class PlayerLogicDelegator
{
    private readonly IDatabaseManager dbm;
    private readonly IAuthenticatorModule authModule;

    private PlayerLogicDelegator() { }

    internal PlayerLogicDelegator(IDatabaseManager databaseManager)
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

        dbm.PersistPlayer(player);

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
            player.LastAction = DateTime.Now.ToShortDateString();

            dbm.PersistPlayer(player);
        }
        else
        {
            return null;
        }

        return player.Identity.Token;
    }

    internal bool RemovePlayer(string playerId)
    {
        var player = dbm.Snapshot.Players!.FirstOrDefault(p => p.Identity.Id == playerId);

        if (player != null)
        {
            dbm.Snapshot.Players!.Remove(player);

            dbm.RemovePlayer(player);

            return true;
        } 
        else
        {
            return false;
        }
    }
}
