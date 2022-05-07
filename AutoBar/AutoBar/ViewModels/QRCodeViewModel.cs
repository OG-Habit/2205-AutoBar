using System;
using System.Collections.Generic;
using System.Text;
using static AutoBar.Constants;

namespace AutoBar.ViewModels
{
    public class QRCodeViewModel : BaseViewModel
    {
        public QRCodeViewModel()
        {
            AdjustedQRLength = (int)ScaleCS.ScaleHeight(IMG_QR_LENGTH);
            Hash = "email";
        }

        private int adjustedQRLengthValue;
        public int AdjustedQRLength
        {
            get { return adjustedQRLengthValue; }
            set { SetProperty(ref adjustedQRLengthValue, value); }
        }

        private string hashValue;
        public string Hash
        {
            get { return hashValue; }
            set { SetProperty(ref hashValue, value); }
        }
    }
}
