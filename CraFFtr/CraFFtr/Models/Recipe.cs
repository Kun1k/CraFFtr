using System;
using System.Collections.Generic;
using System.Text;

namespace CraFFtr.Models
{
    public class Recipe
    {
        public string Id { get; set; }                   
        public Dictionary<Item, int> ItemIngredient0 { get; set; }
        public Dictionary<Item, int> ItemIngredient1 { get; set; }
        public Dictionary<Item, int> ItemIngredient2 { get; set; }
        public Dictionary<Item, int> ItemIngredient3 { get; set; }
        public Dictionary<Item, int> ItemIngredient4 { get; set; }
        public Dictionary<Item, int> ItemIngredient5 { get; set; }
        public Dictionary<Item, int> ItemIngredient6 { get; set; }
        public Dictionary<Item, int> ItemIngredient7 { get; set; }
        public Dictionary<Item, int> ItemIngredient8 { get; set; }
    }
}
