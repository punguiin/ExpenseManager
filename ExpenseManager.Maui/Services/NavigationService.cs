using ExpenseManager.Maui.ViewModels;

namespace ExpenseManager.Maui.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        private INavigation Navigation => Application.Current!.MainPage!.Navigation;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToWalletDetailsAsync(int walletId)
        {
            var viewModel = _serviceProvider.GetRequiredService<WalletDetailsViewModel>();
            viewModel.Load(walletId);
            await Navigation.PushAsync(new WalletDetailsPage(viewModel));
        }

        public async Task NavigateToTransactionDetailsAsync(int transactionId)
        {
            var viewModel = _serviceProvider.GetRequiredService<TransactionDetailsViewModel>();
            viewModel.Load(transactionId);
            await Navigation.PushAsync(new TransactionDetailsPage(viewModel));
        }

        public async Task GoBackAsync()
        {
            await Navigation.PopAsync();
        }
    }
}
