﻿namespace Data_Mapping_Containers.Dtos;

public class Database
{
    public DateTime DbDate { get; set; }
    public List<CharacterStub> CharacterStubs { get; set; }
    public List<Item> Items { get; set; }
    public List<HeroicTrait> Traits { get; set; }
    public List<Party> Parties { get; set; }
}
