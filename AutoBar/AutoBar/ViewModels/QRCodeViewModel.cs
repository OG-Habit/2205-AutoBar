using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using static AutoBar.Constants;
using System.IO;
using Xamarin.Forms;

namespace AutoBar.ViewModels
{
    public class QRCodeViewModel : BaseViewModel
    {
        public string qr;
        public string QR
        {
            get => qr;
            set => SetProperty(ref qr, value);
        }
        public QRCodeViewModel()
        {
            AdjustedQRLength = (int)ScaleCS.ScaleWidth(IMG_QR_LENGTH);
            QR = Xamarin.Essentials.SecureStorage.GetAsync("qr").Result;
            QRKey = qr;
        }

        private int adjustedQRLength;
        public int AdjustedQRLength
        {
            get { return adjustedQRLength; }
            set { SetProperty(ref adjustedQRLength, value); }
        }

        private string qrKey;
        public string QRKey
        {
            get { return qrKey; }
            set 
            {
                SHA256 sha = new SHA256Managed();
                byte[] b = Encoding.ASCII.GetBytes(value);
                SetProperty(ref qrKey, Convert.ToBase64String(sha.ComputeHash(b)));
            }
        }
    }
}
