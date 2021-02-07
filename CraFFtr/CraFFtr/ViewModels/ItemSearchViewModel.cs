using CraFFtr.Models;
using CraFFtr.REST;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CraFFtr.ViewModels
{
    class ItemSearchViewModel : INotifyPropertyChanged
    {
        public Command SearchItemsCommand { get; }
        public string Text { get; set; }
        public ObservableCollection<Item> Items { get; private set; }        
        public ICommand ItemSelectionChangedCommand => new Command(ItemSelectionChanged);


        public ItemSearchViewModel()
        {            
            SearchItemsCommand = new Command(OnSearchItem);                        
        }

        void ItemSelectionChanged()
        {
            
        }        

        private async void OnSearchItem(object obj)
        {
            
            var text = obj.ToString();

            var sc = new SearchCommand(string.Format("search?string={0}&columns=ID,Name,Icon,UrlType&limit=20",text));
            
            var itemsFound = await GetSearchedItems(sc);

            Items = new ObservableCollection<Item>(itemsFound.Where(x => x.UrlType=="Item"));

            OnPropertyChanged("Items");
            //Items = new ObservableCollection<Item>(new List<Item>() { new Item { Name="Test1", Icon= "https://xivapi.com/i/030000/030630.png" } });
            
        }

        private async Task<List<Item>> GetSearchedItems(SearchCommand sc)
        {
            HttpClient client = new HttpClient();

            string uri = sc.Query;

            HttpResponseMessage response = await client.GetAsync(uri);

            var contentString = response.Content.ReadAsStringAsync().Result;            

            var jObject = JObject.Parse(contentString);            
            var itemArray = (JArray)jObject["Results"];

            var foundItems = itemArray.ToObject<List<Item>>();
            


            client.Dispose();
            return foundItems;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
