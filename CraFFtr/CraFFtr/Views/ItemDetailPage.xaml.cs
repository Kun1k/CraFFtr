
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
    }
}