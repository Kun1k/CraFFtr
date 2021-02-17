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

        //It has to be set as property {get; set;} to work with CollectionView
        public ObservableCollection<Item> Items { get; private set; }        

        public ObservableCollection<ClassJob> JobCategories { get; private set; }
        public ICommand ItemSelectionChangedCommand => new Command(ItemSelectionChanged);


        public ItemSearchViewModel()
        {
            //Serch for items based on selected classes
            //https://xivapi.com/search?string=exarchic&columns=ID,Name,Icon,UrlType&filters=Recipes.ID>1, ClassJobCategory.BLM=1&limit=40

            SearchItemsCommand = new Command(OnSearchItem);

            OnInitPage();
        }

        void ItemSelectionChanged()
        {
            
        }


        private async void OnInitPage()
        {
            var jobs = await GetAllClassJobs();

            JobCategories = new ObservableCollection<ClassJob>(jobs);

            OnPropertyChanged("JobCategories");
        }

        private async void OnSearchItem(object obj)
        {
            
            var text = obj.ToString();

            var sc = new SearchCommand(string.Format("search?string={0}&columns=ID,Name,Icon,UrlType&filters=Recipes.ID>1&limit=40&", text));
            
            var itemsFound = await GetSearchedItems(sc);

            Items = new ObservableCollection<Item>(itemsFound.Where(x => x.UrlType=="Item"));

            OnPropertyChanged("Items");
            //Items = new ObservableCollection<Item>(new List<Item>() { new Item { Name="Test1", Icon= "https://xivapi.com/i/030000/030630.png" } });
            
        }

        private async Task<List<Item>> GetSearchedItems(SearchCommand sc)
        {
            //todo: Maybe put this logic into a REST client class to be used everywhere else

            HttpClient client = new HttpClient();

            string uri = sc.Query;

            HttpResponseMessage response = await client.GetAsync(uri);

            var contentString = response.Content.ReadAsStringAsync().Result;            
            //end todo


            var jObject = JObject.Parse(contentString);            
            var itemArray = (JArray)jObject["Results"];

            var foundItems = itemArray.ToObject<List<Item>>();                     

            client.Dispose();
            return foundItems;
        }

        private async Task<List<ClassJob>> GetAllClassJobs()
        {
            //todo: Maybe put this logic into a REST client class to be used everywhere else

            HttpClient client = new HttpClient();
            var sc = new SearchCommand(string.Format("ClassJob?columns=ID,Name,Icon,Abbreviation&"));
            string uri = sc.Query;

            HttpResponseMessage response = await client.GetAsync(uri);

            var contentString = response.Content.ReadAsStringAsync().Result;
            //end todo


            var jObject = JObject.Parse(contentString);
            var itemArray = (JArray)jObject["Results"];
            var jobs = JsonConvert.DeserializeObject<List<ClassJob>>(itemArray.ToString()).Where(x => x.Id >7).ToList();            



            client.Dispose();
            return jobs;
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
