using ExpenseManager.Maui.ViewModels;

namespace ExpenseManager.Maui
{
    public partial class TransactionEditPage : ContentPage
    {
        public TransactionEditPage(TransactionEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
