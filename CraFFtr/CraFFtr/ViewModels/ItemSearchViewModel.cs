using CraFFtr.Models;
using CraFFtr.REST;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
        public ICommand RefreshCommand { get; }        

        public string SearchText { get; set; }

        //It has to be set as property {get; set;} to work with CollectionView
        public ObservableCollection<Item> Items { get; private set; }                
        public ObservableCollection<ClassJob> JobCategories { get; private set; }
        public List<ClassJob> SelectedJobs { get; set; }

        private SearchCommand SearchCmd;

        bool isRefreshing;

        public ItemSearchViewModel()
        {
            var jobList = GetAllClassJobs();
            
            SearchItemsCommand = new Command(OnSearchItem);
            RefreshCommand = new Command(ExecuteRefreshCommand);

            JobCategories = new ObservableCollection<ClassJob>(jobList);
            OnPropertyChanged("JobCategories");
        }        
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        public async void ExecuteRefreshCommand()
        {
            
            if(SearchCmd != null)
            {                
                Items = new ObservableCollection<Item>(await GetSearchedItems());                
            }

            // Stop refreshing
            OnPropertyChanged("Items");
            IsRefreshing = false;
        }

        private async void OnSearchItem(object obj)
        {                                   
            var itemsFound = await GetSearchedItems();
            Items = new ObservableCollection<Item>(itemsFound);
            OnPropertyChanged("Items");                        
        }

        private async Task<List<Item>> GetSearchedItems()
        {
            var classString = new List<string>();
            var filterString = string.Empty;

            //Get selected classes ready for query -> use Abbreviations
            if (SelectedJobs != null)
            {
                foreach (ClassJob cj in SelectedJobs)
                {
                    classString.Add(@"ClassJobCategory." + cj.Abbreviation + "=1");
                }

                filterString = "," + string.Join(",", classString);
            }

            SearchCmd = new SearchCommand(string.Format("search?string={0}&columns=ID,Name,Icon,UrlType&filters=Recipes.ID>1{1}&limit=40&", SearchText, filterString));

            //todo: Maybe put this logic into a REST client class to be used everywhere else

            HttpClient client = new HttpClient();

            string uri = SearchCmd.Query;

            HttpResponseMessage response = await client.GetAsync(uri);

            var contentString = response.Content.ReadAsStringAsync().Result;            
            //end todo


            var jObject = JObject.Parse(contentString);            
            var itemArray = (JArray)jObject["Results"];

            var foundItems = itemArray.ToObject<List<Item>>();                     


            client.Dispose();
            return foundItems;
        }

        private List<ClassJob> GetAllClassJobs()
        {            
                                 
            var jobs = JsonConvert.DeserializeObject<List<ClassJob>>(Properties.Resources.JobCategories);            
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
