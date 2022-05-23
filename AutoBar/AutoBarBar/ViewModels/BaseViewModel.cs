using AutoBarBar.Models;
using AutoBarBar.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Customer> CustomerDataStore => DependencyService.Get<IDataStore<Customer>>();
        public IDataStore<Product> ProductDataStore => DependencyService.Get<IDataStore<Product>>();
        public IDataStore<OrderLine> OrderLineDataStore => DependencyService.Get<IDataStore<OrderLine>>();
        public IDataStore<Order> OrderDataStore => DependencyService.Get<IDataStore<Order>>();
        public IDataStore<Reward> RewardDataStore => DependencyService.Get<IDataStore<Reward>>();
        public IDataStore<Bartender> BartenderDataStore => DependencyService.Get<IDataStore<Bartender>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
