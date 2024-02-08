using Data_Mapping_Containers.Dtos;
using Newtonsoft.Json;

namespace Service_Delegators;

public interface IDatabaseImportLogic
{
    void ImportPlayer(string playerJsonString);
}

public class DatabaseImportLogic : IDatabaseImportLogic
{
    private Snapshot snapshot;

    private readonly object _lock = new();

    public DatabaseImportLogic(
        Snapshot snapshot)
    {
        this.snapshot = snapshot;
    }

    public void ImportPlayer(string playerJsonString)
    {
        lock (_lock)
        {
            var import = JsonConvert.DeserializeObject<Player>(playerJsonString)!;
            var player = snapshot.Players.Find(p => p.Identity.Name == import.Identity.Name)!;

            import.Characters.ForEach(c => 
            {
                c.Identity.Id = Guid.NewGuid().ToString();
                player.Characters.Add(c);
            });
        }
    }
}
