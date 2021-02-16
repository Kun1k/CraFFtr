
using CraFFtr.Models;
using CraFFtr.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraFFtr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        public List<string> SelectedItemIds;


        ItemDetailViewModel _viewModel;

        public ItemDetailPage(List<Item> items)
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemDetailViewModel(items);
        }

        //public static async Task<ItemDetailPage> CreateItemDetailPage(List<Item> items)
        //{
        //    ItemDetailPage page = new ItemDetailPage(items);

        //    BindingContext = _viewModel = new ItemDetailViewModel(items);

        //    return page;
        //}


        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();

        //    var recipes = (IEnumerable<Recipe>)await _viewModel.CalculateIngredients();
        //    recipes = new List<Recipe>(recipes);

        //    PopulateRecipeGrid(recipes);

        //}

        private void PopulateRecipeGrid(IEnumerable<Recipe> recipes)
        {
            
            //var grid = Gri            

        }

        private void chckSubmatsPerRecipe_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            //var chckBox = (RadioButton)sender;

            //if (chckBox.IsChecked)
            //{
            //    ItemIngredients.IsVisible = false;
            //    AllSubmatsForItem.IsEnabled = true;
            //    AllSubmatsForItem.IsVisible = true;
            //}
            //else
            //{
            //    ItemIngredients.IsVisible = true;

            //    AllSubmatsForItem.IsEnabled = false;
            //    AllSubmatsForItem.IsVisible = false;
            //}
        }

        private void ChckAllSubmats_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void ItemIngredients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //ItemIngredients.IsVisible = false;
            //AllSubmatsForItem.IsEnabled = true;
            //AllSubmatsForItem.IsVisible = true;

            //var selectedRecipe = (Recipe)ItemIngredients.SelectedItem;
            //Create RowDefinitions based on 
        }
    }
}