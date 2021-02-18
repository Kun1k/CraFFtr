using CraFFtr.Models;
using CraFFtr.REST;
using CraFFtr.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraFFtr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemSearch : ContentPage
    {
        ItemSearchViewModel _viewModel;
        

        public ItemSearch()
        {
            InitializeComponent();
            

            BindingContext = _viewModel = new ItemSearchViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //_viewModel.OnAppearing();
        }

        private async void Calculate_Clicked(object sender, EventArgs e)
        {
            var selectedItems = ItemsResult.SelectedItems;
            var itemIds = new List<string>();

            foreach(Item item in selectedItems)
            {
                itemIds.Add(item.Id);
            }

            var items = _viewModel.Items.Where(x => itemIds.Contains(x.Id)).ToList();

            await Navigation.PushAsync(new ItemDetailPage(items));
        }

        private void JobsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var jobList = new List<ClassJob>();

            foreach(ClassJob job in JobsView.SelectedItems)
            {
                jobList.Add(job);
            }

            _viewModel.SelectedJobs = jobList;
        }        
    }
}