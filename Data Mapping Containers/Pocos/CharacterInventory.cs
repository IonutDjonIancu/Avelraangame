using Data_Mapping_Containers.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Data_Mapping_Containers.Pocos;

public class CharacterInventory
{
    public Item? Head { get; set; }
    public Item? Body { get; set; }
    public Item? Mainhand { get; set; }
    public Item? Offhand { get; set; }
    public Item? Ranged { get; set; }
    public Item? Serviceweapon { get; set; }

    [MaxLength(5, ErrorMessage = "Heraldry cannot support more items.")]
    public List<Item>? Heraldry { get; set; } = new List<Item>();

    public List<Item> AllItems
    {
        get
        {
            var allItems = new List<Item>
            {
                Head!, 
                Body!, 
                Mainhand!, 
                Offhand!, 
                Ranged!, 
                Serviceweapon!
            };

            if (Heraldry == null) return allItems;
            
            foreach (var herald in Heraldry)
            {
                allItems.Add(herald);
            }

            return allItems;
        }
    }
}
