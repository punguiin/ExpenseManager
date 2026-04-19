using ExpenseManager.Maui.ViewModels;

namespace ExpenseManager.Maui
{
    public partial class WalletDetailsPage : ContentPage
    {
        private readonly WalletDetailsViewModel _viewModel;

        public WalletDetailsPage(WalletDetailsViewModel viewModel)
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
