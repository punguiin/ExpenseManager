namespace ExpenseManager.Maui.Services
{
    public interface INavigationService
    {
        Task NavigateToWalletDetailsAsync(int walletId);
        Task NavigateToTransactionDetailsAsync(int transactionId);
        Task GoBackAsync();
    }
}
