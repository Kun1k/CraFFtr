
using CraFFtr.Models;
using CraFFtr.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();            
        }

        private void chckSubmatsPerRecipe_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var chckBox = (RadioButton)sender;

            if (chckBox.IsChecked)
            {
                ItemIngredients.IsVisible = false;
                RecipeSubmats.IsEnabled = true;
                RecipeSubmats.IsVisible= true;
            }
            else
            {
                ItemIngredients.IsVisible = true;
                
                RecipeSubmats.IsEnabled = false;
                RecipeSubmats.IsVisible = false;
            }
        }

        private void ChckAllSubmats_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }
    }
}