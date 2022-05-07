using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using static AutoBar.Constants;
using System.IO;

namespace AutoBar.ViewModels
{
    public class QRCodeViewModel : BaseViewModel
    {
        public QRCodeViewModel()
        {
            AdjustedQRLength = (int)ScaleCS.ScaleWidth(IMG_QR_LENGTH);
            Hash = "ivan@gmail.com";
        }

        private int adjustedQRLength;
        public int AdjustedQRLength
        {
            get { return adjustedQRLength; }
            set { SetProperty(ref adjustedQRLength, value); }
        }

        private string hash;
        public string Hash
        {
            get { return hash; }
            set 
            {
                SHA256 sha = new SHA256Managed();
                byte[] b = Encoding.ASCII.GetBytes(value);
                SetProperty(ref hash, Convert.ToBase64String(sha.ComputeHash(b)));
            }
        }
    }
}
