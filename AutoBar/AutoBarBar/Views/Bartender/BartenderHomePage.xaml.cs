﻿
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AutoBarBar.ViewModels;

namespace AutoBarBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BartenderHomePage : ContentPage
    {
        public BartenderHomePage()
        {
            InitializeComponent();
            BindingContext = BartenderHomePageViewModel.Instance;
        }
    }
}