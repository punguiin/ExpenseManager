namespace ExpenseManager.Storage
{
    public class ExpenseRepository : IExpenseRepository
    {
        public List<WalletEntity> GetAllWallets()
        {
            return ExampleDataStorage.Wallets;
        }

        public WalletEntity? GetWalletById(int walletId)
        {
            return ExampleDataStorage.Wallets.FirstOrDefault(w => w.Id == walletId);
        }

        public List<TransactionEntity> GetTransactionsByWalletId(int walletId)
        {
            return ExampleDataStorage.Transactions
                .Where(t => t.WalletId == walletId)
                .ToList();
        }

        public TransactionEntity? GetTransactionById(int transactionId)
        {
            return ExampleDataStorage.Transactions.FirstOrDefault(t => t.Id == transactionId);
        }
    }
}
