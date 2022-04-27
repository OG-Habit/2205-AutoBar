using AutoBar.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AutoBar.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}