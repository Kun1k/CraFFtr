using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CraFFtr.REST
{    
    public class RecipeCommand : BaseCommand
    {                   
        public RecipeCommand(string query)
        {
            base.CreateCommand(query);
        }        

    }
}
