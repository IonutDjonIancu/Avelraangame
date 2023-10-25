namespace Data_Mapping_Containers.Dtos;

public class Battleboard
{
    public string Id { get; set; }

    public string LastActionResult { get; set; }
    public bool IsInCombat { get; set; }
    public bool CanLvlUp { get; set; }
    public int RoundNr { get; set; }
    public int EffortLvl { get; set; }

    public string GoodGuyPartyLead { get; set; }
    public List<Character> GoodGuys { get; set; } = new();

    public string BadGuyPartyLead { get; set; }
    public List<Character> BadGuys { get; set; } = new();
    
    public List<string> BattleOrder { get; set; }

    public List<Character> GetAllCharacters()
    {
        var combatants = new List<Character>();
        GoodGuys.ForEach(s => combatants.Add(s));
        BadGuys.ForEach(s => combatants.Add(s));

        return combatants;
    }
}
