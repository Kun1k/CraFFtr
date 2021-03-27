using System;
using System.Collections.Generic;

namespace CraFFtr.Models
{
    public class Item
    {
        public string _url;

        public string Id { get; set; }        
        public string Icon
        {
            get
            {
                return _url;
            }
            set
            {
                if (!value.StartsWith("http"))
                    _url = "https://xivapi.com" + value;
                else
                    _url = value;
            }
        }
        public string Name { get; set; }
        public int Ammount { get; set; }

        public string UrlType { get; set; }
        public Recipe ItemRecipe { get; set; }

        public List<Recipe> ItemRecipes { get; set; }
    }
}