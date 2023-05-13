using Service_Delegators;
using Data_Mapping_Containers;

namespace Avelraangame.Controllers.Validators;

public class ControllerValidator
{
    private readonly IDatabaseService dbs;

    public ControllerValidator(IDatabaseService manager)
    {
        dbs = manager;
    }

    public void ValidateRequestObject(Request request)
    {
        if (request == null) throw new Exception("Request object found null.");
        if (string.IsNullOrWhiteSpace(request.PlayerName)) throw new Exception("Player name is missing or invalid.");
        if (!Guid.TryParse(request.Token, out var _)) throw new Exception("Request token is invalid.");
    }

    public string ValidateRequesterAndReturnId(Request request)
    {
        var player = dbs.Snapshot.Players.Find(p => p.Identity.Name == request.PlayerName) ?? throw new Exception("Player not found.");

        if (dbs.Snapshot.Banned.Contains(player.Identity.Name.ToLower())) throw new Exception("Player is banned.");

        if (request.Token != player.Identity.Token) throw new Exception("Token mismatch");

        return player.Identity.Id;
    }
}
