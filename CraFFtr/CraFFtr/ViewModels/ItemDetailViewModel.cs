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
        #region Definitions
        private readonly string TextShowAllMaterials = "Show all base materials as list";
        private readonly string TextShowPreviousItemList = "Show previous item list";

        //It has to be set as property {get; set;} to work with CollectionView
        public ObservableCollection<Item> Items { get; private set; }

        public ObservableCollection<Recipe> Recipes { get; set; }        

        public bool IsRefreshing { get; set; }

        public List<Item> AllBaseMaterials { get; set; }
        public bool ShowAllMats { get; set; }
        public string ButtonText { get; set; }

        public Command TotalMaterialsShowCommand { get; set; }

        #endregion

        public ItemDetailViewModel(List<Item> selectedItems)
        {
            //Defaults
            this.ShowAllMats = false;
            this.TotalMaterialsShowCommand = new Command(ButtonAllMaterialsClicked);
            this.Items = new ObservableCollection<Item>(selectedItems);
            this.ButtonText = TextShowAllMaterials;


            OnPropertyChanged("Items");

            CalculateIngredients();
        }

        public async void CalculateIngredients()
        {
            IsRefreshing = true;

            Recipes = new ObservableCollection<Recipe>();

            foreach (var item in Items)
            {
                await GetRecipeForItem(item, true);
            }


            OnPropertyChanged("Recipes");

            OnPropertyChanged("Ingredients");
            IsRefreshing = false;


        }

        private async Task<List<Recipe>> GetRecipeForItem(Item item, bool isMainRecipe, int ammountFactor = 1)
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

                if (jObj.HasValues)
                {
                    var recipesList = (JArray)jObj;
                    //foreach (var obj in recipesList)
                    //{
                    var obj = recipesList[0];
                    var id = obj["ID"];
                    var rc = new RecipeCommand(string.Format("recipe/{0}?", id));
                    uri = rc.Query;
                    response = await client.GetAsync(uri);
                    contentString = response.Content.ReadAsStringAsync().Result.ToString();

                    jObj = JObject.Parse(contentString);
                    recipes = await CreateRecipe(jObj, isMainRecipe, ammountFactor);
                    //}
                }

                return recipes;
            }
            catch (Exception ex)
            {
                var kk = ex.Message;
                return new List<Recipe>();
            }

        }

        private async Task<List<Recipe>> CreateRecipe(JToken jObj, bool isMainRecipe, int ammountFactor = 1)
        {
            //Ingredients = new ObservableCollection<Tuple<Item, int>>();

            var recipes = new List<Recipe>();

            var ingredientTokens = new List<IngredientJson>();
            var recipeTokens = new List<JArray>();
            var ingredients = new List<Item>();


            //Generate Recipe Item from Json
            var recipe = GenerateRecipeFromJToken(jObj);

            //Getting Ingredient JTokens
            for (var i = 0; i < 10; i++)
            {
                var ingredientToken = new IngredientJson();
                var ammount = int.Parse(jObj["AmountIngredient" + i].ToString());


                if ((jObj["ItemIngredient" + i].Type != JTokenType.Null) && ammount > 0)
                {
                    ingredientToken.Ammount = ammount * ammountFactor;
                    ingredientToken.IngredientToken = jObj["ItemIngredient" + i];

                    //Recipe for first level of Subitem -> can't go deeper than 1 Level of item
                    //API doesn't allow it
                    if ((jObj["ItemIngredientRecipe" + i].Type != JTokenType.Null))
                        ingredientToken.RecipesArray = (JArray)jObj["ItemIngredientRecipe" + i];

                    ingredientTokens.Add(ingredientToken);
                }

            }


            foreach (var token in ingredientTokens)
            {
                var itemName = token.IngredientToken["Name"].ToString();
                var itemId = token.IngredientToken["ID"].ToString();
                var itemIcon = token.IngredientToken["Icon"].ToString();
                var ammount = token.Ammount;
                var mainIngredient = new Item() { Name = itemName, Id = itemId, Icon = itemIcon, Ammount = ammount };
                var type = token.IngredientToken["ItemSearchCategory"]["Name"].ToString();

                if (token.RecipesArray != null)
                {
                    //todo pass all recipes here, not only the first one
                    var subItemRecipes = await GetRecipesForSubitem(token.RecipesArray, ammount);
                    mainIngredient.ItemRecipe = subItemRecipes.FirstOrDefault();
                }
                
                if (type != "Crystals")
                    ingredients.Add(mainIngredient);
            }

            recipe.Ingredients = ingredients;

            if (isMainRecipe)
                Recipes.Add(recipe);

            recipes.Add(recipe);

            return recipes;
        }


        internal class IngredientJson
        {
            public int Ammount { get; set; }

            public JToken IngredientToken { get; set; }

            public JArray RecipesArray { get; set; }
        }

        private Recipe GenerateRecipeFromJToken(JToken jObj)
        {
            var recipe = new Recipe();
            var recipeItemName = jObj["ItemResult"]["Name"].ToString();
            var recipeItemIcon = jObj["ItemResult"]["Icon"].ToString();
            var recipeItemId = jObj["ItemResult"]["ID"].ToString();

            recipe.Id = jObj["ID"].ToString();
            //todo: Remove Object Item from Recipe Class -> set Icon and ItemName directly on Recipe Class
            //IT's stupid to have Recipe <-> Item call the same object --> Recipe

            //Base Item
            recipe.Item = new Item() { Id = recipeItemId, Name = recipeItemName, Icon = recipeItemIcon, UrlType = "Item" };

            return recipe;
        }

        private async Task<List<Recipe>> GetRecipesForSubitem(JArray JArrayRecipes, int ammountFactor)
        {
            var recipes = new List<Recipe>();
            var ingredients = new List<Item>();
            var ingredientTokens = new List<IngredientJson>();

            //Implement when Logic for multiple recipes is here
            //For now just take the first recipe in the list
            //foreach (var jObj in JArrayRecipes)
            //{                
            var jObj = JArrayRecipes[0];
            var recipe = GenerateRecipeFromJToken(jObj);


            //Getting Ingredient JTokens
            for (var i = 0; i < 10; i++)
            {
                var ingredientToken = new IngredientJson();
                var ammount = int.Parse(jObj["AmountIngredient" + i].ToString());


                if ((jObj["ItemIngredient" + i].Type != JTokenType.Null) && ammount > 0)
                {
                    ingredientToken.Ammount = ammount * ammountFactor;
                    ingredientToken.IngredientToken = jObj["ItemIngredient" + i];
                    ingredientTokens.Add(ingredientToken);
                }


                //var ammount = int.Parse(jObj["AmountIngredient" + i].ToString());

                //if ((jObj["ItemIngredient" + i].Type != JTokenType.Null) && ammount > 0)
                //    ingredientTokens.Add(jObj["ItemIngredient" + i], ammount);                    
            }

            //Getting Dictionary Ingredient, Ammount => still JTokens (parsing from Json)                
            foreach (var token in ingredientTokens)
            {
                var itemName = token.IngredientToken["Name"].ToString();
                var itemId = token.IngredientToken["ID"].ToString();
                var itemIcon = token.IngredientToken["Icon"].ToString();                
                var canHQ = int.Parse(token.IngredientToken["CanBeHq"].ToString());                
                var type = token.IngredientToken["ItemSearchCategory"]["Name"].ToString();
                var ammount = token.Ammount;


                var item = new Item() { Name = itemName, Id = itemId, Icon = itemIcon, Ammount = ammount };

                //This is for the last level of recipe
                if (canHQ == 1)
                {
                    var recipesForItem = await GetRecipeForItem(item, false, ammount);
                    item.ItemRecipe = recipesForItem.FirstOrDefault();
                }

                if(type != "Crystals")
                    ingredients.Add(item);


            }

            recipe.Ingredients = ingredients;
            recipes.Add(recipe);
            //}

            return recipes;
        }

        public void ButtonAllMaterialsClicked()
        {

            ShowAllMats = (ShowAllMats) ? false : true;

            if (ShowAllMats)
            {
                ButtonText = TextShowPreviousItemList;

                //Logic for display all base mats
                GetAllBaseMatsFromRecipes();

            }
            else
            {
                ButtonText = TextShowAllMaterials;
            }
                
            

            OnPropertyChanged("ShowAllMats");
            OnPropertyChanged("ButtonText");
        }

        private void GetAllBaseMatsFromRecipes()
        {

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
