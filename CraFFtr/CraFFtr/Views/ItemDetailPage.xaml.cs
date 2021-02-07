using CraFFtr.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace CraFFtr.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public List<string> SelectedItemIds;

        public ItemDetailPage(List<string> itemIds)
        {
            InitializeComponent();

            SelectedItemIds = itemIds;

            BindingContext = new ItemDetailViewModel();
        }
    }
}