using CraFFtr.Models;
using CraFFtr.REST;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CraFFtr.ViewModels
{    
    public class ItemDetailViewModel : INotifyPropertyChanged
    {

        //It has to be set as property {get; set;} to work with CollectionView
        public ObservableCollection<Item> Items { get; private set; }

        public ObservableCollection<Recipe> Recipes { get; set; }        

        //public ObservableCollection<Tuple<Item, int>> Ingredients { get; set; }

        public ItemDetailViewModel(List<Item> selectedItems)
        {            
            this.Items = new ObservableCollection<Item>(selectedItems);
            OnPropertyChanged("Items");
            
            CalculateIngredients();
        }
        


        public async void CalculateIngredients()
        {
            Recipes = new ObservableCollection<Recipe>();

            foreach(var item in Items)
            {
                await GetRecipeForItem(item, true);
            }


            OnPropertyChanged("Recipes");

            OnPropertyChanged("Ingredients");           
        }

        private async Task<List<Recipe>> GetRecipeForItem(Item item, bool isMainRecipe)
        {
            var recipes = new List<Recipe>();

            var sc = new SearchCommand(string.Format("item/{0}?columns=Recipes&", item.Id));

            HttpClient client = new HttpClient();

            string uri = sc.Query;

            HttpResponseMessage response = await client.GetAsync(uri);

            var contentString = response.Content.ReadAsStringAsync().Result.ToString();

            try
            {
                var jObj = JObject.Parse(contentString)["Recipes"];

                if(jObj.HasValues)
                {
                    var recipesList = (JArray)jObj;
                    foreach (var obj in recipesList)
                    {
                        var id = obj["ID"];
                        var rc = new RecipeCommand(string.Format("recipe/{0}?", id));
                        uri = rc.Query;
                        response = await client.GetAsync(uri);
                        contentString = response.Content.ReadAsStringAsync().Result.ToString();

                        jObj = JObject.Parse(contentString);
                        recipes = await CreateRecipe(jObj, isMainRecipe);
                    }
                }
                
                return recipes;
            }
            catch(Exception ex)
            {
                var kk = ex.Message;
                return new List<Recipe>();
            }
            
        }

        private async Task<List<Recipe>> CreateRecipe(JToken jObj, bool isMainRecipe)
        {
            //Ingredients = new ObservableCollection<Tuple<Item, int>>();

            var recipes = new List<Recipe>();
            //Creat Recipe
            var recipe = new Recipe();
            var tokens = new Dictionary<JToken, int>();            
            var ingredients = new List<Item>();

            //Get Recipe Item
            var recipeItemName = jObj["ItemResult"]["Name"].ToString();
            var recipeItemIcon = jObj["ItemResult"]["Icon"].ToString();
            var recipeItemId = jObj["ItemResult"]["ID"].ToString();

            recipe.Id = jObj["ID"].ToString();
            //todo: Remove Object Item from Recipe Class -> set Icon and ItemName directly on Recipe Class
            //IT's stupid to have Recipe <-> Item call the same object --> Recipe

            //Base Item
            recipe.Item = new Item() { Id = recipeItemId, Name = recipeItemName, Icon = recipeItemIcon, UrlType = "Item" };

            //Getting Ingredient JTokens
            for (var i=0; i < 10; i++)
            {
                var ammount = int.Parse(jObj["AmountIngredient" + i].ToString());

                if ((jObj["ItemIngredient" + i].Type != JTokenType.Null) && ammount > 0)
                    tokens.Add(jObj["ItemIngredient" + i], ammount);
                        //tokens.Add(jObj["ItemIngredient"+i]);                
            }

            //Getting Dictionary Ingredient, Ammount => still JTokens (parsing from Json)
            foreach (var token in tokens)
            {                              
                var itemName = token.Key["Name"].ToString();
                var itemId = token.Key["ID"].ToString();
                var itemIcon = token.Key["Icon"].ToString();
                var ammount = token.Value;
                var item = new Item() { Name = itemName, Id = itemId, Icon = itemIcon, Ammount = ammount };

                var recipesForItem = await GetRecipeForItem(item, false);
                item.ItemRecipe = recipesForItem.FirstOrDefault();

                //var tuple = Tuple.Create(item, ammount);
                ingredients.Add(item);

                //Property for View
                //Ingredients.Add(tuple);
            }
       
            //recipe.Ingredients = ingredients;                       
            
            if(isMainRecipe)
                Recipes.Add(recipe);

            recipes.Add(recipe);


            return recipes;
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
