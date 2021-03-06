﻿using System;

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
                _url = "https://xivapi.com" + value;
            }
        }
        public string Name { get; set; }
        public string UrlType { get; set; }
    }
}