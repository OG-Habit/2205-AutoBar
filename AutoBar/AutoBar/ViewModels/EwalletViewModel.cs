using Xamarin.Forms;

namespace AutoBar.ViewModels
{
    public class EwalletViewModel
    {
        public double Balance { get; }
        public string ImageLink { get; }

        public EwalletViewModel()
        {
            Balance = 1200.00;
            ImageLink = "default_pic";
        }
    }
}
