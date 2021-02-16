using System;
using System.Collections.Generic;
using System.Text;

namespace CraFFtr.Models
{
    public class Recipe
    {
        public string Id { get; set; }
        
        public Item Item { get; set; }

        //Item and its ammount needed for the craft
        public List<Item> Ingredients { get; set; }
    }
}
