using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Net.Mobile.Forms;
using ZXing.Common;
using static AutoBar.Constants;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;   

namespace AutoBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRCodePage : ContentPage
    {
        public QRCodePage()
        {
            InitializeComponent();
            ImgQR.BarcodeOptions.Height = (int) ScaleCS.ScaleWidth(IMG_QR_LENGTH);
            ImgQR.BarcodeOptions.Width = (int) ScaleCS.ScaleWidth(IMG_QR_LENGTH);
        }
    }
}