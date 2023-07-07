using Data_Mapping_Containers.Dtos;

namespace Service_Delegators;

internal static class Utils
{
    internal static string GetLocationFullName(Position position)
    {
        return $"{position.Region}_{position.Subregion}_{position.Land}_{position.Location}";
    }
}
