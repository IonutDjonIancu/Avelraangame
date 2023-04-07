using Data_Mapping_Containers.Validators;

namespace Persistance_Manager.Validators;

public class DatabaseManagerValidator : ValidatorBase
{
    private readonly DatabaseManager dbm;

    public DatabaseManagerValidator(DatabaseManager manager)
    {
        dbm = manager;
    }

    public void KeyInSecretKeys(string secretKey)
    {
        ValidateString(secretKey);
        if (!dbm.info.SecretKeys.Contains(secretKey)) Throw($"Key {secretKey} not found in secret keys.");
    }

    public void TriesLimit(int tries)
    {
        if (tries >= 3) Throw($"Number of tries {tries} exceeded.");
    }

    public void FileAtPath(string path)
    {
        ValidateString(path);
        if (!File.Exists(path)) Throw($"File not found at path {path}");
    }

    public void PlayerShouldBeAdmin(string playerName)
    {
        ValidateString(playerName);
        if (!dbm.info.Admins.Contains(playerName)) Throw($"Email {playerName} is not an admin.");
    }

    public void DaysLimit(int days)
    {
        ValidateNumber(days);
        if (days >= 8) Throw($"Days {days} out of acceptable range.");
    }
}
