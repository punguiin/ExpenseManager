using ExpenseManager.Maui.ViewModels;

namespace ExpenseManager.Maui
{
    public partial class WalletsPage : ContentPage
    {
        public WalletsPage(WalletsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
