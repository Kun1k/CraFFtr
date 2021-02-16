using System;
using System.Collections.Generic;
using System.Text;

namespace CraFFtr.REST
{    
    public class BaseCommand
    {
        public string Query { get; set; }
        
        public string ApiKey = "c8be512484b54723b719660d93440e068128b77df85d400ba90acd94e286f669";        

        public void CreateCommand(string query)
        {            
            var privateKey = "private_key=" + ApiKey;

            Query = "https://xivapi.com/" + query + privateKey;
        }
    }
}
