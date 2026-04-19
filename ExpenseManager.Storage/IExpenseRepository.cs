namespace ExpenseManager.Storage
{
    public interface IExpenseRepository
    {
        Task<List<WalletEntity>> GetAllWalletsAsync();
        Task<WalletEntity?> GetWalletByIdAsync(int walletId);
        Task<List<TransactionEntity>> GetTransactionsByWalletIdAsync(int walletId);
        Task<TransactionEntity?> GetTransactionByIdAsync(int transactionId);

        Task<WalletEntity> AddWalletAsync(string name, Currency currency);
        Task UpdateWalletAsync(int walletId, string name, Currency currency);
        Task DeleteWalletAsync(int walletId);

        Task<TransactionEntity> AddTransactionAsync(int walletId, decimal amount, TransactionCategory category, string description, DateTime date);
        Task UpdateTransactionAsync(int transactionId, decimal amount, TransactionCategory category, string description, DateTime date);
        Task DeleteTransactionAsync(int transactionId);
    }
}
