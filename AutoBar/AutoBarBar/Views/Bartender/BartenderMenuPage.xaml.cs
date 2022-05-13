using AutoBarBar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBarBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BartenderMenuPage : ContentPage
    {
        public BartenderMenuPage()
        {
            InitializeComponent();
            BindingContext = BartenderHomePageViewModel.Instance;
        }
    }
}