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
    }
}