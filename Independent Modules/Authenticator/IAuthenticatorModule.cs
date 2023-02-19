namespace Independent_Modules
{
    public interface IAuthenticatorModule
    {
        (string imageUrl, string code) GenerateSetupCode(string playerId, string playerName);
        bool ValidateTfApin(string playerId, string playerName, string code);
    }
}