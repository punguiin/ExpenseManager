using ExpenseManager.Maui.ViewModels;

namespace ExpenseManager.Maui
{
    public partial class TransactionDetailsPage : ContentPage
    {
        public TransactionDetailsPage(TransactionDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
