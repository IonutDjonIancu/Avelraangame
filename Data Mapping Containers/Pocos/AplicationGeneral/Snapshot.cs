using Data_Mapping_Containers.Dtos;

namespace Data_Mapping_Containers.Pocos;

public class Snapshot
{
    public List<CharacterStub> Stubs { get; set; } = new();
    public List<Player> Players { get; set; } = new();
    public List<Location> Locations { get; set; } = new();







    // all these will be moved to each individual services
    // locks will be held on the properties of what the snapshot is sharing


    //#region create
    //public void CreatePlayer(Player player) 
    //{
    //    lock (_lock)
    //    {
    //        _players.Add(player);
    //    }
    //}
    //#endregion

    //#region read
    //public Player GetPlayer(string id)
    //{
    //    lock ( _lock)
    //    {
    //        return _players.Find(s => s.Identity.Id == id) ?? throw new Exception("Player not found");
    //    }
    //}

    //public Character GetCharacter(string id, string playerId)
    //{
    //    lock (_lock)
    //    {
    //        return GetPlayer(playerId).Characters.Find(s => s.Identity.Id == id) ?? throw new Exception("Character not found");
    //    }
    //}

    //#endregion

    //#region update
    //public void UpdatePlayer(Player player)
    //{
    //    lock ( _lock)
    //    {
    //        var playerToRemove = _players.Find(s => s.Identity.Id == player.Identity.Id)!;
    //        _players.Remove(playerToRemove);
    //        _players.Add(playerToRemove);
    //    }
    //}
    //#endregion



}
