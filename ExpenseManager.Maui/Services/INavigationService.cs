namespace ExpenseManager.Maui.Services
{
    public interface INavigationService
    {
        Task NavigateToWalletDetailsAsync(int walletId);
        Task NavigateToWalletEditAsync(int? walletId);
        Task NavigateToTransactionDetailsAsync(int transactionId);
        Task NavigateToTransactionEditAsync(int walletId, int? transactionId);
        Task GoBackAsync();
    }
}
