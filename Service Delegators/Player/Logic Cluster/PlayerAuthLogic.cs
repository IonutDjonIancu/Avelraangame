using Data_Mapping_Containers.Dtos;
using Independent_Modules;

namespace Service_Delegators;

public interface IPlayerAuthLogic
{
    (Authenticator auth, Player player) AuthenticatePlayer(string playerName);
    Player AuthorizePlayer(PlayerLogin login);
}

public class PlayerAuthLogic : IPlayerAuthLogic
{
    private readonly object playersLock = new();
    private readonly AppSettings appSettings;
    private readonly Snapshot snapshot;
    private readonly IAuthenticatorModule authModule;

    public PlayerAuthLogic(
        AppSettings appSettings,
        Snapshot snapshot)
    {
        this.appSettings = appSettings;
        this.snapshot = snapshot;
        authModule = new AuthenticatorModule();
    }

    public (Authenticator auth, Player player) AuthenticatePlayer(string playerName)
    {
        var id = Guid.NewGuid().ToString();
        var token = Guid.NewGuid().ToString();

        lock (playersLock)
        {
            var player = new Player()
            {
                Identity = new PlayerIdentity
                {
                    Name = playerName,
                    Id = id,
                    Token = token,
                },
                LastAction = DateTime.Now.ToShortDateString(),
                IsAdmin = appSettings.AdminData.Admins.Contains(playerName),
            };

            snapshot.Players.Add(player);

            var (imageUrl, code) = authModule.GenerateSetupCode(player.Identity.Id, player.Identity.Name);

            var auth = new Authenticator()
            {
                SetupCode = code,
                SetupImage = imageUrl,
            };

            return (auth, player);
        }
    }

    public Player AuthorizePlayer(PlayerLogin login)
    {
        lock (playersLock)
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
}
