using System.Data;

namespace Data_Mapping_Containers.Dtos;

public class CharacterPaperdoll
{
    private readonly Acronyms acronyms;

    private CharacterPaperdoll() { }
    public CharacterPaperdoll(Acronyms acronyms)
    {
        this.acronyms = acronyms;
    }

    public CharacterStats Stats { get; set; } = new();
    public CharacterAssets Assets { get; set; } = new();
    public CharacterSkills Skills { get; set; } = new();

    // these are the activate Heroic Traits which can be used during combat
    public List<HeroicTrait> SpecialSkills { get; set; }

    public int InterpretFormula(string formula)
    {
        var table = new DataTable();

        var strCol = new DataColumn
        {
            DataType = Type.GetType("System.Decimal"),
            ColumnName = acronyms.Stats.Strength,
            DefaultValue = Stats.Strength
        };
        var conCol = new DataColumn
        {
            DataType = Type.GetType("System.Decimal"),
            ColumnName = acronyms.Stats.Constitution,
            DefaultValue = Stats.Constitution
        };
        var agiCol = new DataColumn
        {
            DataType = Type.GetType("System.Decimal"),
            ColumnName = acronyms.Stats.Agility,
            DefaultValue = Stats.Agility
        };
        var wilCol = new DataColumn
        {
            DataType = Type.GetType("System.Decimal"),
            ColumnName = acronyms.Stats.Willpower,
            DefaultValue = Stats.Willpower
        };
        var perCol = new DataColumn
        {
            DataType = Type.GetType("System.Decimal"),
            ColumnName = acronyms.Stats.Perception,
            DefaultValue = Stats.Perception
        };
        var absCol = new DataColumn
        {
            DataType = Type.GetType("System.Decimal"),
            ColumnName = acronyms.Stats.Abstract,
            DefaultValue = Stats.Abstract
        };

        var totalCol = new DataColumn
        {
            DataType = Type.GetType("System.Decimal"),
            ColumnName = "Total",
            Expression = formula
        };

        table.Columns.Add(strCol);
        table.Columns.Add(conCol);
        table.Columns.Add(agiCol);
        table.Columns.Add(wilCol);
        table.Columns.Add(perCol);
        table.Columns.Add(absCol);
        table.Columns.Add(totalCol);

        DataRow row = table.NewRow();
        table.Rows.Add(row);

        var listOfCells = new List<decimal>();

        var resultRow = table.Rows[0].ItemArray;

        foreach (var item in resultRow)
        {
            if (item == null) continue;

            listOfCells.Add((decimal)item);
        }

        listOfCells.Reverse();

        var result = listOfCells.First();

        return (int)Math.Floor(result);
    }
}
