namespace Data_Mapping_Containers.Dtos;

public class QuestEncounter
{
    public string Id { get; set; }
    public string PartyLeadId { get; set; }

    public bool HasGeneratedCombat { get; set; }
    public string CombatId { get; set; }
    public List<CharacterPaperdoll> GoodGuys { get; set; } = new();
    public List<CharacterPaperdoll> BadGuys { get; set; } = new();

    public string Type { get; set; }
    public string DiplomacyChoice { get; set; }
    public string DiplomacyPass { get; set; }
    public string DiplomacyFail { get; set; }
    public int DiplomacyRoll { get; set; }
    public string DiplomacyRollType { get; set; }

    // add the rest of the encounter options

}
