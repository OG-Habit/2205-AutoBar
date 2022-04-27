using AutoBarBar.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AutoBarBar.Views
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