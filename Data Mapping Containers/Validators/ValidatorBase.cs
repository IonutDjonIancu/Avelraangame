namespace Data_Mapping_Containers.Validators;

public class ValidatorBase
{
    public void ValidateString(string str, string message = "")
    {
        if (string.IsNullOrWhiteSpace(str)) Throw(message.Length > 0 ? message : "The provided string is invalid.");
    }

    public void ValidateObject(object? obj, string message = "")
    {
        if (obj == null) Throw(message.Length > 0 ? message : $"Object found null.");
    }

    public void ValidateNumber(int num, string message = "")
    {
        if (num == 0) Throw(message.Length > 0 ? message : "Number cannot be zero.");
    }

    public void ValidateNumberHigherOrEqualThan(int comparer, int compared)
    {
        if (comparer < compared) Throw($"Number {comparer} cannot be smaller than {compared}.");
    }

    public void ValidateGuid(string str, string message = "")
    {
        ValidateString(str);

        var isGuidValid = Guid.TryParse(str, out var id);

        if (!isGuidValid) Throw(message.Length > 0 ? message : "Invalid guid.");

        if (id == Guid.Empty) Throw("Guid cannot be an empty guid.");
    }

    public void Throw(string message)
    {
        throw new Exception(message);
    }
}
