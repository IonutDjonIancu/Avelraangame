using Google.Authenticator;
using System.Text;

namespace Independent_Modules;

public class AuthenticatorModule : IAuthenticatorModule
{
    private readonly TwoFactorAuthenticator tfa;

    public AuthenticatorModule()
    {
        tfa = new TwoFactorAuthenticator();
    }

    public (string imageUrl, string code) GenerateSetupCode(string playerId, string playerName)
    {
        var playerIdNoDashes = playerId.Replace("-", string.Empty);
        byte[] bytes = Encoding.ASCII.GetBytes($"{playerName.ToLower()}{playerIdNoDashes}");

        var setupInfo = tfa.GenerateSetupCode("Avelraangame", playerName, bytes, 3, true);
        var imageUrl = setupInfo.QrCodeSetupImageUrl;
        var code = setupInfo.ManualEntryKey;

        return (imageUrl, code);
    }

    public bool ValidateTfAPin(string playerId, string playerName, string code)
    {
        var playerIdNoDashes = playerId.Replace("-", string.Empty);
        byte[] bytes = Encoding.ASCII.GetBytes($"{playerName.ToLower()}{playerIdNoDashes}");

        var isValid = tfa.ValidateTwoFactorPIN(bytes, code);

        return isValid;
    }
}
