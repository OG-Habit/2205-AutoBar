﻿using AutoBar.Models;
using AutoBar.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace AutoBar.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Product> ProductDataStore => DependencyService.Get<IDataStore<Product>>();
        public IDataStore<OrderLine> OrderLineDataStore => DependencyService.Get<IDataStore<OrderLine>>();
        public IDataStore<Order> OrderDataStore => DependencyService.Get<IDataStore<Order>>();
        public IDataStore<Reward> RewardDataStore => DependencyService.Get<IDataStore<Reward>>();
        public IDataStore<TransactionHistory> TransactionHistoryDataStore => DependencyService.Get<IDataStore<TransactionHistory>>();
        public IDataStore<PointsHistory> PointsHistoryDataStore => DependencyService.Get<IDataStore<PointsHistory>>();

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

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
