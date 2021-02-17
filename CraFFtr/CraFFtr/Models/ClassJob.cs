using System;
using System.Collections.Generic;
using System.Text;

namespace CraFFtr.Models
{
    public class ClassJob
    {
        public string _url;

        public string Abbreviation { get; set; }

        public int Id { get; set; }

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
    }
}
