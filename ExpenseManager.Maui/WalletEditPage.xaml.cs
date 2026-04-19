using ExpenseManager.Maui.ViewModels;

namespace ExpenseManager.Maui
{
    public partial class WalletEditPage : ContentPage
    {
        public WalletEditPage(WalletEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
