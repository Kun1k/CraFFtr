using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CraFFtr.REST
{    
    public class SearchCommand : BaseCommand
    {                   
        public SearchCommand(string query)
        {
            base.CreateCommand(query);
        }

        public async System.Threading.Tasks.Task SearchAsync()
        {            
            //HttpClient client = new HttpClient();

            //string uri = this.Query;

            //HttpResponseMessage response = await client.GetAsync(uri);

            //string contentString = await response.Content.ReadAsStringAsync();
            
            //dynamic parsedJson = JsonConvert.DeserializeObject(contentString);

            //Console.WriteLine(parsedJson);
        }

    }
}
