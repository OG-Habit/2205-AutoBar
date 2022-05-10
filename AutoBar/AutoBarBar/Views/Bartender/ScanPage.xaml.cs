using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace AutoBarBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
            scan.OnScanResult += (result) => Device.BeginInvokeOnMainThread(() =>
            {
                
                //if (IsMatch(result.Text))
                //{
                //    LabelQRResult.Text = "true";
                //}
                //else
                //{
                //    LabelQRResult.Text = "false";
                //}
                IsMatch(result.Text);
            });
        }

        private bool IsMatch(string result)
        {
            string email = "ivan@gmail.com", emailVal;
            SHA256 sha = new SHA256Managed();

            LabelSha.Text = emailVal = Convert.ToBase64String(sha.ComputeHash(Encoding.ASCII.GetBytes(email)));
            LabelQRResult.Text = result;

            return string.Equals(result, emailVal);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}