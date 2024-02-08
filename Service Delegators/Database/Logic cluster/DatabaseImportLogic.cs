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
                var newCharacterId = Guid.NewGuid().ToString();
                c.Identity.Id = newCharacterId;
                c.Identity.PlayerId = player.Identity.Id;
                
                c.Mercenaries.ForEach(m => m.Identity.PlayerId = player.Identity.Id);

                c.Inventory.Supplies.ForEach(i => i.Identity.CharacterId = newCharacterId);

                if (c.Inventory.Head != null) c.Inventory.Head.Identity.CharacterId = newCharacterId;
                if (c.Inventory.Body != null) c.Inventory.Body.Identity.CharacterId = newCharacterId;
                if (c.Inventory.Mainhand != null) c.Inventory.Mainhand.Identity.CharacterId = newCharacterId;
                if (c.Inventory.Offhand != null) c.Inventory.Offhand.Identity.CharacterId = newCharacterId;
                if (c.Inventory.Ranged != null) c.Inventory.Ranged.Identity.CharacterId = newCharacterId;
                if (c.Inventory.Heraldry!.Count > 0)
                {
                    c.Inventory.Heraldry.ForEach(h => h.Identity.CharacterId = newCharacterId);
                }

                c.Status.Gameplay.BattleboardId = "";
                c.Status.Gameplay.IsHidden = false;
                c.Status.Gameplay.IsLocked = false;
                
                player.Characters.Add(c);
            });
        }
    }
}
