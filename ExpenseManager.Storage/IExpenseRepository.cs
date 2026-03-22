namespace ExpenseManager.Storage
{
    public interface IExpenseRepository
    {
        List<WalletEntity> GetAllWallets();
        WalletEntity? GetWalletById(int walletId);
        List<TransactionEntity> GetTransactionsByWalletId(int walletId);
        TransactionEntity? GetTransactionById(int transactionId);
    }
}
