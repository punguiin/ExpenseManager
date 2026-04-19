using ExpenseManager.Maui.ViewModels;

namespace ExpenseManager.Maui
{
    public partial class TransactionDetailsPage : ContentPage
    {
        private readonly TransactionDetailsViewModel _viewModel;

        public TransactionDetailsPage(TransactionDetailsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
