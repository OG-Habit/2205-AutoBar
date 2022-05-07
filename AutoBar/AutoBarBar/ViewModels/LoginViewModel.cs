using AutoBarBar.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
        }
    }
}
