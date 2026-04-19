using ExpenseManager.Maui.ViewModels;

namespace ExpenseManager.Maui.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        private INavigation Navigation =>
            Application.Current!.Windows[0].Page!.Navigation;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToWalletDetailsAsync(int walletId)
        {
            var viewModel = _serviceProvider.GetRequiredService<WalletDetailsViewModel>();
            viewModel.Initialize(walletId);
            await Navigation.PushAsync(new WalletDetailsPage(viewModel));
        }

        public async Task NavigateToWalletEditAsync(int? walletId)
        {
            var viewModel = _serviceProvider.GetRequiredService<WalletEditViewModel>();
            await viewModel.InitializeAsync(walletId);
            await Navigation.PushAsync(new WalletEditPage(viewModel));
        }

        public async Task NavigateToTransactionDetailsAsync(int transactionId)
        {
            var viewModel = _serviceProvider.GetRequiredService<TransactionDetailsViewModel>();
            viewModel.Initialize(transactionId);
            await Navigation.PushAsync(new TransactionDetailsPage(viewModel));
        }

        public async Task NavigateToTransactionEditAsync(int walletId, int? transactionId)
        {
            var viewModel = _serviceProvider.GetRequiredService<TransactionEditViewModel>();
            await viewModel.InitializeAsync(walletId, transactionId);
            await Navigation.PushAsync(new TransactionEditPage(viewModel));
        }

        public async Task GoBackAsync()
        {
            await Navigation.PopAsync();
        }
    }
}
