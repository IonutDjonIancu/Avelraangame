using Data_Mapping_Containers.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Data_Mapping_Containers.Pocos;

public class CharacterInventory
{
    public Item? Head { get; set; }
    public Item? Body { get; set; }
    public Item? Shield { get; set; }
    public Item? Mainhand { get; set; }
    public Item? Offhand { get; set; }
    public Item? Ranged { get; set; }

    public List<Item>? Heraldry { get; set; } = new();

    public int Provisions { get; set; }
    public List<Item> Supplies { get; set; } = new();

    public List<Item> GetAllEquipedItems()
    {
        var listOfItems = new List<Item>();

        if (Head != null) listOfItems.Add(Head);
        if (Body != null) listOfItems.Add(Body);
        if (Shield != null) listOfItems.Add(Shield);
        if (Mainhand != null) listOfItems.Add(Mainhand);
        if (Offhand != null) listOfItems.Add(Offhand);
        if (Ranged != null) listOfItems.Add(Ranged);

        listOfItems.AddRange(from item in Heraldry select item);

        return listOfItems;
    }
}
