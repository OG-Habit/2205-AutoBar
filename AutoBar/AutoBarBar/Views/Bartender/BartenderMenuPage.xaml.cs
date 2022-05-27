using AutoBarBar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoBarBar.DateTimeHelper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBarBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BartenderMenuPage : ContentPage
    {
        BartenderHomePageViewModel vm;

        public BartenderMenuPage()
        {
            InitializeComponent();
            vm = BartenderHomePageViewModel.Instance;
            BindingContext = vm;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    vm.Time = DateTime.UtcNow.AddHours(8).ToString("hh:mm:ss:tt");
                });
                return true;
            });
        }
    }
}