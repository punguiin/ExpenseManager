using ExpenseManager.Services.Dto;
using ExpenseManager.Storage;

namespace ExpenseManager.Services
{
    public interface IExpenseService
    {
        Task<List<WalletListDto>> GetAllWalletsAsync();
        Task<WalletDetailDto?> GetWalletByIdAsync(int walletId);
        Task<TransactionDetailDto?> GetTransactionByIdAsync(int transactionId);

        Task<int> CreateWalletAsync(string name, Currency currency);
        Task UpdateWalletAsync(int walletId, string name, Currency currency);
        Task DeleteWalletAsync(int walletId);

        Task<int> CreateTransactionAsync(int walletId, decimal amount, TransactionCategory category, string description, DateTime date);
        Task UpdateTransactionAsync(int transactionId, decimal amount, TransactionCategory category, string description, DateTime date);
        Task DeleteTransactionAsync(int transactionId);
    }
}
