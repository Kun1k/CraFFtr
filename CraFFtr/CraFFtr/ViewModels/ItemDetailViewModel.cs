using CraFFtr.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CraFFtr.ViewModels
{    
    public class ItemDetailViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Item> Items { get; private set; }

        public ItemDetailViewModel(List<Item> selectedItems)
        {            
            this.Items = new ObservableCollection<Item>(selectedItems);

            OnPropertyChanged("Items");

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
