using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoBarBar.ViewModels
{
    public class BartenderMenuPageViewModel : BaseViewModel
    {
        public BartenderMenuPageViewModel()
        {
            Products = new ObservableCollection<Product>();
            NewOrderLine = new ObservableCollection<OrderLine>();   

            PopulateData();
        }

        async void PopulateData()
        {
            Products.Clear();
            var productsList = await ProductDataStore.GetItemsAsync();
            foreach (var product in productsList)
            {
                Products.Add(product);
            }

            NewOrderLine.Add(new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName = "Apple", Price = 45.50, Quantity = 3, CreatedOn = "7:30PM", ProductImgUrl="default_pic.png" });
        }

        #region Getters and Setters
        ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get => products;
            set => SetProperty(ref products, value);
        }

        ObservableCollection<OrderLine> newOrderLine;
        public ObservableCollection<OrderLine> NewOrderLine
        {
            get => newOrderLine;
            set => SetProperty(ref newOrderLine, value);
        }
        #endregion
    }
}
