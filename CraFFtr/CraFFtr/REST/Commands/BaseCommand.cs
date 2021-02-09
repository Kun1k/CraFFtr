using System;
using System.Collections.Generic;
using System.Text;

namespace CraFFtr.REST
{    
    public class BaseCommand
    {
        public string Query { get; set; }
        
        public string ApiKey = "f4327a11e5334f1a99da01d6040e08a3789f22ab5128425ba69697a574ca6a88";        

        public void CreateCommand(string query)
        {            
            var privateKey = "private_key=" + ApiKey;

            Query = "https://xivapi.com/" + query + privateKey;
        }
    }
}
