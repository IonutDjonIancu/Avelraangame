using Data_Mapping_Containers.Dtos;
using Independent_Modules;

namespace Service_Delegators;

internal class PlayerAuthLogic
{
    private readonly IDatabaseService dbs;
    private readonly IAuthenticatorModule authModule;

    private PlayerAuthLogic() { }
    internal PlayerAuthLogic(
        IDatabaseService databaseService,
        IAuthenticatorModule authenticator)
    {
        dbs = databaseService;
        authModule = authenticator;
    }

    internal Authenticator AuthenticatePlayer(string playerName)
    {
        var id = Guid.NewGuid().ToString();
        var token = Guid.NewGuid().ToString();

        var player = new Player()
        {
            Identity = new PlayerIdentity
            {
                Name = playerName,
                Id = id,
                Token = token,
            },
            LastAction = DateTime.Now.ToShortDateString(),
            IsAdmin = dbs.Snapshot.Admins.Exists(p => p == playerName.ToLower()),
            Characters = new List<Character>()
        };

        dbs.Snapshot.Players!.Add(player);

        dbs.PersistPlayer(id);

        var (imageUrl, code) = authModule.GenerateSetupCode(player.Identity.Id, player.Identity.Name);

        var response = new Authenticator()
        {
            SetupCode = code,
            SetupImage = imageUrl,
        };

        return response;
    }

    internal string? AuthorizePlayer(PlayerLogin login)
    {
        var player = dbs.Snapshot.Players!.Find(p => p.Identity.Name.ToLower() == login.PlayerName.ToLower());

        var isValid = authModule.ValidateTfAPin(player!.Identity.Id, login.PlayerName, login.Code);

        if (!isValid) return null;
        
        player.Identity.Token = Guid.NewGuid().ToString();
        player.LastAction = DateTime.Now.ToShortDateString();

        dbs.PersistPlayer(player.Identity.Id);

        return player.Identity.Token;
    }
}
