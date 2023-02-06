using Data_Mapping_Containers.Validators;
using Persistance_Manager;

namespace Avelraangame.Controllers.Validators;

public class ControllerValidator : ValidatorBase
{
    private readonly IDatabaseManager dbm;

    public ControllerValidator(IDatabaseManager manager)
    {
        dbm = manager;
    }

    public void ValidateIfPlayerIsBanned(string name)
    {
        ValidateString(name);

        if (dbm.Metadata.IsPlayerBanned(name)) Throw($"Player {name} is banned");
    }

    public void MatchingTokens(string token1, string token2)
    {
        ValidateString(token1);
        ValidateString(token2);

        if (token1 != token2) Throw("Token mismatch");
    }
}
