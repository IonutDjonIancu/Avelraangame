#pragma warning disable CS8604 // Possible null reference argument.

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

    [MaxLength(5, ErrorMessage = "Heraldry cannot support more items.")]
    public List<Item>? Heraldry { get; set; } = new List<Item>();

    public List<Item> GetAllEquipedItems()
    {
        var listOfItems = new List<Item>
        {
            Head,
            Body,
            Shield,
            Mainhand,
            Offhand,
            Ranged
        };

        listOfItems.AddRange(from item in Heraldry select item);

        return listOfItems;
    }
}

#pragma warning restore CS8604 // Possible null reference argument.