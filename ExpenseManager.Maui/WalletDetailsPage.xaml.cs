using ExpenseManager.Maui.ViewModels;

namespace ExpenseManager.Maui
{
    public partial class WalletDetailsPage : ContentPage
    {
        public WalletDetailsPage(WalletDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
